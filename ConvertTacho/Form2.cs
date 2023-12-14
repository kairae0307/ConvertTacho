using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace ConvertTacho
{
    public partial class Form2 : Form
    {
        UInt32 cntData;
        public Boolean m_bStart;

        Form1 form1;

        public Form2(Form1 f)
        {
            InitializeComponent();

            m_bStart = true;

            form1 = f;
        }

        public void SetStruct(UInt32 num, bool is1sData)
        {
            cntData = num;

            zg2.GraphPane.CurveList.Clear();
            CreateChart(zg2, is1sData);
            zg2.RestoreScale(zg2.GraphPane);    // "Set Scale to Default"
            zg2.IsShowPointValues = true;       // "Show Point Values"
            SetSize();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //CreateChart(zg2);
            //SetSize();
        }

        // Call this method from the Form_Load method, passing your ZedGraphControl
        public void CreateChart(ZedGraphControl zgc, bool is1sData)
        {
            if (form1.transData == null) return;

            GraphPane myPane = zgc.GraphPane;

            // Set up the title and axis labels
            myPane.Title.Text = "Accelerometer";
            if (is1sData)
                myPane.XAxis.Title.Text = "Time, 1s";
            else
                myPane.XAxis.Title.Text = "Time, 10ms";
            myPane.YAxis.Title.Text = "Accelerometer";

            // Make up some data arrays based on the Sine function
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();

            //for (int i = 0; i < 100; i++)
            for (int i = 0; i < cntData; i++)
            {
                double x = new XDate(form1.transData[i].InforTimeYY, form1.transData[i].InforTimeMM, form1.transData[i].InforTimeDD,
                                    form1.transData[i].InforTimeHour, form1.transData[i].InforTimeMin, form1.transData[i].InforTimeSec, (form1.transData[i].InforTimeMSec * 10));
                //double x = (double)i;
                double y1 = form1.transData[i].CarAccelX; // accelX
                double y2 = form1.transData[i].CarAccelY; // accelY
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a Red curve with circle
            LineItem myCurve1 = myPane.AddCurve("X-Accel", list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            LineItem myCurve2 = myPane.AddCurve("Y-Accel", list2, Color.Blue, SymbolType.Circle);

            // Change the color of the title
            myPane.Title.FontSpec.FontColor = Color.Green;

            // Add gridlines to the plot, and make them gray
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.Color = Color.LightGray;

            // Move the legend location
            myPane.Legend.Position = ZedGraph.LegendPos.Bottom;

            // Make both curves thicker
            myCurve1.Line.Width = 2.0F;
            myCurve2.Line.Width = 2.0F;

            // Fill the area under the curves
            //myCurve1.Line.Fill = new Fill(Color.White, Color.Red, 45F);
            //myCurve2.Line.Fill = new Fill(Color.White, Color.Blue, 45F);

            // Increase the symbol sizes, and fill them with solid white
            myCurve1.Symbol.Size = 2.0F;
            myCurve2.Symbol.Size = 2.0F;
            myCurve1.Symbol.Fill = new Fill(Color.White);
            myCurve2.Symbol.Fill = new Fill(Color.White);

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;

            // Add a background gradient fill to the axis frame
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 210), -45F);

            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg2.Location = new Point(0, 0);
            // Leave a small margin around the outside of the control
            zg2.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            if (m_bStart)
                e.Cancel = true;
            else
                e.Cancel = false;
        }
    }
}