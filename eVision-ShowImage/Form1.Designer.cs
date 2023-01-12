namespace eVision_ShowImage
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnGray = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.btnRecord = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDiameter = new System.Windows.Forms.Label();
            this.btnMeasureItem = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblDiameterErr = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblHeightErr = new System.Windows.Forms.Label();
            this.lblWidthErr = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnBatchSetting = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSaveSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Item_Open_Old_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.關閉ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.標準設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotGridSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_Camera = new System.Windows.Forms.Button();
            this.btn_Adjust = new System.Windows.Forms.Button();
            this.list_Of_Camera_Devices = new System.Windows.Forms.ComboBox();
            this.btn_Batch_Search = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(239, 208);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(640, 480);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.pbImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbImage_Down);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnGray
            // 
            this.btnGray.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGray.Image = global::eVision_ShowImage.Properties.Resources.data_storage;
            this.btnGray.Location = new System.Drawing.Point(30, 47);
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(100, 100);
            this.btnGray.TabIndex = 2;
            this.btnGray.Text = "載入圖像";
            this.btnGray.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGray.UseVisualStyleBackColor = true;
            this.btnGray.Click += new System.EventHandler(this.btnGray_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(1059, 83);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(121, 184);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1062, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "寬:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1062, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "高:";
            // 
            // lblWidth
            // 
            this.lblWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblWidth.Location = new System.Drawing.Point(1105, 278);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(75, 12);
            this.lblWidth.TabIndex = 7;
            // 
            // lblHeight
            // 
            this.lblHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblHeight.Location = new System.Drawing.Point(1105, 303);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(75, 12);
            this.lblHeight.TabIndex = 8;
            // 
            // btnRecord
            // 
            this.btnRecord.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRecord.Image = global::eVision_ShowImage.Properties.Resources.graphic_design;
            this.btnRecord.Location = new System.Drawing.Point(51, 393);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(100, 100);
            this.btnRecord.TabIndex = 10;
            this.btnRecord.Text = "量測標準物";
            this.btnRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1062, 328);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "直徑:";
            // 
            // lblDiameter
            // 
            this.lblDiameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblDiameter.Location = new System.Drawing.Point(1118, 328);
            this.lblDiameter.Name = "lblDiameter";
            this.lblDiameter.Size = new System.Drawing.Size(62, 12);
            this.lblDiameter.TabIndex = 13;
            // 
            // btnMeasureItem
            // 
            this.btnMeasureItem.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMeasureItem.Image = global::eVision_ShowImage.Properties.Resources.measure;
            this.btnMeasureItem.Location = new System.Drawing.Point(438, 47);
            this.btnMeasureItem.Name = "btnMeasureItem";
            this.btnMeasureItem.Size = new System.Drawing.Size(100, 100);
            this.btnMeasureItem.TabIndex = 15;
            this.btnMeasureItem.Text = "量測待測物";
            this.btnMeasureItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMeasureItem.UseVisualStyleBackColor = true;
            this.btnMeasureItem.Click += new System.EventHandler(this.btnMeasureItem_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(1064, 393);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(147, 184);
            this.listBox2.TabIndex = 16;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1061, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "孔洞清單:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(1064, 374);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 18;
            this.label5.Text = "NG孔洞清單:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1062, 692);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "尺寸NG門檻值:";
            // 
            // numThreshold
            // 
            this.numThreshold.DecimalPlaces = 1;
            this.numThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numThreshold.Location = new System.Drawing.Point(1152, 690);
            this.numThreshold.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(59, 22);
            this.numThreshold.TabIndex = 20;
            this.numThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numThreshold.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // lblDiameterErr
            // 
            this.lblDiameterErr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblDiameterErr.Location = new System.Drawing.Point(1123, 663);
            this.lblDiameterErr.Name = "lblDiameterErr";
            this.lblDiameterErr.Size = new System.Drawing.Size(62, 12);
            this.lblDiameterErr.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1067, 663);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "直徑:";
            // 
            // lblHeightErr
            // 
            this.lblHeightErr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblHeightErr.Location = new System.Drawing.Point(1110, 638);
            this.lblHeightErr.Name = "lblHeightErr";
            this.lblHeightErr.Size = new System.Drawing.Size(75, 12);
            this.lblHeightErr.TabIndex = 24;
            // 
            // lblWidthErr
            // 
            this.lblWidthErr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblWidthErr.Location = new System.Drawing.Point(1110, 613);
            this.lblWidthErr.Name = "lblWidthErr";
            this.lblWidthErr.Size = new System.Drawing.Size(75, 12);
            this.lblWidthErr.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1067, 638);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "高:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1067, 613);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "寬:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1067, 588);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 12);
            this.label13.TabIndex = 27;
            this.label13.Text = "誤差值:";
            // 
            // btnBatchSetting
            // 
            this.btnBatchSetting.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBatchSetting.Location = new System.Drawing.Point(51, 557);
            this.btnBatchSetting.Name = "btnBatchSetting";
            this.btnBatchSetting.Size = new System.Drawing.Size(121, 30);
            this.btnBatchSetting.TabIndex = 28;
            this.btnBatchSetting.Text = "批次設定尺寸";
            this.btnBatchSetting.UseVisualStyleBackColor = true;
            this.btnBatchSetting.Click += new System.EventHandler(this.btnBatchSetting_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(49, 649);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "標準高:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(49, 624);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "標準寬:";
            // 
            // numWidth
            // 
            this.numWidth.DecimalPlaces = 1;
            this.numWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numWidth.Location = new System.Drawing.Point(94, 622);
            this.numWidth.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(73, 22);
            this.numWidth.TabIndex = 31;
            this.numWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numHeight
            // 
            this.numHeight.DecimalPlaces = 1;
            this.numHeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numHeight.Location = new System.Drawing.Point(94, 647);
            this.numHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(73, 22);
            this.numHeight.TabIndex = 32;
            this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem,
            this.標準設定ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1321, 24);
            this.menuStrip1.TabIndex = 33;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemOpenSetting,
            this.MenuItemSaveSetting,
            this.Menu_Item_Open_Old_File,
            this.toolStripSeparator1,
            this.關閉ToolStripMenuItem});
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // MenuItemOpenSetting
            // 
            this.MenuItemOpenSetting.Name = "MenuItemOpenSetting";
            this.MenuItemOpenSetting.Size = new System.Drawing.Size(180, 22);
            this.MenuItemOpenSetting.Text = "開啟設定檔";
            this.MenuItemOpenSetting.Click += new System.EventHandler(this.MenuItemOpenSetting_Click);
            // 
            // MenuItemSaveSetting
            // 
            this.MenuItemSaveSetting.Name = "MenuItemSaveSetting";
            this.MenuItemSaveSetting.Size = new System.Drawing.Size(180, 22);
            this.MenuItemSaveSetting.Text = "儲存設定檔";
            this.MenuItemSaveSetting.Click += new System.EventHandler(this.MenuItemSaveSetting_Click);
            // 
            // Menu_Item_Open_Old_File
            // 
            this.Menu_Item_Open_Old_File.Name = "Menu_Item_Open_Old_File";
            this.Menu_Item_Open_Old_File.Size = new System.Drawing.Size(180, 22);
            this.Menu_Item_Open_Old_File.Text = "開啟已量測檔案";
            this.Menu_Item_Open_Old_File.Click += new System.EventHandler(this.Menu_Item_Open_Old_File_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 關閉ToolStripMenuItem
            // 
            this.關閉ToolStripMenuItem.Name = "關閉ToolStripMenuItem";
            this.關閉ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.關閉ToolStripMenuItem.Text = "關閉";
            // 
            // 標準設定ToolStripMenuItem
            // 
            this.標準設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dotGridSettingToolStripMenuItem});
            this.標準設定ToolStripMenuItem.Name = "標準設定ToolStripMenuItem";
            this.標準設定ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.標準設定ToolStripMenuItem.Text = "標準設定";
            // 
            // dotGridSettingToolStripMenuItem
            // 
            this.dotGridSettingToolStripMenuItem.Name = "dotGridSettingToolStripMenuItem";
            this.dotGridSettingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dotGridSettingToolStripMenuItem.Text = "Dot Grid Setting";
            this.dotGridSettingToolStripMenuItem.Click += new System.EventHandler(this.Menu_Dot_Grid_Setting_Click);
            // 
            // btn_Camera
            // 
            this.btn_Camera.Font = new System.Drawing.Font("新細明體", 12F);
            this.btn_Camera.Image = global::eVision_ShowImage.Properties.Resources.video_camera;
            this.btn_Camera.Location = new System.Drawing.Point(176, 47);
            this.btn_Camera.Name = "btn_Camera";
            this.btn_Camera.Size = new System.Drawing.Size(100, 100);
            this.btn_Camera.TabIndex = 34;
            this.btn_Camera.Text = "使用相機";
            this.btn_Camera.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Camera.UseVisualStyleBackColor = true;
            this.btn_Camera.Click += new System.EventHandler(this.btn_Camera_Click);
            // 
            // btn_Adjust
            // 
            this.btn_Adjust.Font = new System.Drawing.Font("新細明體", 12F);
            this.btn_Adjust.Image = global::eVision_ShowImage.Properties.Resources.maintenance;
            this.btn_Adjust.Location = new System.Drawing.Point(304, 47);
            this.btn_Adjust.Name = "btn_Adjust";
            this.btn_Adjust.Size = new System.Drawing.Size(100, 100);
            this.btn_Adjust.TabIndex = 35;
            this.btn_Adjust.Text = "校正影像";
            this.btn_Adjust.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Adjust.UseVisualStyleBackColor = true;
            this.btn_Adjust.Click += new System.EventHandler(this.btn_Adjust_Click);
            // 
            // list_Of_Camera_Devices
            // 
            this.list_Of_Camera_Devices.FormattingEnabled = true;
            this.list_Of_Camera_Devices.Location = new System.Drawing.Point(199, 10);
            this.list_Of_Camera_Devices.Name = "list_Of_Camera_Devices";
            this.list_Of_Camera_Devices.Size = new System.Drawing.Size(121, 20);
            this.list_Of_Camera_Devices.TabIndex = 37;
            this.list_Of_Camera_Devices.Visible = false;
            // 
            // btn_Batch_Search
            // 
            this.btn_Batch_Search.Font = new System.Drawing.Font("新細明體", 12F);
            this.btn_Batch_Search.Location = new System.Drawing.Point(51, 521);
            this.btn_Batch_Search.Name = "btn_Batch_Search";
            this.btn_Batch_Search.Size = new System.Drawing.Size(121, 30);
            this.btn_Batch_Search.TabIndex = 38;
            this.btn_Batch_Search.Text = "搜尋相同尺寸";
            this.btn_Batch_Search.UseVisualStyleBackColor = true;
            this.btn_Batch_Search.Click += new System.EventHandler(this.btn_Batch_Search_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 797);
            this.Controls.Add(this.btn_Batch_Search);
            this.Controls.Add(this.list_Of_Camera_Devices);
            this.Controls.Add(this.btn_Adjust);
            this.Controls.Add(this.btn_Camera);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnBatchSetting);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblDiameterErr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblHeightErr);
            this.Controls.Add(this.lblWidthErr);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numThreshold);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.btnMeasureItem);
            this.Controls.Add(this.lblDiameter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnGray);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = ".";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // picture box
        private System.Windows.Forms.PictureBox pbImage;

        // file dialog
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

        // button
        private System.Windows.Forms.Button btnGray;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnMeasureItem;
        private System.Windows.Forms.Button btnBatchSetting;
        private System.Windows.Forms.Button btn_Camera;

        // list box
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;

        //label
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblDiameter;
        private System.Windows.Forms.Label lblDiameterErr;
        private System.Windows.Forms.Label lblHeightErr;
        private System.Windows.Forms.Label lblWidthErr;

        // numeric
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.NumericUpDown numThreshold;

        // menu
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenSetting;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveSetting;
        private System.Windows.Forms.ToolStripMenuItem 關閉ToolStripMenuItem;
        private System.Windows.Forms.Button btn_Adjust;
        private System.Windows.Forms.ComboBox list_Of_Camera_Devices;
        private System.Windows.Forms.Button btn_Batch_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_Item_Open_Old_File;
        private System.Windows.Forms.ToolStripMenuItem 標準設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dotGridSettingToolStripMenuItem;
    }
}

