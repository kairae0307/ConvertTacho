namespace ConvertTacho
{
    partial class Form3
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
            this.zg3 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zg3
            // 
            this.zg3.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg3.Location = new System.Drawing.Point(0, 0);
            this.zg3.Name = "zg3";
            this.zg3.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg3.ScrollGrace = 0;
            this.zg3.ScrollMaxX = 0;
            this.zg3.ScrollMaxY = 0;
            this.zg3.ScrollMaxY2 = 0;
            this.zg3.ScrollMinX = 0;
            this.zg3.ScrollMinY = 0;
            this.zg3.ScrollMinY2 = 0;
            this.zg3.Size = new System.Drawing.Size(580, 400);
            this.zg3.TabIndex = 4;
            // 
            // Form3
            // 
            this.ClientSize = new System.Drawing.Size(580, 400);
            this.Controls.Add(this.zg3);
            this.Name = "Form3";
            this.Text = "속도/브레이크 신호/엔진 회전수";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Resize += new System.EventHandler(this.Form3_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zg3;
    }
}