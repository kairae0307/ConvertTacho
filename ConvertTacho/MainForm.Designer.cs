namespace ConvertTacho
{
	partial class MainForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxLat = new System.Windows.Forms.TextBox();
            this.textBoxLng = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.comboBoxMapType = new System.Windows.Forms.ComboBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBoxPlacemarkInfo = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panelMenu = new BSE.Windows.Forms.Panel();
            this.PORT = new System.Windows.Forms.Label();
            this.comboBoxSerialPort = new System.Windows.Forms.ComboBox();
            this.checkBox_DrawIndex = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.MainMap = new Demo.WindowsForms.Map();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxLat
            // 
            this.textBoxLat.Location = new System.Drawing.Point(11, 23);
            this.textBoxLat.Name = "textBoxLat";
            this.textBoxLat.Size = new System.Drawing.Size(142, 21);
            this.textBoxLat.TabIndex = 1;
            // 
            // textBoxLng
            // 
            this.textBoxLng.Location = new System.Drawing.Point(11, 53);
            this.textBoxLng.Name = "textBoxLng";
            this.textBoxLng.Size = new System.Drawing.Size(142, 21);
            this.textBoxLng.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 32);
            this.button1.TabIndex = 3;
            this.button1.Text = "Go To!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(164, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Lat";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(164, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Lng";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBoxLng);
            this.groupBox1.Controls.Add(this.textBoxLat);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.groupBox1.Location = new System.Drawing.Point(51, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 225);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coordinates";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(154, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 11);
            this.label5.TabIndex = 47;
            this.label5.Text = "Address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 80);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(141, 87);
            this.textBox1.TabIndex = 46;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(105, 178);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(81, 32);
            this.button6.TabIndex = 6;
            this.button6.Text = "Map Save";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // comboBoxMapType
            // 
            this.comboBoxMapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapType.FormattingEnabled = true;
            this.comboBoxMapType.Location = new System.Drawing.Point(9, 24);
            this.comboBoxMapType.Name = "comboBoxMapType";
            this.comboBoxMapType.Size = new System.Drawing.Size(143, 20);
            this.comboBoxMapType.TabIndex = 7;
            this.comboBoxMapType.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapType_SelectedIndexChanged);
            this.comboBoxMapType.DropDownClosed += new System.EventHandler(this.comboBoxMapType_DropDownClosed);
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(9, 53);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(143, 20);
            this.comboBoxMode.TabIndex = 8;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            this.comboBoxMode.DropDownClosed += new System.EventHandler(this.comboBoxMode_DropDownClosed);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxMode);
            this.groupBox2.Controls.Add(this.comboBoxMapType);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.groupBox2.Location = new System.Drawing.Point(51, 262);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 75);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(162, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(162, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Type";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(110, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 17);
            this.button2.TabIndex = 6;
            this.button2.Text = "Add Route";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 19);
            this.button3.TabIndex = 42;
            this.button3.Text = "Start Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(15, 33);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(81, 17);
            this.button4.TabIndex = 43;
            this.button4.Text = "Add Marker";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(110, 55);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(81, 34);
            this.button5.TabIndex = 44;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.checkBoxPlacemarkInfo);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.groupBox3.Location = new System.Drawing.Point(51, 385);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(204, 97);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Routing";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(110, 18);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(81, 19);
            this.button7.TabIndex = 46;
            this.button7.Text = "End Set";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // checkBoxPlacemarkInfo
            // 
            this.checkBoxPlacemarkInfo.AutoSize = true;
            this.checkBoxPlacemarkInfo.Checked = true;
            this.checkBoxPlacemarkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPlacemarkInfo.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxPlacemarkInfo.Location = new System.Drawing.Point(7, 58);
            this.checkBoxPlacemarkInfo.Name = "checkBoxPlacemarkInfo";
            this.checkBoxPlacemarkInfo.Size = new System.Drawing.Size(96, 24);
            this.checkBoxPlacemarkInfo.TabIndex = 45;
            this.checkBoxPlacemarkInfo.Text = "Place Info";
            this.checkBoxPlacemarkInfo.UseVisualStyleBackColor = true;
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.AliceBlue;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(4, 64);
            this.trackBar1.Maximum = 17;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 389);
            this.trackBar1.TabIndex = 29;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 12;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // panelMenu
            // 
            this.panelMenu.AssociatedSplitter = null;
            this.panelMenu.BackColor = System.Drawing.Color.Transparent;
            this.panelMenu.CaptionFont = new System.Drawing.Font("굴림", 12.5F, System.Drawing.FontStyle.Bold);
            this.panelMenu.CaptionHeight = 27;
            this.panelMenu.Controls.Add(this.button11);
            this.panelMenu.Controls.Add(this.PORT);
            this.panelMenu.Controls.Add(this.comboBoxSerialPort);
            this.panelMenu.Controls.Add(this.checkBox_DrawIndex);
            this.panelMenu.Controls.Add(this.button9);
            this.panelMenu.Controls.Add(this.button8);
            this.panelMenu.Controls.Add(this.label7);
            this.panelMenu.Controls.Add(this.label6);
            this.panelMenu.Controls.Add(this.button10);
            this.panelMenu.Controls.Add(this.button12);
            this.panelMenu.Controls.Add(this.pictureBox2);
            this.panelMenu.Controls.Add(this.pictureBox1);
            this.panelMenu.Controls.Add(this.trackBar1);
            this.panelMenu.Controls.Add(this.groupBox3);
            this.panelMenu.Controls.Add(this.groupBox1);
            this.panelMenu.Controls.Add(this.groupBox2);
            this.panelMenu.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(65)))), ((int)(((byte)(118)))));
            this.panelMenu.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panelMenu.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))));
            this.panelMenu.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.panelMenu.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(222)))));
            this.panelMenu.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(203)))), ((int)(((byte)(136)))));
            this.panelMenu.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.ContentGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this.panelMenu.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.panelMenu.CustomColors.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelMenu.Image = null;
            this.panelMenu.Location = new System.Drawing.Point(868, 0);
            this.panelMenu.MinimumSize = new System.Drawing.Size(27, 27);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelMenu.ShowExpandIcon = true;
            this.panelMenu.Size = new System.Drawing.Size(270, 689);
            this.panelMenu.TabIndex = 40;
            this.panelMenu.Text = "Menu";
            this.panelMenu.ToolTipTextCloseIcon = null;
            this.panelMenu.ToolTipTextExpandIconPanelCollapsed = "maximize";
            this.panelMenu.ToolTipTextExpandIconPanelExpanded = "minimize";
            this.panelMenu.CloseClick += new System.EventHandler<System.EventArgs>(this.panelMenu_CloseClick);
            // 
            // PORT
            // 
            this.PORT.AutoSize = true;
            this.PORT.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.PORT.ForeColor = System.Drawing.Color.Black;
            this.PORT.Location = new System.Drawing.Point(29, 349);
            this.PORT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PORT.Name = "PORT";
            this.PORT.Size = new System.Drawing.Size(77, 13);
            this.PORT.TabIndex = 63;
            this.PORT.Text = "PORT : Select";
            // 
            // comboBoxSerialPort
            // 
            this.comboBoxSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSerialPort.FormattingEnabled = true;
            this.comboBoxSerialPort.Location = new System.Drawing.Point(131, 349);
            this.comboBoxSerialPort.Name = "comboBoxSerialPort";
            this.comboBoxSerialPort.Size = new System.Drawing.Size(87, 20);
            this.comboBoxSerialPort.TabIndex = 62;
            this.comboBoxSerialPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxSerialPort_SelectedIndexChanged);
            // 
            // checkBox_DrawIndex
            // 
            this.checkBox_DrawIndex.AutoSize = true;
            this.checkBox_DrawIndex.Location = new System.Drawing.Point(155, 500);
            this.checkBox_DrawIndex.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_DrawIndex.Name = "checkBox_DrawIndex";
            this.checkBox_DrawIndex.Size = new System.Drawing.Size(141, 16);
            this.checkBox_DrawIndex.TabIndex = 61;
            this.checkBox_DrawIndex.Text = "Draw Index Number ";
            this.checkBox_DrawIndex.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button9.Location = new System.Drawing.Point(25, 619);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(112, 32);
            this.button9.TabIndex = 60;
            this.button9.Text = "matching XMLs";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button8.Location = new System.Drawing.Point(131, 561);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(112, 32);
            this.button8.TabIndex = 59;
            this.button8.Text = "CovertSHP to XML";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 555);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 12);
            this.label7.TabIndex = 58;
            this.label7.Text = "label7";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(6, 515);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 12);
            this.label6.TabIndex = 57;
            this.label6.Text = "0";
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button10.ForeColor = System.Drawing.Color.Crimson;
            this.button10.Location = new System.Drawing.Point(25, 520);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(215, 32);
            this.button10.TabIndex = 56;
            this.button10.Text = "시경계 데이터 불러오기";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click_2);
            // 
            // button12
            // 
            this.button12.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button12.Location = new System.Drawing.Point(25, 495);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(125, 21);
            this.button12.TabIndex = 47;
            this.button12.Text = "DivideXML";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.AliceBlue;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(4, 451);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(41, 44);
            this.pictureBox2.TabIndex = 47;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(41, 44);
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(536, 6);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 21);
            this.label8.TabIndex = 41;
            this.label8.Text = "label8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(536, 31);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 21);
            this.label9.TabIndex = 42;
            this.label9.Text = "label9";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainMap
            // 
            this.MainMap.BackColor = System.Drawing.SystemColors.Window;
            this.MainMap.Bearing = 0F;
            this.MainMap.CanDragMap = true;
            this.MainMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMap.GrayScaleMode = false;
            this.MainMap.LevelsKeepInMemmory = 5;
            this.MainMap.Location = new System.Drawing.Point(0, 0);
            this.MainMap.MarkersEnabled = true;
            this.MainMap.MaxZoom = 17;
            this.MainMap.MinZoom = 2;
            this.MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MainMap.Name = "MainMap";
            this.MainMap.NegativeMode = false;
            this.MainMap.PolygonsEnabled = true;
            this.MainMap.RetryLoadTile = 0;
            this.MainMap.RoutesEnabled = true;
            this.MainMap.ShowTileGridLines = false;
            this.MainMap.Size = new System.Drawing.Size(868, 689);
            this.MainMap.TabIndex = 0;
            this.MainMap.Zoom = 0D;
            this.MainMap.Load += new System.EventHandler(this.MainMap_Load);
            // 
            // button11
            // 
            this.button11.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button11.Location = new System.Drawing.Point(25, 581);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(92, 32);
            this.button11.TabIndex = 64;
            this.button11.Text = "좌표 불러오기";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1138, 689);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.MainMap);
            this.Controls.Add(this.panelMenu);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GMap";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxLat;
		private System.Windows.Forms.TextBox textBoxLng;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboBoxMapType;
		private System.Windows.Forms.ComboBox comboBoxMode;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox checkBoxPlacemarkInfo;
		private System.Windows.Forms.TrackBar trackBar1;
		private BSE.Windows.Forms.Panel panelMenu;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button10;
        public Demo.WindowsForms.Map MainMap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox_DrawIndex;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label PORT;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button11;
	}
}