using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PrintableListView;
using System.Data.SqlClient;

namespace ConvertTacho
{
    public partial class DataForm : Form
    {
        UInt32 cntData;
        public Boolean m_bStart;

        Form1 form1;
        public PrintableListView.PrintableListView m_list;
        // List<string> GPS_Sel_Lng = new List<string>();
        //  List<string> GPS_Sel_Lat = new List<string>();

        public DataForm(Form1 f)
        {
            InitializeComponent();
            m_bStart = true;
            m_list = new PrintableListView.PrintableListView();
            form1 = f;
        }

        public void SetStruct(UInt32 num, UInt32 page, UInt32 numOfOnePage, bool is1sData)
        {
            cntData = num;

            if (cntData == 0x7d1)
            {
                cntData -= 1;
            }
            //  if (listView1.Items.Count > 0)
            if (dragAndDropListView1.Items.Count > 0)
            {
                // listView1.Items.Clear();
                dragAndDropListView1.Items.Clear();
            }
            //     num = 10000;
            FillData(page, num, numOfOnePage);
        }
        public void FillList(ListView list, ListView table)
        {
            list.SuspendLayout();

            // Clear list
            list.Items.Clear();
            list.Columns.Clear();

            // Columns
            int nCol = 0;
            /*	foreach (ColumnHeader col in table.Columns)
                {
                    ColumnHeader ch = new ColumnHeader();
                    ch.Text = col.Text;
                    ch.TextAlign = HorizontalAlignment.Right;
                    switch (nCol)
                    {
                        case 0: ch.Width = 60; break;

                        case 1: ch.Width = 80; break;
                        case 3: ch.Width = 100; break;
                        case 4:
                            ch.TextAlign = HorizontalAlignment.Left;
                            ch.Width = 180;
                            break;
                        case 5: ch.Width = 180; break;
                        case 6: ch.Width = 90; break;
                        case 8:
                        case 9: ch.Width = 100; break;
                        case 10: ch.Width = 70; break;

                        case 2:
                        case 7:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15: ch.Width = 80; break;
                        default:
                            ch.Width = 100;
                            break;
                    }
                    list.Columns.Add(ch);
                    nCol++;
                }*/

            // Rows
            for (int n = 0; n < table.Items.Count; n++)
            {
                ListViewItem item = new ListViewItem();
                //item.Text = row[0].ToString();
                item.Text = table.Items[n].Text;

                for (int i = 1; i < table.Columns.Count; i++)
                {
                    item.SubItems.Add(table.Items[n].SubItems[i].Text);
                }
                list.Items.Add(item);
            }




            list.ResumeLayout();
        }
        void FillData(UInt32 pageNum, UInt32 cnt, UInt32 numOfOnePage)
        {
            string formCaption = "", carType = "";
            for (int i = 0; i < 10; i++)
                formCaption += form1.transDataHeader.ModelName[10 + i];

            this.Text = formCaption;

            Color LightPink = Color.FromArgb(255, 182, 193);

            // listView1.View = View.Details;
            //  listView1.GridLines = true;
            // listView1.FullRowSelect = true;


            dragAndDropListView1.View = View.Details;
            dragAndDropListView1.GridLines = true;
            dragAndDropListView1.FullRowSelect = true;



            label7.Text = ": " + formCaption;
            label8.Text = ": " + form1.transDataHeader.CarNum;

            switch (Convert.ToInt32(form1.transDataHeader.CarType))
            {
                case 11: carType = "시내버스"; break;
                case 12: carType = "농어촌버스"; break;
                case 13: carType = "마을버스"; break;
                case 14: carType = "시외버스"; break;
                case 15: carType = "고속버스"; break;
                case 16: carType = "전세버스"; break;
                case 17: carType = "특수여객자동차"; break;
                case 21: carType = "일반택시"; break;
                case 22: carType = "개인택시"; break;
                case 31: carType = "일반화물자동차"; break;
                case 32: carType = "개별화물자동차"; break;
                case 41: carType = "비사업용자동차"; break;
                default: carType = "알수 없는 차"; break;
            }
            label9.Text = ": " + carType;
            label10.Text = ": " + form1.transDataHeader.CarRegistNum;

            label11.Text = ": " + form1.transDataHeader.OperatorRegistNum;

            string dCode = "";
            for (int i = 0; i < form1.transDataHeader.DriverCode.Length/*18*/; i++)
            {
                if (form1.transDataHeader.DriverCode[i] != '#')
                    dCode += form1.transDataHeader.DriverCode[i];
                else
                    continue;
            }
            label12.Text = ": " + dCode;

            for (UInt32 i = 0; i < cntData; i++)
            {
                // 구분
                //((pageNum - 1) * numOfOnePage)
                ListViewItem a = new ListViewItem(Convert.ToString((long)(((pageNum - 1) * numOfOnePage) + i + 1)));

                string timeData = String.Format("{0:D4}.{1:D2}.{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D2}",
                                        form1.transData[i].InforTimeYY, form1.transData[i].InforTimeMM, form1.transData[i].InforTimeDD,
                                        form1.transData[i].InforTimeHour, form1.transData[i].InforTimeMin, form1.transData[i].InforTimeSec,
                                        form1.transData[i].InforTimeMSec);
                a.SubItems.Add(timeData);

                if (form1.transData[i].MachineStatus == "XX")
                {
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");

                    string carGpsX = String.Format("{0:D9}", (int)form1.transData[i].CarGpsX);
                    a.SubItems.Add(carGpsX);

                    string carGpsY = String.Format("{0:D9}", (int)form1.transData[i].CarGpsY);
                    a.SubItems.Add(carGpsY);

                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");
                    a.SubItems.Add(" ");

                    a.BackColor = System.Drawing.Color.DarkGray;
                    a.ForeColor = System.Drawing.Color.White;
                }
                else
                {

                    string ddDistance = String.Format("{0:D}", form1.transData[i].DayDriveDistance);
                    a.SubItems.Add(ddDistance);

                    string tdDistance = String.Format("{0:D}", form1.transData[i].TotalDriveDistance);/////////////////////////////
                    a.SubItems.Add(tdDistance);





                    string carSpeed = String.Format("{0:D}", form1.transData[i].CarSpeed);
                    a.SubItems.Add(carSpeed);

                    if (i != 0)
                    {
                        /*   if (form1.transData[i].CarSpeed > form1.transData[i - 1].CarSpeed)
                           {
                               int temp = form1.transData[i].CarSpeed - form1.transData[i - 1].CarSpeed;
                               if (temp >= 2)
                               {
                                   a.BackColor = LightPink;
                                   MessageBox.Show("Speed! Line" + (i + 1).ToString());

                               }

                           }
                           else
                           {
                               int temp = form1.transData[i - 1].CarSpeed - form1.transData[i].CarSpeed;
                               if (temp >= 2)
                               {
                                   a.BackColor = LightPink;
                                   MessageBox.Show("Speed! Line" + (i + 1).ToString());

                               }

                           }*/
                    }

                    string carRPM = String.Format("{0:D}", form1.transData[i].CarRPM);
                    a.SubItems.Add(carRPM);
                    if (i != 0)
                    {
                        if (form1.transData[i].CarRPM > form1.transData[i - 1].CarRPM)
                        {
                            int temp = form1.transData[i].CarRPM - form1.transData[i - 1].CarRPM;
                            if (temp >= 100)
                            {
                                a.BackColor = LightPink;
                                MessageBox.Show("RPM! Line" + (i + 1).ToString());
                            }

                        }
                        else
                        {
                            int temp = form1.transData[i - 1].CarRPM - form1.transData[i].CarRPM;
                            if (temp >= 100)
                            {
                                a.BackColor = LightPink;
                                MessageBox.Show("RPM! Line" + (i + 1).ToString());

                            }

                        }
                    }

                    string breakSignal = (form1.transData[i].CarBreak ? "1" : "0");
                    a.SubItems.Add(breakSignal);
                    //if (form1.transData[i].CarBreak)
                    //label12.Text = "어";

                    string carGpsX = String.Format("{0:F6}", form1.transData[i].CarGpsX);
                    a.SubItems.Add(carGpsX);
                    int temp1 = (int)form1.transData[i].CarGpsX;
                    if (temp1 != 126)
                    {
                        a.BackColor = LightPink;
                    }

                    string carGpsY = String.Format("{0:F6}", form1.transData[i].CarGpsY);
                    a.SubItems.Add(carGpsY);

                    string carDegree = String.Format("{0}", form1.transData[i].CarDegree);
                    a.SubItems.Add(carDegree);

                    string carAccelX = String.Format("{0}", form1.transData[i].CarAccelX);
                    a.SubItems.Add(carAccelX);

                    string carAccelY = String.Format("{0}", form1.transData[i].CarAccelY);
                    a.SubItems.Add(carAccelY);

                    a.SubItems.Add(form1.transData[i].MachineStatus);

                    if (i != 0)
                    {
                        if (form1.transData[i].TotalDriveDistance < form1.transData[i - 1].TotalDriveDistance)
                        {
                            //    MessageBox.Show("Test");
                            a.BackColor = LightPink;

                        }

                        if (form1.transData[i].DayDriveDistance < form1.transData[i - 1].DayDriveDistance)
                        {
                            //    MessageBox.Show("Test");
                            a.BackColor = LightPink;

                        }

                    }


                    //a.BackColor = System.Drawing.Color.White;
                    //a.ForeColor = System.Drawing.Color.Black;
                }

                //   listView1.Items.Add(a);
                dragAndDropListView1.Items.Add(a);
                //	FillList(this.m_list, listView1);
            }
        }

