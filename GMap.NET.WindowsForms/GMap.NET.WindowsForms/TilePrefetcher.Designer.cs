namespace GMap.NET
{
   partial class TilePrefetcher
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
         if(disposing && (components != null))
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
		  this.progressBar1 = new System.Windows.Forms.ProgressBar();
		  this.label1 = new System.Windows.Forms.Label();
		  this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		  this.tableLayoutPanel1.SuspendLayout();
		  this.SuspendLayout();
		  // 
		  // progressBar1
		  // 
		  this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.progressBar1.Location = new System.Drawing.Point(4, 15);
		  this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		  this.progressBar1.Name = "progressBar1";
		  this.progressBar1.Size = new System.Drawing.Size(464, 26);
		  this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		  this.progressBar1.TabIndex = 0;
		  // 
		  // label1
		  // 
		  this.label1.AutoSize = true;
		  this.label1.Location = new System.Drawing.Point(4, 0);
		  this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		  this.label1.Name = "label1";
		  this.label1.Size = new System.Drawing.Size(38, 12);
		  this.label1.TabIndex = 1;
		  this.label1.Text = "label1";
		  // 
		  // tableLayoutPanel1
		  // 
		  this.tableLayoutPanel1.ColumnCount = 1;
		  this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
		  this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
		  this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 1);
		  this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
		  this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		  this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		  this.tableLayoutPanel1.RowCount = 2;
		  this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
		  this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
		  this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 44);
		  this.tableLayoutPanel1.TabIndex = 2;
		  // 
		  // TilePrefetcher
		  // 
		  this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
		  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		  this.BackColor = System.Drawing.Color.AliceBlue;
		  this.ClientSize = new System.Drawing.Size(480, 52);
		  this.ControlBox = false;
		  this.Controls.Add(this.tableLayoutPanel1);
		  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		  this.KeyPreview = true;
		  this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		  this.MaximizeBox = false;
		  this.MinimizeBox = false;
		  this.Name = "TilePrefetcher";
		  this.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		  this.ShowIcon = false;
		  this.ShowInTaskbar = false;
		  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		  this.Text = "GMap.NET - esc to cancel fetching";
		  this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Prefetch_FormClosed);
		  this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Prefetch_PreviewKeyDown);
		  this.tableLayoutPanel1.ResumeLayout(false);
		  this.tableLayoutPanel1.PerformLayout();
		  this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ProgressBar progressBar1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
   }
}