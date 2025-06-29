using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfiumViewer;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDF_ImageFlex
{
    public partial class Form1 : Form
    {
        public static string currentVersion = "1.0";

        private string lastOutputFolder = null;
        private bool isBulgarianLanguage = false;
        private int initialHeight = 290;
        private List<string> recentFiles = new List<string>();
        private const int maxRecentFiles = 10;
        private string recentFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PDF_ImageFlex_recent.txt");

        public Form1()
        {
            InitializeComponent();

            LoadRecentFiles();

            this.Height = initialHeight;
            cmbFormat.SelectedIndex = 0;

            // Drag & Drop
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

            btnOpenFolder.Enabled = false;

            // Стартова тема (system)
            ApplyTheme("light");

            SetCompactHeight();

            UpdateRecentMenu();
        }

        // --- Меню: Теми ---
        private void menuLightTheme_Click(object sender, EventArgs e)
        {
            ApplyTheme("light");
        }
        private void menuDarkTheme_Click(object sender, EventArgs e)
        {
            ApplyTheme("dark");
        }
        private void menuSystemTheme_Click(object sender, EventArgs e)
        {
            ApplyTheme("system");
        }

        private void ApplyTheme(string theme)
        {
            string appliedTheme = theme;
            if (theme == "system")
            {
                try
                {
                    var registry = Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                    if (registry != null)
                    {
                        object val = registry.GetValue("AppsUseLightTheme");
                        if (val != null && (int)val == 0)
                            appliedTheme = "dark";
                        else
                            appliedTheme = "light";
                    }
                }
                catch { appliedTheme = "light"; }
            }

            bool dark = appliedTheme == "dark";
            var backColor = dark ? Color.FromArgb(34, 34, 34) : Color.White;
            var foreColor = dark ? Color.White : Color.Black;
            var buttonBackColor = dark ? Color.FromArgb(64, 64, 64) : SystemColors.Control;
            var buttonForeColor = dark ? Color.White : Color.Black;

            this.BackColor = backColor;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label || ctrl is CheckBox || ctrl is ListBox || ctrl is NumericUpDown || ctrl is System.Windows.Forms.TextBox)
                {
                    ctrl.BackColor = backColor;
                    ctrl.ForeColor = foreColor;
                }
                else if (ctrl is System.Windows.Forms.Button btn)
                {
                    btn.BackColor = buttonBackColor;
                    btn.ForeColor = buttonForeColor;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = dark ? Color.Gray : Color.DarkGray;
                }
                // ComboBox НЕ се бара!
            }

            // MenuStrip остава с класически цвят – не го оцветяваме

            // Без отметки пред избора на тема
            menuLightTheme.Checked = false;
            menuDarkTheme.Checked = false;
            menuSystemTheme.Checked = false;
        }

        // --- Drag & Drop и обработка на PDF ---
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && files[0].ToLower().EndsWith(".pdf"))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // Винаги първо скрий долните елементи и изчисти лога
            progressConversion.Visible = false;
            lstLog.Visible = false;
            btnOpenFolder.Visible = false;
            lstLog.Items.Clear();

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 1 && files[0].ToLower().EndsWith(".pdf"))
            {
                // Можеш да оставиш това, ако искаш статус и в лога:
                // lstLog.Items.Add(isBulgarianLanguage ? "Файлът се обработва, изчакайте..." : "File is processing, please wait...");
                string format = cmbFormat.SelectedItem.ToString().ToLower();

                try
                {
                    using (var document = PdfDocument.Load(files[0]))
                    {
                        int maxPage = document.PageCount;
                        string errorMsg;
                        List<int> pagesToExport = ParsePageRanges(txtPages.Text, maxPage, out errorMsg);
                        if (pagesToExport == null)
                        {
                            MessageBox.Show(errorMsg, isBulgarianLanguage ? "Грешка" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Проверка дали файловете вече съществуват (ако всички са налични)
                        string pdfFileName = Path.GetFileNameWithoutExtension(files[0]);
                        string pdfDir = Path.GetDirectoryName(files[0]);
                        string outputFolder = Path.Combine(pdfDir, pdfFileName);
                        bool allExist = pagesToExport.All(page =>
                            File.Exists(Path.Combine(outputFolder, $"{pdfFileName}_page{page}.{format}")));

                        if (allExist)
                        {
                            MessageBox.Show(
                                isBulgarianLanguage ? "Всички избрани страници вече са експортирани!" : "All selected pages have already been exported!", isBulgarianLanguage ? "Информация" : "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Показвай долните елементи и resize чак тук!
                        progressConversion.Visible = true;
                        lstLog.Visible = true;
                        btnOpenFolder.Visible = true;
                        this.Height = 450;

                        // Долният код вмъкни след като е сигурно, че файлът ще се обработва!
                        if (!recentFiles.Contains(files[0]))
                            recentFiles.Insert(0, files[0]);
                        else
                        {
                            recentFiles.Remove(files[0]);
                            recentFiles.Insert(0, files[0]);
                        }
                        if (recentFiles.Count > maxRecentFiles)
                            recentFiles.RemoveAt(recentFiles.Count - 1);

                        SaveRecentFiles();
                        UpdateRecentMenu();

                        // Старт на обработката
                        ConvertPdfToImages(files[0], format, pagesToExport, (int)numDPI.Value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        isBulgarianLanguage ? "Грешка при зареждане на PDF: " + ex.Message : "Error loading PDF: " + ex.Message, isBulgarianLanguage ? "Грешка" : "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    isBulgarianLanguage ? "Моля, прикачете само PDF файл." : "Please upload only a PDF file.", isBulgarianLanguage ? "Грешка" : "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private List<int> ParsePageRanges(string input, int maxPage, out string errorMsg)
        {
            errorMsg = "";
            var pages = new List<int>();
            if (string.IsNullOrWhiteSpace(input) || input.Trim().ToLower() == "all")
            {
                for (int i = 1; i <= maxPage; i++)
                    pages.Add(i);
                return pages;
            }

            string[] parts = input.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part.Contains("-"))
                {
                    string[] range = part.Split('-');
                    if (range.Length != 2 ||
                        !int.TryParse(range[0], out int start) ||
                        !int.TryParse(range[1], out int end) ||
                        start > end)
                    {
                        errorMsg = isBulgarianLanguage ? $"Невалиден диапазон: {part}" : $"Invalid range: {part}";
                        return null;
                    }
                    for (int i = start; i <= end; i++)
                    {
                        if (i < 1 || i > maxPage)
                        {
                            errorMsg = isBulgarianLanguage ? $"Страница {i} не съществува (файлът има {maxPage} страници)." : $"Page {i} does not exist (file has {maxPage} pages).";
                            return null;
                        }
                        pages.Add(i);
                    }
                }
                else
                {
                    if (!int.TryParse(part, out int num))
                    {
                        errorMsg = isBulgarianLanguage
                            ? $"Невалиден номер на страница: {part}"
                            : $"Invalid page number: {part}";
                        return null;
                    }
                    if (num < 1 || num > maxPage)
                    {
                        errorMsg = isBulgarianLanguage
                            ? $"Страница {num} не съществува (файлът има {maxPage} страници)."
                            : $"Page {num} does not exist (file has {maxPage} pages).";
                        return null;
                    }
                    pages.Add(num);
                }
            }
            return pages.Distinct().OrderBy(x => x).ToList();
        }

        private string GroupRanges(List<int> pages)
        {
            if (pages == null || pages.Count == 0) return "";
            pages = pages.OrderBy(x => x).ToList();
            List<string> result = new List<string>();
            int start = pages[0], end = pages[0];
            for (int i = 1; i < pages.Count; i++)
            {
                if (pages[i] == end + 1)
                {
                    end = pages[i];
                }
                else
                {
                    result.Add(start == end ? $"{start}" : $"{start}-{end}");
                    start = end = pages[i];
                }
            }
            result.Add(start == end ? $"{start}" : $"{start}-{end}");
            return string.Join(",", result);
        }

        private void ConvertPdfToImages(string pdfPath, string imageFormat, List<int> pagesToExport, int dpi)
        {
            List<int> exportedPages = new List<int>();
            List<int> skippedPages = new List<int>();
            List<int> invalidPages = new List<int>();

            lstLog.Items.Clear();

            try
            {
                string pdfFileName = Path.GetFileNameWithoutExtension(pdfPath);
                string pdfDir = Path.GetDirectoryName(pdfPath);

                string outputFolder = Path.Combine(pdfDir, pdfFileName);
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                using (var document = PdfDocument.Load(pdfPath))
                {
                    int maxPage = document.PageCount;

                    progressConversion.Minimum = 0;
                    progressConversion.Maximum = pagesToExport.Count;
                    progressConversion.Value = 0;

                    foreach (int page in pagesToExport)
                    {
                        int pageIndex = page - 1;

                        if (page < 1 || page > maxPage)
                        {
                            invalidPages.Add(page);
                            lstLog.Items.Add(isBulgarianLanguage ? $"Невалидна страница: {page}" : $"Invalid page: {page}");
                            lstLog.TopIndex = lstLog.Items.Count - 1;
                            continue;
                        }

                        string ext = imageFormat;
                        string outputPath = Path.Combine(
                            outputFolder,
                            $"{pdfFileName}_page{page}.{ext}"
                        );

                        if (File.Exists(outputPath))
                        {
                            skippedPages.Add(page);
                            lstLog.Items.Add(isBulgarianLanguage ? $"Пропусната (вече съществува) страница {page}: {outputPath}" : $"Skipped (already exists) page {page}: {outputPath}"); lstLog.TopIndex = lstLog.Items.Count - 1;
                            continue;
                        }

                        using (var image = document.Render(pageIndex, dpi, dpi, true))
                        {
                            switch (ext)
                            {
                                case "png":
                                    image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                                case "bmp":
                                    image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                case "gif":
                                    image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                case "tiff":
                                    image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Tiff);
                                    break;
                                default:
                                    image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                            }
                        }
                        exportedPages.Add(page);
                        lstLog.Items.Add(isBulgarianLanguage ? $"Експортирана страница {page}: {outputPath}" : $"Exported page {page}: {outputPath}");
                        lstLog.TopIndex = lstLog.Items.Count - 1;
                        progressConversion.Value += 1;
                        progressConversion.Refresh();
                    }
                }

                string msg = "";
                if (exportedPages.Count > 0)
                {
                    if (isBulgarianLanguage)
                    {
                        msg += $"📤 Обработени страници: {GroupRanges(exportedPages)}\n\n";
                        msg += $"📁 Файловете се намират в:\n{Path.Combine(Path.GetDirectoryName(pdfPath), Path.GetFileNameWithoutExtension(pdfPath))}\n";
                    }
                    else
                    {
                        msg += $"📤 Processed pages: {GroupRanges(exportedPages)}\n\n";
                        msg += $"📁 The files are located in:\n{Path.Combine(Path.GetDirectoryName(pdfPath), Path.GetFileNameWithoutExtension(pdfPath))}\n";
                    }
                    lastOutputFolder = outputFolder;
                    btnOpenFolder.Enabled = true;
                }
                else
                {
                    lastOutputFolder = null;
                    btnOpenFolder.Enabled = false;
                }

                if (skippedPages.Count > 0)
                    msg += isBulgarianLanguage
                        ? $"\nℹ️ Следните страници вече съществуват и не бяха извлечени повторно: {GroupRanges(skippedPages)}\n"
                        : $"\nℹ️ These pages already exist and were not re-exported: {GroupRanges(skippedPages)}\n";
                if (invalidPages.Count > 0)
                    msg += isBulgarianLanguage
                        ? $"\n⚠️ Невалидни страници (файлът има {exportedPages.Count + skippedPages.Count + invalidPages.Count}): {GroupRanges(invalidPages)}\n"
                        : $"\n⚠️ Invalid pages (file has {exportedPages.Count + skippedPages.Count + invalidPages.Count}): {GroupRanges(invalidPages)}\n";
                if (msg == "")
                    msg = isBulgarianLanguage
                        ? "Нито една от въведените страници не е обработена."
                        : "None of the entered pages have been processed.";

                MessageBox.Show(msg, "PDF ImageFlex", MessageBoxButtons.OK, MessageBoxIcon.Information);
                progressConversion.Value = 0;
            }
            catch (Exception ex)
            {
                if (isBulgarianLanguage)
                    MessageBox.Show("Грешка при конвертиране: " + ex.Message, "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error while converting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressConversion.Value = 0;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lastOutputFolder) && Directory.Exists(lastOutputFolder))
            {
                System.Diagnostics.Process.Start("explorer.exe", lastOutputFolder);
            }
            else
            {
                if (isBulgarianLanguage)
                    MessageBox.Show("Папката не съществува!", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("The folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuOpenPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";
                ofd.Title = "Open PDF file";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Симулираме drag&drop обработката:
                    string[] files = new string[] { ofd.FileName };
                    Form1_DragDrop(this, new DragEventArgs(
                        new DataObject(DataFormats.FileDrop, files),
                        0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy
                    ));
                }
            }
        }

        private void menuRefresh_Click(object sender, EventArgs e)
        {
            // Скрий лог, прогрес и бутона
            progressConversion.Visible = false;
            lstLog.Visible = false;
            btnOpenFolder.Visible = false;

            // Върни формата към компактната ѝ височина
            SetCompactHeight();

            // Изчисти/ресетни полетата
            lstLog.Items.Clear();
            progressConversion.Value = 0;
            cmbFormat.SelectedIndex = 0;
            txtPages.Text = "";
            numDPI.Value = 300;

            this.Height = initialHeight;

        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            using (var about = new AboutForm(isBulgarianLanguage))
            {
                about.ShowDialog(this);
            }
        }

        private void menuInstructions_Click(object sender, EventArgs e)
        {
            using (var instr = new InstructionsForm(isBulgarianLanguage))
            {
                instr.ShowDialog(this);
            }
        }

        private async void menuCheckUpdates_Click(object sender, EventArgs e)
        {
           // string checkingText = isBulgarianLanguage ? "Проверка за обновления..." : "Checking for updates..."; MessageBox.Show(checkingText, "PDF ImageFlex", MessageBoxButtons.OK, MessageBoxIcon.Information);

            string latest = await GetLatestVersionFromGitHub();

            if (latest == null)
            {
                MessageBox.Show(isBulgarianLanguage ? "Грешка при връзка с интернет или GitHub!" : "Error connecting to the Internet or GitHub!", "PDF ImageFlex", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (latest == Form1.currentVersion)
            {
                MessageBox.Show(isBulgarianLanguage ? "Използвате последната версия на PDF ImageFlex." : "You are using the latest version of PDF ImageFlex.", "PDF ImageFlex", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult res = MessageBox.Show(
                    (isBulgarianLanguage
                        ? $"Налична е нова версия: v{latest}\n\nДа отворя страницата в GitHub?" : $"A new version is available: v{latest}\n\nOpen GitHub page?"), "PDF ImageFlex", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://github.com/hmartinov/PDF_ImageFlex",
                        UseShellExecute = true
                    });
                }
            }
        }

        private void menuLangEnglish_Click(object sender, EventArgs e)
        {
            isBulgarianLanguage = false;

            // Менюта
            menuFile.Text = "File";
            menuOpenPDF.Text = "Open PDF";
            menuRecent.Text = "Recent";
            menuRefresh.Text = "Refresh";
            menuExit.Text = "Exit";
            menuView.Text = "View";
            menuTheme.Text = "Theme";
            menuSystemTheme.Text = "System Theme";
            menuLightTheme.Text = "Light Theme";
            menuDarkTheme.Text = "Dark Theme";
            menuHelp.Text = "Help";
            menuAbout.Text = "About";
            menuInstructions.Text = "Instructions";
            menuCheckUpdates.Text = "Check for Updates";
            menuLanguage.Text = "🌐";
            menuLangEnglish.Text = "English";
            menuLangBulgarian.Text = "Bulgarian";

            // Етикети, бутони, tooltips
            lblDropInfo.Text = "Drop PDF file here or select from menu";
            lblFileType.Text = "Choose Format:";
            lblPagesInfo.Text = "Pages to export\r\n(e.g. 1-3,5,7 or all):";
            lblSelectDPI.Text = "Quality (DPI):";
            btnOpenFolder.Text = "Open Output Folder";
            toolTipDPI.SetToolTip(numDPI, "Enter DPI (from 72 to 2400)");

            menuLangEnglish.Font = new Font(menuLangEnglish.Font, FontStyle.Bold);
            menuLangBulgarian.Font = new Font(menuLangBulgarian.Font, FontStyle.Regular);

            UpdateRecentMenu();
        }

        private void menuLangBulgarian_Click(object sender, EventArgs e)
        {
            isBulgarianLanguage = true;

            // Менюта
            menuFile.Text = "Файл";
            menuOpenPDF.Text = "Отвори PDF";
            menuRecent.Text = "Скорошни";
            menuRefresh.Text = "Опресни";
            menuExit.Text = "Изход";
            menuView.Text = "Изглед";
            menuTheme.Text = "Тема";
            menuSystemTheme.Text = "Системна тема";
            menuLightTheme.Text = "Светла тема";
            menuDarkTheme.Text = "Тъмна тема";
            menuHelp.Text = "Помощ";
            menuAbout.Text = "За програмата";
            menuInstructions.Text = "Инструкции";
            menuCheckUpdates.Text = "Провери за обновления";
            menuLanguage.Text = "🌐";
            menuLangEnglish.Text = "Английски";
            menuLangBulgarian.Text = "Български";

            // Етикети, бутони, tooltips
            lblDropInfo.Text = "Пуснете PDF файла тук или\r\nизберете от менюто";
            lblFileType.Text = "Избери формат:";
            lblPagesInfo.Text = "Страници за експортиране\r\n(напр. 1-3,5,7 или all):";
            lblSelectDPI.Text = "Качество (DPI):";
            btnOpenFolder.Text = "Отвори изходната папка";
            toolTipDPI.SetToolTip(numDPI, "Въведете DPI (от 72 до 2400)");

            menuLangEnglish.Font = new Font(menuLangEnglish.Font, FontStyle.Regular);
            menuLangBulgarian.Font = new Font(menuLangBulgarian.Font, FontStyle.Bold);

            UpdateRecentMenu();
        }

        private void SetCompactHeight()
        {
            int minHeight = 250;
            int maxBottom = 0;

            // Менюто (MenuStrip) не е част от Controls, но ClientRectangle започва ПОД него!
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Visible && !(ctrl is MenuStrip))
                {
                    int ctrlBottom = ctrl.Bottom;
                    if (ctrlBottom > maxBottom)
                        maxBottom = ctrlBottom;
                }
            }

            // Вземи рамката на прозореца (заглавна лента + border)
            int border = this.Height - this.ClientSize.Height;

            // Новата височина: най-долния видим + padding + border
            int newHeight = maxBottom + 70 + border;
            if (newHeight < minHeight + border)
                newHeight = minHeight + border;

            this.Height = newHeight;
        }

        private void UpdateRecentMenu()
        {
            menuRecent.DropDownItems.Clear();

            if (recentFiles.Count == 0)
            {
                var emptyItem = new ToolStripMenuItem(isBulgarianLanguage ? "Няма скорошни файлове" : "No recent files");
                emptyItem.Enabled = false;
                menuRecent.DropDownItems.Add(emptyItem);
            }
            else
            {
                foreach (var file in recentFiles)
                {
                    var item = new ToolStripMenuItem(Path.GetFileName(file));
                    item.ToolTipText = file;
                    item.Click += (s, e) => OpenRecentFile(file);
                    menuRecent.DropDownItems.Add(item);
                }

                // Разделител и бутон "Изчисти списъка"
                menuRecent.DropDownItems.Add(new ToolStripSeparator());
                var clearItem = new ToolStripMenuItem(isBulgarianLanguage ? "Изчисти списъка" : "Clear list");
                clearItem.Click += (s, e) => ClearRecentList();
                menuRecent.DropDownItems.Add(clearItem);
            }
        }

        private void OpenRecentFile(string file)
        {
            if (File.Exists(file))
            {
                // Симулирай Drag&Drop обработка:
                string[] files = new string[] { file };
                Form1_DragDrop(this, new DragEventArgs(
                    new DataObject(DataFormats.FileDrop, files),
                    0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy
                ));

                // Фикс: Ако не е стартирал експорт, върни прозореца в компактен вид!
                if (!progressConversion.Visible)
                {
                    this.Height = initialHeight;
                }
            }
            else
            {
                MessageBox.Show(isBulgarianLanguage ? "Файлът не съществува!" : "File does not exist!","PDF ImageFlex", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                recentFiles.Remove(file);
                SaveRecentFiles();
                UpdateRecentMenu();
            }
        }

        private void ClearRecentList()
        {
            recentFiles.Clear();
            SaveRecentFiles();
            UpdateRecentMenu();
        }

        private void LoadRecentFiles()
        {
            recentFiles.Clear();
            if (File.Exists(recentFilePath))
            {
                foreach (var line in File.ReadAllLines(recentFilePath))
                {
                    if (!string.IsNullOrWhiteSpace(line) && File.Exists(line))
                        recentFiles.Add(line);
                }
            }
            UpdateRecentMenu();
        }

        private void SaveRecentFiles()
        {
            try
            {
                File.WriteAllLines(recentFilePath, recentFiles);
            }
            catch { /* игнорирай грешки */ }
        }
        private async Task<string> GetLatestVersionFromGitHub()
        {
            string url = "https://raw.githubusercontent.com/hmartinov/PDF_ImageFlex/b1962074880eeba00c5c4625fc9c36110ab39719/version.txt";
        
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    return response.Trim();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