        private void DataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            if (m_bStart)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listView1.EnsureVisible(600);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_list.Title = "데이터 출력";
            //m_list.FitToPage = m_cbFitToPage.Checked;
            m_list.FitToPage = true;
            m_list.Print();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_list.Title = "데이터 출력";
            //m_list.FitToPage = m_cbFitToPage.Checked;
            m_list.FitToPage = true;
            m_list.PrintPreview();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //form1.gpsx = listView1.FocusedItem.SubItems[7].Text;
            //   form1.gpsy = listView1.FocusedItem.SubItems[8].Text;


            //    MessageBox.Show("test");

        }
        private void gPS좌표복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.GPS_Sel_Lat.Clear();
            form1.GPS_Sel_Lng.Clear();
            form1.GPS_Sel_ID.Clear();

            form1.gpsID = true;

            for (int i = 0; i < dragAndDropListView1.SelectedItems.Count; i++)
            {
                form1.GPS_Sel_Lat.Add(dragAndDropListView1.SelectedItems[i].SubItems[8].Text);  // lat
                form1.GPS_Sel_Lng.Add(dragAndDropListView1.SelectedItems[i].SubItems[7].Text);  // lng

                form1.GPS_Sel_ID.Add(Int32.Parse(dragAndDropListView1.SelectedItems[i].SubItems[0].Text));  //id

            }
            MessageBox.Show("GPS 좌표복사 완료");
        }

        private void 엑셀출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cnt = dragAndDropListView1.SelectedItems.Count - 1;
            form1.starttext = dragAndDropListView1.SelectedItems[0].SubItems[0].Text;
            form1.endtext = dragAndDropListView1.SelectedItems[cnt].SubItems[0].Text;

            form1.Excel_print();
        }




    }
}