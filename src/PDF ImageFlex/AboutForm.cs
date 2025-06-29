using System.Diagnostics;
using System.Windows.Forms;

namespace PDF_ImageFlex
{
    public partial class AboutForm : Form
    {
        private bool isBulgarian;

        public AboutForm(bool isBulgarianLanguage)
        {
            InitializeComponent();
            isBulgarian = isBulgarianLanguage;
            SetContent();
        }

        private void SetContent()
        {
            string programName = "PDF ImageFlex";
            string version = "v" + Form1.currentVersion;
            string author = "H. Martinov";
            string email = "hmartinov@dmail.ai";
            string github = "https://github.com/hmartinov/PDF_ImageFlex";
            if (!isBulgarian)
            {
                lblTitle.Text = $"{programName} {version}";
                lblDescription.Text =
                    "Description:\r\n" +
                    "A modern and easy-to-use tool for converting PDF files to image formats (JPG, PNG, TIFF, BMP, GIF). " +
                    "Supports batch export, selection of page ranges, DPI and language (English/Bulgarian). Recent files and theme customization included.\r\n\r\n" +
                    "Main features:\r\n" +
                    "• Drag and drop PDF export\r\n" +
                    "• Batch and page range export\r\n" +
                    "• Multiple output formats\r\n" +
                    "• Quality (DPI) selection\r\n" +
                    "• Dark and light theme\r\n" +
                    "• English / Bulgarian interface\r\n" +
                    "• Recent files list\r\n\r\n" +
                    $"Author: {author}\r\nEmail: {email}\r\n\r\n" +
                    "License: MIT License\r\n" +
                    "GitHub: ";
                lblLicense.Text = "MIT License – free for personal and commercial use. See GitHub for details.";
                linkGithub.Text = github;
            }
            else
            {
                lblTitle.Text = $"{programName} {version}";
                lblDescription.Text =
                    "Описание:\r\n" +
                    "Съвременен и удобен инструмент за конвертиране на PDF файлове в изображения (JPG, PNG, TIFF, BMP, GIF). " +
                    "Поддържа пакетен експорт, избор на страници, DPI, двуезичен интерфейс (български/английски), скорошни файлове и смяна на тема.\r\n\r\n" +
                    "Основни функции:\r\n" +
                    "• Drag & drop експорт\r\n" +
                    "• Пакетен и по-страници експорт\r\n" +
                    "• Различни изходни формати\r\n" +
                    "• Избор на качество (DPI)\r\n" +
                    "• Светла и тъмна тема\r\n" +
                    "• Интерфейс на български/английски\r\n" +
                    "• Списък със скорошни файлове\r\n\r\n" +
                    $"Автор: {author}\r\nИмейл: {email}\r\n\r\n" +
                    "Лиценз: MIT License\r\n" +
                    "GitHub: ";
                lblLicense.Text = "MIT лиценз – свободен за лично и търговско ползване. Виж GitHub за подробности.";
                linkGithub.Text = github;
            }
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(linkGithub.Text) { UseShellExecute = true });
        }
    }
}
