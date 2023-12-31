using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Demo.WindowsForms.CustomMarkers;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace ConvertTacho
{
	public partial class MainForm : Form
	{
		Form1 form1;

        public int GpsCnt = 0;
		// marker
		GMapMarker currentMarker;
		GMapMarker center;
		Placemark p = null;
		public bool  isGPSsend = false;
	
		// polygons
		GMapPolygon polygon;
		GMapPolygon polygon1;
		GMapPolygon polygon2;
	

		// layers
		GMapOverlay top;
		internal GMapOverlay objects;
		internal GMapOverlay routes;

		internal GMapOverlay polygons;


		public PrintableListView.PrintableListView m_list;
		// etc
		readonly Random rnd = new Random();
		readonly DescendingComparer ComparerIpStatus = new DescendingComparer();
		GMapMarkerRect CurentRectMarker = null;
		string mobileGpsLog = string.Empty;
		bool isMouseDown = false;
		PointLatLng start;
		PointLatLng end;
		GMapMarker currentTransport;

		bool Gps_Second_hours = false;
		bool Gps_Third_hours = false;
		bool Gps_First_hours = false;
		bool Gps_1Sec = false;

        public byte[] TrackingData = new byte[64];
        public double posY = 0;
        public double posX = 0; 
        long[,] ptArraySrc;

        private int numOfPoints; //ptArraySrc 에 선언한 포인트 개수

        //SerialPort SP = new SerialPort();
        public byte[] g_sendMsg = new byte[1024];
        public byte[] g_sendMsg_1 = new byte[1024];
        public byte g_chksum;
        int sendMsgCnt = 0;


        public struct myPoint
        {
            public long x;
            public long y;

        }

        myPoint[] ptArray = new myPoint[100000];
    
        public MainForm(Form1 f)
		{
			InitializeComponent();
			form1 = f;
			CheckForIllegalCrossThreadCalls = false;    // Thread Exception ignore !!!!

			m_list = new PrintableListView.PrintableListView();  // print

			try
			{
				System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.google.com");
			}
			catch
			{
				MainMap.Manager.Mode = AccessMode.CacheOnly;
				MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET - Demo.WindowsForms", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			 // config map 

            this.Height = Screen.PrimaryScreen.Bounds.Height-200;
			MainMap.MapType = MapType.GoogleMap;
			MainMap.MinZoom = 1;
			MainMap.MaxZoom = 17;
			MainMap.Zoom = 14;
			
			// map events
			MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
			//	MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
			//	MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
			MainMap.OnMarkerClick += new MarkerClick(MainMap_OnMarkerClick);
			MainMap.OnMapZoomChanged += new MapZoomChanged(MainMap_OnMapZoomChanged);
			MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
			MainMap.MouseMove += new MouseEventHandler(MainMap_MouseMove);
			MainMap.MouseDown += new MouseEventHandler(MainMap_MouseDown);
			MainMap.MouseUp += new MouseEventHandler(MainMap_MouseUp);
			MainMap.OnMarkerEnter += new MarkerEnter(MainMap_OnMarkerEnter);
			MainMap.OnMarkerLeave += new MarkerLeave(MainMap_OnMarkerLeave);
			MainMap.KeyDown += new KeyEventHandler(MainMap_KeyDown);

			routes = new GMapOverlay(MainMap, "routes");
			MainMap.Overlays.Add(routes);
///////////////////////////////////////////////////////////////////////
			polygons = new GMapOverlay(MainMap, "polygons");
			MainMap.Overlays.Add(polygons);

		
///////////////////////////////////////////////////////////////////////
			objects = new GMapOverlay(MainMap, "objects");
			MainMap.Overlays.Add(objects);

			top = new GMapOverlay(MainMap, "top");
			MainMap.Overlays.Add(top);

			routes.Routes.CollectionChanged += new GMap.NET.ObjectModel.NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
			objects.Markers.CollectionChanged += new GMap.NET.ObjectModel.NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
			  
				// get map type
				comboBoxMapType.DataSource = Enum.GetValues(typeof(MapType));
				comboBoxMapType.SelectedItem = MainMap.MapType;

				// acccess mode
				comboBoxMode.DataSource = Enum.GetValues(typeof(AccessMode));			
				comboBoxMode.SelectedItem = GMaps.Instance.Mode;
				
				// get position
			//	textBoxLat.Text = MainMap.Position.Lat.ToString(CultureInfo.InvariantCulture);
			//	textBoxLng.Text = MainMap.Position.Lng.ToString(CultureInfo.InvariantCulture);

				// set current marker
				currentMarker = new GMapMarkerGoogleRed(MainMap.Position);
				currentMarker.IsHitTestVisible = true;
				center = new GMapMarkerGoogleRed(MainMap.Position);

				try
				{

					if (form1.Gpx_data == false)
					{

						MainMap.Position = new PointLatLng(37.4761012096049, 126.882938146591); // default position
					//	p = GMaps.Instance.GetPlacemarkFromGeocoder(MainMap.Position);
					//	textBox1.Text = p.Address;


						GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(MainMap.Position);
						GMapMarkerCross start_cross_marker = new GMapMarkerCross(MainMap.Position);

						start_green_marker.ToolTipMode = MarkerTooltipMode.Always;
						start_green_marker.ToolTipText = "Welcome to JOONG ANG SAN JUN CO.,LTD.!";

						objects.Markers.Add(start_green_marker);
					//	objects.Markers.Add(start_cross_marker);

					}
					else
					{
						MainMap.Position = new PointLatLng(37.4761012096049, 126.882938146591); // default position
						p = GMaps.Instance.GetPlacemarkFromGeocoder(MainMap.Position);
					//	textBox1.Text = p.Address;


						GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(MainMap.Position);
						GMapMarkerCross start_cross_marker = new GMapMarkerCross(MainMap.Position);

						start_green_marker.ToolTipMode = MarkerTooltipMode.Always;
						start_green_marker.ToolTipText = "Welcome to JOONG ANG SAN JUN CO.,LTD.!";

						objects.Markers.Add(start_green_marker);
				
					}
				}
				catch(Exception e)
				{
					MessageBox.Show(e.Message);
				}

				textBoxLat.Text = MainMap.Position.Lat.ToString(CultureInfo.InvariantCulture);
				textBoxLng.Text = MainMap.Position.Lng.ToString(CultureInfo.InvariantCulture);


                foreach (string comport in SerialPort.GetPortNames())
                {
                    comboBoxSerialPort.Items.Add(comport);
                }

                //시리얼 포트 세팅
                serialPort1.BaudRate = (int)9600;
                serialPort1.DataBits = (int)8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
             //   serialPort1.ReadTimeout = (int)2000;
             //   serialPort1.WriteTimeout = (int)2000;
             //   serialPort1.ReadBufferSize = 0x10000;

		}
		////////
		void RegeneratePolygon()
		{

			try
			{
				List<PointLatLng> polygonPoints = new List<PointLatLng>();


				foreach (GMapMarker m in objects.Markers)
				{
					if (m is GMapMarkerRect)
					{
						m.Tag = polygonPoints.Count;
						polygonPoints.Add(m.Position);
					}
				}


				////////////
				if (Gps_Second_hours == true)
				{
					if (polygon1 == null)
					{
						polygon1 = new GMapPolygon(polygonPoints, "polygon1");
						polygons.Polygons.Add(polygon1);
					}
					else
					{
					/*	polygon1.Points.Clear();
						polygon1.Points.AddRange(polygonPoints);

						if (polygons.Polygons.Count == 0)
						{
							polygons.Polygons.Add(polygon1);
						}
						else
						{
							MainMap.UpdatePolygonLocalPosition(polygon1);
						}*/
                        polygon1 = new GMapPolygon(polygonPoints, "polygon1");
                        polygons.Polygons.Add(polygon1);
					}
				}
				///////
				if (Gps_Third_hours == true)
				{

					if (polygon2 == null)
					{
						polygon2 = new GMapPolygon(polygonPoints, "polygon2");
						polygons.Polygons.Add(polygon2);
					}
					else
					{
						/*polygon2.Points.Clear();
						polygon2.Points.AddRange(polygonPoints);

						if (polygons.Polygons.Count == 0)
						{
							polygons.Polygons.Add(polygon2);
						}
						else
						{
							MainMap.UpdatePolygonLocalPosition(polygon2);
						}*/
                        	polygon2 = new GMapPolygon(polygonPoints, "polygon2");
						polygons.Polygons.Add(polygon2);
					}
				}
				/////
				else if (Gps_First_hours == true )
				{


					if (polygon == null)
					{
						polygon = new GMapPolygon(polygonPoints, "polygon");
						polygons.Polygons.Add(polygon);
					}
					else
					{
						polygon.Points.Clear();
						polygon.Points.AddRange(polygonPoints);

						if (polygons.Polygons.Count == 0)
						{
							polygons.Polygons.Add(polygon);
						}
						else
						{
							MainMap.UpdatePolygonLocalPosition(polygon);
						}
					}
				}
				//////////////////////////////
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.Message);
			}
			
		}
		///
		public void DoWork()     // Thread Start;
		{

            int cnt;
			int i = 0;
			uint gps_data_ea= 0;
			bool gps_second_chk = false; 

			if (Gps_1Sec == false)
			{

				if (form1.Data_Length < 3600)
				{
					gps_data_ea = form1.Data_Length;  //1시간 이하
					Gps_Second_hours = false;
					Gps_Third_hours = false;
				}
				else
				{
					gps_data_ea = 3600;
				}
				if (form1.Data_Length > 7200 && form1.Data_Length < 10000)
				{
					gps_second_chk = true;
				}

				if (Gps_Second_hours == true)
				{

					i = 3600;
					gps_data_ea = 7200;
					cnt = 60;
				}
				else if (Gps_Third_hours == true)
				{

					i = 7200;
					cnt = 60;
					if (gps_second_chk == false)
					{


						gps_data_ea = 10000;
					}
					else
					{
						gps_data_ea = form1.Data_Length;
					}
					
				}

			}
					
			try
			{
				
				for (int k =i ; k < gps_data_ea; k += 60)  
				{

				//	Cursor.Position = new Point(10, 10);

				//	Cursor.Clip = new Rectangle(10, 10, 10, 10);					

					Thread.Sleep(300);
					
					MainMap.Position = new PointLatLng(form1.transData[k].CarGpsY, form1.transData[k].CarGpsX);
						
					GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(MainMap.Position);
					GMapMarkerCross start_cross_marker = new GMapMarkerCross(MainMap.Position);
					GMapMarkerCross_Green start_cross_marker_Green = new GMapMarkerCross_Green(MainMap.Position);
					GMapMarkerCross_Blue start_cross_marker_Blue = new GMapMarkerCross_Blue(MainMap.Position);
		

					GMapMarkerRect mBorders = new GMapMarkerRect(MainMap.Position);
				
									
					Placemark p1 = null;

					if (checkBoxPlacemarkInfo.Checked)
					{
						p1 = GMaps.Instance.GetPlacemarkFromGeocoder(MainMap.Position);
					}

					if (p1 != null)
					{

						if (k == i)
						{


							string timeData = String.Format("[START]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
								   form1.transData[k].InforTimeYY, form1.transData[k].InforTimeMM, form1.transData[k].InforTimeDD,
								   form1.transData[k].InforTimeHour, form1.transData[k].InforTimeMin, form1.transData[k].InforTimeSec,
								   form1.transData[k].InforTimeMSec);

							mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
						}
						else if(k==gps_data_ea-60)
						{
							string timeData = String.Format("[END]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
								   form1.transData[k].InforTimeYY, form1.transData[k].InforTimeMM, form1.transData[k].InforTimeDD,
								   form1.transData[k].InforTimeHour, form1.transData[k].InforTimeMin, form1.transData[k].InforTimeSec,
								   form1.transData[k].InforTimeMSec);

							mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
					
						}

					}
					else
					{
						if (k == i)
						{


							string timeData = String.Format("[START]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
								   form1.transData[k].InforTimeYY, form1.transData[k].InforTimeMM, form1.transData[k].InforTimeDD,
								   form1.transData[k].InforTimeHour, form1.transData[k].InforTimeMin, form1.transData[k].InforTimeSec,
								   form1.transData[k].InforTimeMSec);

							mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
						}
						else if (k == gps_data_ea-60)
						{
							string timeData = String.Format("[END]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
								   form1.transData[k].InforTimeYY, form1.transData[k].InforTimeMM, form1.transData[k].InforTimeDD,
								   form1.transData[k].InforTimeHour, form1.transData[k].InforTimeMin, form1.transData[k].InforTimeSec,
								   form1.transData[k].InforTimeMSec);

							mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁

						}
					}

					//	objects.Markers.Add(start_green_marker);
					if (Gps_Second_hours == true)
					{
						Gps_Third_hours = false;
						Gps_First_hours = false;
						if (MainMap.Position.Lat > 32 && MainMap.Position.Lng > 122)
						objects.Markers.Add(start_cross_marker_Green);
						

					}
					else if(Gps_Third_hours == true)
					{
						Gps_Second_hours = false;
						Gps_First_hours = false;
						if (MainMap.Position.Lat > 32 && MainMap.Position.Lng > 122)
						objects.Markers.Add(start_cross_marker_Blue);
					
					}
					else if (Gps_First_hours == true )
					{

						Gps_Second_hours = false;
						Gps_Third_hours = false;
						if (MainMap.Position.Lat > 32 && MainMap.Position.Lng > 122)
						objects.Markers.Add(start_cross_marker);
					}
				//////////////////
						{
								mBorders.InnerMarker = start_green_marker;
								if (polygon != null)
								{
									mBorders.Tag = polygon.Points.Count;
								}

								else if (polygon1 != null)
								{
									mBorders.Tag = polygon1.Points.Count;

								}
								else if (polygon2 != null)
								{
									mBorders.Tag = polygon2.Points.Count;

								}
										
								mBorders.ToolTipMode = MarkerTooltipMode.Always;
						}
				/////////////////
				     if(MainMap.Position.Lat > 32  && MainMap.Position.Lng >122)
					objects.Markers.Add(mBorders);						

					RegeneratePolygon();

				}
			}
			catch (System.Exception e)
			{
				{
					MessageBox.Show(e.Message);
				}
			}
			MessageBox.Show("Data loading is complete!!");

			Gps_First_hours = false;
			Gps_Second_hours = false;
			Gps_Third_hours = false;
			Gps_1Sec = false;
		
			
		}
		public void RequestStop()
		{
			

		}

		// current point changed
		void MainMap_OnCurrentPositionChanged(PointLatLng point)
		{
			center.Position = point;
		}

		// MapZoomChanged
		void MainMap_OnMapZoomChanged()
		{
			trackBar1.Value = (int)(MainMap.Zoom);
			
			center.Position = MainMap.Position;
		}

		void MainMap_OnMapTypeChanged(MapType type)
		{
			comboBoxMapType.SelectedItem = MainMap.MapType;

			trackBar1.Minimum = MainMap.MinZoom;
			trackBar1.Maximum = MainMap.MaxZoom;

			if (routes.Routes.Count > 0)
			{
				MainMap.ZoomAndCenterRoutes(null);
			}

		/*	if (radioButtonTransport.Checked)
			{
				MainMap.ZoomAndCenterMarkers("objects");
			}*/
		}
		// MapZoomChanged
	/*	void MainMap_OnMapZoomChanged()
		{
			trackBar1.Value = (int)(MainMap.Zoom);
		//	textBoxZoomCurrent.Text = MainMap.Zoom.ToString();
			center.Position = MainMap.Position;
		}*/

		private void button1_Click(object sender, EventArgs e)
		{
			double lat = double.Parse(textBoxLat.Text, CultureInfo.InvariantCulture);
			double lng = double.Parse(textBoxLng.Text, CultureInfo.InvariantCulture);

			MainMap.Position = new PointLatLng(lat, lng);
			currentMarker.Position = currentMarker.Position;

            p = GMaps.Instance.GetPlacemarkFromGeocoder(MainMap.Position);

            if (p != null)
            {

                textBox1.Text = p.Address;
            }
         

            GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(MainMap.Position);
            GMapMarkerCross start_cross_marker = new GMapMarkerCross(MainMap.Position);

            start_green_marker.ToolTipMode = MarkerTooltipMode.Always;
            if (p != null)
            {
                start_green_marker.ToolTipText = p.Address;
            }

            objects.Markers.Add(start_green_marker);
          
		}
		// change map type
		private void comboBoxMapType_DropDownClosed(object sender, EventArgs e)
		{
			MainMap.MapType = (MapType)comboBoxMapType.SelectedValue;
		}
		// change mdoe
		private void comboBoxMode_DropDownClosed(object sender, EventArgs e)
		{
			GMaps.Instance.Mode = (AccessMode)comboBoxMode.SelectedValue;
			MainMap.ReloadMap();
		}
		//zoom
		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			MainMap.Zoom = (trackBar1.Value);
		}
		void MainMap_MouseMove(object sender, MouseEventArgs e)
		{
		}

		void MainMap_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				isMouseDown = false;
			}
		}
		void MainMap_MouseDown(object sender, MouseEventArgs e)
		{
			 p = null;
			
				 if (e.Button == MouseButtons.Left)
				 {
					 isMouseDown = true;
                     

					 if (currentMarker.IsVisible)
					 {
						 currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);

						 textBoxLat.Text = currentMarker.Position.Lat.ToString(CultureInfo.InvariantCulture);
						 textBoxLng.Text = currentMarker.Position.Lng.ToString(CultureInfo.InvariantCulture);

						 if (checkBoxPlacemarkInfo.Checked)
						 {
							 p = GMaps.Instance.GetPlacemarkFromGeocoder(currentMarker.Position);
                             
						 }

						 if (p != null)
							 textBox1.Text = p.Address;
						 else
							 textBox1.Text = "";
					 }


                     myPoint curLocation;

                  
                     int ret;

                     curLocation.y = (int)(currentMarker.Position.Lat*C_PT_PRECISION);  
                     curLocation.x = (int)(currentMarker.Position.Lng*C_PT_PRECISION);
                 

                     label8.Text = "";
                     ret = PointInPolygon(curLocation, ptArray, numOfPoints);
                     if (ret == 1)
                     {
                         label8.Text = "Inside";
                     }
                     else
                     {
                         label8.Text = "OutSide";
                     }

                   /*  ret = wn_PnPoly(curLocation, ptArray, numOfPoints);
                     if (ret != 0)
                     {
                         label9.Text = String.Format("{0:D}, {1:D}", curLocation.x, curLocation.y)+ "Inside";
                     }
                     else
                     {
                         label9.Text = String.Format("{0:D}, {1:D}", curLocation.x, curLocation.y) + "OutSide";
                     }
                     */

                     //현재 좌표를 GPS데이터로 만들어서 보낸다.
                     if (serialPort1.IsOpen)
                     {

                         isGPSsend = true;
                         timer1.Start();
                         
                         string strGPGGA = "$GPGGA,";//"$GPGGA,000221.00,3728.5207,N,12707.7641,E,1,7,1.000,10.000,M,0,M,0,*7D";
                         string strGPRMC = "$GPRMC,"; //$GPRMC,114455.532,A,3735.0079,N,12701.6446,E,0.000000,121.61,110706,,*0A https://bumnux.tistory.com/415
                         DateTime dt = System.DateTime.Now.ToUniversalTime();
                         string strTime = dt.ToString("HHmm");//https://ds4ogl.blog.me/80116732812
                         string strDate = dt.ToString("ddMMyy");

                        // myPoint curLocation_Convert;
                         long D_ = (long)(currentMarker.Position.Lat / 1);
                         long Y_ = (long)((currentMarker.Position.Lat - D_) * 10000000);
                         Y_ = (Y_ / 100) * 60;
                         posY = (D_ * 10000000) + Y_;
                         posY = posY / 10000000;


                         D_ = (long)(currentMarker.Position.Lng / 1);
                         Y_ = (long)((currentMarker.Position.Lng - D_) * 10000000);
                         Y_ = (Y_ / 100) * 60;
                         posX = (D_ * 10000000) + Y_;
                         posX = posX / 10000000;

                      //  curLocation_Convert  
                          string strLat = posY.ToString(CultureInfo.InvariantCulture);
                          string strLong = posX.ToString(CultureInfo.InvariantCulture);
                          strLong += "0";
                        //string strLat = currentMarker.Position.Lat.ToString(CultureInfo.InvariantCulture);   //원래 코드 
                       // string strLong = currentMarker.Position.Lng.ToString(CultureInfo.InvariantCulture);


                         string[] strTemp = new string[10];

                         if (strLat.Length > 8)
                         {
                             strLat = strLat.Substring(0, 9);
                         }

                         if (strLong.Length > 9)
                         {
                             strLong = strLong.Substring(0, 10);
                         }
                         
                         strTemp = strLat.Split('.'); // . 의 위치를 GPS 데이터 포맷에 맞춘다. 
                         strLat = strTemp[0] + strTemp[1];
                         strLat = strLat.Insert(4, ".");

                         strTemp = strLong.Split('.');
                         strLong = strTemp[0] + strTemp[1];
                         strLong = strLong.Insert(5, ".");

                         strTime = strTime + "00.000"; //GPS 데이터 포맷 hhmmss.sss
                         strGPGGA = strGPGGA + strTime + "," + strLat + ",N," + strLong + ",E,1,7,1.000,10.000,M,0,M,0,*"; //뉴프로에서 첵섬 확인하지 않아서 그냥 필드 채운다. 

                         strGPRMC = strGPRMC + strTime + ",A," + strLat + ",N," + strLong + ",E,0.000000,121.61," + strDate + ",0,*";

                         /////////////////////////////////////// sample

                      //  strGPGGA = "$GPGGA,092725.00,4717.11399,N,00833.91590,E,1,08,1.01,499.6,M,48.0,M,,*5B  ";
                         
                         //////////////////////////////////////

                       



                         /////////////////////////////////////////////// GPGGA //////////////////////////////////////////////////////////
                         g_sendMsg = System.Text.Encoding.ASCII.GetBytes(strGPGGA); //https://blog.naver.com/pushesp/221048564242


                         byte  CheckSum =0;
                         for (int i = 0; i < g_sendMsg.Length; i++)
                         {
                             if (g_sendMsg[i] != '$' &&  g_sendMsg[i] != '*' )
                             {
                                 CheckSum ^=g_sendMsg[i];
                             }
                         }

                         byte CheckSum_0 = (byte)((CheckSum & 0xF0) >> 4);
                         byte CheckSum_1 = (byte)(CheckSum & 0x0F);

                         if (CheckSum_0 >= 0xA)
                         {
                             CheckSum_0 = (byte)(CheckSum_0 +0x37);
                         }
                         else
                         {
                             CheckSum_0 = (byte)(CheckSum_0 +'0'); 
                         }



                         if (CheckSum_1 >= 0xA)
                         {
                             CheckSum_1 = (byte)(CheckSum_1 +0x37);
                         }
                         else
                         {
                             CheckSum_1 = (byte)(CheckSum_1 + '0');
                         }

                        
                         strGPGGA += " ";
                         strGPGGA += " ";
                         strGPGGA += " ";
                         strGPGGA += " ";


                         g_sendMsg = System.Text.Encoding.ASCII.GetBytes(strGPGGA);
                         g_sendMsg[g_sendMsg.Length - 4] = CheckSum_0;
                         g_sendMsg[g_sendMsg.Length - 3] = CheckSum_1;
                         g_sendMsg[g_sendMsg.Length - 2] = 0x0D;
                         g_sendMsg[g_sendMsg.Length - 1] = 0x0A;

                     
                       //  sendBuf(g_sendMsg, g_sendMsg.Length);


                   

                         /////////////////////////////////////////////// GPRMC //////////////////////////////////////////////////////////
                         g_sendMsg_1 = System.Text.Encoding.ASCII.GetBytes(strGPRMC); //https://blog.naver.com/pushesp/221048564242


                          CheckSum = 0;
                         for (int i = 0; i < g_sendMsg_1.Length; i++)
                         {
                             if (g_sendMsg_1[i] != '$' && g_sendMsg_1[i] != '*')
                             {
                                 CheckSum ^= g_sendMsg_1[i];
                             }
                         }

                          CheckSum_0 = (byte)((CheckSum & 0xF0) >> 4);
                          CheckSum_1 = (byte)(CheckSum & 0x0F);

                         if (CheckSum_0 >= 0xA)
                         {
                             CheckSum_0 = (byte)(CheckSum_0 + 0x37);
                         }
                         else
                         {
                             CheckSum_0 = (byte)(CheckSum_0 + '0');
                         }



                         if (CheckSum_1 >= 0xA)
                         {
                             CheckSum_1 = (byte)(CheckSum_1 + 0x37);
                         }
                         else
                         {
                             CheckSum_1 = (byte)(CheckSum_1 + '0');
                         }


                         strGPRMC += " ";
                         strGPRMC += " ";
                         strGPRMC += " ";
                         strGPRMC += " ";


                         g_sendMsg_1 = System.Text.Encoding.ASCII.GetBytes(strGPRMC);
                         g_sendMsg_1[g_sendMsg_1.Length - 4] = CheckSum_0;
                         g_sendMsg_1[g_sendMsg_1.Length - 3] = CheckSum_1;
                         g_sendMsg_1[g_sendMsg_1.Length - 2] = 0x0D;
                         g_sendMsg_1[g_sendMsg_1.Length - 1] = 0x0A;

                      
                       //  sendBuf(g_sendMsg_1, g_sendMsg_1.Length);
                         ///////////////////////////////////////////////////////////////////////////////////////////

                         


                     }

				 }
			
		}

       
		void MainMap_OnMarkerLeave(GMapMarker item)
		{
			if (item is GMapMarkerRect)
			{
				CurentRectMarker = null;

				GMapMarkerRect rc = item as GMapMarkerRect;
				rc.Pen.Color = Color.Blue;
				MainMap.Invalidate(false);

				Debug.WriteLine("OnMarkerLeave: " + item.Position);
			}
		}

		void MainMap_OnMarkerEnter(GMapMarker item)
		{
			if (item is GMapMarkerRect)
			{
				GMapMarkerRect rc = item as GMapMarkerRect;
				rc.Pen.Color = Color.Red;
				MainMap.Invalidate(false);

				CurentRectMarker = rc;

				Debug.WriteLine("OnMarkerEnter: " + item.Position);
			}
		}
			
		// set route 
		private void button3_Click(object sender, EventArgs e)
		{
			start = currentMarker.Position;
		}
		// add test route
		private void button2_Click(object sender, EventArgs e)
		{
				  // add test route
				 MapRoute route = GMaps.Instance.GetRouteBetweenPoints(start, end, false, (int) MainMap.Zoom);
				 if(route != null)
				 {
					// add route
					GMapRoute r = new GMapRoute(route.Points, route.Name);
					routes.Routes.Add(r);

					// add route start/end marks
					GMapMarker m1 = new GMapMarkerGoogleRed(start);
					m1.ToolTipText = "Start: " + route.Name;
					m1.ToolTipMode = MarkerTooltipMode.Always;

					GMapMarker m2 = new GMapMarkerGoogleGreen(end);
					m2.ToolTipText = "End: " + end.ToString();
					m2.ToolTipMode = MarkerTooltipMode.Always;

					objects.Markers.Add(m1);
					objects.Markers.Add(m2);

					MainMap.ZoomAndCenterRoute(r);

				 }
		}
		void Markers_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
		{
		//	textBoxMarkerCount.Text = objects.Markers.Count.ToString();
		}
		void Routes_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
		{
		//	textBoxrouteCount.Text = routes.Routes.Count.ToString();
		}
		

		// add Marker
		private void button4_Click(object sender, EventArgs e)
		{
			GMapMarkerGoogleGreen m = new GMapMarkerGoogleGreen(currentMarker.Position);
			GMapMarkerCross x = new GMapMarkerCross(currentMarker.Position);
			GMapMarkerRect mBorders = new GMapMarkerRect(currentMarker.Position);
		
			{
				mBorders.InnerMarker = m;
				if (polygon != null)
				{
					x.Tag = polygon.Points.Count;
				}
				x.ToolTipMode = MarkerTooltipMode.Always;
			}

			
			Placemark p = null;

			if (checkBoxPlacemarkInfo.Checked)
			{
				p = GMaps.Instance.GetPlacemarkFromGeocoder(currentMarker.Position);
			}

			if (p != null)
			{
				x.ToolTipText = p.Address;
			
			}
			else
			{
				x.ToolTipText = currentMarker.Position.ToString();
			}

			objects.Markers.Add(m);
			objects.Markers.Add(x);
			objects.Markers.Add(mBorders);

			RegeneratePolygon();
		}

		private void button5_Click(object sender, EventArgs e)
		{
            int i;
			objects.Markers.Clear();
		//	routes.Routes.Clear();
			polygons.Polygons.Clear();
            for (i = 0; i<8000; i++)
            {
                ptArray[i].x = 0;
                ptArray[i].y = 0;
            }


		
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (objects.Markers.Count > 0)
			{
				MainMap.ZoomAndCenterMarkers(null);
				trackBar1.Value = 15;
			}
            this.Location = new Point(0, 0);
            this.Height = Screen.PrimaryScreen.Bounds.Height;
		}

		// save map
		private void button6_Click(object sender, EventArgs e)
		{
			try
			{
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					sfd.Filter = "JPG (*.png)|*.jpg";
					sfd.FileName = "GMap.jpg";

					Image tmpImage = MainMap.ToImage();
					if (tmpImage != null)
					{
						using (tmpImage)
						{
							if (sfd.ShowDialog() == DialogResult.OK)
							{
								tmpImage.Save(sfd.FileName);

								MessageBox.Show("Image saved: " + sfd.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Image failed to save: " + ex.Message, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
		{
				
		//	if (MainMap.Focused)
		//	{
				if (e.KeyChar == '+')
				{
					MainMap.Zoom += 1;
				}
				else if (e.KeyChar == '-')
				{
					MainMap.Zoom -= 1;
				}
				else if (e.KeyChar == 'a')
				{
					MainMap.Bearing--;
				}
				else if (e.KeyChar == 'z')
				{
					MainMap.Bearing++;
				}
		//	}
		}
		
		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{

			int offset = -22;

			if (e.KeyCode == Keys.Left)
			{
				MainMap.Offset(-offset, 0);
			}
			else if (e.KeyCode == Keys.Right)
			{
				MainMap.Offset(offset, 0);
			}
			else if (e.KeyCode == Keys.Up)
			{
				MainMap.Offset(0, -offset);
			}
			else if (e.KeyCode == Keys.Down)
			{
				MainMap.Offset(0, offset);
			}
			else if (e.KeyCode == Keys.Delete)
			{
				if (CurentRectMarker != null)
				{
					objects.Markers.Remove(CurentRectMarker);

					if (CurentRectMarker.InnerMarker != null)
					{
						objects.Markers.Remove(CurentRectMarker.InnerMarker);
					}
					CurentRectMarker = null;

					RegeneratePolygon();
				}
			}
			else if (e.KeyCode == Keys.Escape)
			{
				MainMap.Bearing = 0;

				if (currentTransport != null && !MainMap.IsMouseOverMarker)
				{
					currentTransport.ToolTipMode = MarkerTooltipMode.OnMouseOver;
					currentTransport = null;
				}
			}
		}

		void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (item is GMapMarkerRect)
				{
					Placemark pos = GMaps.Instance.GetPlacemarkFromGeocoder(item.Position);
					if (pos != null)
					{
						GMapMarkerRect v = item as GMapMarkerRect;
						{
							v.ToolTipText = pos.Address;
						}
						MainMap.Invalidate(false);
					}
				}
				else
				{
					if (item.Tag != null)
					{
						if (currentTransport != null)
						{
							currentTransport.ToolTipMode = MarkerTooltipMode.OnMouseOver;
							currentTransport = null;
						}
						currentTransport = item;
						currentTransport.ToolTipMode = MarkerTooltipMode.Always;
					}
				}
			}
		}

		private void button7_Click(object sender, EventArgs e)
		{
			int offset = -22;
			MainMap.Offset(-offset, 0);
		}
		void MainMap_KeyDown(object sender, KeyEventArgs e)
		{
			int offset = -22;
			if (e.KeyCode == Keys.Left)
			{
				MainMap.Offset(-offset, 0);
			}
			else if (e.KeyCode == Keys.Right)
			{
				MainMap.Offset(offset, 0);
			}
			else if (e.KeyCode == Keys.Up)
			{
				MainMap.Offset(0, -offset);
			}
			else if (e.KeyCode == Keys.Down)
			{
				MainMap.Offset(0, offset);
			}
			else if (e.KeyCode == Keys.Delete)
			{
				if (CurentRectMarker != null)
				{
					objects.Markers.Remove(CurentRectMarker);

					if (CurentRectMarker.InnerMarker != null)
					{
						objects.Markers.Remove(CurentRectMarker.InnerMarker);
					}
					CurentRectMarker = null;

					RegeneratePolygon();
				}
			}
			else if (e.KeyCode == Keys.Escape)
			{
				MainMap.Bearing = 0;

				if (currentTransport != null && !MainMap.IsMouseOverMarker)
				{
					currentTransport.ToolTipMode = MarkerTooltipMode.OnMouseOver;
					currentTransport = null;
				}
			}
			
		}

		private void button7_Click_1(object sender, EventArgs e)
		{
			end = currentMarker.Position;
		}

		private void Up_button_Click(object sender, EventArgs e)
		{
			int offset = -22;

			MainMap.Offset(0, -offset);
		}

		private void Left_button_Click(object sender, EventArgs e)
		{
			int offset = -22;
			MainMap.Offset(-offset, 0);
		}

		private void Right_button_Click(object sender, EventArgs e)
		{
			int offset = -22;
			MainMap.Offset(offset, 0);
		}

		private void button10_Click(object sender, EventArgs e)
		{
			int offset = -22;
			MainMap.Offset(0, offset);
		
		
		}

		public void Map_gps()
		{

		
			Thread workerThread = new Thread(DoWork);
			workerThread.Start();

	
		}

	    private void button10_Click_1(object sender, EventArgs e)
        {
            textBoxLat.Text = form1.gpsy;
            textBoxLng.Text = form1.gpsx;
        }
        public void Gps_route()
        {
              try
            {

                if (form1.gpsID == true)
                {
                    form1.gpsID = false;
                    if (form1.nDataPageStatus[0] != 1)
                    {
                        int num = 0;

                        num = (int)(form1.nDataPageStatus[0] - 1) * 10000;

                        for (int i = 0; i < form1.GPS_Sel_ID.Count; i++)
                        {
                            form1.GPS_Sel_ID[i] = form1.GPS_Sel_ID[i] - num;
                        }


                    }
                }
               
                for (int k = 0; k < form1.GPS_Sel_Lat.Count; k++)
                {

                    //	Cursor.Position = new Point(10, 10);

                    //	Cursor.Clip = new Rectangle(10, 10, 10, 10);					

                    Thread.Sleep(200);

                    double lat = double.Parse(form1.GPS_Sel_Lat[k], CultureInfo.InvariantCulture);
                    double lng = double.Parse(form1.GPS_Sel_Lng[k], CultureInfo.InvariantCulture);

                    MainMap.Position = new PointLatLng(lat, lng);

                   // MainMap.Position = new PointLatLng(form1.transData[k].CarGpsY, form1.transData[k].CarGpsX);

                    GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(MainMap.Position);
                    GMapMarkerCross start_cross_marker = new GMapMarkerCross(MainMap.Position);
                    GMapMarkerCross_Green start_cross_marker_Green = new GMapMarkerCross_Green(MainMap.Position);
                    GMapMarkerCross_Blue start_cross_marker_Blue = new GMapMarkerCross_Blue(MainMap.Position);


                    GMapMarkerRect mBorders = new GMapMarkerRect(MainMap.Position);


                    Placemark p1 = null;

                    if (checkBoxPlacemarkInfo.Checked)
                    {
                        p1 = GMaps.Instance.GetPlacemarkFromGeocoder(MainMap.Position);
                    }

                    if (p1 != null)
                    {

                       


                        int time_id = form1.GPS_Sel_ID[k];

                        time_id -= 1;

                      
                       if (k == 0)
                        {
                           

                            string timeData = String.Format("[START]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
                                   form1.transData[time_id].InforTimeYY, form1.transData[time_id].InforTimeMM, form1.transData[time_id].InforTimeDD,
                                   form1.transData[time_id].InforTimeHour, form1.transData[time_id].InforTimeMin, form1.transData[time_id].InforTimeSec,
                                   form1.transData[time_id].InforTimeMSec);

                            mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
                        }
                        else if (k == form1.GPS_Sel_Lat.Count-1)
                        {
                            string timeData = String.Format("[END]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
                                                               form1.transData[time_id].InforTimeYY, form1.transData[time_id].InforTimeMM, form1.transData[time_id].InforTimeDD,
                                                               form1.transData[time_id].InforTimeHour, form1.transData[time_id].InforTimeMin, form1.transData[time_id].InforTimeSec,
                                                               form1.transData[time_id].InforTimeMSec);

                            mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
                        }
                    }
                    else
                    {
                        int time_id = form1.GPS_Sel_ID[k];

                        time_id -= 1;
                        if (k == 0)
                        {


                            string timeData = String.Format("[START]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
                                   form1.transData[time_id].InforTimeYY, form1.transData[time_id].InforTimeMM, form1.transData[time_id].InforTimeDD,
                                   form1.transData[time_id].InforTimeHour, form1.transData[time_id].InforTimeMin, form1.transData[time_id].InforTimeSec,
                                   form1.transData[time_id].InforTimeMSec);

                            mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
                        }
                        else if (k == form1.GPS_Sel_Lat.Count - 1)
                        {
                            string timeData = String.Format("[END]  {0:D4}년{1:D2}월{2:D2}일 {3:D2}시{4:D2}분{5:D2}.{6:D2}초",
                                                               form1.transData[time_id].InforTimeYY, form1.transData[time_id].InforTimeMM, form1.transData[time_id].InforTimeDD,
                                                               form1.transData[time_id].InforTimeHour, form1.transData[time_id].InforTimeMin, form1.transData[time_id].InforTimeSec,
                                                               form1.transData[time_id].InforTimeMSec);

                            mBorders.ToolTipText = timeData.ToString(); // 시간 툴팁
                        }
                    }
                    //////////////////
                    {
                        mBorders.InnerMarker = start_green_marker;
                        if (polygon != null)
                        {
                            mBorders.Tag = polygon.Points.Count;
                        }

                        else if (polygon1 != null)
                        {
                            mBorders.Tag = polygon1.Points.Count;

                        }
                        else if (polygon2 != null)
                        {
                            mBorders.Tag = polygon2.Points.Count;

                        }

                        mBorders.ToolTipMode = MarkerTooltipMode.Always;
                    }
                    /////////////////
                    if (MainMap.Position.Lat > 32 && MainMap.Position.Lng > 122)
                        objects.Markers.Add(mBorders);

                  //  RegeneratePolygon();
                    ///////////////////////////////////////////////////////////////////////////
                    try
                    {
                        List<PointLatLng> polygonPoints = new List<PointLatLng>();


                        foreach (GMapMarker m in objects.Markers)
                        {
                            if (m is GMapMarkerRect)
                            {
                                m.Tag = polygonPoints.Count;
                                polygonPoints.Add(m.Position);
                            }
                        }


                        ////////////
                                          

                            if (polygon == null)
                            {
                                polygon = new GMapPolygon(polygonPoints, "polygon");
                                polygons.Polygons.Add(polygon);
                            }
                            else
                            {
                                polygon.Points.Clear();
                                polygon.Points.AddRange(polygonPoints);

                                if (polygons.Polygons.Count == 0)
                                {
                                    polygons.Polygons.Add(polygon);
                                }
                                else
                                {
                                    MainMap.UpdatePolygonLocalPosition(polygon);
                                }
                            }
                       
                       
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    ///////////////////////////////////////////////////////////////////////////
                    objects.Markers.Add(start_cross_marker);
                }
            }
            catch (System.Exception q)
            {
                {
                    MessageBox.Show(q.Message);
                }
            }
            MessageBox.Show("완료");
        }

        //yjk
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.Filter = "SHP|*.shp";
            fileOpen.InitialDirectory = ".";
            if (fileOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {

            }
        }

        int C_PT_PRECISION = 10000; //경계 계산시에 더블 계산 대신 롱 계산으로 변환 (원데이터가 소수 점 이하 4째 자리)
        // a Point is defined by its coordinates {int x, y;}

        // isLeft(): tests if a point is Left|On|Right of an infinite line.
        //    Input:  three points P0, P1, and P2
        //    Return: >0 for P2 left of the line through P0 and P1
        //            =0 for P2  on the line
        //            <0 for P2  right of the line
        //    See: Algorithm 1 "Area of Triangles and Polygons"
        int isLeft(myPoint P0, myPoint P1, myPoint P2)
        {
            return (int)((P1.x - P0.x) * (P2.y - P0.y)
                    - (P2.x - P0.x) * (P1.y - P0.y));
        }

        // cn_PnPoly(): crossing number test for a point in a polygon
        //      Input:   P = a point,
        //               V[] = vertex points of a polygon V[n+1] with V[n]=V[0]
        //      Return:  0 = outside, 1 = inside
        // This code is patterned after [Franklin, 2000]

        int cn_PnPoly(myPoint P, myPoint[] V, int n)
        {
            int cn = 0;    // the  crossing number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {    // edge from V[i]  to V[i+1]
                if (((V[i].y <= P.y) && (V[i + 1].y > P.y))     // an upward crossing
                 || ((V[i].y > P.y) && (V[i + 1].y <= P.y)))
                { // a downward crossing
                    // compute  the actual edge-ray intersect x-coordinate
                    float vt = (float)(P.y - V[i].y) / (V[i + 1].y - V[i].y);
                    if (P.x < V[i].x + vt * (V[i + 1].x - V[i].x)) // P.x < intersect
                        ++cn;   // a valid crossing of y=P.y right of P.x
                }
            }
            return (cn & 1);    // 0 if even (out), and 1 if  odd (in)

        }


        // wn_PnPoly(): winding number test for a point in a polygon
        //      Input:   P = a point,
        //               V[] = vertex points of a polygon V[n+1] with V[n]=V[0]
        //      Return:  wn = the winding number (=0 only when P is outside)
        int
        wn_PnPoly(myPoint P, myPoint[] V, int n)
        {
            int wn = 0;    // the  winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {   // edge from V[i] to  V[i+1]
                if (V[i].y <= P.y)
                {          // start y <= P.y
                    if (V[i + 1].y > P.y)      // an upward crossing
                        if (isLeft(V[i], V[i + 1], P) > 0)  // P left of  edge
                           wn++;// ++wn;            // have  a valid up intersect
                }
                else
                {                        // start y > P.y (no test needed)
                    if (V[i + 1].y <= P.y)     // a downward crossing
                        if (isLeft(V[i], V[i + 1], P) < 0)  // P right of  edge
                            wn--;//--wn;            // have  a valid down intersect
                }
            }
            return wn;
        }


        int PointInPolygon(myPoint P, myPoint[] V, int n)    //(Point point, Polygon polygon) //from  https://stackoverflow.com/questions/11716268/point-in-polygon-algorithm 
        {
            int i, j, nvert = n;
            int c = 0;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((V[i].y >= P.y) != (V[j].y >=P.y)) &&
                    (P.x <= (V[j].x - V[i].x) * (P.y - V[i].y) / (V[j].y - V[i].y) + V[i].x)
                  )
                    c = 1-c;
            }

            return c;
        }


        private void sendBuf(byte[] byCmd, int nlen)
        {
            int i = 0;
            byte[] lub_sendBuf = new byte[2];
            int k = 0;
            int j = 0;

            if (serialPort1.IsOpen)
            {
                serialPort1.Write(byCmd, 0, nlen);

                /*         for (i = 0; i < nlen; i++)
                         {
                             lub_sendBuf[0] = byCmd[i];  // 
                                                         //  허브에서 속도 문제로 보내는 데이터 못보내는 것을 방지
                             serialPort1.Write(lub_sendBuf , 0, 1);
                             if ( (i % 500) == 0)
                             {
                                 for (k = 0; k < 10000000; k++)  //50000 일때 증상 개선은 되나, 멈추는 증상 나오긴 함. 100000 일때 20번중 3번
                                 {
                                     j++;
                                 }
                             }
                         }
                */
            }
        }
    

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.Filter = "SHP|*.shp";
            fileOpen.InitialDirectory = ".";
            if (fileOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                int i = 0;
                string str = null;
                string defuaultStr = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
                string defuaultStr1 = "<gps>";
                string defuaultStr2 = "<name>KOREA</name>";
                string defuaultStr3 = "</gps>";

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fileOpen.FileName);
                var root = xmldoc.DocumentElement;
                var nodes = root.ChildNodes;
                string nodeData;
                string nodeDataY;

                FileStream fs, fs1;
                StreamWriter sw;
                StreamReader sr;
                fs1 = new FileStream(fileOpen.FileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs1);

                int fileNum = 0;
                fs = new FileStream("gpsData" + Convert.ToString(fileNum) + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);

                try
                {
                    do
                    {
                        nodeData = sr.ReadLine();
                        if (nodeData.Contains("<RECORD"))
                        {
                            sw.Close();
                            //newfile 
                            fs = new FileStream("gpsData" + Convert.ToString(fileNum) + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
                            fileNum++; //gpsData0, gpsData1....
                            sw = new StreamWriter(fs);
                            sw.WriteLine(defuaultStr);
                            sw.WriteLine(defuaultStr1);
                            sw.WriteLine(defuaultStr2);
                        }
                        else if (nodeData.Contains("<VERTEX"))
                        {
                            sw.WriteLine(nodeData);
                        }
                        else if (nodeData.Contains("</RECORD>"))
                        {
                            sw.WriteLine(defuaultStr3);
                        }

                    } while (!sr.EndOfStream);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return;
                }

            }
        } 

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.Filter = "XML|*.xml";  
            fileOpen.InitialDirectory = ".";
            if (fileOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                int i = 0;
                string str = null;
                string defuaultStr = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
                string defuaultStr1 = "<gps>";
                string defuaultStr2 = "<name>";//KOREA</name>";
                string defuaultStr3 = "</gps>";

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fileOpen.FileName);
                var root = xmldoc.DocumentElement;
                var nodes = root.ChildNodes;
                string nodeData;
                string nodeDataY;
               
               FileStream fs,fs1;
               StreamWriter sw;
               StreamReader sr;
               fs1 = new FileStream(fileOpen.FileName, FileMode.Open, FileAccess.Read);
               sr = new StreamReader(fs1);

                int fileNum = 0;
                fs = new FileStream("gpsData" + Convert.ToString(fileNum) + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);

                try
                {
                    do
                    {
                        nodeData = sr.ReadLine();
                        if (nodeData.Contains("<RECORD"))
                        {
                            sw.Close();
                            //newfile 
                            fs = new FileStream("gpsData" + Convert.ToString(fileNum) + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
                            fileNum++; //gpsData0, gpsData1....
                            sw = new StreamWriter(fs);
                            sw.WriteLine(defuaultStr);
                            sw.WriteLine(defuaultStr1);
                            defuaultStr2 = defuaultStr2 + Convert.ToString(fileNum) + "</name>";
                            sw.WriteLine(defuaultStr2);
                        }
                        else if (nodeData.Contains("<VERTEX"))
                        {
                            sw.WriteLine(nodeData);
                        }
                        else if (nodeData.Contains("</RECORD>"))
                        {
                            sw.WriteLine(defuaultStr3);
                            defuaultStr2 = "<name>";
                        }

                    }while(!sr.EndOfStream);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return;
                }
                
            }
            
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            int pointIdx = 0; //경계지점 저장하는 어레이 인덱스 초기화
            int i = 0;

            //좌표 불러오기 (파일 오픈)
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.Filter = "XML|*.xml";
            fileOpen.InitialDirectory = ".";
            if (fileOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                string str = null;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fileOpen.FileName);
                var root = xmldoc.DocumentElement;
                var nodes = root.ChildNodes;
                string nodeData;
                string nodeDataY;
                string cityNumber = " ";

                Placemark p = null;
                PointLatLng dataPoint = new PointLatLng(37.4761012096049, 126.882938146591);
                
                string[] strDataConv = new string[4];
                numOfPoints = 0; //경계지점 개수 초기화 
                int C_PT_BASE = 0; // 경계지점이 번호대로 잘 그려지는지 확인하기 위해서 색을 구분한다. 1000개 단위로 본다. 0~1000보려면 BASE =0, 1000~2000 보려면 BASE=1000 ...
                int exit = 0;

                Gps_First_hours = true;
                try
                {
                    foreach (System.Xml.XmlNode OutNode in nodes)
                    {
                        switch (OutNode.Name)
                        {
                            case "name":
                                {
                                    //nodeData =OutNode.Attributes.GetNamedItem()
                                    cityNumber = OutNode.InnerText;
                                    label7.Text = "City: " + cityNumber;
                                } break;
                            case "VERTEX":
                                {
                                    nodeData = OutNode.Attributes.GetNamedItem("Y").InnerText;
                                    nodeDataY = OutNode.Attributes.GetNamedItem("X").InnerText;
                                    dataPoint.Lat = Convert.ToDouble(nodeData);
                                    dataPoint.Lng = Convert.ToDouble(nodeDataY);
                                    //ptArray[pointIdx].y = (int)(dataPoint.Lat * C_PT_PRECISION);  // 127.123456 ... 을 정수로 바꿔서 확인한다. (속도때문에...정수로 바꿔서 소수 이하 6째 자리만 보는 걸로한다)
                                    //ptArray[pointIdx].x = (int)(dataPoint.Lng * C_PT_PRECISION);   //!!!이렇게 하면 마지막 자리 값이 1차이가 난다. 더블 ==> int 변환시에...
                                    strDataConv = nodeData.Split('.');
                                    nodeData = strDataConv[0] + strDataConv[1];
                                    ptArray[pointIdx].y = Convert.ToInt32(nodeData);
                                    strDataConv = nodeDataY.Split('.');
                                    nodeDataY = strDataConv[0] + strDataConv[1];
                                    ptArray[pointIdx].x = Convert.ToInt32(nodeDataY);

                                    if (pointIdx > 1)
                                    {
                                        if (ptArray[pointIdx].y == ptArray[pointIdx - 1].y && ptArray[pointIdx].x == ptArray[pointIdx - 1].x)
                                        {
                                            break; //중복되는 점이 많이 있어서 넣음. 바로 전 데이터와 같은 데이터면 포인터 증가 하지 않고 그리지 않음.
                                        }
                                    }

                                    if (numOfPoints == 8172)
                                    {
                                        MessageBox.Show("test");
                                    }
                               
                                        GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(dataPoint);
                                        GMapMarkerGoogleGreen m = new GMapMarkerGoogleGreen(dataPoint);
                                        GMapMarkerCross x = new GMapMarkerCross(dataPoint);
                                        GMapMarkerRect mBorders = new GMapMarkerRect(dataPoint);
                                        GMapMarkerCross_Green start_cross_marker_Green = new GMapMarkerCross_Green(dataPoint);
                                        GMapMarkerCross_Blue start_cross_marker_Blue = new GMapMarkerCross_Blue(dataPoint);
                                        
                                            mBorders.InnerMarker = m;
                                            
                                             if (polygon != null)
                                             {
                                                 x.Tag = polygon.Points.Count;
                                             }
                                             if (checkBox_DrawIndex.Checked == true) //포인트마다 인덱스 표시 해?
                                             {
                                                 x.ToolTipMode = MarkerTooltipMode.Always;
                                                 x.ToolTipText = Convert.ToString(pointIdx);
                                                 // 경계지점이 번호대로 잘 그려지는지 확인하기 위해서 색을 구분한다. 1000개 단위로 본다. 0~1000보려면 BASE =0, 1000~2000 보려면 BASE=1000 ...
                                                  if (pointIdx < C_PT_BASE + 100)
                                                      x.ToolTip.Stroke = Pens.AliceBlue;
                                                  else if (pointIdx < C_PT_BASE + 200)
                                                      x.ToolTip.Stroke = Pens.Beige;
                                                  else if (pointIdx < C_PT_BASE + 300)
                                                      x.ToolTip.Stroke = Pens.Black;
                                                  else if (pointIdx < C_PT_BASE + 400)
                                                      x.ToolTip.Stroke = Pens.Brown;
                                                  else if (pointIdx < C_PT_BASE + 500)
                                                      x.ToolTip.Stroke = Pens.Crimson;
                                                  else if (pointIdx < C_PT_BASE + 600)
                                                      x.ToolTip.Stroke = Pens.DarkOrange;
                                                  else if (pointIdx < C_PT_BASE + 700)
                                                      x.ToolTip.Stroke = Pens.Green;
                                                  else if (pointIdx < C_PT_BASE + 800)
                                                      x.ToolTip.Stroke = Pens.Pink;
                                                  else if (pointIdx < C_PT_BASE + 900)
                                                      x.ToolTip.Stroke = Pens.Yellow;
                                                  else
                                                      x.ToolTip.Stroke = Pens.Violet;

                                                 if (pointIdx % 100 == 0)  //100개 마다 숫자 표시
                                                 {
                                                     objects.Markers.Add(x);  //tooltip 표시
                                                 }
                                             }
                                       // objects.Markers.Add(mBorders);  //선표시
                                        if (pointIdx % 3 == 0)
                                        {
                                            objects.Markers.Add(start_cross_marker_Green); // 점 표시
                                        }
                                        else if (pointIdx % 3 == 1)
                                        {
                                            objects.Markers.Add(start_cross_marker_Blue); // 점 표시 
                                        }
                                        else
                                        {
                                            objects.Markers.Add(x); // 점 표시 
                                        }
                                        RegeneratePolygon();
                                
                                        numOfPoints++; //현재 경계의 개수 증가
                                        pointIdx++;
                                } break;
                            default:
                                break;
                            }
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);  
                    //return;
                }

                MainMap.Position = dataPoint;
            ptValidityCheck();

            FileStream fs, fsBin;
            StreamWriter sw;
            BinaryWriter swBin;

            string[] strFName= new string[3];
            strFName = fileOpen.FileName.Split('.');
            byte chksum = 0; // MAP 파일에 저장할 첵섬
            byte[] bytes = new byte[100];
            fs = new FileStream(strFName[0]+".txt", FileMode.OpenOrCreate, FileAccess.Write);
            fsBin = new FileStream(strFName[0] + ".map", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            swBin = new BinaryWriter(fsBin);
            Int32 intTemp;

            PointLatLng point = new PointLatLng(37.4761012096049, 126.882938146591);

    
            chksum = 0;
            bytes[0] = 0xA1;
            chksum += bytes[0];
            swBin.Write(bytes, 0, 1);
            swBin.Write(Convert.ToInt16(cityNumber));

            bytes[0] = 0xA2;
            chksum += bytes[0];
            swBin.Write(bytes, 0, 1);
     
          //  swBin.Write(numOfPoints);
            bytes = BitConverter.GetBytes(numOfPoints);
            chksum += (byte)( bytes[0] + bytes[1] + bytes[2]); //int 64비트지만 거의 0이므로 하위 3바이트만 첵섬에 더한다. 
          
            swBin.Write(bytes, 0, 3);


            sw.WriteLine("const GpsPoint GPSEdgeData[] = {" + cityNumber + String.Format(",{0:d},", numOfPoints)); //TXT 로 좌표 저장.

            bytes[0] = 0xA3;
            chksum += bytes[0];
            swBin.Write(bytes, 0, 1);
           // swBin.Write(0xA3);
            string str_ = "";
            for (i = 0; i < numOfPoints; i++)
            {

                if (i == numOfPoints - 1)
                {
                    str_ = "{";
                    str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                    str_ += "}";
                    sw.Write(str_);
                    // sw.WriteLine(String.Format("{0:d}, {1:d}", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.
                }
                else if (i %4 ==0 )
                {

                    str_ = "{";
                    str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                    str_ += "},\n";
                    sw.Write(str_);
                    // sw.WriteLine(String.Format("{0:d}, {1:d},", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.  
                }
                else
                {
                    str_ = "{";
                    str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                    str_ += "},";
                    sw.Write(str_);
                    // sw.Write(String.Format("{0:d}, {1:d},", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.  
                }
              /*  if (i % 4 != 3) //txt 파일 줄맞춤
                {
                    if (i == numOfPoints - 1) //마지막 자료는 콤마 찍지 않는다. 
                    {
                        str_ = "{";
                        str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                        str_ += "}";
                        sw.Write(str_);
                        // sw.Write(String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장. 

                    }
                    else
                    {

                        str_ = "{";
                        str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                        str_ += "},";
                        sw.Write(str_);
                    }
                       // sw.Write(String.Format("{0:d}, {1:d},", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장. 
                }
                else //좌표 4개마다 줄 바꿈. 
                {
                    if (i == numOfPoints - 1)
                    {
                        str_ = "{";
                        str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                        str_ += "}";
                        sw.Write(str_);
                       // sw.WriteLine(String.Format("{0:d}, {1:d}", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.
                    }
                    else if (i != 0)
                    {

                        str_ = "{";
                        str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                        str_ += "},\n";
                        sw.Write(str_);
                       // sw.WriteLine(String.Format("{0:d}, {1:d},", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.  
                    }
                    else
                    {
                        str_ = "{";
                        str_ += (String.Format("{0:d}, {1:d}", +ptArray[i].x, ptArray[i].y));
                        str_ += "},";
                        sw.Write(str_);
                       // sw.Write(String.Format("{0:d}, {1:d},", ptArray[i].x, ptArray[i].y)); //TXT 로 좌표 저장.  
                    }
                }*/
                //binary 파일에 저장
                intTemp = Convert.ToInt32(ptArray[i].x);
                bytes = BitConverter.GetBytes(intTemp);
                swBin.Write(bytes,0,4);
                chksum = (byte)(chksum+bytes[0] + bytes[1] + bytes[2]);

                intTemp= Convert.ToInt32(ptArray[i].y);
                bytes = BitConverter.GetBytes(intTemp);
                swBin.Write(bytes, 0, 4);
                chksum = (byte)(chksum+bytes[0] + bytes[1] + bytes[2]);

                /*To test ptArray on a MAP only
                 *             point.Lat = Convert.ToDouble(ptArray[i].y * 0.0001);
                               point.Lng = Convert.ToDouble(ptArray[i].x * 0.0001);

                               GMapMarkerGoogleGreen start_green_marker = new GMapMarkerGoogleGreen(point);
                               GMapMarkerGoogleGreen m = new GMapMarkerGoogleGreen(point);
                               GMapMarkerCross x = new GMapMarkerCross(point);
                               GMapMarkerRect mBorders = new GMapMarkerRect(point);
                               GMapMarkerCross_Green start_cross_marker_Green = new GMapMarkerCross_Green(point);
                               {
                                   mBorders.InnerMarker = m;

                                   if (polygon != null)
                                   {
                                       x.Tag = polygon.Points.Count;
                                   }
                                   x.ToolTipMode = MarkerTooltipMode.Always;
                                   x.ToolTipText = Convert.ToString(i);
                                   //objects.Markers.Add(x);  //tooltip 표시
                                   RegeneratePolygon();
                               }
                */
            }
            sw.WriteLine("};");
            sw.Close();
         //   swBin.Write(chksum);
            swBin.Close();
            }
        }

        // 경계선 내에 같은 점이 있는지 찾아서 메세지 띄워준다. 
        private void ptValidityCheck()
        {
            int i=0;
            int j=0;
            string strResult="same data at ";
            int k=0;
            for(i=0; i<numOfPoints; i++)
            {
                if(k==10) //결과는 최대 10개씩만 보여준다.
                    break;
                for(j=i+1; j< numOfPoints ; j++)
                {
                    if( ptArray[i].x == ptArray[j].x)
                    {
                        if(ptArray[i].y == ptArray[j].y)
                        {
                            k++; //결과는 최대 10개씩만 보여준다.
                            strResult = strResult + String.Format("({0:d}, {1:d},{2:d},{3:d}),", i, j, ptArray[i].x, ptArray[i].y);
                        }
                    }
                }
            }
            if (k == 0)
            {
                MessageBox.Show("Data check OK");  
            }
            else
                MessageBox.Show(strResult);  

        }

        public double distance(double P1_latitude, double P1_longitude, double P2_latitude, double P2_longitude)
        {
            if ((P1_latitude == P2_latitude) && (P1_longitude == P2_longitude))
            {
                return 0;
            }
            double e10 = P1_latitude * Math.PI / 180;
            double e11 = P1_longitude * Math.PI / 180;
            double e12 = P2_latitude * Math.PI / 180;
            double e13 = P2_longitude * Math.PI / 180;
            /* 타원체 GRS80 */
            double c16 = 6356752.314140910;
            double c15 = 6378137.000000000;
            double c17 = 0.0033528107;
            double f15 = c17 + c17 * c17;
            double f16 = f15 / 2;
            double f17 = c17 * c17 / 2;
            double f18 = c17 * c17 / 8;
            double f19 = c17 * c17 / 16;
            double c18 = e13 - e11;
            double c20 = (1 - c17) * Math.Tan(e10);
            double c21 = Math.Atan(c20);
            double c22 = Math.Sin(c21);
            double c23 = Math.Cos(c21);
            double c24 = (1 - c17) * Math.Tan(e12);
            double c25 = Math.Atan(c24);
            double c26 = Math.Sin(c25);
            double c27 = Math.Cos(c25);
            double c29 = c18;
            double c31 = (c27 * Math.Sin(c29) * c27 * Math.Sin(c29)) + (c23 * c26 - c22 * c27 * Math.Cos(c29)) * (c23 * c26 - c22 * c27 * Math.Cos(c29));
            double c33 = (c22 * c26) + (c23 * c27 * Math.Cos(c29)); double c35 = Math.Sqrt(c31) / c33;
            double c36 = Math.Atan(c35); double c38 = 0; if (c31 == 0) { c38 = 0; }
            else
            {
                c38 = c23 * c27 * Math.Sin(c29) / Math.Sqrt(c31);
            }
            double c40 = 0;
            if ((Math.Cos(Math.Asin(c38)) * Math.Cos(Math.Asin(c38))) == 0)
            {
                c40 = 0;
            }
            else
            {
                c40 = c33 - 2 * c22 * c26 / (Math.Cos(Math.Asin(c38)) * Math.Cos(Math.Asin(c38)));
            }
            double c41 = Math.Cos(Math.Asin(c38)) * Math.Cos(Math.Asin(c38)) * (c15 * c15 - c16 * c16) / (c16 * c16); double c43 = 1 + c41 / 16384 * (4096 + c41 * (-768 + c41 * (320 - 175 * c41)));

            double c45 = c41 / 1024 * (256 + c41 * (-128 + c41 * (74 - 47 * c41))); double c47 = c45 * Math.Sqrt(c31) * (c40 + c45 / 4 * (c33 * (-1 + 2 * c40 * c40) - c45 / 6 * c40 * (-3 + 4 * c31) * (-3 + 4 * c40 * c40)));
            double c50 = c17 / 16 * 
                Math.Cos(Math.Asin(c38)) * Math.Cos(Math.Asin(c38)) * (4 + c17 * (4 - 3 * Math.Cos(Math.Asin(c38)) * Math.Cos(Math.Asin(c38)))); 
            double c52 = c18 + (1 - c50) * c17 * c38 * 
                (Math.Acos(c33) + c50 * Math.Sin(Math.Acos(c33)) * (c40 + c50 * c33 * (-1 + 2 * c40 * c40)));

            double c54 = c16 * c43 * (Math.Atan(c35) - c47);  // return distance in meter  
            return c54;
        }

        public double calDistance(double lat1, double lon1, double lat2, double lon2)
        {

            double theta, dist;
            theta = lon1 - lon2;
            dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1))
                  * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);

            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;    // 단위 mile 에서 km 변환.  
            dist = dist * 1000.0;      // 단위  km 에서 m 로 변환  

            return dist;
        }

        // 주어진 도(degree) 값을 라디언으로 변환  
        private double deg2rad(double deg)
        {
            return (double)(deg * Math.PI / (double)180d);
        }

        // 주어진 라디언(radian) 값을 도(degree) 값으로 변환  
        private double rad2deg(double rad)
        {
            return (double)(rad * (double)180d / Math.PI);
        }

        private void MainMap_Load(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void panelMenu_CloseClick(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void comboBoxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBoxSerialPort.SelectedItem.ToString();

            serialPort1.Open();

            if (serialPort1.IsOpen)
            {
                PORT.Text = "PORT : OPEN";
            }
            else
            {
                PORT.Text = "PORT : Select";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isGPSsend == true)
            {
                sendBuf(g_sendMsg, g_sendMsg.Length);

                sendBuf(g_sendMsg_1, g_sendMsg_1.Length);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
             Thread workerThread = new Thread(Gps_route);
            workerThread.Start();
        }

      





		
		

	}
}