using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvertTacho
{
    public partial class FormInputTerm : Form
    {
        Form1 form1;

        public FormInputTerm(Form1 f)
        {
            InitializeComponent();

            form1 = f;
        }

        //public struct ViewSetting
        //{
        //    public bool isSimpleView;
        //    public UInt32 nNumOfOnePageView;

        //    public DateTime startDateTime;
        //    public DateTime endDateTime;
        //}

        //public ViewSetting stViewSetting;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int nErrorPosition = 0;

            try
            {
                if (radioButtonBasicView.Checked)
                {
                    nErrorPosition = 1;
                    form1.stViewSetting.isSimpleView = true;
                    form1.stViewSetting.nNumOfOnePageView = Convert.ToUInt32(textBox1.Text);
                    //nErrorPosition = 2;
                }
                else
                {
                    nErrorPosition = 3;
                    int yyS = dateTimePicker1.Value.Year;
                    int mmS = dateTimePicker1.Value.Month;
                    int ddS = dateTimePicker1.Value.Day;

                    nErrorPosition = 4;
                    int hS = Convert.ToInt32(textBox2.Text);
                    nErrorPosition = 5;
                    int mS = Convert.ToInt32(textBox3.Text);
                    nErrorPosition = 6;
                    int sS = Convert.ToInt32(textBox4.Text);
                    nErrorPosition = 7;
                    int msS = Convert.ToInt32(textBox5.Text);

                    nErrorPosition = 8;
                    DateTime sT = new DateTime(yyS, mmS, ddS, hS, mS, sS, msS);
                    form1.stViewSetting.startDateTime = sT;

                    //nErrorPosition = 9;

                    nErrorPosition = 10;
                    int yyE = dateTimePicker2.Value.Year;
                    int mmE = dateTimePicker2.Value.Month;
                    int ddE = dateTimePicker2.Value.Day;

                    nErrorPosition = 11;
                    int hE = Convert.ToInt32(textBox6.Text);
                    nErrorPosition = 12;
                    int mE = Convert.ToInt32(textBox7.Text);
                    nErrorPosition = 13;
                    int sE = Convert.ToInt32(textBox8.Text);
                    nErrorPosition = 14;
                    int msE = Convert.ToInt32(textBox9.Text);

                    nErrorPosition = 15;
                    DateTime eT = new DateTime(yyE, mmE, ddE, hE, mE, sE, msE);
                    form1.stViewSetting.endDateTime = eT;

                    nErrorPosition = 20;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                switch (nErrorPosition)
                {
                    case 1:
                        textBox1.Focus();
                        labelError1.Text = "1 ~ N 사이의 정수를 입력해 주세요";
                        break;
                    case 3:
                        labelErrorStart.Text = "";
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        labelErrorStart.Text = "1 ~ N 사이의 정수를 입력해 주세요";
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        labelErrorEnd.Text = "1 ~ N 사이의 정수를 입력해 주세요";
                        break;
                    default:
                        // All OK
                        break;
                }
            }
        }

        private void changeEnableControl(bool sel)
        {
            textBox1.Enabled = !sel;
            label1.Enabled = !sel;

            label2.Enabled = sel;
            label3.Enabled = sel;
            dateTimePicker1.Enabled = sel;
            dateTimePicker2.Enabled = sel;
            textBox2.Enabled = sel;
            textBox3.Enabled = sel;
            textBox4.Enabled = sel;
            textBox5.Enabled = sel;
            textBox6.Enabled = sel;
            textBox7.Enabled = sel;
            textBox8.Enabled = sel;
            textBox9.Enabled = sel;
            label4.Enabled = sel;
            label5.Enabled = sel;
            label6.Enabled = sel;
            label7.Enabled = sel;
            label8.Enabled = sel;
            label9.Enabled = sel;
            label10.Enabled = sel;
            label11.Enabled = sel;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                buttonOK_Click(sender, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox3.Focus();
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox7.Focus();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox4.Focus();
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox8.Focus();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox5.Focus();
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                textBox9.Focus();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                dateTimePicker2.Focus();
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
                buttonOK_Click(sender, e);
        }

        private void radioButtonBasicView_CheckedChanged(object sender, EventArgs e)
        {
            labelError1.Text = "";
            labelErrorStart.Text = "";
            labelErrorEnd.Text = "";

            if (radioButtonBasicView.Checked)
                changeEnableControl(false);
            else
                changeEnableControl(true);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textBox6.Focus();
        }
    }
}