using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvertTacho
{
    public partial class FormExecel : Form
    {
        Form1 form1;
        public FormExecel(Form1 f)
        {
            form1 = f;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int start =Int32.Parse(StarttextBox.Text);
            int end= Int32.Parse(EndtextBox.Text);

            if (StarttextBox.Text == "" || EndtextBox.Text == "")
            {
                MessageBox.Show("������ �Է��� ���ּ���");
            }
            else if (start > end)
            {
                MessageBox.Show("�߸� �Է� �Ͽ����ϴ�. �ٽ� �Է��Ͽ��ּ���");
            }
            else
            {
                form1.starttext = StarttextBox.Text;
                form1.endtext = EndtextBox.Text;
                form1.Excel_print();
                this.Close();
            }
        }
    }
}