namespace WordProcessing.Example
{
    partial class frmWordProcessing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabAnsTmplt = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.txtMerge = new System.Windows.Forms.TextBox();
            this.btnBrowsMerg = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnMerge = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnDistribute = new System.Windows.Forms.Button();
            this.txtJurisdiction = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabQTemplt = new System.Windows.Forms.TabPage();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGenHeader = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtQcount = new System.Windows.Forms.TextBox();
            this.txtSHCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMenu = new System.Windows.Forms.TabControl();
            this.tabExecute = new System.Windows.Forms.TabPage();
            this.btnRevert = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.btnExc = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnPrepare = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.tabReport = new System.Windows.Forms.TabPage();
            this.btnReport = new System.Windows.Forms.Button();
            this.checkedListCompany = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowseExc = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnBrowsPrep = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabAnsTmplt.SuspendLayout();
            this.tabQTemplt.SuspendLayout();
            this.tabMenu.SuspendLayout();
            this.tabExecute.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // tabAnsTmplt
            // 
            this.tabAnsTmplt.Controls.Add(this.label19);
            this.tabAnsTmplt.Controls.Add(this.btnDisplay);
            this.tabAnsTmplt.Controls.Add(this.txtMerge);
            this.tabAnsTmplt.Controls.Add(this.btnBrowsMerg);
            this.tabAnsTmplt.Controls.Add(this.label13);
            this.tabAnsTmplt.Controls.Add(this.btnMerge);
            this.tabAnsTmplt.Controls.Add(this.label12);
            this.tabAnsTmplt.Controls.Add(this.btnConvert);
            this.tabAnsTmplt.Controls.Add(this.label11);
            this.tabAnsTmplt.Controls.Add(this.btnDistribute);
            this.tabAnsTmplt.Controls.Add(this.txtJurisdiction);
            this.tabAnsTmplt.Controls.Add(this.txtCompany);
            this.tabAnsTmplt.Controls.Add(this.label8);
            this.tabAnsTmplt.Controls.Add(this.label9);
            this.tabAnsTmplt.Controls.Add(this.label10);
            this.tabAnsTmplt.Location = new System.Drawing.Point(4, 22);
            this.tabAnsTmplt.Name = "tabAnsTmplt";
            this.tabAnsTmplt.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnsTmplt.Size = new System.Drawing.Size(630, 364);
            this.tabAnsTmplt.TabIndex = 1;
            this.tabAnsTmplt.Text = "Answer Template";
            this.tabAnsTmplt.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(8, 198);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(101, 13);
            this.label19.TabIndex = 50;
            this.label19.Text = "Document to Merge";
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(499, 255);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(75, 23);
            this.btnDisplay.TabIndex = 37;
            this.btnDisplay.Text = "Display";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // txtMerge
            // 
            this.txtMerge.Location = new System.Drawing.Point(107, 195);
            this.txtMerge.Name = "txtMerge";
            this.txtMerge.Size = new System.Drawing.Size(386, 20);
            this.txtMerge.TabIndex = 49;
            // 
            // btnBrowsMerg
            // 
            this.btnBrowsMerg.Location = new System.Drawing.Point(499, 192);
            this.btnBrowsMerg.Name = "btnBrowsMerg";
            this.btnBrowsMerg.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsMerg.TabIndex = 48;
            this.btnBrowsMerg.Text = "Browse";
            this.btnBrowsMerg.UseVisualStyleBackColor = true;
            this.btnBrowsMerg.Click += new System.EventHandler(this.btnBrowsMerg_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 260);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(149, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "7) Display the Document Data";
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(499, 221);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 35;
            this.btnMerge.Text = "Generate";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 171);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "6) Merge the documents";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(499, 122);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 33;
            this.btnConvert.Text = "Generate";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(215, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "5) Convert Answer Template for Legal Users";
            // 
            // btnDistribute
            // 
            this.btnDistribute.Location = new System.Drawing.Point(499, 90);
            this.btnDistribute.Name = "btnDistribute";
            this.btnDistribute.Size = new System.Drawing.Size(75, 23);
            this.btnDistribute.TabIndex = 31;
            this.btnDistribute.Text = "Generate";
            this.btnDistribute.UseVisualStyleBackColor = true;
            this.btnDistribute.Click += new System.EventHandler(this.btnDistribute_Click);
            // 
            // txtJurisdiction
            // 
            this.txtJurisdiction.Location = new System.Drawing.Point(107, 90);
            this.txtJurisdiction.Name = "txtJurisdiction";
            this.txtJurisdiction.Size = new System.Drawing.Size(190, 20);
            this.txtJurisdiction.TabIndex = 30;
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(107, 60);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(190, 20);
            this.txtCompany.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Jurisdiction Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Company Name:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "4) Distribute the Document";
            // 
            // tabQTemplt
            // 
            this.tabQTemplt.Controls.Add(this.btnUpdate);
            this.tabQTemplt.Controls.Add(this.label7);
            this.tabQTemplt.Controls.Add(this.btnGenHeader);
            this.tabQTemplt.Controls.Add(this.txtDesc);
            this.tabQTemplt.Controls.Add(this.txtName);
            this.tabQTemplt.Controls.Add(this.txtQcount);
            this.tabQTemplt.Controls.Add(this.txtSHCount);
            this.tabQTemplt.Controls.Add(this.label6);
            this.tabQTemplt.Controls.Add(this.label5);
            this.tabQTemplt.Controls.Add(this.label4);
            this.tabQTemplt.Controls.Add(this.btnGenerate);
            this.tabQTemplt.Controls.Add(this.label3);
            this.tabQTemplt.Controls.Add(this.label2);
            this.tabQTemplt.Controls.Add(this.label1);
            this.tabQTemplt.Location = new System.Drawing.Point(4, 22);
            this.tabQTemplt.Name = "tabQTemplt";
            this.tabQTemplt.Padding = new System.Windows.Forms.Padding(3);
            this.tabQTemplt.Size = new System.Drawing.Size(630, 364);
            this.tabQTemplt.TabIndex = 0;
            this.tabQTemplt.Text = "Question Template";
            this.tabQTemplt.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(342, 223);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 19;
            this.btnUpdate.Text = "Generate";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(330, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "3) Generate the updated document if any Question deleted or Added";
            // 
            // btnGenHeader
            // 
            this.btnGenHeader.Location = new System.Drawing.Point(342, 166);
            this.btnGenHeader.Name = "btnGenHeader";
            this.btnGenHeader.Size = new System.Drawing.Size(75, 23);
            this.btnGenHeader.TabIndex = 17;
            this.btnGenHeader.Text = "Generate";
            this.btnGenHeader.UseVisualStyleBackColor = true;
            this.btnGenHeader.Click += new System.EventHandler(this.btnGenHeader_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(112, 168);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(190, 20);
            this.txtDesc.TabIndex = 16;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(112, 138);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(190, 20);
            this.txtName.TabIndex = 15;
            // 
            // txtQcount
            // 
            this.txtQcount.Location = new System.Drawing.Point(270, 62);
            this.txtQcount.Name = "txtQcount";
            this.txtQcount.Size = new System.Drawing.Size(32, 20);
            this.txtQcount.TabIndex = 8;
            // 
            // txtSHCount
            // 
            this.txtSHCount.Location = new System.Drawing.Point(145, 63);
            this.txtSHCount.Name = "txtSHCount";
            this.txtSHCount.Size = new System.Drawing.Size(32, 20);
            this.txtSHCount.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Survey Description:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Survey Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "2) Add Header and Description";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(342, 63);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "No of Question:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "No of SubHeading:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "1) Question Template Generation(select only output location)";
            // 
            // tabMenu
            // 
            this.tabMenu.Controls.Add(this.tabQTemplt);
            this.tabMenu.Controls.Add(this.tabAnsTmplt);
            this.tabMenu.Controls.Add(this.tabExecute);
            this.tabMenu.Controls.Add(this.tabReport);
            this.tabMenu.Location = new System.Drawing.Point(12, 119);
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.SelectedIndex = 0;
            this.tabMenu.Size = new System.Drawing.Size(638, 390);
            this.tabMenu.TabIndex = 34;
            this.tabMenu.SelectedIndexChanged += new System.EventHandler(this.tab_Changed);
            // 
            // tabExecute
            // 
            this.tabExecute.Controls.Add(this.btnRevert);
            this.tabExecute.Controls.Add(this.label18);
            this.tabExecute.Controls.Add(this.btnExc);
            this.tabExecute.Controls.Add(this.label15);
            this.tabExecute.Controls.Add(this.btnPrepare);
            this.tabExecute.Controls.Add(this.label14);
            this.tabExecute.Location = new System.Drawing.Point(4, 22);
            this.tabExecute.Name = "tabExecute";
            this.tabExecute.Padding = new System.Windows.Forms.Padding(3);
            this.tabExecute.Size = new System.Drawing.Size(630, 364);
            this.tabExecute.TabIndex = 2;
            this.tabExecute.Text = "Execute";
            this.tabExecute.UseVisualStyleBackColor = true;
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(257, 130);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(75, 23);
            this.btnRevert.TabIndex = 41;
            this.btnRevert.Text = "Generate";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(6, 130);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(198, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "11)Revert back the document for editing";
            // 
            // btnExc
            // 
            this.btnExc.Location = new System.Drawing.Point(257, 84);
            this.btnExc.Name = "btnExc";
            this.btnExc.Size = new System.Drawing.Size(75, 23);
            this.btnExc.TabIndex = 39;
            this.btnExc.Text = "Generate";
            this.btnExc.UseVisualStyleBackColor = true;
            this.btnExc.Click += new System.EventHandler(this.btnExc_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(6, 84);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(131, 13);
            this.label15.TabIndex = 38;
            this.label15.Text = "10)Execute the Document";
            // 
            // btnPrepare
            // 
            this.btnPrepare.Location = new System.Drawing.Point(257, 38);
            this.btnPrepare.Name = "btnPrepare";
            this.btnPrepare.Size = new System.Drawing.Size(75, 23);
            this.btnPrepare.TabIndex = 35;
            this.btnPrepare.Text = "Generate";
            this.btnPrepare.UseVisualStyleBackColor = true;
            this.btnPrepare.Click += new System.EventHandler(this.btnPrepare_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 38);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(170, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "9) Prepare document for execution";
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.btnReport);
            this.tabReport.Controls.Add(this.checkedListCompany);
            this.tabReport.Controls.Add(this.checkedListBox);
            this.tabReport.Controls.Add(this.label22);
            this.tabReport.Controls.Add(this.label21);
            this.tabReport.Controls.Add(this.label20);
            this.tabReport.Controls.Add(this.textBox1);
            this.tabReport.Controls.Add(this.btn);
            this.tabReport.Location = new System.Drawing.Point(4, 22);
            this.tabReport.Name = "tabReport";
            this.tabReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabReport.Size = new System.Drawing.Size(630, 364);
            this.tabReport.TabIndex = 3;
            this.tabReport.Text = "Report";
            this.tabReport.UseVisualStyleBackColor = true;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(462, 136);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(116, 23);
            this.btnReport.TabIndex = 61;
            this.btnReport.Text = "Generate Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // checkedListCompany
            // 
            this.checkedListCompany.FormattingEnabled = true;
            this.checkedListCompany.Location = new System.Drawing.Point(262, 75);
            this.checkedListCompany.Name = "checkedListCompany";
            this.checkedListCompany.Size = new System.Drawing.Size(120, 34);
            this.checkedListCompany.TabIndex = 60;
            // 
            // checkedListBox
            // 
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Items.AddRange(new object[] {
            "1.1",
            "1.2",
            "1.3",
            "1.4",
            "2.1",
            "2.2",
            "2.3",
            "2.4",
            "3.1",
            "3.2",
            "3.3",
            "3.4"});
            this.checkedListBox.Location = new System.Drawing.Point(262, 125);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(120, 34);
            this.checkedListBox.TabIndex = 59;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(6, 125);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(94, 13);
            this.label22.TabIndex = 57;
            this.label22.Text = "Question Numbers";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(6, 75);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(239, 13);
            this.label21.TabIndex = 55;
            this.label21.Text = "Company Names(Browse files to populate this list)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(6, 23);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(181, 13);
            this.label20.TabIndex = 49;
            this.label20.Text = "Input(files-Choose more than one file)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(193, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(397, 20);
            this.textBox1.TabIndex = 48;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(462, 49);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(116, 23);
            this.btn.TabIndex = 47;
            this.btn.Text = "Browse";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(101, 92);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(397, 20);
            this.txtOutput.TabIndex = 45;
            // 
            // btnBrowseExc
            // 
            this.btnBrowseExc.Location = new System.Drawing.Point(504, 89);
            this.btnBrowseExc.Name = "btnBrowseExc";
            this.btnBrowseExc.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseExc.TabIndex = 44;
            this.btnBrowseExc.Text = "Browse";
            this.btnBrowseExc.UseVisualStyleBackColor = true;
            this.btnBrowseExc.Click += new System.EventHandler(this.btnBrowseExc_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(101, 54);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(397, 20);
            this.txtInput.TabIndex = 43;
            // 
            // btnBrowsPrep
            // 
            this.btnBrowsPrep.Location = new System.Drawing.Point(504, 52);
            this.btnBrowsPrep.Name = "btnBrowsPrep";
            this.btnBrowsPrep.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsPrep.TabIndex = 42;
            this.btnBrowsPrep.Text = "Browse";
            this.btnBrowsPrep.UseVisualStyleBackColor = true;
            this.btnBrowsPrep.Click += new System.EventHandler(this.btnBrowsPrep_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(13, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(50, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "Input(file)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(13, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 47;
            this.label17.Text = "Output(location)";
            // 
            // frmWordProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 556);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnBrowseExc);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnBrowsPrep);
            this.Controls.Add(this.tabMenu);
            this.Name = "frmWordProcessing";
            this.Text = "Word Processing Form";
            this.Load += new System.EventHandler(this.frmWordProcessing_Load);
            this.tabAnsTmplt.ResumeLayout(false);
            this.tabAnsTmplt.PerformLayout();
            this.tabQTemplt.ResumeLayout(false);
            this.tabQTemplt.PerformLayout();
            this.tabMenu.ResumeLayout(false);
            this.tabExecute.ResumeLayout(false);
            this.tabExecute.PerformLayout();
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tabAnsTmplt;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnDistribute;
        private System.Windows.Forms.TextBox txtJurisdiction;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabQTemplt;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGenHeader;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtQcount;
        private System.Windows.Forms.TextBox txtSHCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabMenu;
        private System.Windows.Forms.TabPage tabExecute;
        private System.Windows.Forms.Button btnExc;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnPrepare;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabReport;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnBrowseExc;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnBrowsPrep;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtMerge;
        private System.Windows.Forms.Button btnBrowsMerg;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.CheckedListBox checkedListCompany;
        private System.Windows.Forms.Button btnReport;
    }
}

