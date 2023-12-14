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
    public partial class Form3 : Form
    {
        UInt32 cntData;
        public Boolean m_bStart;

        Form1 form1;

        public Form3(Form1 f)
        {
            InitializeComponent();

            m_bStart = true;

            form1 = f;
        }

        public void SetStruct(UInt32 num, bool is1sData)
        {
            cntData = num;

            zg3.GraphPane.CurveList.Clear();
            if(zg3.GraphPane.YAxisList.Count > 1)
                zg3.GraphPane.YAxisList.RemoveAt(1);

            CreateChart(zg3, is1sData);
            zg3.RestoreScale(zg3.GraphPane);    // "Set Scale to Default"
            zg3.IsShowPointValues = true;       // "Show Point Values"
            SetSize();
        }

        // Call this method from the Form_Load method, passing your ZedGraphControl
        public void CreateChart(ZedGraphControl zgc, bool is1sData)
        {
            if (form1.transData == null) return;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Speed, Break-Signal & RPM\n"+form1.PlateNoTemp + " " + form1.DateTemp;
            if (is1sData)
                myPane.XAxis.Title.Text = "Time, 1s";
            else
                myPane.XAxis.Title.Text = "Time, 10ms";
            myPane.YAxis.Title.Text = "Speed Km/h";
            myPane.Y2Axis.Title.Text = "RPM";

            // Make up some data points based on the Sine function
            PointPairList rList = new PointPairList();
            PointPairList sList = new PointPairList();
            PointPairList bList = new PointPairList();

            // Fabricate some data values
//            double first = new XDate(transData[0].InforTimeYY, transData[0].InforTimeMM, transData[0].InforTimeDD,
//                                transData[0].InforTimeHour, transData[0].InforTimeMin, transData[0].InforTimeSec, transData[0].InforTimeMSec);
//            bList.Add(first, 5);
            for (int i = 0; i < cntData; i++)
            {
                double time = new XDate(form1.transData[i].InforTimeYY, form1.transData[i].InforTimeMM, form1.transData[i].InforTimeDD,
                                    form1.transData[i].InforTimeHour, form1.transData[i].InforTimeMin, form1.transData[i].InforTimeSec, (form1.transData[i].InforTimeMSec * 10));
                //double time = i;
                double rpm = form1.transData[i].CarRPM;                           // RPM
                double speed = form1.transData[i].CarSpeed;                       // Speed
                double break_sig = 0;

                if (i == 0)
                {
                    break_sig = 5;                                          // Break-signal first data
                    bList.Add(time, break_sig);
                }
                break_sig = (double)(form1.transData[i].CarBreak ? 1 : 0);        // Break-signal
                if (i == (cntData - 1))
                {
                    break_sig = 5;                                          // Break-signal last data
                    bList.Add(time, break_sig);
                }

                rList.Add(time, rpm);
                sList.Add(time, speed);
                bList.Add(time, break_sig);
            }

            // Generate a red curve with diamond symbols, and "Speed" in the legend
            LineItem myCurve = myPane.AddCurve("Speed", sList, Color.Red, SymbolType.Diamond);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Size = 1.0F;

            // Generate a blue curve with circle symbols, and "RPM" in the legend
            myCurve = myPane.AddCurve("RPM", rList, Color.Blue, SymbolType.Circle);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Size = 1.0F;
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;

            // Generate a green curve with square symbols, and "Break_signal" in the legend
            myCurve = myPane.AddCurve("Break_signal", bList, Color.Green, SymbolType.Square);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Size = 1.0F;
            // Associate this curve with the second Y axis
            myCurve.YAxisIndex = 1;
            // Associate this curve with the Y2 axis
            //myCurve.IsY2Axis = true;

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
            myPane.YAxis.Scale.Max = 300;

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
            //myPane.Y2Axis.Scale.Min = 1.5;
            myPane.Y2Axis.Scale.Max = 9999;

            // Create a second Y Axis, green
            YAxis yAxis3 = new YAxis("Break-signal");
            myPane.YAxisList.Add(yAxis3);
            yAxis3.Scale.FontSpec.FontColor = Color.Green;
            yAxis3.Title.FontSpec.FontColor = Color.Green;
            yAxis3.Color = Color.Green;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            yAxis3.MajorTic.IsInside = false;
            yAxis3.MinorTic.IsInside = false;
            yAxis3.MajorTic.IsOpposite = false;
            yAxis3.MinorTic.IsOpposite = false;
            // Align the Y2 axis labels so they are flush to the axis
            yAxis3.Scale.Align = AlignP.Inside;

            yAxis3.Scale.Min = 0;
            yAxis3.Scale.Max = 1000;

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;

            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

            zgc.AxisChange();

        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg3.Location = new Point(0, 0);
            // Leave a small margin around the outside of the control
            zg3.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
            //zg2.Size = new Size(580, 400);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            if (m_bStart)
                e.Cancel = true;
            else
                e.Cancel = false;
        }
    }
}