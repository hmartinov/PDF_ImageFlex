using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PDF_ImageFlex
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressConversion = new System.Windows.Forms.ProgressBar();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.lblPagesInfo = new System.Windows.Forms.Label();
            this.txtPages = new System.Windows.Forms.TextBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.lblFileType = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.numDPI = new System.Windows.Forms.NumericUpDown();
            this.lblSelectDPI = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSystemTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLightTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDarkTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInstructions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLangEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLangBulgarian = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipDPI = new System.Windows.Forms.ToolTip(this.components);
            this.lblDropInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numDPI)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressConversion
            // 
            resources.ApplyResources(this.progressConversion, "progressConversion");
            this.progressConversion.Name = "progressConversion";
            // 
            // cmbFormat
            // 
            resources.ApplyResources(this.cmbFormat, "cmbFormat");
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            resources.GetString("cmbFormat.Items"),
            resources.GetString("cmbFormat.Items1"),
            resources.GetString("cmbFormat.Items2"),
            resources.GetString("cmbFormat.Items3"),
            resources.GetString("cmbFormat.Items4")});
            this.cmbFormat.Name = "cmbFormat";
            // 
            // lblPagesInfo
            // 
            resources.ApplyResources(this.lblPagesInfo, "lblPagesInfo");
            this.lblPagesInfo.Name = "lblPagesInfo";
            // 
            // txtPages
            // 
            resources.ApplyResources(this.txtPages, "txtPages");
            this.txtPages.Name = "txtPages";
            // 
            // lstLog
            // 
            resources.ApplyResources(this.lstLog, "lstLog");
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Name = "lstLog";
            // 
            // lblFileType
            // 
            resources.ApplyResources(this.lblFileType, "lblFileType");
            this.lblFileType.Name = "lblFileType";
            // 
            // btnOpenFolder
            // 
            resources.ApplyResources(this.btnOpenFolder, "btnOpenFolder");
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // numDPI
            // 
            resources.ApplyResources(this.numDPI, "numDPI");
            this.numDPI.Maximum = new decimal(new int[] {
            2400,
            0,
            0,
            0});
            this.numDPI.Minimum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numDPI.Name = "numDPI";
            this.toolTipDPI.SetToolTip(this.numDPI, resources.GetString("numDPI.ToolTip"));
            this.numDPI.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // lblSelectDPI
            // 
            resources.ApplyResources(this.lblSelectDPI, "lblSelectDPI");
            this.lblSelectDPI.Name = "lblSelectDPI";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuHelp,
            this.menuLanguage});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.TabStop = true;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenPDF,
            this.menuRecent,
            this.menuRefresh,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            resources.ApplyResources(this.menuFile, "menuFile");
            // 
            // menuOpenPDF
            // 
            this.menuOpenPDF.Name = "menuOpenPDF";
            resources.ApplyResources(this.menuOpenPDF, "menuOpenPDF");
            this.menuOpenPDF.Click += new System.EventHandler(this.menuOpenPDF_Click);
            // 
            // menuRecent
            // 
            this.menuRecent.Name = "menuRecent";
            resources.ApplyResources(this.menuRecent, "menuRecent");
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            resources.ApplyResources(this.menuRefresh, "menuRefresh");
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            resources.ApplyResources(this.menuExit, "menuExit");
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTheme});
            this.menuView.Name = "menuView";
            resources.ApplyResources(this.menuView, "menuView");
            // 
            // menuTheme
            // 
            this.menuTheme.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSystemTheme,
            this.menuLightTheme,
            this.menuDarkTheme});
            resources.ApplyResources(this.menuTheme, "menuTheme");
            this.menuTheme.Name = "menuTheme";
            // 
            // menuSystemTheme
            // 
            this.menuSystemTheme.Name = "menuSystemTheme";
            resources.ApplyResources(this.menuSystemTheme, "menuSystemTheme");
            // 
            // menuLightTheme
            // 
            this.menuLightTheme.Name = "menuLightTheme";
            resources.ApplyResources(this.menuLightTheme, "menuLightTheme");
            // 
            // menuDarkTheme
            // 
            this.menuDarkTheme.Name = "menuDarkTheme";
            resources.ApplyResources(this.menuDarkTheme, "menuDarkTheme");
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout,
            this.menuInstructions,
            this.menuCheckUpdates});
            this.menuHelp.Name = "menuHelp";
            resources.ApplyResources(this.menuHelp, "menuHelp");
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            resources.ApplyResources(this.menuAbout, "menuAbout");
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // menuInstructions
            // 
            this.menuInstructions.Name = "menuInstructions";
            resources.ApplyResources(this.menuInstructions, "menuInstructions");
            this.menuInstructions.Click += new System.EventHandler(this.menuInstructions_Click);
            // 
            // menuCheckUpdates
            // 
            this.menuCheckUpdates.Name = "menuCheckUpdates";
            resources.ApplyResources(this.menuCheckUpdates, "menuCheckUpdates");
            this.menuCheckUpdates.Click += new System.EventHandler(this.menuCheckUpdates_Click);
            // 
            // menuLanguage
            // 
            this.menuLanguage.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLangEnglish,
            this.menuLangBulgarian});
            this.menuLanguage.Name = "menuLanguage";
            resources.ApplyResources(this.menuLanguage, "menuLanguage");
            // 
            // menuLangEnglish
            // 
            this.menuLangEnglish.Name = "menuLangEnglish";
            resources.ApplyResources(this.menuLangEnglish, "menuLangEnglish");
            this.menuLangEnglish.Click += new System.EventHandler(this.menuLangEnglish_Click);
            // 
            // menuLangBulgarian
            // 
            this.menuLangBulgarian.Name = "menuLangBulgarian";
            resources.ApplyResources(this.menuLangBulgarian, "menuLangBulgarian");
            this.menuLangBulgarian.Click += new System.EventHandler(this.menuLangBulgarian_Click);
            // 
            // lblDropInfo
            // 
            resources.ApplyResources(this.lblDropInfo, "lblDropInfo");
            this.lblDropInfo.Name = "lblDropInfo";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblSelectDPI);
            this.Controls.Add(this.numDPI);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.lblFileType);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.txtPages);
            this.Controls.Add(this.lblPagesInfo);
            this.Controls.Add(this.cmbFormat);
            this.Controls.Add(this.progressConversion);
            this.Controls.Add(this.lblDropInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numDPI)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ProgressBar progressConversion;
        private ComboBox cmbFormat;
        private Label lblPagesInfo;
        private TextBox txtPages;
        private ListBox lstLog;
        private Label lblFileType;
        private Button btnOpenFolder;
        private NumericUpDown numDPI;
        private Label lblSelectDPI;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuFile;
        private ToolTip toolTipDPI;
        private ToolStripMenuItem menuOpenPDF;
        private ToolStripMenuItem menuRefresh;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;
        private ToolStripMenuItem menuInstructions;
        private ToolStripMenuItem menuCheckUpdates;
        private ToolStripMenuItem menuTheme;
        private ToolStripMenuItem menuSystemTheme;
        private ToolStripMenuItem menuLightTheme;
        private ToolStripMenuItem menuDarkTheme;
        private ToolStripMenuItem menuLanguage;
        private ToolStripMenuItem menuLangEnglish;
        private ToolStripMenuItem menuLangBulgarian;
        private ToolStripMenuItem menuRecent;
        private Label lblDropInfo;
    }
}