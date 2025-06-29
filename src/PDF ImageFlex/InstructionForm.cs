using System.Windows.Forms;

namespace PDF_ImageFlex
{
    public partial class InstructionsForm : Form
    {
        public InstructionsForm(bool isBulgarianLanguage)
        {
            InitializeComponent();
            SetupUI(isBulgarianLanguage);
        }

        private void SetupUI(bool isBulgarian)
        {
            if (!isBulgarian)
            {
                this.Text = "Instructions";
                lblTitle.Text = "How to use PDF ImageFlex";
                lblInstructions.Text =
@"1. Select output format (JPG, PNG, etc.) from the dropdown menu.
2. (Optional) Enter pages to export (e.g. 1-3,5,8 or 'all' for all pages).
3. (Optional) Choose desired DPI (image quality).
4. Drag and drop your PDF file in the window or use File > Open PDF.
5. Wait for the conversion to complete.
6. Open output folder from the button or manually.

Tips:
- You can enter single pages, ranges or 'all'.
- Exported files are placed in a new folder with the PDF name.
- The app remembers recent files.
- You can change the interface language and theme from the menu.

For help, check the About menu or our GitHub page.";
            }
            else
            {
                this.Text = "Инструкции";
                lblTitle.Text = "Как да използвате PDF ImageFlex";
                lblInstructions.Text =
@"1. Изберете изходен формат (JPG, PNG и др.) от падащото меню.
2. (По избор) Въведете страници за експортиране (напр. 1-3,5,8 или 'all' за всички).
3. (По избор) Изберете желан DPI (качество на изображението).
4. Плъзнете и пуснете PDF файл в прозореца или изберете Файл > Отвори PDF.
5. Изчакайте приключване на конвертирането.
6. Отворете изходната папка с бутона или ръчно.

Съвети:
- Можете да въвеждате отделни страници, диапазони или 'all'.
- Експортираните файлове се слагат в нова папка с името на PDF-а.
- Приложението помни скорошни файлове.
- Можете да смените език и тема от менюто.

За помощ вижте меню ""За страницата"" или страницата ни в GitHub.";
            }
        }
    }
}
