namespace ConvertTacho
{
	partial class Form5
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.Number = new System.Windows.Forms.ColumnHeader();
			this.FileName = new System.Windows.Forms.ColumnHeader();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.AllowColumnReorder = true;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Number,
            this.FileName});
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(4, 5);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(948, 228);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// Number
			// 
			this.Number.Text = "번호";
			this.Number.Width = 40;
			// 
			// FileName
			// 
			this.FileName.Text = "파일이름";
			this.FileName.Width = 734;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(7, 245);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(94, 24);
			this.button1.TabIndex = 3;
			this.button1.Text = "등록";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// Form5
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(953, 287);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.listView1);
			this.Name = "Form5";
			this.Text = "SaveFile_List";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader Number;
		private System.Windows.Forms.ColumnHeader FileName;
		private System.Windows.Forms.Button button1;
	}
}