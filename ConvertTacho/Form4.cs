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
    public partial class Form4 : Form
    {
        UInt32 cntData;
        public Boolean m_bStart;

        Form1 form1;

        public Form4(Form1 f)
        {
            InitializeComponent();

            m_bStart = true;

            form1 = f;
        }

        public void SetStruct(UInt32 num, bool is1sData)
        {
            cntData = num;

            zg4.GraphPane.CurveList.Clear();
            CreateChart(zg4, is1sData);
            zg4.RestoreScale(zg4.GraphPane);    // "Set Scale to Default"
            zg4.IsShowPointValues = true;       // "Show Point Values"
            SetSize();
        }

        // Call this method from the Form_Load method, passing your ZedGraphControl
        public void CreateChart(ZedGraphControl zgc, bool is1sData)
        {
            if (form1.transData == null) return;

            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Date / Total Drive Distance";
            if (is1sData)
                myPane.XAxis.Title.Text = "Time, 1s";
            else
                myPane.XAxis.Title.Text = "Time, 10ms";

            myPane.YAxis.Title.Text = "Date";
            myPane.Y2Axis.Title.Text = "Total";

            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();

            for (int i = 0; i < cntData; i++)
            {
                double x = new XDate(form1.transData[i].InforTimeYY, form1.transData[i].InforTimeMM, form1.transData[i].InforTimeDD,
                                    form1.transData[i].InforTimeHour, form1.transData[i].InforTimeMin, form1.transData[i].InforTimeSec, (form1.transData[i].InforTimeMSec * 10));
                //double x = i;
                double y1 = form1.transData[i].DayDriveDistance;      // DayDriveDistance
                double y2 = form1.transData[i].TotalDriveDistance;    // TotalDriveDistance
                list.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond symbols, and "Date" in the legend
            LineItem myCurve = myPane.AddCurve("Date", list, Color.Red, SymbolType.Diamond);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Size = 2.0F;

            // Generate a blue curve with circle symbols, and "Total" in the legend
            myCurve = myPane.AddCurve("Total", list2, Color.Blue, SymbolType.Circle);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Size = 2.0F;
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = -30;
            myPane.YAxis.Scale.Max = 30;

            // Enable the Y2 axis display
            myPane.Y2Axis.IsVisible = true;
            // Make the Y2 axis scale blue
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            // Display the Y2 axis grid lines
            myPane.Y2Axis.MajorGrid.IsVisible = true;
            // Align the Y2 axis labels so they are flush to the axis
            myPane.Y2Axis.Scale.Align = AlignP.Inside;

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;

            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
        }

        private void Form4_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg4.Location = new Point(0, 0);
            // Leave a small margin around the outside of the control
            zg4.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            if (m_bStart)
                e.Cancel = true;
            else
                e.Cancel = false;
        }
    }
}