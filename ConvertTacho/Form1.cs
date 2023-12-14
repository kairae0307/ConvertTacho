using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
using System.Data.OleDb;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Sockets;
using System.Net;
	
//zedgraph.org

namespace ConvertTacho
{
    public partial class Form1 : Form
    {
        private Form2 frmGraph2;
        private Form3 frmGraph3;
        private Form4 frmGraph4;
        public string starttext = "";
        public string endtext = "";
        public string HeaderStr = "";
        public string PlateNoTemp = "";
        public string DateTemp = "";
        private DataForm dataForm;

		public PrintableListView.PrintableListView m_list;

        public List<string> GPS_Sel_Lng = new List<string>();
        public List<string> GPS_Sel_Lat = new List<string>();
        public List<int> GPS_Sel_ID = new List<int>();
        private iniClass inicls = new iniClass();


        public bool TotalVersion = false;
        private bool bIsDataFull = false;
        private string strOpenedFileName;

        private uint nNumberOfOnePage = 10000;
        public uint[] nDataPageStatus;
		public bool LastPage_check= false;
		public bool Gpx_data=false;
        public bool gpsID = false;
		public uint Data_Length = 0;
		public uint LastPage = 0
            ;
        public UInt32 OldCarGpsX;                  // 9
        public UInt32 OldCarGpsY;

        public string gpsx = "";
        public string gpsy = "";

        public int SecTime_Length = 0;


        public bool ReTime = false;


        public UInt16 DayDriveDistance_old = 0;
        public UInt32 TotalDriveDistance_old = 0;

        
 
		public enum TextType
		{
			UNICODE_LE,
			UNICODE_BE,
			UTF_8,
			ANSI
		}

        public struct ViewSetting
        {
            public bool isSimpleView;
            public UInt32 nNumOfOnePageView;

            public DateTime startDateTime;
            public DateTime endDateTime;
        }

        public ViewSetting stViewSetting;

        public Form1()
        {
            InitializeComponent();
            frmGraph2 = new Form2(this);
            frmGraph3 = new Form3(this);
            frmGraph4 = new Form4(this);
		
            dataForm = new DataForm(this);

            nDataPageStatus = new uint[2];
            nDataPageStatus[0] = 1; // 첫 페이지 숫자는 1
            nDataPageStatus[1] = 0; // 총 페이지 수는 0
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width-100;
            stViewSetting = new ViewSetting();

            CheckForIllegalCrossThreadCalls = false; 
        }
		public struct JTF_Header
		{
			public byte[] TachoTransTime;           // 4, 타코 자료 저장시작일, YYYY/MM/DD
			public byte[] TachoDataSize;            // 3, 타코 자료 크기
			public byte[] ModelName;                // 10, 모델명 : "NEWPRO"(6Bytes) + 버전번호(4Bytes)
			public byte[] CarNum;                   // 17, 차대번호
			public byte CarType;                    // 1, 자동차 유형
			public byte[] CarRegistNum;             // 6, 자동차 등록번호
			public byte[] OperatorRegistNum;        // 5, 운송사업자 등록번호
			public byte[] DriverCode;               // 9, 운전자코드
			public bool HeaderCheckSum;  
		}
		public struct JTF_RecordB
		{
			public byte[] status;        // 2000  2byte
			public byte[] status_temp;
			public byte[] longitude;	 // 40 경도 4byte
			public byte[] longitude_temp;
			public byte[] reserved1;	 // 4
			public byte[] reserved2;     // 4
			public byte[] ibArea;        // 4 
			public byte[] latitude;		// 40 위도 4byte
			public byte[] latitude_temp;
			public byte[] azimuth;		// 20 방위각 2byte
			public byte[] azimuth_temp;
		}
		public struct JTF_RecordA
		{
			public byte[] accelX;		// 1000 1byte   1000ea
			public byte[] accelX_temp;
			public byte[] accelY;		// 1000	
			public byte[] accelY_temp;
			public byte[] distDaily;	// 20 2byte		10ea
			public byte[] distDaily_temp;
			public byte[] rpm;			// 20 2byte
			public byte[] rpm_temp;
			public byte[] reserved1;	// 4			1a
			public byte[] reserved2;	// 4
			public byte[] ibArea;		// 4	
			public byte[] reserved3;	// 4
			public byte[] speed;        // 10 1byte
			public byte[] speed_temp;
			public byte[] distAccum;    // 30 3byte
			public byte[] distAccum_temp;
			public byte[] msIdx;        // 2 2byte
			public byte[] scIdx;        // 2 2byte
			public byte msec;			// 1 1byte
			public byte[] mstime;       // 6
			public byte[] dummy;		// 2 1byte
			public byte chsum;		// 1 1byte
		}
		public struct JTF_RecordB_Back
		{
			public byte[] status;        // 2000  2byte
			public byte[] status_temp; 
			public byte[] longitude;	 // 40 경도 4byte
			public byte[] longitude_temp;
			public byte[] reserved1;	 // 4
			public byte[] reserved2;     // 4
			public byte[] ibArea;        // 4 
			public byte[] latitude;		// 40 위도 4byte
			public byte[] latitude_temp				;
			public byte[] azimuth;		// 20 방위각 2byte
			public byte[] azimuth_temp;
		}
		public struct JTF_RecordA_BAck
		{
			public byte[] accelX;		// 1000 1byte   1000ea
			public byte[] accelX_temp;
			public byte[] accelY;		// 1000	
			public byte[] accelY_temp;
			public byte[] distDaily;	// 20 2byte		10ea
			public byte[] distDaily_temp;
			public byte[] rpm;			// 20 2byte
			public byte[] rpm_temp;
			public byte[] reserved1;	// 4			1a
			public byte[] reserved2;	// 4
			public byte[] ibArea;		// 4	
			public byte[] reserved3;	// 4
			public byte[] speed;        // 10 1byte
			public byte[] speed_temp;
			public byte[] distAccum;    // 30 3byte
			public byte[] distAccum_temp;
			public byte[] msIdx;        // 2 2byte
			public byte[] scIdx;        // 2 2byte
			public byte msec;			// 1 1byte
			public byte[] mstime;       // 6
			public byte[] dummy;		// 2 1byte
			public byte chsum;		// 1 1byte
		}
		public struct JTF_Time
		{
			public byte Year;
			public byte Month;
			public byte Dayy;
			public byte Hour;			// 1 1byte
			public byte Minute;
			public byte Sec;
			public byte Msec;
			public byte[] time;
			public byte mmsec;
		}
		public struct Convert_Time
		{
			public byte[] Year;
			public byte[] Month;
			public byte[] Dayy;
			public byte[] Hour;			// 1 1byte
			public byte[] Minute;
			public byte[] Sec;
			public byte[] Msec;
			
		}


        // *.TMF 파일에서 읽어온 Data 저장용 - Header 부분
        public struct NewProInputHeader             // 56
        {
            public byte[] TachoTransTime;           // 4, 타코 자료 저장시작일, YYYY/MM/DD
            public byte[] TachoDataSize;            // 3, 타코 자료 크기
            public byte[] ModelName;                // 10, 모델명 : "NEWPRO"(6Bytes) + 버전번호(4Bytes)
            public byte[] CarNum;                   // 17, 차대번호
            public byte CarType;                    // 1, 자동차 유형
            public byte[] CarRegistNum;             // 6, 자동차 등록번호
            public byte[] OperatorRegistNum;        // 5, 운송사업자 등록번호
            public byte[] DriverCode;               // 9, 운전자코드
            public bool HeaderCheckSum;             // 1, Header checksum
        }

        // *.TMF 파일에서 읽어온 Data 저장용 - Data 부분
        public struct NewProInputData               // 32
        {
            public byte[] DayDriveDistance;         // 2, 일일 누적 주행거리 (Km)
            public byte[] TotalDriveDistance;       // 3, 총 누적 주행거리 (Km)
            public byte[] InforTime;                // 7, 정보 발생 일시 (YY/MM/DD hh:mm:ss.ss)
            public byte[] CarRPM;                   // 2, RPM
            public byte[] Reserved;                 // 2, 예약 (Default: 0x0000)
            public byte[] InforGPSX;                // 4, GPS 경도
            public byte[] InforGPSY;                // 4, GPS 위도
            public byte[] CarDegree;                // 2, GPS 방위각
            public byte AccelX;                     // 1, 가속도 X
            public byte AccelY;                     // 1, 가속도 Y
            public byte[] MachineStatus;            // 2, 운행기록계 상태
            public byte CarSpeed;                   // 1, 차량 속도
            public bool DataCheckSum;               // 1, Data checksum
        }

        public struct TempNewProInputData               // 32
        {
            public byte[] DayDriveDistance;         // 2, 일일 누적 주행거리 (Km)
            public byte[] TotalDriveDistance;       // 3, 총 누적 주행거리 (Km)
            public byte[] InforTime;                // 7, 정보 발생 일시 (YY/MM/DD hh:mm:ss.ss)
            public byte[] CarRPM;                   // 2, RPM
            public byte[] Reserved;                 // 2, 예약 (Default: 0x0000)
            public byte[] InforGPSX;                // 4, GPS 경도
            public byte[] InforGPSY;                // 4, GPS 위도
            public byte[] CarDegree;                // 2, GPS 방위각
            public byte AccelX;                     // 1, 가속도 X
            public byte AccelY;                     // 1, 가속도 Y
            public byte[] MachineStatus;            // 2, 운행기록계 상태
            public byte CarSpeed;                   // 1, 차량 속도
            public bool DataCheckSum;               // 1, Data checksum
        }



        // *.TXT 파일로 저장할 Data 저장용 - Header 부분
        public struct NewProOutputHeader            // 79
        {
			public string ModelName;                // 20
            public string CarNum;                   // 17
            public ushort CarType;                  // 2
            public string CarRegistNum;             // 12
            public string OperatorRegistNum;        // 10
            public string DriverCode;               // 18
			
		}
		private List<string> DriverCodeTemp = new List<string>();			// 18, 운전자코드 Temp

        // *.TXT 파일로 저장할 Data 저장용 - Data 부분
        public struct NewProOutputData              // 68
        {
            public bool DataTrust;
            public UInt16 DayDriveDistance;         // 4
            public UInt32 TotalDriveDistance;       // 7
            public byte[] InforTime;                // 14
            public byte CarSpeed;                   // 3
            public UInt16 CarRPM;                   // 4
            public byte CarBreak;                   // 1
            public UInt32 CarGpsX;                  // 9
            public UInt32 CarGpsY;                  // 9
            public UInt16 CarDegree;                // 3
            public string CarAccelX;                // 6
            public string CarAccelY;                // 6
            public byte MachineStatus;              // 2
        }

        // *.TXT 파일에서 읽어온 Data 저장용 - Header 부분
        public struct TransDataHeader
        {
            public string ModelName;
            public string CarNum;
            public string CarType;
            public string CarRegistNum;
            public string OperatorRegistNum;
            public string DriverCode;
        }

        // *.TXT 파일에서 읽어온 Data 저장용 - Data 부분
        public struct TransData
        {
            public UInt16 DayDriveDistance;
            public UInt32 TotalDriveDistance;
            public UInt16 InforTimeYY;
            public UInt16 InforTimeMM;
            public UInt16 InforTimeDD;
            public UInt16 InforTimeHour;
            public UInt16 InforTimeMin;
            public UInt16 InforTimeSec;
            public UInt16 InforTimeMSec;
            public UInt16 CarSpeed;
            public UInt16 CarRPM;
            public Boolean CarBreak;
            public double CarGpsX;
            public double CarGpsY;
            public UInt16 CarDegree;
            public double CarAccelX;
            public double CarAccelY;
            public string MachineStatus;
        }

        private int BcdToDecimal(byte bTemp)
        {
            return (((bTemp >> 4) * 10) + (bTemp & 0x0F));
        }

        public void LoadingWork()    // LoadThread Funtion
        {
            bool _shouldStop;
            string strFname;

            if (TotalVersion == false)
            {
                ConvertFunction(Tempstr, Tempdirname);
            }
            else
            {
                TotalConvertFunction(Tempstr, Tempdirname);
            }

            _shouldStop = true;
        }
        public class Worker
        {
            // This method will be called when the thread is started.			
            FormLoad formload = new FormLoad();
            public void DoWork()                             // Loading Form
            {

                formload.StartPosition = FormStartPosition.Manual;

                formload.Location = new Point(800, 400);
                formload.ShowDialog();

            }
            public void RequestStop()
            {
                _shouldStop = true;

                formload.Close();

            }
            private volatile bool _shouldStop;
        }
        public TransDataHeader transDataHeader;
        public TransData[] transData;

        private void 운행기록파일열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open File";
            openFileDialog1.Filter = "*.tmf|*.TMF";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
				
                string dirName = openFileDialog1.FileName;
                char[] fileOnly = new char[openFileDialog1.SafeFileName.Length];
               // fileOnly[0] = '\\';
              //  fileOnly[1] = '\\';
                for (int i = 0; i < openFileDialog1.SafeFileName.Length; i++)
                    fileOnly[i] = openFileDialog1.SafeFileName[i];
                dirName = dirName.TrimEnd(fileOnly);

             //   ConvertFunction(openFileDialog1.FileName, dirName);

                 Tempstr = openFileDialog1.FileName;

                 Tempdirname = dirName;

                Thread LoadingThread = new Thread(LoadingWork);
                LoadingThread.Start();
            }
        }

        private void 변환파일열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open File";
            openFileDialog1.Filter = "*.txt|*.TXT";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nDataPageStatus[0] = 1;
                nDataPageStatus[1] = 0;
                strOpenedFileName = openFileDialog1.FileName;
				Gpx_data = true;
                ArtWorkFunction(strOpenedFileName);
            }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            frmGraph2.m_bStart = false;
            frmGraph3.m_bStart = false;
            frmGraph4.m_bStart = false;
            dataForm.m_bStart = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmGraph2.m_bStart = true;
            frmGraph3.m_bStart = true;
            frmGraph4.m_bStart = true;
            dataForm.m_bStart = true;

            foreach (System.Windows.Forms.Form TheForm in this.MdiChildren)
            {
                TheForm.Dispose();
            }

            if (e.Cancel == true)
                e.Cancel = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void horizentalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void mDIAllCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGraph2.Visible = false;
            frmGraph3.Visible = false;
            frmGraph4.Visible = false;
            dataForm.Visible = false;
        }

        private void 가로HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 세로VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void 계단CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            frmGraph2.Visible = false;
            frmGraph3.Visible = false;
            frmGraph4.Visible = false;
            dataForm.Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            운행기록파일열기ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            변환파일열기ToolStripMenuItem_Click(sender, e);
        
		}

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            DlgForm helpDlg = new DlgForm();

            helpDlg.Owner = this;
            helpDlg.ShowDialog();
            */
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
          //  MapForm mapForm = new MapForm();

          //  mapForm.Show();
        }

        private void 변환데이터TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bIsDataFull)
                dataForm.Show();
            else
                MessageBox.Show(this, "데이터가 없습니다", "정보", MessageBoxButtons.OK);
        }

        private void 일일누적주행거리DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bIsDataFull)
                frmGraph4.Show();
            else
				MessageBox.Show(this, "데이터가 없습니다", "정보", MessageBoxButtons.OK);
        }

        private void 속도브레이크신호RPMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bIsDataFull)
                frmGraph3.Show();
            else
				MessageBox.Show(this, "데이터가 없습니다", "정보", MessageBoxButtons.OK);
        }

        private void 가속도AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bIsDataFull)
                frmGraph2.Show();
            else
				MessageBox.Show(this, "데이터가 없습니다", "정보", MessageBoxButtons.OK);
        }
        string Tempstr = "";
        string Tempdirname = "";

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string str1 = "";
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            
             Tempdirname =  Path.GetDirectoryName( files[0]);

             Tempdirname += @"\";

             Tempstr = Path.GetFullPath(files[0]);

             //   DirectoryInfo dir = new DirectoryInfo(path);
               
                if (files != null)
                {
                    string str = files[0].ToLower();
                    if (str.EndsWith(".txt"))
                    {
                        nDataPageStatus[0] = 1;
                        nDataPageStatus[1] = 0;
                        strOpenedFileName = files[0].ToString();
						Gpx_data = true;
                    
                        ArtWorkFunction(strOpenedFileName);
                    }
                    else if (str.EndsWith(".tmf"))
                    {
                      /*  char[] sep = { '\\', '\\' };
                        string dirName = files[0].ToString();
                        string[] nStrFileName = dirName.Split(sep);
                        string fon = "";
                        foreach (string s in nStrFileName)
                            fon = s;
                        char[] fileOnly = new char[fon.Length];
                   
                        for (int i = 0; i < fon.Length; i++)
                            fileOnly[i ] = fon[i];
                        dirName = dirName.TrimEnd(fileOnly);
                        str1 = dirName;

                
                       Tempstr = files[0].ToString();
                        Tempdirname = str1;*/


                        Thread LoadingThread = new Thread(LoadingWork);
                        LoadingThread.Start();


                     //   ConvertFunction(files[0].ToString(), str1);
                     //   ConvertFunction(files[0].ToString(), dirName);
                    }
                    else if (str.EndsWith(".jtf"))
                    {

                        char[] sep = { '\\', '\\' };
                        string dirName = files[0].ToString();
                        string[] nStrFileName = dirName.Split(sep);
                        string fon = "";
                        foreach (string s in nStrFileName)
                            fon = s;
                        char[] fileOnly = new char[fon.Length];
                    //    fileOnly[0] = '\\';
                     //   fileOnly[1] = '\\';
                        for (int i = 0; i < fon.Length; i++)
                            fileOnly[i] = fon[i];
                        dirName = dirName.TrimEnd(fileOnly);
                   
				        Jtf_Convert(files[0].ToString(), dirName);
                    }
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // 파일이 드래그 될 경우에만 Copy 효과
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //< Fucnion:        private void ArtWorkFunction(string strFname)
        //< Input value:    (string)strFname
        //< Return value:   
        //< Documents:      *.TXT 파일을 입력받아 Data 및 Graph로 표현
        private void ArtWorkFunction(string strFname)
        {
			
            int i, arrAddedNum;
			
            uint j = 0;

			bool bIsDriverCodeChanged = false;


            if (toolStripButtonGo.Enabled)
            {
                toolStripButtonGo.Enabled = false;
                toolStripButtonLeftPage.Enabled = false;
                toolStripButtonRightPage.Enabled = false;
                toolStripTextBoxNowPage.Enabled = false;
                toolStripLabelTotalPage.Enabled = false;
            }

            this.Text = "TachoConvert - " + strFname;

            // *.TXT 파일에서 추출할 각 데이터를 담기위한 변수들
            string DayDriveDistance = "",   // 일일 주행 거리
                    TotalDriveDistance = "",// 총 주행 거리
                    InforTime = "",         // 정보 발생 시간
                    InforTimeYY = "",       // 정보 발생 시간 - 년
                    InforTimeMM = "",       // 정보 발생 시간 - 월
                    InforTimeDD = "",       // 정보 발생 시간 - 일
                    InforTimeHour = "",     // 정보 발생 시간 - 시
                    InforTimeMin = "",      // 정보 발생 시간 - 분
                    InforTimeSec = "",      // 정보 발생 시간 - 초
                    InforTimeMSec = "",     // 정보 발생 시간 - 밀리초
                    CarSpeed = "",          // 속도
                    CarRPM = "",            // RPM
                    CarBreak = "",          // 브레이크 신호
                    CarGpsX = "",           // 차량 위치 X 좌표
                    CarGpsY = "",           // 차량 위치 Y 좌표
                    CarDegree = "",         // GPS 데이터의 각도(Degree)
                    CarAccelX = "",         // 가속도 X 값
                    CarAccelY = "";         // 가속도 Y 값

            try
			{
				TextType textCode = TextType.UNICODE_LE;
				byte[] ByteArray = new byte[50];
				#region DetectedTextType	// UniCode(Little/Big Endian), UTF-8, ANSI 등의 Text type을 확인
				{
					FileStream fs = new FileStream(strFname, FileMode.Open, FileAccess.Read);
					BinaryReader br = new BinaryReader(fs);

					byte[] bbb = br.ReadBytes(3);

					if ((bbb[0] == 0xFF) && (bbb[1] == 0xFE))							// Unicode Little Endian
						textCode = TextType.UNICODE_LE;
					else if ((bbb[0] == 0xFE) && (bbb[1] == 0xFF))						// Unicode Big Endian
						textCode = TextType.UNICODE_BE;
					else if ((bbb[0] == 0xEF) && (bbb[1] == 0xBB) && (bbb[2] == 0xBF))	// UTF-8
						textCode = TextType.UTF_8;
					else
						textCode = TextType.ANSI;
				ByteArray =	br.ReadBytes(17);
				ByteArray = br.ReadBytes(17);
				ByteArray = br.ReadBytes(2);
				ByteArray = br.ReadBytes(12);    // 헤더 차 넘버를 가져온다
					br.Close();
					fs.Close();
				}
				#endregion

				System.Text.Encoding euckr = System.Text.Encoding.GetEncoding(51949);


				using (StreamReader sr = new StreamReader(strFname, euckr))
				{


                    	char[] line = new char[68]; // 줄없음 사용시 활성
					char[] lineH = new char[85];
                 //   String line;  
					
				
					int indexOfStartPos = 0;
					long datanum = 0;

					if (sr.BaseStream.Length < 79)	// Header 정보도 다 채워져 있지 않음
					{
						MessageBox.Show(this, "운행기록 기본 정보 부족", "오류");
						return;
					}
					else if (sr.BaseStream.Length < (79 + 68))	// 첫 데이터 정보도 다 채워져 있지 않음
					{
						MessageBox.Show(this, "운행기록 데이터 부족", "오류");
						return;
					}

					if (nDataPageStatus[1] == 0)
					{
                        if ((textCode == TextType.UNICODE_LE) || (textCode == TextType.UNICODE_BE))
                            nDataPageStatus[1] = (uint)(((((uint)(((sr.BaseStream.Length / 2) - 85) / 68)) - 1) / nNumberOfOnePage) + 1);
                        else
                        {
                            //   nDataPageStatus[1] = (uint)(((((uint)((sr.BaseStream.Length - 85) / 68)) - 1) / nNumberOfOnePage) + 1);
                            nDataPageStatus[1] = (uint)(((((uint)((sr.BaseStream.Length + 1 - 79) / 68)) - 1) / nNumberOfOnePage) + 1);



                        }

					}
					//nDataPageStatus[1] = (uint)(((((uint)((sr.BaseStream.Length - 83) / 70)) - 1) / nNumberOfOnePage) + 1);

                  


				/*	uint temp = (uint)(((sr.BaseStream.Length+1 - 79) / 69)) / 10000;   // 18.
					
					temp *= 10000;

					LastPage = (uint)(((sr.BaseStream.Length+1 - 79) / 69))  - temp;*/



                    uint temp = (uint)(((sr.BaseStream.Length + 1 - 79) / 68)) / 10000;

                    temp *= 10000;

                    LastPage = (uint)(((sr.BaseStream.Length + 1 - 79) / 68)) - temp;


					transDataHeader = new TransDataHeader();
					transData = new TransData[nNumberOfOnePage];



					
				
					// 헤더 정보 채우기
					//if ((line = sr.ReadLine()) != null)
					if ((textCode == TextType.UNICODE_BE) || (textCode == TextType.UNICODE_LE) || (textCode == TextType.UTF_8))
					{
						sr.Read(lineH, 0, 76);
						{
							for (i = 0; i < 20; i++)
								transDataHeader.ModelName += lineH[i];   // 모델명
							for (i = 0; i < 17; i++)
								transDataHeader.CarNum += lineH[20 + i]; // 차대번호
							for (i = 0; i < 2; i++)
								transDataHeader.CarType += lineH[20 + 17 + i];   // 자동차 타입
							for (i = 0; i < 9; i++)
								transDataHeader.CarRegistNum += lineH[20 + 17 + 2 + i];  // 자동차 등록번호
							for (i = 0; i < 10; i++)
								transDataHeader.OperatorRegistNum += lineH[20 + 17 + 2 + 9 + i]; // 운송사업자 등록번호
							for (i = 0; i < 18; i++)
								transDataHeader.DriverCode += lineH[20 + 17 + 2 + 9 + 10 + i];   // 운전자 코드
						}
					}
					else
					{
						/*sr.Read(lineH, 0, 79);
						{
							for (i = 0; i < 20; i++)
								transDataHeader.ModelName += lineH[i];   // 모델명
							for (i = 0; i < 17; i++)
								transDataHeader.CarNum += lineH[20 + i]; // 차대번호
							for (i = 0; i < 2; i++)
								transDataHeader.CarType += lineH[20 + 17 + i];   // 자동차 타입


							for (i = 0; i < 12; i++)							
								transDataHeader.CarRegistNum += lineH[20 + 17 + 2 + i];  // 자동차 등록번호	
														
							for (i = 0; i < 10; i++)
								transDataHeader.OperatorRegistNum += lineH[20 + 17 + 2 + 12 + i]; // 운송사업자 등록번호
							for (i = 0; i < 18; i++)
								transDataHeader.DriverCode += lineH[20 + 17 + 2 + 12 + 10 + i];   // 운전자 코드
							
						}*/
                        
						sr.Read(lineH, 0, 76);   // 줄없음 사용시 활성
						{
							for (i = 0; i < 20; i++)
								transDataHeader.ModelName += lineH[i];   // 모델명
							for (i = 0; i < 17; i++)
								transDataHeader.CarNum += lineH[20 + i]; // 차대번호
							for (i = 0; i < 2; i++)
								transDataHeader.CarType += lineH[20 + 17 + i];   // 자동차 타입
							for (i = 0; i < 9; i++)
								transDataHeader.CarRegistNum += lineH[20 + 17 + 2 + i];  // 자동차 등록번호
							for (i = 0; i < 10; i++)
								transDataHeader.OperatorRegistNum += lineH[20 + 17 + 2 + 9 + i]; // 운송사업자 등록번호
							for (i = 0; i < 18; i++)
								transDataHeader.DriverCode += lineH[20 + 17 + 2 + 9 + 10 + i];   // 운전자 코드
						}
                        
                        /*
                        if ((line = sr.ReadLine()) != null)
                        {
                            for (i = 0; i < 20; i++)
                                transDataHeader.ModelName += line[i];
                            for (i = 0; i < 17; i++)
                                transDataHeader.CarNum += line[20 + i];
                            for (i = 0; i < 2; i++)
                                transDataHeader.CarType += line[20 + 17 + i];
                            //for (i = 0; i < 12; i++)
                            for (i = 0; i < 9; i++)
                                transDataHeader.CarRegistNum += line[20 + 17 + 2 + i];
                            for (i = 0; i < 10; i++)
                                transDataHeader.OperatorRegistNum += line[20 + 17 + 2 + 9 + i];
                            for (i = 0; i < 18; i++)
                                transDataHeader.DriverCode += line[20 + 17 + 2 + 9 + 10 + i];
                        }*/
                        PlateNoTemp = transDataHeader.CarRegistNum;
					}

				//	header = lineH;


					/*		byte[] buffer = enc.GetBytes(strCRN); 
								System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
								Encoding littleendian = Encoding.Unicode;
								Encoding bigendian = Encoding.BigEndianUnicode;
								Byte[] ByteArray;
								string str;
								ByteArray = utf8.GetBytes(strCRN);   
								// 우선 UTF8 스트링을 바이트로 쪼개서
								ByteArray = Encoding.Convert(utf8, littleendian, ByteArray);  // 다음 UTF8 바이트 코드를 LittleEndian 바이트 코드로 변환하고 					                                                    // 변환된 바이트 코드를 다시 스트링으로 합치는 거에여..
								strCRN = littleendian.GetString(ByteArray);*/
					//	byte[] buffer = utf8.GetBytes(strCRN);

				
				
				//	 ByteArray = euckr.GetBytes(transDataHeader.CarRegistNum);
				
					//transDataHeader.CarRegistNum = euckr.GetString(ByteArray);  // 차넘버 바이트 -> 스트링 변환(euckr)

					transDataHeader.CarRegistNum = euckr.GetString(ByteArray); 
					
					//indexOfStartPos =  85+ (int)((nDataPageStatus[0] - 1) * nNumberOfOnePage);

                   
				// 선택 page  건너뛰기
                 
                  //  for (long l = 0; l < (long)((nDataPageStatus[0] - 1) * nNumberOfOnePage); l++)
                  //  line = sr.ReadLine();
						for (long l = 0; l < (long)((nDataPageStatus[0] - 1) * nNumberOfOnePage); l++)
						{
                            	sr.Read(line,0, 68); // 줄없음 사용시 활성
                          //  line = sr.ReadLine();
                            
						}

					datanum = (long)((nDataPageStatus[0] - 1) * nNumberOfOnePage);
					

				/*	if(line[65]=='.')
					{
						sr.Read(line, 0, 1);
					}
					if (line[66] == '.')
					{
						sr.Read(line, 0, 2);
					}
					if (line[67] == '.')
					{
						sr.Read(line, 0, 3);
					}
					if (line[64] == '+')
					{
						sr.Read(line, 0, 4);
					}
					if (line[65] == '+')
					{
						sr.Read(line, 0, 5);
					}
					if (line[66] == '+')
					{
						sr.Read(line, 0, 6);
					}
					if (line[67] == '+')
					{
						sr.Read(line, 0, 6);
					}*/
					
					for (j = 0; j < nNumberOfOnePage; j++)
					{
					//	line = null;



                        for (int a = 0; a < 68; a++)
                        {
                            line[a] =' ';
                        }
                       
                      
						bIsDriverCodeChanged = false;

                      //  line = sr.ReadLine();
                   
                         sr.Read(line, 0, 68);   // 줄없음 사용시 활성

                       // if (line == null)
                       //     break;

                         if (line[0] == ' ' && line[1] == ' ')
                         {
                         //    MessageBox.Show("test");
                             break;
                         }


                         if (line.Length != 68)
                         {
                         //    MessageBox.Show("test");
                             continue;
                         }


                        

                        
						for (i = 0; i < 4; i++)
							DayDriveDistance += line[i];
						transData[j].DayDriveDistance = Convert.ToUInt16(DayDriveDistance); // 일일 주행 거리
						arrAddedNum = 4;

						for (i = 0; i < 7; i++)
							TotalDriveDistance += line[arrAddedNum + i];
						transData[j].TotalDriveDistance = Convert.ToUInt32(TotalDriveDistance);   // 총 주행 거리
						arrAddedNum += 7;

                       

                        DayDriveDistance_old = transData[j].DayDriveDistance;
                        TotalDriveDistance_old = transData[j].TotalDriveDistance;





						for (i = 0; i < 14; i++)
							InforTime += line[arrAddedNum + i]; // 정보 발생 일시

						InforTimeYY += InforTime[0]; InforTimeYY += InforTime[1];
						InforTimeMM += InforTime[2]; InforTimeMM += InforTime[3];
						InforTimeDD += InforTime[4]; InforTimeDD += InforTime[5];
						InforTimeHour += InforTime[6]; InforTimeHour += InforTime[7];
						InforTimeMin += InforTime[8]; InforTimeMin += InforTime[9];
						InforTimeSec += InforTime[10]; InforTimeSec += InforTime[11];
						InforTimeMSec += InforTime[12]; InforTimeMSec += InforTime[13];

                        DateTemp = "20" +InforTimeYY + "년" + InforTimeMM +"월" + InforTimeDD +"일";

						transData[j].InforTimeYY = Convert.ToUInt16(InforTimeYY); transData[j].InforTimeYY += 2000; // 정보 발생 일시 - 년
						transData[j].InforTimeMM = Convert.ToUInt16(InforTimeMM);       // 정보 발생 일시 - 월
						transData[j].InforTimeDD = Convert.ToUInt16(InforTimeDD);       // 정보 발생 일시 - 일
						transData[j].InforTimeHour = Convert.ToUInt16(InforTimeHour);   // 정보 발생 일시 - 시
						transData[j].InforTimeMin = Convert.ToUInt16(InforTimeMin);     // 정보 발생 일시 - 분
						transData[j].InforTimeSec = Convert.ToUInt16(InforTimeSec);     // 정보 발생 일시 - 초
						transData[j].InforTimeMSec = Convert.ToUInt16(InforTimeMSec);   // 정보 발생 일시 - 밀리초

						arrAddedNum += 14;

						for (i = 0; i < 3; i++)
							CarSpeed += line[arrAddedNum + i];
						transData[j].CarSpeed = Convert.ToUInt16(CarSpeed); // 속도
						arrAddedNum += 3;

						for (i = 0; i < 4; i++)
							CarRPM += line[arrAddedNum + i];
						transData[j].CarRPM = Convert.ToUInt16(CarRPM);     // RPM
						arrAddedNum += 4;

						for (i = 0; i < 1; i++) 
							CarBreak += line[arrAddedNum + i];
						if (CarBreak == "1")
							transData[j].CarBreak = true;   // 브레이크 신호 - 있음
						else
							transData[j].CarBreak = false;  // 브레이크 신호 - 없음
						arrAddedNum += 1;

						if (CarBreak == "2")
							bIsDriverCodeChanged = true;

						if (bIsDriverCodeChanged)
						{
							transDataHeader.DriverCode += " / " + (j+1).ToString();

							for (i = 0; i < 9; i++)
							{
								CarGpsX += line[arrAddedNum + i];
							}
							transData[j].CarGpsX = Convert.ToDouble(CarGpsX);   // GPS X 좌료
							arrAddedNum += 9;

							for (i = 0; i < 9; i++)
							{
								CarGpsY += line[arrAddedNum + i];
							}
							transData[j].CarGpsY = Convert.ToDouble(CarGpsY);   // GPS Y 좌표
							arrAddedNum += 9;

							transData[j].CarDegree = 0;
							arrAddedNum += 3;
						}
						else
						{
							for (i = 0; i < 9; i++)
							{
								CarGpsX += line[arrAddedNum + i];
								if (i == 2)
									CarGpsX += ".";
							}
							transData[j].CarGpsX = Convert.ToDouble(CarGpsX);   // GPS X 좌료
							arrAddedNum += 9;

							for (i = 0; i < 9; i++)
							{
								CarGpsY += line[arrAddedNum + i];
								if (i == 2)
									CarGpsY += ".";
							}
							transData[j].CarGpsY = Convert.ToDouble(CarGpsY);   // GPS Y 좌표
							arrAddedNum += 9;

							for (i = 0; i < 3; i++)
								CarDegree += line[arrAddedNum + i];
							transData[j].CarDegree = Convert.ToUInt16(CarDegree);   // GPS Degree
							arrAddedNum += 3;
						}

						for (i = 0; i < 6; i++)
							CarAccelX += line[arrAddedNum + i];
						transData[j].CarAccelX = Convert.ToDouble(CarAccelX);   // 가속도 X 값
						arrAddedNum += 6;

						for (i = 0; i < 6; i++)
							CarAccelY += line[arrAddedNum + i];
						transData[j].CarAccelY = Convert.ToDouble(CarAccelY);   // 가속도 Y 값
						arrAddedNum += 6;

						if (bIsDriverCodeChanged)
						{
							transData[j].MachineStatus = "XX";
						}
						else
						{
							for (i = 0; i < 2; i++)
								transData[j].MachineStatus += line[arrAddedNum + i];    // 기계 상태
						}

						// Initialized Temporary value
						DayDriveDistance = TotalDriveDistance = InforTime = InforTimeYY = InforTimeMM =
						InforTimeDD = InforTimeHour = InforTimeMin = InforTimeSec = InforTimeMSec =
						CarSpeed = CarRPM = CarBreak = CarGpsX = CarGpsY = CarDegree = CarAccelX = CarAccelY = "";

						indexOfStartPos += 68;

					if ((textCode == TextType.UNICODE_LE) || (textCode == TextType.UNICODE_BE))
						{
							if ((sr.BaseStream.Length / 2) <= indexOfStartPos)
								break;
						}
						else
						{
							if (sr
								.BaseStream.Length <= indexOfStartPos)
								break;
						}
					}
				}
			}
            catch (IndexOutOfRangeException)
            {
                // 로드한 *.TXT 파일의 길이가 계산된 데이터 길이와 맞지 않는 경우
				MessageBox.Show(this, "불러온 파일이 형식에 맞지 않습니다", "에러");
            }
            catch (Exception e)
            {
                // 기타 오류 처리
			//	MessageBox.Show(this, "알 수 없는 파일 손상", "에러");
				MessageBox.Show(e.Message);
            }

            bool is1sData = false;

            // 최초 4개의 '초'데이터 값이 서로 다 다들 경우 '1초 데이터'로 설정
            if ((transData[0].InforTimeSec != transData[1].InforTimeSec)
                && (transData[1].InforTimeSec != transData[2].InforTimeSec)
                && (transData[2].InforTimeSec != transData[3].InforTimeSec))
                is1sData = true;
            else
                is1sData = false;

            // form2.Copy(구조체)
            frmGraph2.SetStruct(j, is1sData);
            frmGraph2.MdiParent = this;

            frmGraph3.SetStruct(j, is1sData);
            frmGraph3.MdiParent = this;

            frmGraph4.SetStruct(j, is1sData);
            frmGraph4.MdiParent = this;

            //dataForm.SetStruct(transDataHeader, transData, j/*nNumberOfOnePage*/, nDataPageStatus[0], nNumberOfOnePage, is1sData);

			if (LastPage_check == true)
			{
				dataForm.SetStruct(LastPage, nDataPageStatus[0], nNumberOfOnePage, is1sData);
				dataForm.MdiParent = this;
				LastPage_check = false;
			}
			else
			{
				dataForm.SetStruct(j, nDataPageStatus[0], nNumberOfOnePage, is1sData);
				dataForm.MdiParent = this;
			}


			Data_Length = j;

            frmGraph2.Show();
            frmGraph3.Show();
            frmGraph4.Show();
            dataForm.Show();

            bIsDataFull = true;

            toolStripLabelTotalPage.Text = String.Format("{0} Page", nDataPageStatus[1]);
            toolStripTextBoxNowPage.Text = Convert.ToString(nDataPageStatus[0]);

            if (!toolStripButtonGo.Enabled)
            {
                toolStripButtonGo.Enabled = true;
                toolStripButtonLeftPage.Enabled = true;
                toolStripButtonRightPage.Enabled = true;
                toolStripTextBoxNowPage.Enabled = true;
                toolStripLabelTotalPage.Enabled = true;
            }
        }
/// <summary>
/// jtf -> tmf 
/// </summary>
/// <param name="strFname"></param>
/// <param name="dirName"></param>
		public bool IsLeapYear(byte nYear)
		{
			int year;

			year = nYear + 2000;
			if ((year % 4) != 0) return false;
			if ((year % 100) != 0) return true;
			return (year % 400 == 0);
		}

		public void Jtf_Convert(string strFname, string dirName)
		{
			string saveFname;   // *.TXT 파일의 파일명
			string saveFname2;	// Temporary TXT 파일명
			byte cs=0, csCal=0;     // cs: 파일로부터 읽어들인 CheckSum Data, csCal: 프로그램이 계산한 CheckSum Data
			int i = 0;
			int nDataNum;

			bool bIs1sData = false;
			int nMakedFileNum = 0;

			// *.TXT 파일명 결정용 변수
			int saveYYMMDD = 100101;
			string saveCarNum = "서울01가9876";
	

			bool bIsDriverCodeChanged = false;
			bool bIsSaveDataTrue = false;

		    string tmf = ".TMF";
          //  string newPath = "c:\\0.01초 데이터\\";
            string newPath = dirName;

            System.IO.Directory.CreateDirectory(newPath);
			string strFname2 = Path.GetFileNameWithoutExtension(strFname);

            string CurTIme =  string.Format("_{0:d2}{1:d2}{2:D2}{3:D2}{4:D2}",DateTime.Now.Month,DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            strFname2 = strFname2 + CurTIme + tmf;
            strFname2 = newPath + strFname2;
          


			DriverCodeTemp.Clear();

			FileStream fs1 = new FileStream(strFname, FileMode.Open, FileAccess.Read);
			BinaryReader br1 = new BinaryReader(fs1);

			JTF_Header jtfHeader = new JTF_Header();
			JTF_RecordA jtf_RecordA = new JTF_RecordA();
			JTF_RecordB jtf_RecordB = new JTF_RecordB();
			JTF_RecordA_BAck jtf_RecordA_Back = new JTF_RecordA_BAck();
			JTF_RecordB_Back jtf_RecordB_BAck = new JTF_RecordB_Back();
			JTF_Time jtf_time = new JTF_Time();

			Convert_Time convert_time = new Convert_Time();

			bool timecheck = false;

			int test_count = 1;

			FileStream stream = new FileStream(strFname2, FileMode.Create);
			BinaryWriter writer = new BinaryWriter(stream);
			do
			{
				test_count++;

				int distDailly_count = 0;
				int distAccum_count = 0;
				int count = 0;
				int rpm_count = 0;
				int gpsY_count = 0;
				int gpsX_count = 0;
				int degree_count = 0;
				int status_count = 0;
				int speed_count = 0;


				int distDailly_count1 = 0;
				int distAccum_count1 = 0;

				int rpm_count1 = 0;
				int gpsY_count1 = 0;
				int gpsX_count1 = 0;
				int degree_count1 = 0;
				int status_count1 = 0;
				int speed_count1 = 0;
				
				#region Read & store header information from Input file
				{
					try
					{

						
						/////////////////////////////////////////////////////
						// Read & store header information from Input file
						jtfHeader.TachoTransTime = br1.ReadBytes(4);
						if (jtfHeader.TachoTransTime[0] == 0xff && jtfHeader.TachoTransTime[1] == 0xff && jtfHeader.TachoTransTime[2] == 0xff && jtfHeader.TachoTransTime[3] == 0x14)
						{
							MessageBox.Show(strFname2,"완료");
							br1.Close();
							fs1.Close();
							stream.Close();
							writer.Close();
							return;
						}
						jtfHeader.TachoDataSize = br1.ReadBytes(3);
						jtfHeader.ModelName = br1.ReadBytes(10);
						jtfHeader.CarNum = br1.ReadBytes(17);
						jtfHeader.CarType = br1.ReadByte();
						jtfHeader.CarRegistNum = br1.ReadBytes(6);
						jtfHeader.OperatorRegistNum = br1.ReadBytes(5);
						jtfHeader.DriverCode = br1.ReadBytes(9);
						//cs = br.ReadByte();
						br1.ReadByte();

						/////////////////////////////////////////////////////
						jtf_RecordB_BAck.status_temp = br1.ReadBytes(2000);
						jtf_RecordB_BAck.longitude_temp = br1.ReadBytes(40);
						jtf_RecordB_BAck.reserved1 = br1.ReadBytes(4);
						jtf_RecordB_BAck.reserved2 = br1.ReadBytes(4);
						jtf_RecordB_BAck.ibArea = br1.ReadBytes(4);
						jtf_RecordB_BAck.latitude_temp = br1.ReadBytes(40);
						jtf_RecordB_BAck.azimuth_temp = br1.ReadBytes(20);
						/////////////////////////////////////////////////////
						jtf_RecordA_Back.accelX_temp = br1.ReadBytes(1000);
						jtf_RecordA_Back.accelY_temp = br1.ReadBytes(1000);
						jtf_RecordA_Back.distDaily_temp = br1.ReadBytes(20);
						jtf_RecordA_Back.rpm_temp = br1.ReadBytes(20);
						jtf_RecordA_Back.reserved1 = br1.ReadBytes(4);
						jtf_RecordA_Back.reserved2 = br1.ReadBytes(4);
						jtf_RecordA_Back.ibArea = br1.ReadBytes(4);
						jtf_RecordA_Back.reserved3 = br1.ReadBytes(4);
						jtf_RecordA_Back.speed_temp= br1.ReadBytes(10);
						jtf_RecordA_Back.distAccum_temp = br1.ReadBytes(30);
						jtf_RecordA_Back.msIdx = br1.ReadBytes(2);
						jtf_RecordA_Back.scIdx = br1.ReadBytes(2);
						jtf_RecordA_Back.msec = br1.ReadByte();
						jtf_RecordA_Back.mstime = br1.ReadBytes(6);
						jtf_RecordA_Back.dummy = br1.ReadBytes(2);
						//jtf_RecordA_Back.chsum = br.ReadByte();
						br1.ReadByte();

						br1.ReadBytes(2);

						/////////////////////////////////////////////////////
						jtf_RecordB.status_temp = br1.ReadBytes(2000);
						jtf_RecordB.longitude_temp = br1.ReadBytes(40);
						jtf_RecordB.reserved1 = br1.ReadBytes(4);
						jtf_RecordB.reserved2 = br1.ReadBytes(4);
						jtf_RecordB.ibArea = br1.ReadBytes(4);
						jtf_RecordB.latitude_temp = br1.ReadBytes(40);
						jtf_RecordB.azimuth_temp = br1.ReadBytes(20);
						/////////////////////////////////////////////////////
						jtf_RecordA.accelX_temp = br1.ReadBytes(1000);
						jtf_RecordA.accelY_temp = br1.ReadBytes(1000);
						jtf_RecordA.distDaily_temp = br1.ReadBytes(20);
						jtf_RecordA.rpm_temp = br1.ReadBytes(20);
						jtf_RecordA.reserved1 = br1.ReadBytes(4);
						jtf_RecordA.reserved2 = br1.ReadBytes(4);
						jtf_RecordA.ibArea = br1.ReadBytes(4);
						jtf_RecordA.reserved3 = br1.ReadBytes(4);
						jtf_RecordA.speed_temp = br1.ReadBytes(10);
						jtf_RecordA.distAccum_temp = br1.ReadBytes(30);
						jtf_RecordA.msIdx = br1.ReadBytes(2);
						jtf_RecordA.scIdx = br1.ReadBytes(2);
						jtf_RecordA.msec = br1.ReadByte();
						jtf_RecordA.mstime = br1.ReadBytes(6);
						jtf_RecordA.dummy = br1.ReadBytes(2);
					//	jtf_RecordA.chsum = br.ReadByte();
						br1.ReadByte();
						/////////////////////////////////////////////////////
						br1.ReadBytes(2);

						/////////////////////////////////////////////////////
						#region Time
						// time //
					//	if (timecheck == false)
					//	{
						

						/*  2011.05.20 추가
						 * 
						 *  msec 0x63 이상시 체크  mnsec == 50ms를 쓴다
						 * 
						*/

						if (jtf_RecordA_Back.msec > 0x63)
						{

							jtf_RecordA_Back.msec = 0x32;
						}


							jtf_time.time = jtf_RecordA_Back.mstime;
							jtf_time.Msec = jtf_RecordA_Back.msec;
							jtf_time.mmsec = jtf_time.Msec;				//save
							timecheck = true;
					//	}

						jtf_time.Year = jtf_time.time[5];
						jtf_time.Month = jtf_time.time[4];
						jtf_time.Dayy = jtf_time.time[3];
						jtf_time.Hour = jtf_time.time[2];
						jtf_time.Minute = jtf_time.time[1];
						jtf_time.Sec = jtf_time.time[0];

						if(jtf_time.Year==0x00 || jtf_time.Month==0x00 || jtf_time.Dayy ==0x00)
						{
							return;
						}
						if(jtf_time.Hour > 0x17 || jtf_time.Minute > 0x3b || jtf_time.Sec > 0x3b)
						{
							return;
						}




						byte[] Year = new byte[2001];
						byte[] Month = new byte[2001];
						byte[] Dayy = new byte[2001];
						byte[] Hour = new byte[2001];
						byte[] Minute = new byte[2001];
						byte[] Sec = new byte[2001];
						byte[] mSec = new byte[2001];
								
						for(int a=999; a>=0; a--)
						{
							--jtf_time.Msec;
							if(jtf_time.Msec == 0xff)
							{
								jtf_time.Msec = 99;
								--jtf_time.Sec;

								if(jtf_time.Sec ==0xff)
								{
									jtf_time.Sec = 59;
									--jtf_time.Minute;
									if(jtf_time.Minute==0xff)
									{
										jtf_time.Minute =59;

										--jtf_time.Hour;
										if(jtf_time.Hour==0xff)
										{
											jtf_time.Hour=23;
											jtf_time.Dayy--;

											

											switch (jtf_time.Month)
											{
													
												case 3:
													if (IsLeapYear( jtf_time.Year))												   
													{
														jtf_time.Month--;
														jtf_time.Dayy = 29;
													}
													else
													{
														jtf_time.Month--;
														jtf_time.Dayy = 28;
													}
													break;

												case 5:
												case 7:
												case 10:
												case 12:
													jtf_time.Month--;
													jtf_time.Dayy = 30;
													break;

												default:
													jtf_time.Month--;
													jtf_time.Dayy = 31;
													if (jtf_time.Month == 0)
													{
														jtf_time.Month = 12;
														jtf_time.Year--;
														if (jtf_time.Year == 0xFF)
														{
															jtf_time.Year= 99;
														}
													}
													break;
											}
											

										}
									}							 

								}
							}
							Year[a] = jtf_time.Year;
							Month[a] = jtf_time.Month;
							Dayy[a] = jtf_time.Dayy;
							Hour[a] = jtf_time.Hour;
							Minute[a] = jtf_time.Minute;
							Sec[a] = jtf_time.Sec;
							mSec[a] = jtf_time.Msec;

						
						}
						//////////////////////////////

					//	jtf_time.time = jtf_RecordA_Back.mstime;

						jtf_time.Msec = jtf_time.mmsec;

						jtf_time.Year = jtf_time.time[5];
						jtf_time.Month = jtf_time.time[4];
						jtf_time.Dayy = jtf_time.time[3];
						jtf_time.Hour = jtf_time.time[2];
						jtf_time.Minute = jtf_time.time[1];
						jtf_time.Sec = jtf_time.time[0];

						Year[1000] = jtf_time.Year;
						Month[1000] = jtf_time.Month;
						Dayy[1000] = jtf_time.Dayy;
						Hour[1000] = jtf_time.Hour;
						Minute[1000] = jtf_time.Minute;
						Sec[1000] = jtf_time.Sec;
						mSec[1000] = jtf_time.Msec;


						

						for (int a = 1001; a<= 2000; a++)
						{
							++jtf_time.Msec;
							if (jtf_time.Msec > 99)
							{
								jtf_time.Msec = 0;
								jtf_time.Sec++;

								if (jtf_time.Sec > 59)
								{
									jtf_time.Sec = 0;
									
									jtf_time.Minute++;
									if (jtf_time.Minute > 59)
									{
										jtf_time.Minute = 0;

										jtf_time.Hour++;
										if (jtf_time.Hour > 23)
										{
											jtf_time.Hour = 0;
											jtf_time.Dayy++;
											switch (jtf_time.Month)
											{
												case 2:
													if (IsLeapYear(jtf_time.Year))
													{
														if (jtf_time.Dayy > 29)
														{
															jtf_time.Dayy = 1;
															jtf_time.Month++;
														}
													}
													else
													{
														if (jtf_time.Dayy > 28)
														{
															jtf_time.Dayy = 1;
															jtf_time.Month++;
														}
													}
													break;

												case 4:
												case 6:
												case 9:
												case 11:
													if (jtf_time.Dayy > 30)
													{
														jtf_time.Dayy = 1;
														jtf_time.Month++;
													}
													break;

												default:
													if (jtf_time.Dayy > 31)
													{
														jtf_time.Dayy = 1;
														jtf_time.Month++;

														if (jtf_time.Month > 12)
														{
															jtf_time.Month = 1;
															jtf_time.Year++;

															if (jtf_time.Year > 99)
															{
																jtf_time.Year = 0;
															}
														}
													}
													break;
											}

										}
									}

								}
							}
							Year[a] = jtf_time.Year;
							Month[a] = jtf_time.Month;
							Dayy[a] = jtf_time.Dayy;
							Hour[a] = jtf_time.Hour;
							Minute[a] = jtf_time.Minute;
							Sec[a] = jtf_time.Sec;
							mSec[a] = jtf_time.Msec;

						}


						#endregion

						/////////////////////////////////////////////////////
						
						int arrNum = (int)((jtfHeader.TachoDataSize[2] << 16) | (jtfHeader.TachoDataSize[1] << 8) | jtfHeader.TachoDataSize[0]);

						if (arrNum == 0) //YJK TachoDataSize 가 0이면 전체 파일 읽어오기 임. 
						{
							arrNum = (int)(fs1.Length - 0x38); // 파일 크기에서 헤더 사이즈 뺀다. 
						}

						arrNum /= 32;
					
						writer.Write(jtfHeader.TachoTransTime);
						writer.Write(jtfHeader.TachoDataSize);
						writer.Write(jtfHeader.ModelName);
						writer.Write(jtfHeader.CarNum);
						writer.Write(jtfHeader.CarType);
						writer.Write(jtfHeader.CarRegistNum);
						writer.Write(jtfHeader.OperatorRegistNum);
						writer.Write(jtfHeader.DriverCode);
					    writer.Write(cs);
						
						////////////////////////// Index Sort //////////////////////
#region  First Sort

						UInt16 Index = (UInt16)((jtf_RecordA_Back.msIdx[1] << 8) | (jtf_RecordA_Back.msIdx[0]));   //1000 use


					//	jtf_RecordB_BAck.status = jtf_RecordB_BAck.status_temp;

						int temp_count = Index * 2;
						int temp_count1 = 0;
						int count_status = 0;
						byte[] Temp_Status = new byte[2000];
						byte[] Temp_accelY = new byte[1000];
						byte[] Temp_accelX = new byte[1000];
								

						bool count_chk = false;


							for (int u = 0; u < 1000; u++)  /// status sort
							{

								for (int a = 0; a < 2; a++) //2byte  status
								{

									if (u < (1000 - Index))
									{
										count_chk = false;
										Temp_Status[a + count_status] = jtf_RecordB_BAck.status_temp[temp_count + a];
									}
									else
									{
										count_chk = true;
										Temp_Status[a + (u*2)] = jtf_RecordB_BAck.status_temp[temp_count1 + a];
									}


									if (a == 1)
									{
										if (count_status < 1997)
											count_status += 2;
										if(count_chk == false)
										temp_count += 2;
										else
										temp_count1 += 2;
									}

								}
								if (u < (1000 - Index))
								{

									Temp_accelX[u] = jtf_RecordA_Back.accelX_temp[u + Index];
									Temp_accelY[u] = jtf_RecordA_Back.accelY_temp[u + Index];
								}
								else
								{
									Temp_accelX[u] = jtf_RecordA_Back.accelX_temp[u - (1000 - Index)];
									Temp_accelY[u] = jtf_RecordA_Back.accelY_temp[u - (1000 - Index)];
								}

							}
						jtf_RecordB_BAck.status = Temp_Status;
						jtf_RecordA_Back.accelX = Temp_accelX;
						jtf_RecordA_Back.accelY = Temp_accelY;
						////

						temp_count =0;
						temp_count1 =0;
						
					
						int Index_10use = 10-(Index/100)-1;
						byte[] Temp_gpsY = new byte[40];
						temp_count = (10 - Index_10use) * 4;
						temp_count1 = Index_10use*4;
					

						for (int a = 0; a < 40; a++ )   //logitude
						{

							if (a < Index_10use * 4)
							{
								Temp_gpsY[a] = jtf_RecordB_BAck.longitude_temp[temp_count + a];
							}
							else
							{
								Temp_gpsY[a] = jtf_RecordB_BAck.longitude_temp[a - temp_count1];
							}
								
							
						}
						jtf_RecordB_BAck.longitude = Temp_gpsY;


						////

						temp_count = 0;
						temp_count1 = 0;


						 Index_10use = 10 - (Index / 100) - 1;
						byte[] Temp_gpsX = new byte[40];
						temp_count = (10 - Index_10use) * 4;
						temp_count1 = Index_10use * 4;


						for (int a = 0; a < 40; a++)   //Latitude
						{

							if (a < Index_10use * 4)
							{
								Temp_gpsX[a] = jtf_RecordB_BAck.latitude_temp[temp_count + a];
							}
							else
							{
								Temp_gpsX[a] = jtf_RecordB_BAck.latitude_temp[a - temp_count1];
							}


						}
						jtf_RecordB_BAck.latitude = Temp_gpsX;

						/////////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100) - 1;
						byte[] Temp_azimuth = new byte[20];
						byte[] Temp_distDaily = new byte[20];
						byte[] Temp_rpm = new byte[20];
						temp_count = (10 - Index_10use) * 2;
						temp_count1 = Index_10use * 2;


						for (int a = 0; a < 20; a++)   //azimuth  //disDailly // rpm
						{

							if (a < Index_10use * 2)
							{
								Temp_azimuth[a] = jtf_RecordB_BAck.azimuth_temp[temp_count + a];
								Temp_distDaily[a] = jtf_RecordA_Back.distDaily_temp[temp_count+a];
								Temp_rpm[a] = jtf_RecordA_Back.rpm_temp[temp_count + a];
							}
							else
							{
								Temp_azimuth[a] = jtf_RecordB_BAck.azimuth_temp[a - temp_count1];
								Temp_distDaily[a] = jtf_RecordA_Back.distDaily_temp[a - temp_count1];
								Temp_rpm[a] = jtf_RecordA_Back.rpm_temp[a - temp_count1];
							}


						}
						jtf_RecordB_BAck.azimuth = Temp_azimuth;
						jtf_RecordA_Back.distDaily = Temp_distDaily;
						jtf_RecordA_Back.rpm = Temp_rpm;

						////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100) - 1;				
						byte[] Temp_speed = new byte[10];
						temp_count = (10 - Index_10use) ;
						temp_count1 = Index_10use ;

						for (int a = 0; a < 10; a++)   // speed
						{

							if (a < Index_10use)
							{
								Temp_speed[a] = jtf_RecordA_Back.speed_temp[temp_count + a];
								
							}
							else
							{
								Temp_speed[a] = jtf_RecordA_Back.speed_temp[a - temp_count1];
								
							}


						}
						jtf_RecordA_Back.speed = Temp_speed;

						////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100) - 1;
						
						byte[] Temp_distAccum = new byte[30];
						temp_count = (10 - Index_10use) * 3;
						temp_count1 = Index_10use * 3;


						for (int a = 0; a < 30; a++)   //distAccum
						{

							if (a < Index_10use * 3)
							{

								Temp_distAccum[a] = jtf_RecordA_Back.distAccum_temp[temp_count + a];
							}
							else
							{

								Temp_distAccum[a] = jtf_RecordA_Back.distAccum_temp[a - temp_count1];
							}


						}
						jtf_RecordA_Back.distAccum = Temp_distAccum;
#endregion
						//////////////////////////   test  /////////////////////////////
#region Frist block 100 test


				/*		Index = 100;
						 temp_count = Index * 2;
						 temp_count1 = 0;
						 count_status = 0;
						


						count_chk = false;


						for (int u = 0; u < 1000; u++)  /// status sort
						{

							for (int a = 0; a < 2; a++) //2byte  status
							{

								if (u < (1000 - Index))
								{
									count_chk = false;
									Temp_Status[a + count_status] = jtf_RecordB_BAck.status_temp[temp_count + a];
								}
								else
								{
									count_chk = true;
									Temp_Status[a + (u * 2)] = jtf_RecordB_BAck.status_temp[temp_count1 + a];
								}


								if (a == 1)
								{
									if (count_status < 1997)
										count_status += 2;
									if (count_chk == false)
										temp_count += 2;
									else
										temp_count1 += 2;
								}

							}
							if (u < (1000 - Index))
							{

								Temp_accelX[u] = jtf_RecordA_Back.accelX_temp[u + Index];
								Temp_accelY[u] = jtf_RecordA_Back.accelY_temp[u + Index];
							}
							else
							{
								Temp_accelX[u] = jtf_RecordA_Back.accelX_temp[u - (1000 - Index)];
								Temp_accelY[u] = jtf_RecordA_Back.accelY_temp[u - (1000 - Index)];
							}

						}
						jtf_RecordB_BAck.status = Temp_Status;
						jtf_RecordA_Back.accelX = Temp_accelX;
						jtf_RecordA_Back.accelY = Temp_accelY;
						////

						temp_count = 0;
						temp_count1 = 0;


						 Index_10use = 10 - (Index / 100) ;
						
						temp_count = (10 - Index_10use) * 4;
						temp_count1 = Index_10use * 4;


						for (int a = 0; a < 40; a++)   //logitude
						{

							if (a < Index_10use * 4)
							{
								Temp_gpsY[a] = jtf_RecordB_BAck.longitude_temp[temp_count + a];
							}
							else
							{
								Temp_gpsY[a] = jtf_RecordB_BAck.longitude_temp[a - temp_count1];
							}


						}
						jtf_RecordB_BAck.longitude = Temp_gpsY;


						////

						temp_count = 0;
						temp_count1 = 0;


						Index_10use = 10 - (Index / 100) ;
						
						temp_count = (10 - Index_10use) * 4;
						temp_count1 = Index_10use * 4;


						for (int a = 0; a < 40; a++)   //Latitude
						{

							if (a < Index_10use * 4)
							{
								Temp_gpsX[a] = jtf_RecordB_BAck.latitude_temp[temp_count + a];
							}
							else
							{
								Temp_gpsX[a] = jtf_RecordB_BAck.latitude_temp[a - temp_count1];
							}


						}
						jtf_RecordB_BAck.latitude = Temp_gpsX;

						/////////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100);
					
						temp_count = (10 - Index_10use) * 2;
						temp_count1 = Index_10use * 2;


						for (int a = 0; a < 20; a++)   //azimuth  //disDailly // rpm
						{

							if (a < Index_10use * 2)
							{
								Temp_azimuth[a] = jtf_RecordB_BAck.azimuth_temp[temp_count + a];
								Temp_distDaily[a] = jtf_RecordA_Back.distDaily_temp[temp_count + a];
								Temp_rpm[a] = jtf_RecordA_Back.rpm_temp[temp_count + a];
							}
							else
							{
								Temp_azimuth[a] = jtf_RecordB_BAck.azimuth_temp[a - temp_count1];
								Temp_distDaily[a] = jtf_RecordA_Back.distDaily_temp[a - temp_count1];
								Temp_rpm[a] = jtf_RecordA_Back.rpm_temp[a - temp_count1];
							}


						}
						jtf_RecordB_BAck.azimuth = Temp_azimuth;
						jtf_RecordA_Back.distDaily = Temp_distDaily;
						jtf_RecordA_Back.rpm = Temp_rpm;

						////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100);
					
						temp_count = (10 - Index_10use);
						temp_count1 = Index_10use;

						for (int a = 0; a < 10; a++)   // speed
						{

							if (a < Index_10use)
							{
								Temp_speed[a] = jtf_RecordA_Back.speed_temp[temp_count + a];

							}
							else
							{
								Temp_speed[a] = jtf_RecordA_Back.speed_temp[a - temp_count1];

							}


						}
						jtf_RecordA_Back.speed = Temp_speed;

						////
						temp_count = 0;
						temp_count1 = 0;
						Index_10use = 10 - (Index / 100);

					
						temp_count = (10 - Index_10use) * 3;
						temp_count1 = Index_10use * 3;


						for (int a = 0; a < 30; a++)   //distAccum
						{

							if (a < Index_10use * 3)
							{

								Temp_distAccum[a] = jtf_RecordA_Back.distAccum_temp[temp_count + a];
							}
							else
							{

								Temp_distAccum[a] = jtf_RecordA_Back.distAccum_temp[a - temp_count1];
							}


						}
						jtf_RecordA_Back.distAccum = Temp_distAccum;*/
#endregion
						/////////////////////////////////////////////////////////////////////

						/////////////////////////////////////////////////////   Second_Block ////////////////////////////////////
						#region   Second_Block
						

						UInt16 Index1 = 100;   //1000 use


						
						int temp_count2 = Index1 * 2;
						int temp_count3 = 0;
						int count_status1 = 0;
						byte[] Temp_Status1 = new byte[2000];
						byte[] Temp_accelY1 = new byte[1000];
						byte[] Temp_accelX1 = new byte[1000];


						bool count_chk1 = false;

							for (int u = 0; u < 1000; u++)  /// status sort
							{

								for (int a = 0; a < 2; a++) //2byte  status
								{

									if (u < (1000 - Index1))
									{
										count_chk1 = false;
										Temp_Status1[a + count_status1] = jtf_RecordB.status_temp[temp_count2 + a];
									}
									else
									{
										count_chk1 = true;
										Temp_Status1[a + (u * 2)] = jtf_RecordB.status_temp[temp_count3 + a];
									}


									if (a == 1)
									{
										if (count_status1 < 1997)
											count_status1 += 2;
										if (count_chk1 == false)
											temp_count2 += 2;
										else
											temp_count3 += 2;
									}

								}
								if (u < (1000 - Index1))
								{

									Temp_accelX1[u] = jtf_RecordA.accelX_temp[u + Index1];
									Temp_accelY1[u] = jtf_RecordA.accelY_temp[u + Index1];
								}
								else
								{
									Temp_accelX1[u] = jtf_RecordA_Back.accelX_temp[u - (1000 - Index1)];
									Temp_accelY1[u] = jtf_RecordA_Back.accelY_temp[u - (1000 - Index1)];
								}

							}
						jtf_RecordB.status = Temp_Status1;
						jtf_RecordA.accelX = Temp_accelX1;
						jtf_RecordA.accelY = Temp_accelY1;
						////

						temp_count2 = 0;
						temp_count3 = 0;


						int Index_10use1 = 10 - (Index1 / 100) ;
						byte[] Temp_gpsY1 = new byte[40];
						temp_count2 = (10 - Index_10use1) * 4;
						temp_count3 = Index_10use1 * 4;


						for (int a = 0; a < 40; a++)   //logitude
						{

							if (a < Index_10use1 * 4)
							{
								Temp_gpsY1[a] = jtf_RecordB.longitude_temp[temp_count2 + a];
							}
							else
							{
								Temp_gpsY1[a] = jtf_RecordB.longitude_temp[a - temp_count3];
							}


						}
						jtf_RecordB.longitude = Temp_gpsY1;


						////

						temp_count2 = 0;
						temp_count3 = 0;


						Index_10use1 = 10 - (Index1 / 100) ;
						byte[] Temp_gpsX1 = new byte[40];
						temp_count2 = (10 - Index_10use1) * 4;
						temp_count3 = Index_10use1 * 4;


						for (int a = 0; a < 40; a++)   //Latitude
						{

							if (a < Index_10use1 * 4)
							{
								Temp_gpsX1[a] = jtf_RecordB.latitude_temp[temp_count2 + a];
							}
							else
							{
								Temp_gpsX1[a] = jtf_RecordB.latitude_temp[a - temp_count3];
							}


						}
						jtf_RecordB.latitude = Temp_gpsX1;

						/////////
						temp_count2 = 0;
						temp_count3 = 0;
						Index_10use1 = 10 - (Index1 / 100);
						byte[] Temp_azimuth1 = new byte[20];
						byte[] Temp_distDaily1 = new byte[20];
						byte[] Temp_rpm1 = new byte[20];
						temp_count2 = (10 - Index_10use1) * 2;
						temp_count3 = Index_10use1 * 2;


						for (int a = 0; a < 20; a++)   //azimuth  //disDailly // rpm
						{

							if (a < Index_10use1 * 2)
							{
								Temp_azimuth1[a] = jtf_RecordB.azimuth_temp[temp_count2 + a];
								Temp_distDaily1[a] = jtf_RecordA.distDaily_temp[temp_count2 + a];
								Temp_rpm1[a] = jtf_RecordA.rpm_temp[temp_count2 + a];
							}
							else
							{
								Temp_azimuth1[a] = jtf_RecordB.azimuth_temp[a - temp_count3];
								Temp_distDaily1[a] = jtf_RecordA.distDaily_temp[a - temp_count3];
								Temp_rpm1[a] = jtf_RecordA.rpm_temp[a - temp_count3];
							}


						}
						jtf_RecordB.azimuth = Temp_azimuth1;
						jtf_RecordA.distDaily = Temp_distDaily1;
						jtf_RecordA.rpm = Temp_rpm1;

						////
						temp_count2 = 0;
						temp_count3 = 0;
						Index_10use1 = 10 - (Index1 / 100);
						byte[] Temp_speed1 = new byte[10];
						temp_count2 = (10 - Index_10use1);
						temp_count3 = Index_10use1;

						for (int a = 0; a < 10; a++)   // speed
						{

							if (a < Index_10use1)
							{
								Temp_speed1[a] = jtf_RecordA.speed_temp[temp_count2 + a];

							}
							else
							{
								Temp_speed1[a] = jtf_RecordA.speed_temp[a - temp_count3];

							}


						}
						jtf_RecordA.speed = Temp_speed1;

						////
						temp_count2 = 0;
						temp_count3 = 0;
						Index_10use1 = 10 - (Index1 / 100);

						byte[] Temp_distAccum1 = new byte[30];
						temp_count2 = (10 - Index_10use1) * 3;
						temp_count3 = Index_10use1 * 3;


						for (int a = 0; a < 30; a++)   //distAccum
						{

							if (a < Index_10use1 * 3)
							{

								Temp_distAccum1[a] = jtf_RecordA.distAccum_temp[temp_count2 + a];
							}
							else
							{

								Temp_distAccum1[a] = jtf_RecordA.distAccum_temp[a - temp_count3];
							}


						}
						jtf_RecordA.distAccum = Temp_distAccum1;
						
#endregion
						////////////////////// sort end //////////////////////


#region  tmf_array insert

							for (count = 0; count < 2000; count++)
							{

								cs = 0;
								if (count < 1000)
								{
									switch (count)
									{
										case 100: distDailly_count += 2;  // 2,3
											break;
										case 200: distDailly_count += 2;  // 4,5
											break;
										case 300: distDailly_count += 2;   // 6,7
											break;
										case 400: distDailly_count += 2;    //8,9
											break;
										case 500: distDailly_count += 2;   //10,11
											break;
										case 600: distDailly_count += 2;    //12,13
											break;
										case 700: distDailly_count += 2;    //14,15
											break;
										case 800: distDailly_count += 2;    //16,17
											break;
										case 900: distDailly_count += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++) //2byte  distDailly
									{

										writer.Write(jtf_RecordA_Back.distDaily[a + distDailly_count]);
										cs += jtf_RecordA_Back.distDaily[a + distDailly_count];


									}
									////////////////////

									switch (count)
									{
										case 100: distAccum_count += 3;  // 3,4,5
											break;
										case 200: distAccum_count += 3;  // 5,6,7
											break;
										case 300: distAccum_count += 3;   // 
											break;
										case 400: distAccum_count += 3;    //
											break;
										case 500: distAccum_count += 3;   //
											break;
										case 600: distAccum_count += 3;    //
											break;
										case 700: distAccum_count += 3;    //
											break;
										case 800: distAccum_count += 3;    //
											break;
										case 900: distAccum_count += 3;    //
											break;
									}
									for (int a = 0; a < 3; a++)  // //3byte distAccum
									{

										writer.Write(jtf_RecordA_Back.distAccum[a + distAccum_count]);
										cs += jtf_RecordA_Back.distAccum[a + distAccum_count];

									}


									writer.Write(mSec[count]);
									cs += mSec[count];

									writer.Write(Sec[count]);
									cs += Sec[count];

									writer.Write(Minute[count]);
									cs += Minute[count];

									writer.Write(Hour[count]);
									cs += Hour[count];

									writer.Write(Dayy[count]);
									cs += Dayy[count];


									writer.Write(Month[count]);
									cs += Month[count];

									writer.Write(Year[count]);
									cs += Year[count];



									///////////////
									switch (count)
									{
										case 100: rpm_count += 2;  // 2,3
											break;
										case 200: rpm_count += 2;  // 4,5
											break;
										case 300: rpm_count += 2;   // 6,7
											break;
										case 400: rpm_count += 2;    //8,9
											break;
										case 500: rpm_count += 2;   //10,11
											break;
										case 600: rpm_count += 2;    //12,13
											break;
										case 700: rpm_count += 2;    //14,15
											break;
										case 800: rpm_count += 2;    //16,17
											break;
										case 900: rpm_count += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++)  // //2byte rpm
									{

										writer.Write(jtf_RecordA_Back.rpm[a + rpm_count]);
										cs += jtf_RecordA_Back.rpm[a + rpm_count];

									}
									for (int a = 0; a < 2; a++)
									{
										writer.Write(jtf_RecordA_Back.reserved1[a]);  //2byte  reserverd1
										cs += jtf_RecordA_Back.reserved1[a];
									}
									/////////////
									switch (count)
									{
										case 100: gpsY_count += 4;  // 
											break;
										case 200: gpsY_count += 4;  // 
											break;
										case 300: gpsY_count += 4;   // 
											break;
										case 400: gpsY_count += 4;    //
											break;
										case 500: gpsY_count += 4;   //
											break;
										case 600: gpsY_count += 4;    //
											break;
										case 700: gpsY_count += 4;    //
											break;
										case 800: gpsY_count += 4;    //
											break;
										case 900: gpsY_count += 4;    //
											break;
									}
									for (int a = 0; a < 4; a++)  // //4byte   gpsY
									{

										writer.Write(jtf_RecordB_BAck.longitude[a + gpsY_count]);
										cs += jtf_RecordB_BAck.longitude[a + gpsY_count];

									}
									////////////////
									switch (count)
									{
										case 100: gpsX_count += 4;  // 
											break;
										case 200: gpsX_count += 4;  // 
											break;
										case 300: gpsX_count += 4;   // 
											break;
										case 400: gpsX_count += 4;    //
											break;
										case 500: gpsX_count += 4;   //
											break;
										case 600: gpsX_count += 4;    //
											break;
										case 700: gpsX_count += 4;    //
											break;
										case 800: gpsX_count += 4;    //
											break;
										case 900: gpsX_count += 4;    //
											break;
									}
									for (int a = 0; a < 4; a++)  // //4byte   gpsX
									{

										writer.Write(jtf_RecordB_BAck.latitude[a + gpsX_count]);
										cs += jtf_RecordB_BAck.latitude[a + gpsX_count];

									}
									/////////////////
									switch (count)
									{
										case 100: degree_count += 2;  // 2,3
											break;
										case 200: degree_count += 2;  // 4,5
											break;
										case 300: degree_count += 2;   // 6,7
											break;
										case 400: degree_count += 2;    //8,9
											break;
										case 500: degree_count += 2;   //10,11
											break;
										case 600: degree_count += 2;    //12,13
											break;
										case 700: degree_count += 2;    //14,15
											break;
										case 800: degree_count += 2;    //16,17
											break;
										case 900: degree_count += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++) //2byte  degree
									{

										writer.Write(jtf_RecordB_BAck.azimuth[a + degree_count]);
										cs += jtf_RecordB_BAck.azimuth[a + degree_count];

									}


									writer.Write(jtf_RecordA_Back.accelX[count]);  // accel x 1byte
									cs += jtf_RecordA_Back.accelX[count];
									writer.Write(jtf_RecordA_Back.accelY[count]);
									cs += jtf_RecordA_Back.accelY[count];

									////////////////


									////////


									for (int a = 0; a < 2; a++) //2byte  status
									{

										writer.Write(jtf_RecordB_BAck.status[a + status_count]);

										cs += jtf_RecordB_BAck.status[a + status_count];

										if (a == 1)
										{
											if (status_count < 1997)
												status_count += 2;
										}
									}
									///////////////////
									switch (count)
									{
										case 100: speed_count += 1;  // 1
											break;
										case 200: speed_count += 1;  // 
											break;
										case 300: speed_count += 1;   //
											break;
										case 400: speed_count += 1;    //
											break;
										case 500: speed_count += 1;   //
											break;
										case 600: speed_count += 1;    //
											break;
										case 700: speed_count += 1;    //
											break;
										case 800: speed_count += 1;    //
											break;
										case 900: speed_count += 1;    //9
											break;
									}
									for (int a = 0; a < 1; a++) //1byte speed
									{

										writer.Write(jtf_RecordA_Back.speed[a + speed_count]);
										cs += jtf_RecordA_Back.speed[a + speed_count];
									}
									jtf_RecordA_Back.chsum = cs;
									cs = 0;
									writer.Write(jtf_RecordA_Back.chsum);

								}
								//////////////////////////////////////////////////////
								else
								{
									switch (count - 1000)
									{
										case 100: distDailly_count1 += 2;  // 2,3
											break;
										case 200: distDailly_count1 += 2;  // 4,5
											break;
										case 300: distDailly_count1 += 2;   // 6,7
											break;
										case 400: distDailly_count1 += 2;    //8,9
											break;
										case 500: distDailly_count1 += 2;   //10,11
											break;
										case 600: distDailly_count1 += 2;    //12,13
											break;
										case 700: distDailly_count1 += 2;    //14,15
											break;
										case 800: distDailly_count1 += 2;    //16,17
											break;
										case 900: distDailly_count1 += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++) //2byte  distDailly
									{

										writer.Write(jtf_RecordA.distDaily[a + distDailly_count1]);
										cs += jtf_RecordA.distDaily[a + distDailly_count1];

									}
									/////////////////
									switch (count - 1000)
									{
										case 100: distAccum_count1 += 3;  // 3,4,5
											break;
										case 200: distAccum_count1 += 3;  // 5,6,7
											break;
										case 300: distAccum_count1 += 3;   // 
											break;
										case 400: distAccum_count1 += 3;    //
											break;
										case 500: distAccum_count1 += 3;   //
											break;
										case 600: distAccum_count1 += 3;    //
											break;
										case 700: distAccum_count1 += 3;    //
											break;
										case 800: distAccum_count1 += 3;    //
											break;
										case 900: distAccum_count1 += 3;    //
											break;
									}
									for (int a = 0; a < 3; a++)  // //3byte distAccum
									{

										writer.Write(jtf_RecordA.distAccum[a + distAccum_count1]);
										cs += jtf_RecordA.distAccum[a + distAccum_count1];

									}

									writer.Write(mSec[count]);
									cs += mSec[count];

									writer.Write(Sec[count]);
									cs += Sec[count];

									writer.Write(Minute[count]);
									cs += Minute[count];

									writer.Write(Hour[count]);
									cs += Hour[count];

									writer.Write(Dayy[count]);
									cs += Dayy[count];


									writer.Write(Month[count]);
									cs += Month[count];

									writer.Write(Year[count]);
									cs += Year[count];

									/////////////////
									switch (count - 1000)
									{
										case 100: rpm_count1 += 2;  // 2,3
											break;
										case 200: rpm_count1 += 2;  // 4,5
											break;
										case 300: rpm_count1 += 2;   // 6,7
											break;
										case 400: rpm_count1 += 2;    //8,9
											break;
										case 500: rpm_count1 += 2;   //10,11
											break;
										case 600: rpm_count1 += 2;    //12,13
											break;
										case 700: rpm_count1 += 2;    //14,15
											break;
										case 800: rpm_count1 += 2;    //16,17
											break;
										case 900: rpm_count1 += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++)  // //2byte rpm
									{

										writer.Write(jtf_RecordA.rpm[a + rpm_count1]);
										cs += jtf_RecordA.rpm[a + rpm_count1];

									}
									for (int a = 0; a < 2; a++)
									{
										writer.Write(jtf_RecordA.reserved1[a]);  //2byte  reserverd1
										cs += jtf_RecordA.reserved1[a];
									}
									///////////////
									switch (count - 1000)
									{
										case 100: gpsY_count1 += 4;  // 
											break;
										case 200: gpsY_count1 += 4;  // 
											break;
										case 300: gpsY_count1 += 4;   // 
											break;
										case 400: gpsY_count1 += 4;    //
											break;
										case 500: gpsY_count1 += 4;   //
											break;
										case 600: gpsY_count1 += 4;    //
											break;
										case 700: gpsY_count1 += 4;    //
											break;
										case 800: gpsY_count1 += 4;    //
											break;
										case 900: gpsY_count += 4;    //
											break;
									}
									for (int a = 0; a < 4; a++)  // //4byte   gpsY
									{

										writer.Write(jtf_RecordB.longitude[a + gpsY_count1]);
										cs += jtf_RecordB.longitude[a + gpsY_count1];

									}
									///////////////
									switch (count - 1000)
									{
										case 100: gpsX_count1 += 4;  // 
											break;
										case 200: gpsX_count1 += 4;  // 
											break;
										case 300: gpsX_count1 += 4;   // 
											break;
										case 400: gpsX_count1 += 4;    //
											break;
										case 500: gpsX_count1 += 4;   //
											break;
										case 600: gpsX_count1 += 4;    //
											break;
										case 700: gpsX_count1 += 4;    //
											break;
										case 800: gpsX_count1 += 4;    //
											break;
										case 900: gpsX_count1 += 4;    //
											break;
									}
									for (int a = 0; a < 4; a++)  // //4byte   gpsX
									{

										writer.Write(jtf_RecordB.latitude[a + gpsX_count1]);
										cs += jtf_RecordB.latitude[a + gpsX_count1];

									}
									//////////////
									switch (count - 1000)
									{
										case 100: degree_count1 += 2;  // 2,3
											break;
										case 200: degree_count1 += 2;  // 4,5
											break;
										case 300: degree_count1 += 2;   // 6,7
											break;
										case 400: degree_count1 += 2;    //8,9
											break;
										case 500: degree_count1 += 2;   //10,11
											break;
										case 600: degree_count1 += 2;    //12,13
											break;
										case 700: degree_count1 += 2;    //14,15
											break;
										case 800: degree_count1 += 2;    //16,17
											break;
										case 900: degree_count1 += 2;    //18,19
											break;
									}
									for (int a = 0; a < 2; a++) //2byte  degree
									{

										writer.Write(jtf_RecordB.azimuth[a + degree_count1]);
										cs += jtf_RecordB.azimuth[a + degree_count1];

									}


									writer.Write(jtf_RecordA.accelX[count - 1000]);  // accel x 1byte
									cs += jtf_RecordA.accelX[count - 1000];
									writer.Write(jtf_RecordA.accelY[count - 1000]);
									cs += jtf_RecordA.accelY[count - 1000];
									//////////////


									for (int a = 0; a < 2; a++) //2byte  status
									{

										writer.Write(jtf_RecordB.status[a + status_count1]);
										cs += jtf_RecordB.status[a + status_count1];


										if (a == 1)
										{
											if (status_count1 < 1997)
												status_count1 += 2;
										}
									}
									/////////////
									switch (count - 1000)
									{
										case 100: speed_count1 += 1;  // 1
											break;
										case 200: speed_count1 += 1;  // 
											break;
										case 300: speed_count1 += 1;   //
											break;
										case 400: speed_count1 += 1;    //
											break;
										case 500: speed_count1 += 1;   //
											break;
										case 600: speed_count1 += 1;    //
											break;
										case 700: speed_count1 += 1;    //
											break;
										case 800: speed_count1 += 1;    //
											break;
										case 900: speed_count1 += 1;    //9
											break;
									}
									for (int a = 0; a < 1; a++) //1byte speed
									{

										writer.Write(jtf_RecordA.speed[a + speed_count1]);
										cs += jtf_RecordA.speed[a + speed_count1];
									}
									jtf_RecordA.chsum = cs;
									cs = 0;
									writer.Write(jtf_RecordA.chsum);
								}

							}

#endregion

							//	writer.Close();
					
						
					}
					
					catch (EndOfStreamException)
					{
					//	MessageBox.Show(this, "헤더 - 읽기 실패", "결과");
					//	return;
					}
					catch (Exception)
					{
						MessageBox.Show(this, "잘못된 파일", "결과");
						return;
					}
				}
				#endregion // Read & store header information from Input file


			/*	#region Calculate & compare header checkSum
				{
					// Calculate & compare header checkSum
					csCal = 0x00;
					for (i = 0; i < 4; i++)
						csCal += jtfHeader.TachoTransTime[i];
					for (i = 0; i < 3; i++)
						csCal += jtfHeader.TachoDataSize[i];
					for (i = 0; i < 10; i++)
						csCal += jtfHeader.ModelName[i];
					for (i = 0; i < 17; i++)
						csCal += (byte)(jtfHeader.CarNum[i]);
					csCal += jtfHeader.CarType;
					for (i = 0; i < 6; i++)
						csCal += jtfHeader.CarRegistNum[i];
					for (i = 0; i < 5; i++)
						csCal += jtfHeader.OperatorRegistNum[i];
					for (i = 0; i < 9; i++)
						csCal += jtfHeader.DriverCode[i];

					if (cs == csCal)
					//if (true)
					{
						jtfHeader.HeaderCheckSum = true;
					}
					else
					{
						jtfHeader.HeaderCheckSum = false;
						MessageBox.Show(this, "Header Checksum Error", "결과");
						return;
					}
				}
				#endregion // Calculate & compare header checkSum*/


				if (fs1.Position == fs1.Length)
				{
					
					br1.Close();
					fs1.Close();
					stream.Close();
					writer.Close();
					return;
					
				}
				
			} while (true);
			

			br1.Close();
			fs1.Close();
			stream.Close();
			writer.Close();
            MessageBox.Show(strFname2, "완료");

		}
        //< Fucnion:        private void ConvertFunction(string strFname)
        //< Input value:    (string)strFname
        //< Return value:   
        //< Documents:      *.TMF 파일을 입력받아 *.TXT 파일로 저장

		
        private void ConvertFunction(string strFname, string dirName)
        {
            Worker workerObject = new Worker();
            Thread workerThread = new Thread(workerObject.DoWork);
            workerThread.Start();

            //파일 읽어서 버퍼에 저장
            string saveFname;   // *.TXT 파일의 파일명
			string saveFname2;	// Temporary TXT 파일명
            byte cs, csCal;     // cs: 파일로부터 읽어들인 CheckSum Data, csCal: 프로그램이 계산한 CheckSum Data
            int i = 0;
            int nDataNum;
			bool TimeCheck = false; 
			bool bIs1sData = false;
			int nMakedFileNum = 0;
            bool msechk = false;

            // *.TXT 파일명 결정용 변수
            int saveYYMMDD = 100101;
            string saveCarNum = "서울01가9876";
            int headCnt = 1;

			bool bIsDriverCodeChanged = false;
			bool bIsSaveDataTrue = false;
			
			byte[] transTime= new byte[4];
			
			DriverCodeTemp.Clear();

            FileStream fs = new FileStream(strFname, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
			BinaryWriter bw = new BinaryWriter(fs);
			JTF_Time jtf_time = new JTF_Time();
			///bw.Write(0xff);


            //if(fs.Length < MINIMUMDATALENGTH)
            if (fs.Length < (56+32+56))
            {
                // 읽어들인 *.TMF 파일의 길이가 최소 데이터 길이보다 작을 경우
				MessageBox.Show(this, "잘못된 파일", "결과");
                return;
            }

			//int nTotalErrorCount = 0;
			//string strError = strFname + "_Error.txt";
			//using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
			//{
			//    sw.Write("\r\n=============================================\r\n");
			//    sw.Write("f.Position, 에러 발생 시간,      cs,   cal_CS\r\n");
			//    sw.Write("=============================================\r\n");
			//}
			

			//////////// Sampling Time Read/////////////
		
				

            do
			{
				NewProInputHeader newProInHeader = new NewProInputHeader();
               

                //////////////////////////////  24시간 자르기 테스트 //////////////////////////////

              /*  int numtemp = (int)(fs.Length - 56) / 32;
                byte DayTemp = 0;
                numtemp = numtemp / 2;

                br.ReadBytes(56);

                br.ReadBytes(numtemp);

                br.ReadBytes(2);
                br.ReadBytes(3);


                //newProInData[nDataNum].InforTime = br.ReadBytes(7);

                jtf_time.time = br.ReadBytes(7);

                DayTemp = jtf_time.time[4];

                fs.Position = 0;*/
                //////////////////////////////////////////////////////////////////////////////////


			
				//---------------------------------------------------------------  1Sec Time Make End ----------------------------------------------------

				#region Read & store header information from Input file
				{
					try
					{
						// Read & store header information from Input file
						newProInHeader.TachoTransTime = br.ReadBytes(4);

                        //////////////////////////////24시간 자르기 테스트 /////////////////////////
                  /*      newProInHeader.TachoTransTime[3] = 20;
                        newProInHeader.TachoTransTime[2] = jtf_time.time[6];
                        newProInHeader.TachoTransTime[1] = jtf_time.time[5];
                        newProInHeader.TachoTransTime[0] = jtf_time.time[4];*/

                        ////////////////////////////////////////////////////////////////////////////

						newProInHeader.TachoDataSize = br.ReadBytes(3);
						newProInHeader.ModelName = br.ReadBytes(10);
						newProInHeader.CarNum = br.ReadBytes(17);
						newProInHeader.CarType = br.ReadByte();
						newProInHeader.CarRegistNum = br.ReadBytes(6);
						newProInHeader.OperatorRegistNum = br.ReadBytes(5);
						newProInHeader.DriverCode = br.ReadBytes(9);
						cs = br.ReadByte();
					}
					catch (EndOfStreamException)
					{
						MessageBox.Show(this, "헤더 - 읽기 실패", "결과");
						return;
					}
					catch (Exception)
					{
						MessageBox.Show(this, "잘못된 파일", "결과");
						return;
					}
				}
				#endregion // Read & store header information from Input file

				#region Calculate & compare header checkSum
				{
					// Calculate & compare header checkSum
					csCal = 0x00;
					for (i = 0; i < 4; i++)
						csCal += newProInHeader.TachoTransTime[i];
					for (i = 0; i < 3; i++)
						csCal += newProInHeader.TachoDataSize[i];
					for (i = 0; i < 10; i++)
						csCal += newProInHeader.ModelName[i];
					for (i = 0; i < 17; i++)
						csCal += (byte)(newProInHeader.CarNum[i]);
						csCal += newProInHeader.CarType;
					for (i = 0; i < 6; i++)
						csCal += newProInHeader.CarRegistNum[i];
					for (i = 0; i < 5; i++)
						csCal += newProInHeader.OperatorRegistNum[i];
					for (i = 0; i < 9; i++)
						csCal += newProInHeader.DriverCode[i];

					if (cs == csCal)
					//if (true)
					{
						newProInHeader.HeaderCheckSum = true;
					}
					else
					{
					//	newProInHeader.HeaderCheckSum = false;
					//	MessageBox.Show(this, "Header Checksum Error", "결과");
					//	return;
					}
				}
				#endregion // Calculate & compare header checkSum

				int arrNum = (int)((newProInHeader.TachoDataSize[2] << 16) | (newProInHeader.TachoDataSize[1] << 8) | newProInHeader.TachoDataSize[0]);

				if (arrNum == 0) //YJK TachoDataSize 가 0이면 전체 파일 읽어오기 임. 
				{
					arrNum = (int)(fs.Length - 0x38); // 파일 크기에서 헤더 사이즈 뺀다. 
				}

				arrNum /= 32;

			//	byte[] InforTime = new byte[7];
             

				NewProInputData[] newProInData = new NewProInputData[arrNum];
              //  TempNewProInputData[] TempData = new TempNewProInputData[1];

                bool StartChk = false;
             
					#region Read & store data from Input file
					{
						// Read & store data from Input file
					
						for (nDataNum = 0; nDataNum < arrNum; nDataNum++)
						{
                            byte[] InforTime = new byte[7];
							
							try
							{

                               // StartChk = true;

                              


							    	newProInData[nDataNum].DayDriveDistance = br.ReadBytes(2);                               
							    	newProInData[nDataNum].TotalDriveDistance = br.ReadBytes(3);

                                    

                                  

							     	jtf_time.time =  br.ReadBytes(7);

                                    newProInData[nDataNum].InforTime = jtf_time.time;

                                   
                                    newProInData[nDataNum].CarRPM = br.ReadBytes(2);
                                    newProInData[nDataNum].Reserved = br.ReadBytes(2);
                                    newProInData[nDataNum].InforGPSY = br.ReadBytes(4);
                                    newProInData[nDataNum].InforGPSX = br.ReadBytes(4);
                                    newProInData[nDataNum].CarDegree = br.ReadBytes(2);
                                    newProInData[nDataNum].AccelX = br.ReadByte();
                                    newProInData[nDataNum].AccelY = br.ReadByte();
                                    newProInData[nDataNum].MachineStatus = br.ReadBytes(2);
                                    newProInData[nDataNum].CarSpeed = br.ReadByte();
                                    cs = br.ReadByte();
                                  

                                    #region TimeMake
                                    /*
                                     * 
                                     * 
                                       jtf_time.Year = jtf_time.time[6];
										jtf_time.Month = jtf_time.time[5];
										jtf_time.Dayy = jtf_time.time[4];
										jtf_time.Hour = jtf_time.time[3];
										jtf_time.Minute = jtf_time.time[2];
										jtf_time.Sec = jtf_time.time[1];
										jtf_time.time[0] = 0x00;
                                    jtf_time.Sec++;

                                    if (jtf_time.Sec > 59)
                                    {
                                        jtf_time.Sec = 0;

                                        jtf_time.Minute++;
                                        if (jtf_time.Minute > 59)
                                        {
                                            jtf_time.Minute = 0;

                                            jtf_time.Hour++;
                                            if (jtf_time.Hour > 23)
                                            {
                                                jtf_time.Hour = 0;
                                                jtf_time.Dayy++;
                                                switch (jtf_time.Month)
                                                {
                                                    case 2:
                                                        if (IsLeapYear(jtf_time.Year))
                                                        {
                                                            if (jtf_time.Dayy > 29)
                                                            {
                                                                jtf_time.Dayy = 1;
                                                                jtf_time.Month++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (jtf_time.Dayy > 28)
                                                            {
                                                                jtf_time.Dayy = 1;
                                                                jtf_time.Month++;
                                                            }
                                                        }
                                                        break;

                                                    case 4:
                                                    case 6:
                                                    case 9:
                                                    case 11:
                                                        if (jtf_time.Dayy > 30)
                                                        {
                                                            jtf_time.Dayy = 1;
                                                            jtf_time.Month++;
                                                        }
                                                        break;

                                                    default:
                                                        if (jtf_time.Dayy > 31)
                                                        {
                                                            jtf_time.Dayy = 1;
                                                            jtf_time.Month++;

                                                            if (jtf_time.Month > 12)
                                                            {
                                                                jtf_time.Month = 1;
                                                                jtf_time.Year++;

                                                                if (jtf_time.Year > 99)
                                                                {
                                                                    jtf_time.Year = 0;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                }

                                            }
                                        }

                                    }

                                    InforTime[0] = 0x00;
                                    InforTime[1] = jtf_time.Sec;
                                    InforTime[2] = jtf_time.Minute;
                                    InforTime[3] = jtf_time.Hour;
                                    InforTime[4] = jtf_time.Dayy;
                                    InforTime[5] = jtf_time.Month;
                                    InforTime[6] = jtf_time.Year;
                                    #endregion
                                    */
                    #endregion
                                    if (jtf_time.time[3] == 24 && jtf_time.time[2] == 0 && jtf_time.time[1] == 0)
                                    {
                                       nDataNum--;
                                        arrNum--;
                                        continue;
                                    }

                              
                            

                            ///////////////////////////////  24시간 자르기 테스트 /////////////////////////////////////////
                            /*    if (DayTemp != jtf_time.time[4])
                                {
                                   
                                    nDataNum--;
                                    arrNum--;
                                    continue;
                                }
                                else if (jtf_time.time[3] == 24 && jtf_time.time[2] == 0 && jtf_time.time[1] == 0)
                                {
                                    nDataNum--;
                                    arrNum--;
                                    continue;
                                }*/
                          ///////////////////////////////////////////////////////////////////////////////////////////////////


                               
							}
							catch (EndOfStreamException)
							{
								MessageBox.Show(this, "데이터 - 읽기 실패", "결과");
								return;
							}
							catch (Exception e)
							{
							
								MessageBox.Show(this, "잘못된 파일", "결과");
								return;
                            }

                       	// Calculate & Compare CheckSum
                            csCal = 0x00;
							for (i = 0; i < 2; i++)
								csCal += newProInData[nDataNum].DayDriveDistance[i];
							for (i = 0; i < 3; i++)
								csCal += newProInData[nDataNum].TotalDriveDistance[i];
							for (i = 0; i < 7; i++)
								csCal += newProInData[nDataNum].InforTime[i];
							for (i = 0; i < 2; i++)
								csCal += newProInData[nDataNum].CarRPM[i];
							for (i = 0; i < 2; i++)
								csCal += newProInData[nDataNum].Reserved[i];
							for (i = 0; i < 4; i++)
								csCal += newProInData[nDataNum].InforGPSX[i];
							for (i = 0; i < 4; i++)
								csCal += newProInData[nDataNum].InforGPSY[i];
							for (i = 0; i < 2; i++)
								csCal += newProInData[nDataNum].CarDegree[i];
							csCal += newProInData[nDataNum].AccelX;
							csCal += newProInData[nDataNum].AccelY;
							for (i = 0; i < 2; i++)
								csCal += newProInData[nDataNum].MachineStatus[i];
							csCal += newProInData[nDataNum].CarSpeed;

							if (cs == csCal)
							//if (true)
							{
								newProInData[nDataNum].DataCheckSum = true;
							}
							else
							{
							
								//newProInData[nDataNum].DataCheckSum = true;

								// 에러시 fs.Position, time, checksum(cs, calcs)
								// & total err cnt

								//nTotalErrorCount++;

								//using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
								//{
								//    string strETime = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
								//        2000 + newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5], newProInData[nDataNum].InforTime[4],
								//        newProInData[nDataNum].InforTime[3], newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);
								//    string strE = string.Format("0x{0:X8}, {1}, 0x{2:X2}, 0x{3:X2}\r\n",
								//                                fs.Position, strETime, cs, csCal);
								//    sw.Write(strE);
								//}
								
							//	newProInData[nDataNum].DataCheckSum = false;
							//	MessageBox.Show(this, "Data Checksum Error","결과");
							//	return;
							}
							TimeCheck = true;
						}
					}
					#endregion // Read & store data from Input file


					NewProOutputHeader newProOutHeader = new NewProOutputHeader();
					NewProOutputData[] newProOutData = new NewProOutputData[arrNum];
				//	NewProOutputData[] newProOutData = new NewProOutputData[2000];
					string tmpDirverCode = "";

					#region Transfer & store header information
					{
						/* Header 채우기 */
						// [20] 운행기록장치 모델명
						string strMN = "##########";

						

						for (i = 0; i < 10; i++)
						{
							strMN += String.Format("{0:C}", Convert.ToChar(newProInHeader.ModelName[i]));
							//test[i] = 0x23; 
						}

						newProOutHeader.ModelName = strMN;

						// [17] 차대번호
						string strCN = "";
						for (i = 0; i < 17; i++)
						{
							if (newProInHeader.CarNum[16 - i] == 0x00)
								strCN += String.Format("0");
                            else if(newProInHeader.CarNum[16 - i] == 0x1e)
                                strCN += String.Format("0");
							else
								strCN += String.Format("{0:C}", (char)(newProInHeader.CarNum[16 - i]));
						}

						newProOutHeader.CarNum = strCN;

						// [2] 자동차 유형
						ushort CT = (ushort)(newProInHeader.CarType);
						newProOutHeader.CarType = CT;

						// [12] 자동차 등록번호
						string strCRN = "";
					
						
						switch (newProInHeader.CarRegistNum[4])
						{
							case 0x01: strCRN = "서울"; break;
							case 0x02: strCRN = "인천"; break;
							case 0x03: strCRN = "대전"; break;
							case 0x04: strCRN = "광주"; break;
                            case 0x05: strCRN = "대구"; break;
							case 0x06: strCRN = "울산"; break;
							case 0x07: strCRN = "부산"; break;
							case 0x08: strCRN = "경기"; break;
							case 0x09: strCRN = "강원"; break;
							case 0x10: strCRN = "충북"; break;
							case 0x11: strCRN = "충남"; break;
							case 0x12: strCRN = "전북"; break;
							case 0x13: strCRN = "전남"; break;
							case 0x14: strCRN = "경북"; break;
							case 0x15: strCRN = "경남"; break;

							case 0xa: strCRN = "충북"; break;
							case 0xb: strCRN = "충남"; break;
							case 0xc: strCRN = "전북"; break;
							case 0xd: strCRN = "전남"; break;
							case 0xe: strCRN = "경북"; break;
							case 0xf: strCRN = "경남"; break;

							case 0x16: strCRN = "제주"; break;
							default: strCRN = "    "; break;
						}				

						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[3] >> 4));
						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[3] & 0x0F));

						string carRegistNumSign = " 가나다라마바사아자차카타파하거너더러머버서어저처커터퍼허고노도로모보소오조초코토포호구누두루무부수우주추쿠투푸후그느드르므브스으즈츠크트프흐기니디리미비시이지치키티피배";

					//	int tmpCarRegiNum = ((int)((newProInHeader.CarRegistNum[2] >> 4) * 10) + ((int)(newProInHeader.CarRegistNum[2] & 0x0F)));

                        	int tmpCarRegiNum = (int)newProInHeader.CarRegistNum[2];

						if ((tmpCarRegiNum == 0) || (tmpCarRegiNum >= carRegistNumSign.Length))
							tmpCarRegiNum = 0;


                        byte temp = (byte)tmpCarRegiNum;

                        tmpCarRegiNum = BcdToDecimal(temp);


						strCRN += carRegistNumSign[tmpCarRegiNum];
						
					/*	switch (newProInHeader.CarRegistNum[2])
						{
							case 0x04: strCRN += "라"; break;
							case 0x06: strCRN += "바"; break;
							case 0x07: strCRN += "사"; break;
							case 0x08: strCRN += "아"; break;
							case 0x10: strCRN += "차"; break;
							case 0x11: strCRN += "카"; break;
							case 0x12: strCRN += "타"; break;
							case 0x13: strCRN += "파"; break;
							default: strCRN += " "; break;
						}*/

						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[1] >> 4));     //car number
						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[1] & 0x0F));
						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[0] >> 4));
						strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[0] & 0x0F));


						

					//	UTF7Encoding enc = new UTF7Encoding();
					//	UnicodeEncoding enc = new UnicodeEncoding(); 
					//	UTF8Encoding enc = new UTF8Encoding();						
				/*		byte[] buffer = enc.GetBytes(strCRN); 
						System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
						Encoding littleendian = Encoding.Unicode;
						Encoding bigendian = Encoding.BigEndianUnicode;
						Byte[] ByteArray;
						string str;
						ByteArray = utf8.GetBytes(strCRN);   
						// 우선 UTF8 스트링을 바이트로 쪼개서
						ByteArray = Encoding.Convert(utf8, littleendian, ByteArray);  // 다음 UTF8 바이트 코드를 LittleEndian 바이트 코드로 변환하고 					                                                    // 변환된 바이트 코드를 다시 스트링으로 합치는 거에여..
						strCRN = littleendian.GetString(ByteArray);*/
					//	byte[] buffer = utf8.GetBytes(strCRN);

						
						
						newProOutHeader.CarRegistNum = strCRN;

						// [10] 운송사업자 등록번호
						string strORN = "";
						strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[4]);
						strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[3]);
						strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[2]);
						strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[1]);
						strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[0]);

						newProOutHeader.OperatorRegistNum = strORN;


						// [18] 운전자 코드
						string strDC = "";
						int noneZeroPoint = 0;
						for (i = 0; i < 9; i++)
						{
							if (newProInHeader.DriverCode[8 - i] != 0x00)
							{
								noneZeroPoint = 9 - i;
								break;
							}
						}
						if (noneZeroPoint <= 4) noneZeroPoint = 4;
						else noneZeroPoint = 9;

						for (i = 8; i >= 0; i--)
						{
							if (i >= noneZeroPoint)
							{
								strDC += "##";  // 빈자리 '0'으로 채우기 -> 빈자리 '#'으로 채우기
								tmpDirverCode += "00";
							}
							else
							{
								strDC += String.Format("{0:X2}", newProInHeader.DriverCode[i]);
								tmpDirverCode += String.Format("{0:X2}", newProInHeader.DriverCode[i]);
							}
						}
						newProOutHeader.DriverCode = strDC;
					}
					#endregion // Transfer & store header information

					#region Transfer & store data
					{
						/* Data 채우기 */
						for (nDataNum = 0; nDataNum < arrNum; nDataNum++)					
						{
							// Data 무결성 확인
							try
							{
								if (newProInData[nDataNum].DataCheckSum)
								{
									newProOutData[nDataNum].DataTrust = true;
								}
								else
								{
									newProOutData[nDataNum].DataTrust = false;
								//	MessageBox.Show("data error3");
								//	continue;
								}

								bIsDriverCodeChanged = false;
							}
							catch (System.Exception e)
							{
								{
									MessageBox.Show(e.Message);
									MessageBox.Show("data error");

								}
							}


							// [4] 일일 주행거리
							int nDDD = (UInt16)((newProInData[nDataNum].DayDriveDistance[1] << 8) | (newProInData[nDataNum].DayDriveDistance[0]));
                            if (nDDD > 9999)
                            {
                                nDDD = nDDD % 10000;
                            }
							newProOutData[nDataNum].DayDriveDistance = (ushort)nDDD;

                           

                            

							// [7] 누적 주행거리
							UInt32 nTDD = (UInt32)((newProInData[nDataNum].TotalDriveDistance[2] << 16)
													| (newProInData[nDataNum].TotalDriveDistance[1] << 8)
													| newProInData[nDataNum].TotalDriveDistance[0]);
							newProOutData[nDataNum].TotalDriveDistance = nTDD;


                            if (nDataNum != 0)  //  누적 주행 거리 보정
                                                //1.일일주행은 같고 누적 주행이 2이상 차이나며 튄경우  : 1식 차이나야만 정상이다.
                            {                   // 2. 현재 누적주행거리가 과거보다 작게 나온경우  
                                if (newProOutData[nDataNum].DayDriveDistance == newProOutData[nDataNum - 1].DayDriveDistance)
                                {
                                    UInt32 tempd = newProOutData[nDataNum].TotalDriveDistance - newProOutData[nDataNum - 1].TotalDriveDistance;
                                    if (tempd > 1)
                                    {
                                        newProOutData[nDataNum].TotalDriveDistance = newProOutData[nDataNum - 1].TotalDriveDistance;
                                    }

                                    else if (newProOutData[nDataNum].TotalDriveDistance < newProOutData[nDataNum - 1].TotalDriveDistance)
                                    {
                                        newProOutData[nDataNum].TotalDriveDistance = newProOutData[nDataNum - 1].TotalDriveDistance;
                                    }
                                }

                                             // 일일주행거리 보정   --- 14.08.12
                                              // 1.누적주행거리는 같고 일일주행이 2이상 차이나며 튄경우  : : 1식 차이나야만 정상이다.
                                             // 2.현재 일일주행거리가 과거보다 작게 나온겨우 

                              if (newProOutData[nDataNum].TotalDriveDistance == newProOutData[nDataNum - 1].TotalDriveDistance)
                                {
                                    UInt32 tempd = (uint)newProOutData[nDataNum].DayDriveDistance - newProOutData[nDataNum - 1].DayDriveDistance;

                                    if (newProOutData[nDataNum].DayDriveDistance != 0)
                                    {
                                        if (tempd > 1)
                                        {
                                            newProOutData[nDataNum].DayDriveDistance = newProOutData[nDataNum - 1].DayDriveDistance;
                                        }

                                        else if (newProOutData[nDataNum].DayDriveDistance < newProOutData[nDataNum - 1].DayDriveDistance)
                                        {
                                            newProOutData[nDataNum].DayDriveDistance = newProOutData[nDataNum - 1].DayDriveDistance;
                                        }
                                    }
                                }

                              
                            }

							// [14] 정보발생 일시
							byte[] arrIT = new byte[7];
							for (i = 0; i < 7; i++)
							{
								arrIT[i] = newProInData[nDataNum].InforTime[6 - i];
							}
							  // 시간 체크

							newProOutData[nDataNum].InforTime = arrIT;

                            if (nDataNum != 0)
                            {
                              /*  if (newProInData[nDataNum].InforTime[6] > newProInData[nDataNum - 1].InforTime[6])
                                {
                                    //  MessageBox.Show("Year Error");

                                    if (newProInData[nDataNum - 1].InforTime[6] == 0x00)
                                    {
                                        newProInData[nDataNum - 1].InforTime[6] = newProInData[nDataNum].InforTime[6];
                                    }
                                    else
                                    {

                                      

                                        if (newProInData[nDataNum].InforTime[5] == newProInData[nDataNum - 1].InforTime[5])
                                        {

                                            if (newProInData[nDataNum].InforTime[6] == newProInData[nDataNum - 2].InforTime[6])
                                            {
                                                newProInData[nDataNum - 1].InforTime[6] = newProInData[nDataNum].InforTime[6];
                                            }
                                            else
                                            {

                                                newProInData[nDataNum].InforTime[6] = newProInData[nDataNum - 1].InforTime[6];

                                            }
                                        }
                                    }

                                }*/
                              /*  if (newProOutData[nDataNum].InforTime[0] > newProOutData[nDataNum - 1].InforTime[0])
                                {
                                    //  MessageBox.Show("Year Error");

                                    if (newProOutData[nDataNum].InforTime[1] == newProOutData[nDataNum - 1].InforTime[1])
                                    {
                                        if (newProOutData[nDataNum].InforTime[0] == newProOutData[nDataNum - 2].InforTime[0])
                                        {
                                            newProOutData[nDataNum -1].InforTime[0] = newProOutData[nDataNum].InforTime[0];
                                        }
                                        else
                                        {
                                            newProOutData[nDataNum].InforTime[0] = newProOutData[nDataNum - 1].InforTime[0];
                                        }
                                    }

                                }*/
                            }


						/*	if (newProOutData[nDataNum].InforTime[1] == 0x00 || newProOutData[nDataNum].InforTime[2]==0x00)
							{
								TimeCheck = true;
							}*/



							// [3] 차량 속도 (Km/h)
							newProOutData[nDataNum].CarSpeed = newProInData[nDataNum].CarSpeed;

							// [4] 분당 엔진회전수 (RPM)
							ushort ntRPM = (ushort)((newProInData[nDataNum].CarRPM[1] << 8) | newProInData[nDataNum].CarRPM[0]);
							if (ntRPM > 9999) ntRPM = 9999; // '10. 8.26. choi15 비정상 데이터로 FF xx 값이 들어올 경우가 생김
							newProOutData[nDataNum].CarRPM = ntRPM;

							// [1] 브레이크 신호
							if ((newProInData[nDataNum].MachineStatus[1] == 0x38) && (newProInData[nDataNum].MachineStatus[0] == 0x00))
							{
								// 운전자 코드가 변경됐음을 인지
							//	bIsDriverCodeChanged = true;
							//	newProOutData[nDataNum].CarBreak = 2;
                                newProOutData[nDataNum].CarBreak = (byte)(((newProInData[nDataNum].MachineStatus[1] & 0x80) == 0x80) ? 1 : 0);
							}
							else
							{
								newProOutData[nDataNum].CarBreak = (byte)(((newProInData[nDataNum].MachineStatus[1] & 0x80) == 0x80) ? 1 : 0);
							}

							if (bIsDriverCodeChanged)
							{
								newProOutData[nDataNum].CarGpsX = (uint)((newProInData[nDataNum].InforGPSX[3] << 24) | (newProInData[nDataNum].InforGPSX[2] << 16) | (newProInData[nDataNum].InforGPSX[1] << 8) | newProInData[nDataNum].InforGPSX[0]);
                               

								newProOutData[nDataNum].CarGpsY = (uint)((newProInData[nDataNum].InforGPSY[3] << 24) | (newProInData[nDataNum].InforGPSY[2] << 16) | (newProInData[nDataNum].InforGPSY[1] << 8) | newProInData[nDataNum].InforGPSY[0]);
                              
								newProOutData[nDataNum].CarDegree = (ushort)((newProInData[nDataNum].CarDegree[1] << 8) | (newProInData[nDataNum].CarDegree[0]));

								// Header의 운전자 코드를 변경
								byte[] dct = new byte[9];

								//dct[0] = newProInData[nDataNum].InforGPSY[2];
								//dct[1] = newProInData[nDataNum].InforGPSY[1];
								//dct[2] = newProInData[nDataNum].InforGPSY[0];
								//dct[3] = newProInData[nDataNum].CarDegree[1];
								//dct[4] = newProInData[nDataNum].InforGPSY[3];
								//dct[5] = newProInData[nDataNum].InforGPSX[3];
								//dct[6] = newProInData[nDataNum].InforGPSX[2];
								//dct[7] = newProInData[nDataNum].InforGPSX[1];
								//dct[8] = newProInData[nDataNum].InforGPSX[0];

								dct[0] = newProInData[nDataNum].InforGPSY[0];
								dct[1] = newProInData[nDataNum].InforGPSY[1];
								dct[2] = newProInData[nDataNum].InforGPSY[2];
								dct[3] = newProInData[nDataNum].InforGPSY[3];
								dct[4] = newProInData[nDataNum].InforGPSX[3];
								dct[5] = newProInData[nDataNum].CarDegree[1];
								dct[6] = newProInData[nDataNum].InforGPSX[0];
								dct[7] = newProInData[nDataNum].InforGPSX[1];
								dct[8] = newProInData[nDataNum].InforGPSX[2];

								// [18] 운전자 코드
								string strDC = "";
								int noneZeroPoint = 0;
								for (i = 0; i < 9; i++)
								{
									if (dct[8 - i] != 0x00)
									{
										noneZeroPoint = 9 - i;
										break;
									}
								}
								if (noneZeroPoint <= 4) noneZeroPoint = 4;
								else noneZeroPoint = 9;

								for (i = 8; i >= 0; i--)
								{
									if (i >= noneZeroPoint)
									{
										strDC += "##";  // 빈자리 '0'으로 채우기 -> 빈자리 '#'으로 채우기
									}
									else
									{
										strDC += String.Format("{0:X2}", dct[i]);
									}
								}
								DriverCodeTemp.Add(strDC);
							}
							else
							{
								/* [18] 차량위치 */
								// [9] GPS X 좌표

							/*	if (newProInData[nDataNum].InforGPSX[0] == 0x4b && newProInData[nDataNum].InforGPSX[1] == 0x49 && newProInData[nDataNum].InforGPSX[2] == 0x37 && newProInData[nDataNum].InforGPSX[3] == 0x02)
								{
									MessageBox.Show("0x4b");
								}*/

							/*	if (newProInData[nDataNum].InforGPSY[0] == 0x8f && newProInData[nDataNum].InforGPSY[1] == 0x26 && newProInData[nDataNum].InforGPSY[2] == 0x8a && newProInData[nDataNum].InforGPSY[3] == 0x07 && newProInData[nDataNum].InforGPSX[0] == 0x4b && newProInData[nDataNum].InforGPSX[1] == 0x49 && newProInData[nDataNum].InforGPSX[2] == 0x37 && newProInData[nDataNum].InforGPSX[3] == 0x02)
							{
								MessageBox.Show("11");
							}*/
								
							

								UInt32 nIGX = (UInt32)((newProInData[nDataNum].InforGPSX[3] << 24) | (newProInData[nDataNum].InforGPSX[2] << 16) | (newProInData[nDataNum].InforGPSX[1] << 8) | newProInData[nDataNum].InforGPSX[0]);
								UInt32 nIGXD = nIGX / 1000000;
								UInt32 nIGXM = nIGX % 1000000;
								double fIGXM = ((double)nIGXM / 60.0) * 100;
								nIGX = (UInt32)((nIGXD * 1000000) + fIGXM);
								newProOutData[nDataNum].CarGpsX = nIGX;


                                if (newProOutData[nDataNum].CarGpsX != 0 && newProOutData[nDataNum].CarGpsX > 34000000)
                                {
                                    OldCarGpsX = newProOutData[nDataNum].CarGpsX;
                                }

								// [9] GPS Y 좌표
								UInt32 nIGY = (UInt32)((newProInData[nDataNum].InforGPSY[3] << 24) | (newProInData[nDataNum].InforGPSY[2] << 16) | (newProInData[nDataNum].InforGPSY[1] << 8) | newProInData[nDataNum].InforGPSY[0]);
								UInt32 nIGYD = nIGY / 1000000;
								UInt32 nIGYM = nIGY % 1000000;
								double fIGYM = ((double)nIGYM / 60.0) * 100;
								nIGY = (UInt32)((nIGYD * 1000000) + fIGYM);
								newProOutData[nDataNum].CarGpsY = nIGY;

                                if (newProOutData[nDataNum].CarGpsY != 0 && newProOutData[nDataNum].CarGpsY >126000000)
                                {
                                    OldCarGpsY = newProOutData[nDataNum].CarGpsY;
                                }

								// [3] 위성항법장치 방위각 (GPS Degree)
								UInt16 nCD = (UInt16)((newProInData[nDataNum].CarDegree[1] << 8) | (newProInData[nDataNum].CarDegree[0]));

								if (nCD > 359)
								{
									nCD = 359;
								}

								newProOutData[nDataNum].CarDegree = nCD;
							}

							// [12] 가속도 (Km/sec2)
							// [6] △Vx
							byte bDeltaX = newProInData[nDataNum].AccelX;
							string strDeltaX = "";
							float gDeltaX;
							if ((bDeltaX & 0x80) == 0x00)
							{
								gDeltaX = (float)(bDeltaX * 0.018 * 9.80665);
								strDeltaX = "+";

								if (gDeltaX < 1)
									strDeltaX += "000";
								else if (gDeltaX < 10)
									strDeltaX += "00";
								else if (gDeltaX < 100)
									strDeltaX += "0";

								strDeltaX += String.Format("{0:#.#}", gDeltaX);

								if (strDeltaX.Length != 6)
									strDeltaX += String.Format(".0");

								newProOutData[nDataNum].CarAccelX = strDeltaX;
							}
							else
							{
								gDeltaX = (float)((256 - bDeltaX) * 0.018 * 9.80665);
								strDeltaX = "-";
								if (gDeltaX < 1)
									strDeltaX += "000";
								else if (gDeltaX < 10)
									strDeltaX += "00";
								else if (gDeltaX < 100)
									strDeltaX += "0";

								strDeltaX += String.Format("{0:#.#}", gDeltaX);

								if (strDeltaX.Length != 6)
									strDeltaX += String.Format(".0");

								newProOutData[nDataNum].CarAccelX = strDeltaX;
							}

							// [6] △Vy
							byte bDeltaY = newProInData[nDataNum].AccelY;
							string strDeltaY = "";
							float gDeltaY;
							if ((bDeltaY & 0x80) == 0x00)
							{
								gDeltaY = (float)(bDeltaY * 0.018 * 9.80665);
								strDeltaY = "+";
								if (gDeltaY < 1)
									strDeltaY += "000";
								else if (gDeltaY < 10)
									strDeltaY += "00";
								else if (gDeltaY < 100)
									strDeltaY += "0";

								strDeltaY += String.Format("{0:#.#}", gDeltaY);

								if (strDeltaY.Length != 6)
									strDeltaY += String.Format(".0");

								newProOutData[nDataNum].CarAccelY = strDeltaY;
							}
							else
							{
								gDeltaY = (float)((256 - bDeltaY) * 0.018 * 9.80665);
								strDeltaY = "-";
								if (gDeltaY < 1) strDeltaY += "000";
								else if (gDeltaY < 10) strDeltaY += "00";
								else if (gDeltaY < 100) strDeltaY += "0";

								strDeltaY += String.Format("{0:#.#}", gDeltaY);

								if (strDeltaY.Length != 6)
									strDeltaY += String.Format(".0");

								newProOutData[nDataNum].CarAccelY = strDeltaY;
							}

							// [2] 기기 및 통신 상태 코드 (백업 수집 주기 내)
							byte bMS = 0;
							ushort usMS = (ushort)((newProInData[nDataNum].MachineStatus[1] << 8) | (newProInData[nDataNum].MachineStatus[0]));

							if ((usMS & 0x0001) == 0x0001)  // 11 위치추적장치 (GPS수신기) 이상
							{
								bMS = 0x11;
							}
							if ((usMS & 0x0002) == 0x0002)  // 12 속도센서 이상
							{
								bMS = 0x12;
							}
							if ((usMS & 0x0004) == 0x0004)  // 13 RPM 센서 이상
							{
								bMS = 0x13;
							}
							if ((usMS & 0x0008) == 0x0008)  // 14 브레이크 감지센서 이상
							{
							    bMS = 0x14;
							}
							if ((usMS & 0x0010) == 0x0010)  // 21 센서 입력부 장치 이상
							{
								bMS = 0x21;
							}
							if ((usMS & 0x0020) == 0x0020)  // 22 센서 출력부 장치 이상
							{
								bMS = 0x22;
							}
							if ((usMS & 0x0040) == 0x0040)  // 31 데이터 출력부 장치 이상
							{
								bMS = 0x31;
							}
							if ((usMS & 0x0080) == 0x0080)  // 32 통신장치 이상
							{
								bMS = 0x32;
							}
							if ((usMS & 0x0100) == 0x0100)  // 41 운행거리 산정 이상
							{
								bMS = 0x41;
							}
							if ((usMS & 0x0200) == 0x0200)  // 99 전원공급 이상
							{
								bMS = 0x99;
							}

							///
							if ((newProInData[nDataNum].MachineStatus[0] == 0xFF) || (newProInData[nDataNum].MachineStatus[1] == 0xFF))
							{
								//bMS = 0x00;
								if (nDataNum == 0)
									bMS = 0x00;
								else
									bMS = newProOutData[nDataNum - 1].MachineStatus;
							}

							if ((newProOutData[nDataNum].CarGpsX == 0) || (newProOutData[nDataNum].CarGpsY == 0))
								bMS = 0x11; // 위치추적장치 (GPS) 이상으로 강제화
							///

							newProOutData[nDataNum].MachineStatus = bMS;
							bIsSaveDataTrue = true;
						}
					}
					#endregion // Transfer & store data

				//	if (arrNum > 0x7cf)
				//	{
						#region // Make Save File
						{
							bool bIsStandAloneData = false;
							nDataNum = 0;
							int nC = DriverCodeTemp.Count;
							if ((bIsSaveDataTrue) && (nC == 0))
							{	// 저장할 데이터가 있는데, 운전자코드가 한번도 변경되지 않은 경우를 위한 루틴
								nC = 1;
								bIsStandAloneData = true;
								bIsSaveDataTrue = false;
							}
                            DateTime TimeTemp;
                            if (newProInData[nDataNum].InforTime[5] > 12 || newProInData[nDataNum].InforTime[5] == 0 ||
                                                              newProInData[nDataNum].InforTime[4] > 31 || newProInData[nDataNum].InforTime[4] == 0 ||
                                                              newProInData[nDataNum].InforTime[3] > 24 || newProInData[nDataNum].InforTime[2] > 60 ||
                                                              newProInData[nDataNum].InforTime[1] > 60)
                            {
                                nDataNum++;
                                while (newProInData[nDataNum].InforTime[5] > 12 || newProInData[nDataNum].InforTime[5] == 0 ||
                                                              newProInData[nDataNum].InforTime[4] > 31 || newProInData[nDataNum].InforTime[4] == 0 ||
                                                              newProInData[nDataNum].InforTime[3] > 24 || newProInData[nDataNum].InforTime[2] > 60 ||
                                                              newProInData[nDataNum].InforTime[1] > 60)
                                {
                                    nDataNum++;
                                }
                                TimeTemp = new DateTime(newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5], newProInData[nDataNum].InforTime[4]
                               , newProInData[nDataNum].InforTime[3], newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);    // 잘못된 시간 패스 하기
                            }

                            else
                            {
                                TimeTemp = new DateTime(newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5], newProInData[nDataNum].InforTime[4]
                                  , newProInData[nDataNum].InforTime[3], newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);
                            }
                         
                        int Daytemp = newProInData[nDataNum].InforTime[4];



                        bool repeat = false;
							for (int driverN = 0; driverN < nC; driverN++)
							{
                               

								string fileNameLeft, fileNameRight;
								int tempHeadCnt;

                                

								#region // Make Save File Name
								{
									int nYYYYMMDD;
                                    int tempb = newProInHeader.TachoTransTime[1] & 0x0f;


                                    newProInHeader.TachoTransTime[1] = (byte)tempb;

									if (newProInHeader.TachoTransTime[0] == 0x00 || newProInHeader.TachoTransTime[0] == 0x00)
									{
										 nYYYYMMDD = +((int)(transTime[0]) * 1000000)
													+ ((int)(transTime[1]) * 10000)
                                                    + ((int)(newProInData[nDataNum].InforTime[6]) * 100)
													+ (int)(transTime[3]);


                                         nYYYYMMDD = +((int)(transTime[0]) * 1000000)
                                                       + ((int)(transTime[1]) * 10000)
                                                       + ((int)(newProInData[nDataNum].InforTime[6]) * 100)
                                                       + (int)(transTime[3]);

									}
									else
									{


										 nYYYYMMDD = +((int)(newProInHeader.TachoTransTime[3]) * 1000000)
                                                        + ((int)(newProInData[nDataNum].InforTime[6]) * 10000)
														+ ((int)(newProInHeader.TachoTransTime[1]) * 100)
														+ (int)(newProInHeader.TachoTransTime[0]);
									}

									saveFname = String.Format("{0:D}", nYYYYMMDD);
									saveFname += "-";
									saveFname += newProOutHeader.CarRegistNum;
									saveFname += "-";

                                   
									if (arrNum > 2)
									{
										if ((newProInData[0].InforTime[0] == newProInData[1].InforTime[0])
											&& (newProInData[0].InforTime[0] == newProInData[2].InforTime[0]))
										{
											saveFname += "S";	// 1초 데이터
											bIs1sData = true;
										}
										else
										{
											saveFname += "C";	// 0.01초 데이터
                                            msechk = true;
										}
									}
									else
									{
										saveFname += "C";	// 0.01초 데이터
                                        msechk = true;
									}
									saveFname += "-";

									fileNameLeft = saveFname;	// '11. 3. 2(추가)

									if (saveYYMMDD != nYYYYMMDD)
										headCnt = 1;
									if (saveCarNum != newProOutHeader.CarRegistNum)
										headCnt = 1;

									tempHeadCnt = headCnt;

									saveFname += String.Format("{0:D2}", headCnt);
									saveFname += "-";

									if (driverN == nC)
										DriverCodeTemp.Add(newProOutHeader.DriverCode);

									if (bIsStandAloneData)
									{
										saveFname += newProOutHeader.DriverCode.Substring(11, 7);
										fileNameRight = "-" + newProOutHeader.DriverCode.Substring(11, 7) + ".TXT";	// '11. 3. 2(추가)
									}
									else
									{
										saveFname += DriverCodeTemp[driverN].Substring(11, 7);
										fileNameRight = "-" + DriverCodeTemp[driverN].Substring(11, 7) + ".TXT";	// '11. 3. 2(추가)
									}

									saveFname += ".TXT";
									saveFname2 = "_" + saveFname;

									saveYYMMDD = nYYYYMMDD;
									saveCarNum = newProOutHeader.CarRegistNum;
								}
								#endregion // Make Save File Name

								#region Confirm save filename
								{

                                    string newPath = System.IO.Path.Combine(dirName, newProOutHeader.CarRegistNum);
                                    // Create the subfolder
                                    System.IO.Directory.CreateDirectory(newPath);
									// 불러들인 *.TMF 파일의 디렉토리
									DirectoryInfo dir = new DirectoryInfo(dirName+"\\"+newProOutHeader.CarRegistNum);

									// *.TXT 파일명에서 .TXT를 제외한 실제 파일명 추출
									char[] trimChars = { '.', 'T', 'X', 'T' };

									bool bisLoopContinue = true;
									do
									{
										string searchingfor = saveFname.TrimEnd(trimChars);

										// Partial matches
										// 저장할 파일명(.TXT 제외)과 같은 파일이 이미 있는지 조사

                                        try
                                        {
                                            FileInfo[] partial = dir.GetFiles("*" + searchingfor + "*", SearchOption.TopDirectoryOnly);


                                            if (partial.Length != 0)
                                            {
                                                saveFname = fileNameLeft + String.Format("{0:D2}", ++tempHeadCnt) + fileNameRight;
                                                /*
                                                // 같은 파일이 이미 있다면, Dialog를 통해 사용자에게 저장할 파일명 입력받음
                                                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                                                saveFileDialog1.Title = "Create Text File";
                                                saveFileDialog1.InitialDirectory = dirName;
                                                saveFileDialog1.FileName = saveFname;
                                                saveFileDialog1.Filter = "Text file(*.TXT)|*.TXT";

                                                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                                                {
                                                    saveFname = saveFileDialog1.FileName;
                                                    //saveFname2 = saveFileDialog1.F
                                                    saveFname2 = dirName + "\\_" + saveFname2;
                                                }
                                                */
                                            }
                                            else
                                            {

                                            

                                                // 같은 파일이 없다면 불러들인 *.TMF 파일이 있는 디렉토리에 저장

                                                 //    saveFname2 = dirName+ @"\_" + saveFname;
                                                 //    saveFname = dirName + @"\" + saveFname;


                                                saveFname2 = dirName +"\\"+newProOutHeader.CarRegistNum+ @"\" + saveFname;
                                                saveFname = dirName +"\\"+newProOutHeader.CarRegistNum+ @"\" + saveFname;
                                                bisLoopContinue = false;
                                            }
                                        }
                                        catch (System.Exception e)
                                        {
                                            MessageBox.Show("파일 이름이 8자리가 되어야 합니다.");
                                            workerObject.RequestStop();
                                            return;
                                       
                                        }

									} while (bisLoopContinue);
								}
								#endregion // Confirm save filename

																							
									#region Save file
									{

                                      
                                        try
                                        {
                                           
                                            DateTime NowTime = new DateTime(newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5],
                                                          newProInData[nDataNum].InforTime[4], newProInData[nDataNum].InforTime[3],
                                                          newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);

                                            if (msechk == false)
                                            {

                                            /*    if (NowTime < TimeTemp)
                                                {
                                                    //        MessageBox.Show("test");
                                                    nDataNum++;

                                                    repeat = true;


                                                    nC++;


                                                    continue;
                                                }*/

                                            /*    if (NowTime == TimeTemp)
                                                {
                                                    if (repeat == true)
                                                    {
                                                        repeat = false;
                                                    }

                                                    nDataNum++;
                                                    nC++;
                                                    continue;
                                                }
                                                else if (NowTime < TimeTemp)
                                                {

                                                    if (repeat == true)
                                                    {
                                                        repeat = false;
                                                    }

                                                    nDataNum++;
                                                    nC++;
                                                    continue;
                                                }*/
                                            }
                                        }
                                        catch 
                                        {
                                            nDataNum++;
                                            if (nDataNum > arrNum)
                                            {
                                                break;
                                            }
                                            repeat = true;
                                            nC++;
                                            continue;
                                      
                                    }


										Encoding euckr = Encoding.GetEncoding(51949);
										using (StreamWriter sw = new StreamWriter(saveFname, false,euckr))
								                                                                                    	//	using (StreamWriter sw = new StreamWriter(saveFname, false, System.Text.Encoding.UTF8))										
											{
												nMakedFileNum++;	// 총 만들어진 파일 갯수를 알아보기 위한 변수																																
                                            

												sw.Write(newProOutHeader.ModelName);																																
												sw.Write(newProOutHeader.CarNum);
												sw.Write("{0:X2}", newProOutHeader.CarType);																							
												sw.Write(newProOutHeader.CarRegistNum);												
												sw.Write(newProOutHeader.OperatorRegistNum);
											

												if (bIsStandAloneData)
													sw.Write(newProOutHeader.DriverCode);
												else
													sw.Write(DriverCodeTemp[driverN]);

                                                byte[] InforTime = new byte[7];

                                                bool startchk = false;
                                                int linecnt = 0;

                                                int loopback =0;
                                              
                                               


												for (; nDataNum < arrNum; nDataNum++)
												{
                                                   linecnt++;

                                                  
                                                     
                                                    if (newProInData[nDataNum].InforTime[6] == InforTime[6] && newProInData[nDataNum].InforTime[5] == InforTime[5] &&
                                                           newProInData[nDataNum].InforTime[4] == InforTime[4] && newProInData[nDataNum].InforTime[3] == InforTime[3] &&
                                                           newProInData[nDataNum].InforTime[2] == InforTime[2] && newProInData[nDataNum].InforTime[1] == InforTime[1])
                                                    {

                                                   //     MessageBox.Show("test");      // 정상적인 시간임
                                                    }
                                                 
                                                   else
                                                   {
                                                       if (msechk == false)
                                                       {

                                                           if (newProInData[nDataNum].InforTime[5] > 12 || newProInData[nDataNum].InforTime[5] == 0 ||
                                                               newProInData[nDataNum].InforTime[4] > 31 ||  newProInData[nDataNum].InforTime[4] == 0 ||
                                                               newProInData[nDataNum].InforTime[3] > 23 || newProInData[nDataNum].InforTime[2] > 59 ||
                                                               newProInData[nDataNum].InforTime[1] > 59 || newProInData[nDataNum].InforTime[6] == 0)//|| newProInData[nDataNum].InforTime[6] == 0
                                                           {
                                                               if (newProInData[nDataNum].InforTime[6] == 0)
                                                               {
                                                                   newProInData[nDataNum].InforTime[6] = InforTime[6];
                                                               }
                                                            //   MessageBox.Show("Test");
                                                               continue;        // 잘못된 시간 패스 하기
                                                           }
                                                           else if (newProInData[nDataNum].InforTime[4] ==31)
                                                           {

                                                               if(newProInData[nDataNum].InforTime[5] ==4 || newProInData[nDataNum].InforTime[5] ==6 || newProInData[nDataNum].InforTime[5] ==9
                                                                   || newProInData[nDataNum].InforTime[5] ==11)
                                                               {
                                                                   continue;
                                                               }
                                                           }


                                                           if (newProInData[nDataNum].InforTime[6] == 13 && newProInData[nDataNum].InforTime[5] == 2 &&
                                                               newProInData[nDataNum].InforTime[4] == 29)
                                                           {
                                                         //      continue;
                                                           }

                                                           DateTime NowTime = new DateTime(newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5],
                                                        newProInData[nDataNum].InforTime[4], newProInData[nDataNum].InforTime[3],
                                                        newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);


                                                           
                                                           TimeSpan spantime = new TimeSpan();
                                                           if (startchk == true)
                                                           {
                                                               TimeTemp = new DateTime(InforTime[6], InforTime[5], InforTime[4], InforTime[3], InforTime[2], InforTime[1]);
                                                           }
                                                           spantime = (NowTime - TimeTemp);


                                                           if (spantime.Hours == 23 && spantime.Minutes == 59 && spantime.Seconds == 59)
                                                           {
                                                      //         continue;
                                                           }
                                                           else if (spantime.Days > 365)
                                                           {
                                                          //     continue;
                                                           }


                                                           loopback = spantime.Seconds;

                                                           if (spantime.Seconds > 0 && spantime.Seconds <= SecTime_Length && spantime.Minutes < 1 && spantime.Hours < 1)    // 
                                                           {
                                                               if (startchk == true)
                                                               {
                                                            
                                                                  ReTime = true;

                                                               }
                                                                                                                          
                                                           }
                                                           else if (spantime.Seconds < 0 )       // 과거 시간 나올시 패스
                                                           {
                                                         //      nC = nC;
                                                               continue;
                                                           }

                                                           else
                                                           {


                                                               if (startchk == true)
                                                               {
                                                                   nC++;
                                                                   //    nDataNum++;
                                                                   Daytemp = newProInData[nDataNum].InforTime[4];
                                                                   newProInHeader.TachoTransTime[0] = (byte)Daytemp;

                                                                   newProInHeader.TachoTransTime[1] = newProInData[nDataNum].InforTime[5];

                                                                   //	MessageBox.Show("newProOutData[nDataNum].CarBreak == 0x02");


                                                                   if (linecnt < 4)     // 세개 이하 데이터 버림
                                                                   {
                                                                       //    MessageBox.Show("test");

                                                                       sw.Close();
                                                                       File.Delete(saveFname);
                                                                       nMakedFileNum--;
                                                                   }


                                                                   break;
                                                               }
                                                               else if (arrNum - 1 == nDataNum)
                                                               {

                                                                   if (linecnt < 4)
                                                                   {
                                                                       //    MessageBox.Show("test");

                                                                       sw.Close();
                                                                       File.Delete(saveFname);
                                                                       nMakedFileNum--;
                                                                   }
                                                                   break;
                                                               }
                                                           }
                                                       }

                                                   }

                                                    startchk = true;


                                                     if (newProInData[nDataNum].InforTime[4] != Daytemp)        // 날짜 변경시
                                                    {


                                                       
                                                            Daytemp = newProInData[nDataNum].InforTime[4];
                                                            newProInHeader.TachoTransTime[1] = newProInData[nDataNum].InforTime[5];
                                                            newProInHeader.TachoTransTime[0] = (byte)Daytemp;
                                                            nC++;
                                                        //    nDataNum++;

                                                            if (msechk == false)
                                                            {
                                                                if (linecnt < 4)
                                                                {
                                                                    //    MessageBox.Show("test");
                                                                    nMakedFileNum--;
                                                                    sw.Close();
                                                                    File.Delete(saveFname);
                                                                }
                                                            }
                                                         
                                                            break;
                                                       

                                                    }

                                                   

                                                        #region TimeMake



                                                    if (ReTime == false)
                                                    {
                                                        jtf_time.Year = newProInData[nDataNum].InforTime[6];
                                                        jtf_time.Month = newProInData[nDataNum].InforTime[5];
                                                        jtf_time.Dayy = newProInData[nDataNum].InforTime[4];
                                                        jtf_time.Hour = newProInData[nDataNum].InforTime[3];
                                                        jtf_time.Minute = newProInData[nDataNum].InforTime[2];
                                                        jtf_time.Sec = newProInData[nDataNum].InforTime[1];
                                                        jtf_time.time[0] = 0x00;
                                                    }
                                                    else
                                                    {
                                                        if (linecnt != 1)
                                                        {
                                                            jtf_time.Year = InforTime[6];
                                                            jtf_time.Month = InforTime[5];
                                                            jtf_time.Dayy = InforTime[4];
                                                            jtf_time.Hour = InforTime[3];
                                                            jtf_time.Minute = InforTime[2];
                                                            jtf_time.Sec = InforTime[1];
                                                            jtf_time.time[0] = 0x00;
                                                        }
                                                        else
                                                        {
                                                            InforTime[6] = jtf_time.Year;
                                                            InforTime[5] = jtf_time.Month;
                                                            InforTime[4] = jtf_time.Dayy;
                                                            InforTime[3] = jtf_time.Hour;
                                                            InforTime[2] = jtf_time.Minute;
                                                            InforTime[1] = jtf_time.Sec;
                                                            jtf_time.time[0] = 0x00;
                                                        }
                                                    }
                                                    byte[] PrnTIme = new byte[7];
                                                    if (startchk == true)
                                                    {

                                                     //   byte[] PrnTIme = new byte[6];
                                                        PrnTIme[0] = InforTime[6];
                                                        PrnTIme[1] = InforTime[5];
                                                        PrnTIme[2] = InforTime[4];
                                                        PrnTIme[3] = InforTime[3];
                                                        PrnTIme[4] = InforTime[2];
                                                        PrnTIme[5] = InforTime[1];
                                                        PrnTIme[6] = 0x00;
                                                    }

                                                        // TimeTemp = new DateTime(jtf_time.Year, jtf_time.Month, jtf_time.Dayy, jtf_time.Hour, jtf_time.Minute, jtf_time.Sec);

                                                        jtf_time.Sec++;

                                                        if (jtf_time.Sec > 59)
                                                        {
                                                            jtf_time.Sec = 0;

                                                            jtf_time.Minute++;
                                                            if (jtf_time.Minute > 59)
                                                            {
                                                                jtf_time.Minute = 0;

                                                                jtf_time.Hour++;
                                                                if (jtf_time.Hour > 23)
                                                                {
                                                                    jtf_time.Hour = 0;
                                                                    jtf_time.Dayy++;
                                                                    switch (jtf_time.Month)
                                                                    {
                                                                        case 2:
                                                                            if (IsLeapYear(jtf_time.Year))
                                                                            {
                                                                                if (jtf_time.Dayy > 29)
                                                                                {
                                                                                    jtf_time.Dayy = 1;
                                                                                    jtf_time.Month++;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (jtf_time.Dayy > 28)
                                                                                {
                                                                                    jtf_time.Dayy = 1;
                                                                                    jtf_time.Month++;
                                                                                }
                                                                            }
                                                                            break;

                                                                        case 4:
                                                                        case 6:
                                                                        case 9:
                                                                        case 11:
                                                                            if (jtf_time.Dayy > 30)
                                                                            {
                                                                                jtf_time.Dayy = 1;
                                                                                jtf_time.Month++;
                                                                            }
                                                                            break;

                                                                        default:
                                                                            if (jtf_time.Dayy > 31)
                                                                            {
                                                                                jtf_time.Dayy = 1;
                                                                                jtf_time.Month++;

                                                                                if (jtf_time.Month > 12)
                                                                                {
                                                                                    jtf_time.Month = 1;
                                                                                    jtf_time.Year++;

                                                                                    if (jtf_time.Year > 99)
                                                                                    {
                                                                                        jtf_time.Year = 0;
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                    }

                                                                }
                                                            }

                                                        }


                                                        

                                                            InforTime[0] = 0x00;
                                                            InforTime[1] = jtf_time.Sec;
                                                            InforTime[2] = jtf_time.Minute;
                                                            InforTime[3] = jtf_time.Hour;
                                                            InforTime[4] = jtf_time.Dayy;
                                                            InforTime[5] = jtf_time.Month;
                                                            InforTime[6] = jtf_time.Year;

                                                            TimeTemp = new DateTime(jtf_time.Year, jtf_time.Month, jtf_time.Dayy, jtf_time.Hour, jtf_time.Minute, jtf_time.Sec);

                                                       

                                                        #endregion




                                                    if (newProOutData[nDataNum].CarGpsY > 1000000000)
                                                    {
                                                        continue;
                                                    }

														sw.Write("\n");
													if (newProOutData[nDataNum].CarBreak == 0x02)
													{

														// 운전자 코드가 변경된 시점
														// 현재 데이터를 버리고, 다음 데이터로 포인터를 옮긴 후, 현재 파일을 저장하고,
														// 새로운 파일을 만들기 위해 #region Make Save File 최상단 for 문으로 이동
														nDataNum++;
														//	MessageBox.Show("newProOutData[nDataNum].CarBreak == 0x02");
														break;
													}

                                                   

													if (!newProInData[nDataNum].DataCheckSum)
													{
														//continue;
													//	MessageBox.Show("DataCheckSum error!!!!!!!!!");
													}
													/*	else
														{
															MessageBox.Show("error!!!!!!!!!");
														}*/

                                                   
                                                  
													sw.Write("{0:D4}", newProOutData[nDataNum].DayDriveDistance);

                                                  /*  if (newProOutData[nDataNum].TotalDriveDistance > 1000000)
                                                    {
                                                        newProOutData[nDataNum].TotalDriveDistance = newProOutData[nDataNum].TotalDriveDistance % 1000000;

                                                        if (newProOutData[nDataNum].TotalDriveDistance > 100000)
                                                        {
                                                            newProOutData[nDataNum].TotalDriveDistance = newProOutData[nDataNum].TotalDriveDistance % 100000;
                                                        }
                                                    }
                                                    else if(newProOutData[nDataNum].TotalDriveDistance > 100000)
                                                    {                                                                                                                
                                                        newProOutData[nDataNum].TotalDriveDistance = newProOutData[nDataNum].TotalDriveDistance % 100000;
                                                    }*/

													sw.Write("{0:D7}", newProOutData[nDataNum].TotalDriveDistance);

                                                   
													for (int a = 0; a < 7; a++)
													{
                                                        if (ReTime == false)
                                                        {

                                                            sw.Write("{0:D2}", newProOutData[nDataNum].InforTime[a]);
                                                        }
                                                        else
                                                        {
                                                            sw.Write("{0:D2}",PrnTIme[a]);
                                                           
                                                        }
                                                       
													
													}

													sw.Write("{0:D3}", newProOutData[nDataNum].CarSpeed);
													sw.Write("{0:D4}", newProOutData[nDataNum].CarRPM);
													sw.Write("{0:D}", newProOutData[nDataNum].CarBreak);

													

													if (newProOutData[nDataNum].CarBreak == 2)
													{

                                                      

														sw.Write("{0:X8}", newProOutData[nDataNum].CarGpsY);
														sw.Write("{0:X8}", newProOutData[nDataNum].CarGpsX);
														sw.Write("{0:X2}000", (newProOutData[nDataNum].CarDegree & 0xFF00) >> 8);

													
													}
													else
													{
                                                        if (newProOutData[nDataNum].CarGpsY != 0 && newProOutData[nDataNum].CarGpsY < 124000000 || newProOutData[nDataNum].CarGpsY > 131000000)
                                                        {                             
                                                            newProOutData[nDataNum].CarGpsY = OldCarGpsY;
                                                        }
                                                        if (newProOutData[nDataNum].CarGpsX != 0 && newProOutData[nDataNum].CarGpsX < 32000000 || newProOutData[nDataNum].CarGpsX > 38000000)
                                                        {
                                                            newProOutData[nDataNum].CarGpsX = OldCarGpsX;
                                                        }
                                                        
														sw.Write("{0:D9}", newProOutData[nDataNum].CarGpsY);
														sw.Write("{0:D9}", newProOutData[nDataNum].CarGpsX);
														sw.Write("{0:D3}", newProOutData[nDataNum].CarDegree);                 
														
													}

													sw.Write(newProOutData[nDataNum].CarAccelX);
													sw.Write(newProOutData[nDataNum].CarAccelY);
													sw.Write("{0:X2}", newProOutData[nDataNum].MachineStatus);

                                                    if (ReTime == true)
                                                    {
                                                        ReTime = false;
                                                        nDataNum--;

                                                    }

														
												}
											}
								
									}
									#endregion // Save file
						
							}
						}
						#endregion // Make Save File
				

					headCnt++;

					if (fs.Position >= (fs.Length - 0x38))
						break;
			
			} while (true);

            br.Close();
            fs.Close();
			bw.Close();
		/*	using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
			{
			    sw.Write("=============================================\r\n");
			    string strETime = string.Format("Total Error Count: {0:D}\r\n\r\n", nTotalErrorCount);
			    sw.Write(strETime);
			}*/

			string strRe = String.Format("분석이 완료되었습니다.\r\n\r\n{0} 데이터: {1}개", (bIs1sData ? "1초" : "0.01초"), nMakedFileNum);
            MessageBox.Show(this, strRe, "결과");
            workerObject.RequestStop();
        }

        private void 끝내기XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripLabelLeftPage_Click(object sender, EventArgs e)
        {
        }

        private void toolStripLabelRightPage_Click(object sender, EventArgs e)
        {
        }

        private void toolStripLabelGo_Click(object sender, EventArgs e)
        {
        }

        private void toolStripTextBoxNowPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Return)) || (e.KeyChar == Convert.ToChar(Keys.Enter)))
            {
                toolStripLabelGo_Click(sender, e);
            }
        }

        private void toolStripButtonLeftPage_Click(object sender, EventArgs e)
        {
            if (nDataPageStatus[0] == 1)
                nDataPageStatus[0] = nDataPageStatus[1];
            else
                nDataPageStatus[0]--;

            ArtWorkFunction(strOpenedFileName);
        }

        private void toolStripButtonRightPage_Click(object sender, EventArgs e)
        {
            if (nDataPageStatus[0] == nDataPageStatus[1])
                nDataPageStatus[0] = 1;
            else
                nDataPageStatus[0]++;

			if(nDataPageStatus[0] == nDataPageStatus[1])
			{
				LastPage_check = true;
			}

            ArtWorkFunction(strOpenedFileName);
        }

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            bool re = false;

            try
            {
                uint un = Convert.ToUInt32(toolStripTextBoxNowPage.Text);

                if (un > 0 && un <= nDataPageStatus[1])
                {
                    nDataPageStatus[0] = un;
                    re = true;

					if (un == nDataPageStatus[1])
					{
						LastPage_check = true;
					}
                }
            }
            catch
            {
                re = false;
            }
            finally
            {
                if (re)
                {
					
                    ArtWorkFunction(strOpenedFileName);
                }
                else
                {
					MessageBox.Show(this, "정확한 값을 다시 입력해 주세요", "오류");
                    toolStripTextBoxNowPage.Text = "";
                    toolStripTextBoxNowPage.Focus();
                }
            }
        }

        private void toolStripButtonViewSetup_Click(object sender, EventArgs e)
        {
            FormInputTerm fIT = new FormInputTerm(this);
            fIT.StartPosition = FormStartPosition.CenterParent;

            if (fIT.ShowDialog() == DialogResult.OK)
            {
				MessageBox.Show(this, "OK", "OK");
            }
        }

		private void toolStripButton5_Click_1(object sender, EventArgs e)
		{
			string formName = "Form5";

			foreach (System.Windows.Forms.Form theForm in this.MdiChildren)
			{
				if (formName.Equals(theForm.Name))
				{
					//해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
					theForm.BringToFront();
					theForm.Focus();
					return;
				}
			}

			Form5 DbForm = new Form5();
		
			DbForm.MdiParent = this;
			DbForm.Show();
			DbForm.FillData();
		}

		private void 지도ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string formName = "MainForm";

			foreach (System.Windows.Forms.Form theForm in this.MdiChildren)
			{
				if (formName.Equals(theForm.Name))
				{
					//해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
					theForm.BringToFront();
					theForm.Focus();
					return;
				}
			}

			MainForm MainMap = new MainForm(this);

			MainMap.MdiParent = this;
			MainMap.Show();
		
		}

		private void toolStripButton5_Click_2(object sender, EventArgs e)
		{
		 지도ToolStripMenuItem_Click( sender,  e);
		}

		private void toolStripButton8_Click(object sender, EventArgs e)
		{
			jTF파일열기ToolStripMenuItem_Click(sender, e);
		}

		private void jTF파일열기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "Open File";
			openFileDialog1.Filter = "*.JTF|*.JTF";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{

				string dirName = openFileDialog1.FileName;
				char[] fileOnly = new char[openFileDialog1.SafeFileName.Length];
				//fileOnly[0] = '\\';
				//fileOnly[1] = '\\';
				for (int i = 0; i < openFileDialog1.SafeFileName.Length; i++)
					fileOnly[i] = openFileDialog1.SafeFileName[i];
				dirName = dirName.TrimEnd(fileOnly);
				Jtf_Convert(openFileDialog1.FileName, dirName);
			}

		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (dataForm.dragAndDropListView1.Items.Count != 0)
            {
                FormExecel formExecel = new FormExecel(this);

                formExecel.MdiParent = this;
                formExecel.Show();
            }
            else
            {

                MessageBox.Show("운행 데이터가 없습니다.");
            }
          //  Excel_print();
        }
        public void Excel_print()
        {


            Excel.ApplicationClass excel = new Excel.ApplicationClass();
            int colIndex = 0;
            int rowIndex = 1;
            excel.Application.Workbooks.Add(true);

            for (int i = 0; i < dataForm.dragAndDropListView1.Columns.Count; i++)
            {
                colIndex++;
                excel.Cells[1, colIndex] = dataForm.dragAndDropListView1.Columns[i].Text;
            }
            int start = Int32.Parse(starttext);
            int end = Int32.Parse(endtext);

            if (start != 0)
            {
                start -= 1;
            }
            string page = toolStripTextBoxNowPage.Text;
            int n = 0;
            if (start > 999)
            {

            //    page = toolStripTextBoxNowPage.Text;
                n = Int32.Parse(page);
                n =n-1;
                start = start - (n * 10000);
                end = end - (n * 10000);
            }
            if (start > 10000 || end > 10000)
            {
                MessageBox.Show("잘못 입력 하였습니다.");
            }
            else
            {

                for (int i = start; i < end; i++)
                {
                    rowIndex++;
                    colIndex = 0;
                    for (int j = 0; j < dataForm.dragAndDropListView1.Items[i].SubItems.Count; j++)
                    {
                        colIndex++;
                        excel.Cells[rowIndex, colIndex] = dataForm.dragAndDropListView1.Items[i].SubItems[j].Text;
                        //     excel.Cells.AutoOutline();

                    }
                }
                excel.Visible = true;
            }


            //excel.Save(filePath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            string path = Application.StartupPath + "\\ConvertTacho.ini";

            string RValue = "";
            int num = 0;
            RValue = inicls.GetIniValue("Init", "TotalViewer", path);  //파싱 버젼 선택 
            num = Convert.ToInt32(RValue);


            if (num == 1)
            {
                TotalVersion = true;
                label1.Text = "Total Version";
            }
            else
            {
                TotalVersion = false;
            }
           

            RValue = inicls.GetIniValue("Init", "Repair time length", path);  // 
            SecTime_Length = Convert.ToInt32(RValue);

            //KYJ
            string formName = "MainForm";

            foreach (System.Windows.Forms.Form theForm in this.MdiChildren)
            {
                if (formName.Equals(theForm.Name))
                {
                    //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                    theForm.BringToFront();
                    theForm.Focus();
                    return;
                }
            }

            MainForm MainMap = new MainForm(this);

            MainMap.MdiParent = this;

            MainMap.Show();




        }
        private void TotalConvertFunction(string strFname, string dirName)
        {
            //파일 읽어서 버퍼에 저장
            string saveFname;   // *.TXT 파일의 파일명
            string saveFname2;	// Temporary TXT 파일명
            byte cs, csCal;     // cs: 파일로부터 읽어들인 CheckSum Data, csCal: 프로그램이 계산한 CheckSum Data
            int i = 0;
            int nDataNum;
            bool TimeCheck = false;
            bool bIs1sData = false;
            int nMakedFileNum = 0;

            // *.TXT 파일명 결정용 변수
            int saveYYMMDD = 100101;
            string saveCarNum = "서울01가9876";
            int headCnt = 1;

            bool bIsDriverCodeChanged = false;
            bool bIsSaveDataTrue = false;

            byte[] transTime = new byte[4];

            DriverCodeTemp.Clear();

            FileStream fs = new FileStream(strFname, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            BinaryWriter bw = new BinaryWriter(fs);
            JTF_Time jtf_time = new JTF_Time();
            ///bw.Write(0xff);


            //if(fs.Length < MINIMUMDATALENGTH)
            if (fs.Length < (56 + 32 + 56))
            {
                // 읽어들인 *.TMF 파일의 길이가 최소 데이터 길이보다 작을 경우
                MessageBox.Show(this, "잘못된 파일", "결과");
                return;
            }

            //int nTotalErrorCount = 0;
            //string strError = strFname + "_Error.txt";
            //using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
            //{
            //    sw.Write("\r\n=============================================\r\n");
            //    sw.Write("f.Position, 에러 발생 시간,      cs,   cal_CS\r\n");
            //    sw.Write("=============================================\r\n");
            //}


            //////////// Sampling Time Read/////////////



            do
            {
                NewProInputHeader newProInHeader = new NewProInputHeader();


                //---------------------------------------------------------------  1Sec Time Make End ----------------------------------------------------

                #region Read & store header information from Input file
                {
                    try
                    {
                        // Read & store header information from Input file
                        newProInHeader.TachoTransTime = br.ReadBytes(4);
                        newProInHeader.TachoDataSize = br.ReadBytes(3);
                        newProInHeader.ModelName = br.ReadBytes(10);
                        newProInHeader.CarNum = br.ReadBytes(17);
                        newProInHeader.CarType = br.ReadByte();
                        newProInHeader.CarRegistNum = br.ReadBytes(6);
                        newProInHeader.OperatorRegistNum = br.ReadBytes(5);
                        newProInHeader.DriverCode = br.ReadBytes(9);
                        cs = br.ReadByte();
                    }
                    catch (EndOfStreamException)
                    {
                        MessageBox.Show(this, "헤더 - 읽기 실패", "결과");
                        return;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this, "잘못된 파일", "결과");
                        return;
                    }
                }
                #endregion // Read & store header information from Input file

                #region Calculate & compare header checkSum
                {
                    // Calculate & compare header checkSum
                    csCal = 0x00;
                    for (i = 0; i < 4; i++)
                        csCal += newProInHeader.TachoTransTime[i];
                    for (i = 0; i < 3; i++)
                        csCal += newProInHeader.TachoDataSize[i];
                    for (i = 0; i < 10; i++)
                        csCal += newProInHeader.ModelName[i];
                    for (i = 0; i < 17; i++)
                        csCal += (byte)(newProInHeader.CarNum[i]);
                    csCal += newProInHeader.CarType;
                    for (i = 0; i < 6; i++)
                        csCal += newProInHeader.CarRegistNum[i];
                    for (i = 0; i < 5; i++)
                        csCal += newProInHeader.OperatorRegistNum[i];
                    for (i = 0; i < 9; i++)
                        csCal += newProInHeader.DriverCode[i];

                    if (cs == csCal)
                    //if (true)
                    {
                        newProInHeader.HeaderCheckSum = true;
                    }
                    else
                    {
                        //	newProInHeader.HeaderCheckSum = false;
                        //	MessageBox.Show(this, "Header Checksum Error", "결과");
                        //	return;
                    }
                }
                #endregion // Calculate & compare header checkSum

                int arrNum = (int)((newProInHeader.TachoDataSize[2] << 16) | (newProInHeader.TachoDataSize[1] << 8) | newProInHeader.TachoDataSize[0]);

                if (arrNum == 0) //YJK TachoDataSize 가 0이면 전체 파일 읽어오기 임. 
                {
                    arrNum = (int)(fs.Length - 0x38); // 파일 크기에서 헤더 사이즈 뺀다. 
                }

                arrNum /= 32;

                //	byte[] InforTime = new byte[7];

                NewProInputData[] newProInData = new NewProInputData[arrNum];


                #region Read & store data from Input file
                {
                    // Read & store data from Input file

                    for (nDataNum = 0; nDataNum < arrNum; nDataNum++)
                    {

                        byte[] InforTime = new byte[7];
                        try
                        {

                            newProInData[nDataNum].DayDriveDistance = br.ReadBytes(2);
                            newProInData[nDataNum].TotalDriveDistance = br.ReadBytes(3);


                            //newProInData[nDataNum].InforTime = br.ReadBytes(7);

                            jtf_time.time = br.ReadBytes(7);

                            /*	jtf_time.Year = jtf_time.time[6];
                                jtf_time.Month = jtf_time.time[5];
                                jtf_time.Dayy = jtf_time.time[4];
                                jtf_time.Hour = jtf_time.time[3];
                                jtf_time.Minute = jtf_time.time[2];
                                jtf_time.Sec = jtf_time.time[1];
                                jtf_time.time[0] = 0x00;

                                if (jtf_time.Year == 0x00 || jtf_time.Month == 0x00 || jtf_time.Dayy ==0x00)
                                {
                                     br.ReadBytes(2);  //14
                                     br.ReadBytes(2);
                                     br.ReadBytes(4);
                                     br.ReadBytes(4);
                                     br.ReadBytes(2);
                                     br.ReadByte();
                                     br.ReadByte();
                                     br.ReadBytes(2);
                                     br.ReadByte();
                                     br.ReadByte();
                                     nDataNum--;
                                     arrNum --;
                                     continue;
                                }
                                else if (jtf_time.Month > 0x0c || jtf_time.Dayy > 0x1f || jtf_time.Hour > 0x17 || jtf_time.Minute > 0x3b || jtf_time.Sec > 0x3b)
                                {

                                    br.ReadBytes(2);  //14
                                    br.ReadBytes(2);
                                    br.ReadBytes(4);
                                    br.ReadBytes(4);
                                    br.ReadBytes(2);
                                    br.ReadByte();
                                    br.ReadByte();
                                    br.ReadBytes(2);
                                    br.ReadByte();
                                    br.ReadByte();
                                    nDataNum--;
                                    arrNum--;
                                    continue;

                                }
                                else if (jtf_time.Sec > 59)
                                {
                                    jtf_time.Sec = 00;
                                }
                            //	transTime[0] = 0x14;
                            //	transTime[1] = jtf_time.Year;
                            //	transTime[2] = jtf_time.Month; 
                            //	transTime[3] = jtf_time.Dayy;*/

                            newProInData[nDataNum].InforTime = jtf_time.time;


                            //---------------------------------------------------------------  Time Make ----------------------------------------------------
                            /*	if (arrNum > 0x7d0)
                                //if (arrNum > 0)
                                {


                                    if (TimeCheck == false)
                                    {


                                        jtf_time.time = br.ReadBytes(7);
                                        jtf_time.Year = jtf_time.time[6];
                                        jtf_time.Month = jtf_time.time[5];
                                        jtf_time.Dayy = jtf_time.time[4];
                                        jtf_time.Hour = jtf_time.time[3];
                                        jtf_time.Minute = jtf_time.time[2];
                                        jtf_time.Sec = jtf_time.time[1];
                                        jtf_time.time[0] = 0x00;

                                        if (jtf_time.Year == 0x00 || jtf_time.Month == 0x00 || jtf_time.Dayy ==0x00)
                                        {
                                             br.ReadBytes(2);  //14
                                             br.ReadBytes(2);
                                             br.ReadBytes(4);
                                             br.ReadBytes(4);
                                             br.ReadBytes(2);
                                             br.ReadByte();
                                             br.ReadByte();
                                             br.ReadBytes(2);
                                             br.ReadByte();
                                             br.ReadByte();
                                             nDataNum--;
                                             arrNum --;
                                             continue;
                                        }
                                        else if (jtf_time.Month > 0x0c || jtf_time.Dayy > 0x1f || jtf_time.Hour > 0x17 || jtf_time.Minute > 0x3b || jtf_time.Sec > 0x3b)
                                        {

                                            br.ReadBytes(2);  //14
                                            br.ReadBytes(2);
                                            br.ReadBytes(4);
                                            br.ReadBytes(4);
                                            br.ReadBytes(2);
                                            br.ReadByte();
                                            br.ReadByte();
                                            br.ReadBytes(2);
                                            br.ReadByte();
                                            br.ReadByte();
                                            nDataNum--;
                                            arrNum--;
                                            continue;

                                        }

                                        transTime[0] = 0x14;
                                        transTime[1] = jtf_time.Year;
                                        transTime[2] = jtf_time.Month; 
                                        transTime[3] = jtf_time.Dayy;

                                        newProInData[nDataNum].InforTime = jtf_time.time;
                                    }


                                    if (TimeCheck == true)
                                    {
                                        br.ReadBytes(7);

                                        newProInData[nDataNum].InforTime = jtf_time.time;



								
                                        jtf_time.Sec++;

                                        if (jtf_time.Sec > 59)
                                        {
                                            jtf_time.Sec = 0;

                                            jtf_time.Minute++;
                                            if (jtf_time.Minute > 59)
                                            {
                                                jtf_time.Minute = 0;

                                                jtf_time.Hour++;
                                                if (jtf_time.Hour > 23)
                                                {
                                                    jtf_time.Hour = 0;
                                                    jtf_time.Dayy++;
                                                    switch (jtf_time.Month)
                                                    {
                                                        case 2:
                                                            if (IsLeapYear(jtf_time.Year))
                                                            {
                                                                if (jtf_time.Dayy > 29)
                                                                {
                                                                    jtf_time.Dayy = 1;
                                                                    jtf_time.Month++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (jtf_time.Dayy > 28)
                                                                {
                                                                    jtf_time.Dayy = 1;
                                                                    jtf_time.Month++;
                                                                }
                                                            }
                                                            break;

                                                        case 4:
                                                        case 6:
                                                        case 9:
                                                        case 11:
                                                            if (jtf_time.Dayy > 30)
                                                            {
                                                                jtf_time.Dayy = 1;
                                                                jtf_time.Month++;
                                                            }
                                                            break;

                                                        default:
                                                            if (jtf_time.Dayy > 31)
                                                            {
                                                                jtf_time.Dayy = 1;
                                                                jtf_time.Month++;

                                                                if (jtf_time.Month > 12)
                                                                {
                                                                    jtf_time.Month = 1;
                                                                    jtf_time.Year++;

                                                                    if (jtf_time.Year > 99)
                                                                    {
                                                                        jtf_time.Year = 0;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }

                                                }
                                            }

                                        }


									

                                        InforTime[0] = 0x00;
                                        InforTime[1] = jtf_time.Sec;
                                        InforTime[2] =jtf_time.Minute;
                                        InforTime[3] =jtf_time.Hour;
                                        InforTime[4] = jtf_time.Dayy;
                                        InforTime[5] = jtf_time.Month;
                                        InforTime[6] = jtf_time.Year;

                                        newProInData[nDataNum].InforTime = InforTime;

                                    }
                                }
                                else
                                {
                                    newProInData[nDataNum].InforTime = br.ReadBytes(7);
                                }*/
                            //---------------------------------------------------------------  Time Make End ----------------------------------------------------							

                            newProInData[nDataNum].CarRPM = br.ReadBytes(2);
                            newProInData[nDataNum].Reserved = br.ReadBytes(2);
                            newProInData[nDataNum].InforGPSY = br.ReadBytes(4);
                            newProInData[nDataNum].InforGPSX = br.ReadBytes(4);
                            newProInData[nDataNum].CarDegree = br.ReadBytes(2);
                            newProInData[nDataNum].AccelX = br.ReadByte();
                            newProInData[nDataNum].AccelY = br.ReadByte();
                            newProInData[nDataNum].MachineStatus = br.ReadBytes(2);
                            newProInData[nDataNum].CarSpeed = br.ReadByte();
                            cs = br.ReadByte();
                        }
                        catch (EndOfStreamException)
                        {
                            MessageBox.Show(this, "데이터 - 읽기 실패", "결과");
                            return;
                        }
                        catch (Exception)
                        {

                            MessageBox.Show(this, "잘못된 파일", "결과");
                            return;
                        }

                        // Calculate & Compare CheckSum
                        csCal = 0x00;
                        for (i = 0; i < 2; i++)
                            csCal += newProInData[nDataNum].DayDriveDistance[i];
                        for (i = 0; i < 3; i++)
                            csCal += newProInData[nDataNum].TotalDriveDistance[i];
                        for (i = 0; i < 7; i++)
                            csCal += newProInData[nDataNum].InforTime[i];
                        for (i = 0; i < 2; i++)
                            csCal += newProInData[nDataNum].CarRPM[i];
                        for (i = 0; i < 2; i++)
                            csCal += newProInData[nDataNum].Reserved[i];
                        for (i = 0; i < 4; i++)
                            csCal += newProInData[nDataNum].InforGPSX[i];
                        for (i = 0; i < 4; i++)
                            csCal += newProInData[nDataNum].InforGPSY[i];
                        for (i = 0; i < 2; i++)
                            csCal += newProInData[nDataNum].CarDegree[i];
                        csCal += newProInData[nDataNum].AccelX;
                        csCal += newProInData[nDataNum].AccelY;
                        for (i = 0; i < 2; i++)
                            csCal += newProInData[nDataNum].MachineStatus[i];
                        csCal += newProInData[nDataNum].CarSpeed;

                        if (cs == csCal)
                        //if (true)
                        {
                            newProInData[nDataNum].DataCheckSum = true;
                        }
                        else
                        {

                            //newProInData[nDataNum].DataCheckSum = true;

                            // 에러시 fs.Position, time, checksum(cs, calcs)
                            // & total err cnt

                            //nTotalErrorCount++;

                            //using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
                            //{
                            //    string strETime = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
                            //        2000 + newProInData[nDataNum].InforTime[6], newProInData[nDataNum].InforTime[5], newProInData[nDataNum].InforTime[4],
                            //        newProInData[nDataNum].InforTime[3], newProInData[nDataNum].InforTime[2], newProInData[nDataNum].InforTime[1]);
                            //    string strE = string.Format("0x{0:X8}, {1}, 0x{2:X2}, 0x{3:X2}\r\n",
                            //                                fs.Position, strETime, cs, csCal);
                            //    sw.Write(strE);
                            //}

                            //	newProInData[nDataNum].DataCheckSum = false;
                            //	MessageBox.Show(this, "Data Checksum Error","결과");
                            //	return;
                        }
                        TimeCheck = true;
                    }
                }
                #endregion // Read & store data from Input file


                NewProOutputHeader newProOutHeader = new NewProOutputHeader();
                NewProOutputData[] newProOutData = new NewProOutputData[arrNum];
                //	NewProOutputData[] newProOutData = new NewProOutputData[2000];
                string tmpDirverCode = "";

                #region Transfer & store header information
                {
                    /* Header 채우기 */
                    // [20] 운행기록장치 모델명
                    string strMN = "##########";



                    for (i = 0; i < 10; i++)
                    {
                        strMN += String.Format("{0:C}", Convert.ToChar(newProInHeader.ModelName[i]));
                        //test[i] = 0x23; 
                    }

                    newProOutHeader.ModelName = strMN;

                    // [17] 차대번호
                    string strCN = "";
                    for (i = 0; i < 17; i++)
                    {
                        if (newProInHeader.CarNum[16 - i] == 0x00)
                            strCN += String.Format("0");
                        else
                            strCN += String.Format("{0:C}", (char)(newProInHeader.CarNum[16 - i]));
                    }

                    newProOutHeader.CarNum = strCN;

                    // [2] 자동차 유형
                    ushort CT = (ushort)(newProInHeader.CarType);
                    newProOutHeader.CarType = CT;

                    // [12] 자동차 등록번호
                    string strCRN = "";


                    switch (newProInHeader.CarRegistNum[4])
                    {
                        case 0x01: strCRN = "서울"; break;
                        case 0x02: strCRN = "인천"; break;
                        case 0x03: strCRN = "대전"; break;
                        case 0x04: strCRN = "광주"; break;
                        case 0x06: strCRN = "울산"; break;
                        case 0x07: strCRN = "부산"; break;
                        case 0x08: strCRN = "경기"; break;
                        case 0x09: strCRN = "강원"; break;
                        case 0x10: strCRN = "충북"; break;
                        case 0x11: strCRN = "충남"; break;
                        case 0x12: strCRN = "전북"; break;
                        case 0x13: strCRN = "전남"; break;
                        case 0x14: strCRN = "경북"; break;
                        case 0x15: strCRN = "경남"; break;

                        case 0xa: strCRN = "충북"; break;
                        case 0xb: strCRN = "충남"; break;
                        case 0xc: strCRN = "전북"; break;
                        case 0xd: strCRN = "전남"; break;
                        case 0xe: strCRN = "경북"; break;
                        case 0xf: strCRN = "경남"; break;

                        case 0x16: strCRN = "제주"; break;
                        default: strCRN = "없음"; break;
                    }

                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[3] >> 4));
                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[3] & 0x0F));

                    string carRegistNumSign = " 가나다라마바사아자차카타파하거너더러머버서어저처커터퍼허고노도로모보소오조초코토포호구누두루무부수우주추쿠투푸후그느드르므브스으즈츠크트프흐기니디리미비시이지치키티피히";

                    int tmpCarRegiNum = ((int)((newProInHeader.CarRegistNum[2] >> 4) * 10) + ((int)(newProInHeader.CarRegistNum[2] & 0x0F)));

                    if ((tmpCarRegiNum == 0) || (tmpCarRegiNum >= carRegistNumSign.Length))
                        tmpCarRegiNum = 0;




                    strCRN += carRegistNumSign[tmpCarRegiNum];

                    /*	switch (newProInHeader.CarRegistNum[2])
                        {
                            case 0x04: strCRN += "라"; break;
                            case 0x06: strCRN += "바"; break;
                            case 0x07: strCRN += "사"; break;
                            case 0x08: strCRN += "아"; break;
                            case 0x10: strCRN += "차"; break;
                            case 0x11: strCRN += "카"; break;
                            case 0x12: strCRN += "타"; break;
                            case 0x13: strCRN += "파"; break;
                            default: strCRN += " "; break;
                        }*/

                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[1] >> 4));     //car number
                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[1] & 0x0F));
                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[0] >> 4));
                    strCRN += String.Format("{0:D}", (newProInHeader.CarRegistNum[0] & 0x0F));




                    //	UTF7Encoding enc = new UTF7Encoding();
                    //	UnicodeEncoding enc = new UnicodeEncoding(); 
                    //	UTF8Encoding enc = new UTF8Encoding();						
                    /*		byte[] buffer = enc.GetBytes(strCRN); 
                            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                            Encoding littleendian = Encoding.Unicode;
                            Encoding bigendian = Encoding.BigEndianUnicode;
                            Byte[] ByteArray;
                            string str;
                            ByteArray = utf8.GetBytes(strCRN);   
                            // 우선 UTF8 스트링을 바이트로 쪼개서
                            ByteArray = Encoding.Convert(utf8, littleendian, ByteArray);  // 다음 UTF8 바이트 코드를 LittleEndian 바이트 코드로 변환하고 					                                                    // 변환된 바이트 코드를 다시 스트링으로 합치는 거에여..
                            strCRN = littleendian.GetString(ByteArray);*/
                    //	byte[] buffer = utf8.GetBytes(strCRN);



                    newProOutHeader.CarRegistNum = strCRN;

                    // [10] 운송사업자 등록번호
                    string strORN = "";
                    strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[4]);
                    strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[3]);
                    strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[2]);
                    strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[1]);
                    strORN += String.Format("{0:X2}", newProInHeader.OperatorRegistNum[0]);

                    newProOutHeader.OperatorRegistNum = strORN;


                    // [18] 운전자 코드
                    string strDC = "";
                    int noneZeroPoint = 0;
                    for (i = 0; i < 9; i++)
                    {
                        if (newProInHeader.DriverCode[8 - i] != 0x00)
                        {
                            noneZeroPoint = 9 - i;
                            break;
                        }
                    }
                    if (noneZeroPoint <= 4) noneZeroPoint = 4;
                    else noneZeroPoint = 9;

                    for (i = 8; i >= 0; i--)
                    {
                        if (i >= noneZeroPoint)
                        {
                            strDC += "##";  // 빈자리 '0'으로 채우기 -> 빈자리 '#'으로 채우기
                            tmpDirverCode += "00";
                        }
                        else
                        {
                            strDC += String.Format("{0:X2}", newProInHeader.DriverCode[i]);
                            tmpDirverCode += String.Format("{0:X2}", newProInHeader.DriverCode[i]);
                        }
                    }
                    newProOutHeader.DriverCode = strDC;
                }
                #endregion // Transfer & store header information

                #region Transfer & store data
                {
                    /* Data 채우기 */
                    for (nDataNum = 0; nDataNum < arrNum; nDataNum++)
                    {
                        // Data 무결성 확인
                        try
                        {
                            if (newProInData[nDataNum].DataCheckSum)
                            {
                                newProOutData[nDataNum].DataTrust = true;
                            }
                            else
                            {
                                newProOutData[nDataNum].DataTrust = false;
                                //	MessageBox.Show("data error3");
                                //	continue;
                            }

                            bIsDriverCodeChanged = false;
                        }
                        catch (System.Exception e)
                        {
                            {
                                MessageBox.Show(e.Message);
                                MessageBox.Show("data error");

                            }
                        }


                        // [4] 일일 주행거리
                        UInt16 nDDD = (UInt16)((newProInData[nDataNum].DayDriveDistance[1] << 8) | (newProInData[nDataNum].DayDriveDistance[0]));
                        newProOutData[nDataNum].DayDriveDistance = nDDD;

                        // [7] 누적 주행거리
                        UInt32 nTDD = (UInt32)((newProInData[nDataNum].TotalDriveDistance[2] << 16)
                                                | (newProInData[nDataNum].TotalDriveDistance[1] << 8)
                                                | newProInData[nDataNum].TotalDriveDistance[0]);
                        newProOutData[nDataNum].TotalDriveDistance = nTDD;

                        // [14] 정보발생 일시
                        byte[] arrIT = new byte[7];
                        for (i = 0; i < 7; i++)
                        {
                            arrIT[i] = newProInData[nDataNum].InforTime[6 - i];
                        }
                        // 시간 체크

                        newProOutData[nDataNum].InforTime = arrIT;

                        /*	if (newProOutData[nDataNum].InforTime[1] == 0x00 || newProOutData[nDataNum].InforTime[2]==0x00)
                            {
                                TimeCheck = true;
                            }*/



                        // [3] 차량 속도 (Km/h)
                        newProOutData[nDataNum].CarSpeed = newProInData[nDataNum].CarSpeed;

                        // [4] 분당 엔진회전수 (RPM)
                        ushort ntRPM = (ushort)((newProInData[nDataNum].CarRPM[1] << 8) | newProInData[nDataNum].CarRPM[0]);
                        if (ntRPM > 9999) ntRPM = 9999; // '10. 8.26. choi15 비정상 데이터로 FF xx 값이 들어올 경우가 생김
                        newProOutData[nDataNum].CarRPM = ntRPM;

                        // [1] 브레이크 신호
                        if ((newProInData[nDataNum].MachineStatus[1] == 0x38) && (newProInData[nDataNum].MachineStatus[0] == 0x00))
                        {
                            // 운전자 코드가 변경됐음을 인지
                            bIsDriverCodeChanged = true;
                            newProOutData[nDataNum].CarBreak = 2;
                        }
                        else
                        {
                            newProOutData[nDataNum].CarBreak = (byte)(((newProInData[nDataNum].MachineStatus[1] & 0x80) == 0x80) ? 1 : 0);
                        }

                        if (bIsDriverCodeChanged)
                        {
                            newProOutData[nDataNum].CarGpsX = (uint)((newProInData[nDataNum].InforGPSX[3] << 24) | (newProInData[nDataNum].InforGPSX[2] << 16) | (newProInData[nDataNum].InforGPSX[1] << 8) | newProInData[nDataNum].InforGPSX[0]);
                            newProOutData[nDataNum].CarGpsY = (uint)((newProInData[nDataNum].InforGPSY[3] << 24) | (newProInData[nDataNum].InforGPSY[2] << 16) | (newProInData[nDataNum].InforGPSY[1] << 8) | newProInData[nDataNum].InforGPSY[0]);
                            newProOutData[nDataNum].CarDegree = (ushort)((newProInData[nDataNum].CarDegree[1] << 8) | (newProInData[nDataNum].CarDegree[0]));

                            // Header의 운전자 코드를 변경
                            byte[] dct = new byte[9];

                            //dct[0] = newProInData[nDataNum].InforGPSY[2];
                            //dct[1] = newProInData[nDataNum].InforGPSY[1];
                            //dct[2] = newProInData[nDataNum].InforGPSY[0];
                            //dct[3] = newProInData[nDataNum].CarDegree[1];
                            //dct[4] = newProInData[nDataNum].InforGPSY[3];
                            //dct[5] = newProInData[nDataNum].InforGPSX[3];
                            //dct[6] = newProInData[nDataNum].InforGPSX[2];
                            //dct[7] = newProInData[nDataNum].InforGPSX[1];
                            //dct[8] = newProInData[nDataNum].InforGPSX[0];

                            dct[0] = newProInData[nDataNum].InforGPSY[0];
                            dct[1] = newProInData[nDataNum].InforGPSY[1];
                            dct[2] = newProInData[nDataNum].InforGPSY[2];
                            dct[3] = newProInData[nDataNum].InforGPSY[3];
                            dct[4] = newProInData[nDataNum].InforGPSX[3];
                            dct[5] = newProInData[nDataNum].CarDegree[1];
                            dct[6] = newProInData[nDataNum].InforGPSX[0];
                            dct[7] = newProInData[nDataNum].InforGPSX[1];
                            dct[8] = newProInData[nDataNum].InforGPSX[2];

                            // [18] 운전자 코드
                            string strDC = "";
                            int noneZeroPoint = 0;
                            for (i = 0; i < 9; i++)
                            {
                                if (dct[8 - i] != 0x00)
                                {
                                    noneZeroPoint = 9 - i;
                                    break;
                                }
                            }
                            if (noneZeroPoint <= 4) noneZeroPoint = 4;
                            else noneZeroPoint = 9;

                            for (i = 8; i >= 0; i--)
                            {
                                if (i >= noneZeroPoint)
                                {
                                    strDC += "##";  // 빈자리 '0'으로 채우기 -> 빈자리 '#'으로 채우기
                                }
                                else
                                {
                                    strDC += String.Format("{0:X2}", dct[i]);
                                }
                            }
                            DriverCodeTemp.Add(strDC);
                        }
                        else
                        {
                            /* [18] 차량위치 */
                            // [9] GPS X 좌표

                            /*	if (newProInData[nDataNum].InforGPSX[0] == 0x4b && newProInData[nDataNum].InforGPSX[1] == 0x49 && newProInData[nDataNum].InforGPSX[2] == 0x37 && newProInData[nDataNum].InforGPSX[3] == 0x02)
                                {
                                    MessageBox.Show("0x4b");
                                }*/

                            /*	if (newProInData[nDataNum].InforGPSY[0] == 0x8f && newProInData[nDataNum].InforGPSY[1] == 0x26 && newProInData[nDataNum].InforGPSY[2] == 0x8a && newProInData[nDataNum].InforGPSY[3] == 0x07 && newProInData[nDataNum].InforGPSX[0] == 0x4b && newProInData[nDataNum].InforGPSX[1] == 0x49 && newProInData[nDataNum].InforGPSX[2] == 0x37 && newProInData[nDataNum].InforGPSX[3] == 0x02)
                            {
                                MessageBox.Show("11");
                            }*/



                            UInt32 nIGX = (UInt32)((newProInData[nDataNum].InforGPSX[3] << 24) | (newProInData[nDataNum].InforGPSX[2] << 16) | (newProInData[nDataNum].InforGPSX[1] << 8) | newProInData[nDataNum].InforGPSX[0]);
                            UInt32 nIGXD = nIGX / 1000000;
                            UInt32 nIGXM = nIGX % 1000000;
                            double fIGXM = ((double)nIGXM / 60.0) * 100;
                            nIGX = (UInt32)((nIGXD * 1000000) + fIGXM);
                            newProOutData[nDataNum].CarGpsX = nIGX;

                            // [9] GPS Y 좌표
                            UInt32 nIGY = (UInt32)((newProInData[nDataNum].InforGPSY[3] << 24) | (newProInData[nDataNum].InforGPSY[2] << 16) | (newProInData[nDataNum].InforGPSY[1] << 8) | newProInData[nDataNum].InforGPSY[0]);
                            UInt32 nIGYD = nIGY / 1000000;
                            UInt32 nIGYM = nIGY % 1000000;
                            double fIGYM = ((double)nIGYM / 60.0) * 100;
                            nIGY = (UInt32)((nIGYD * 1000000) + fIGYM);
                            newProOutData[nDataNum].CarGpsY = nIGY;

                            // [3] 위성항법장치 방위각 (GPS Degree)
                            UInt16 nCD = (UInt16)((newProInData[nDataNum].CarDegree[1] << 8) | (newProInData[nDataNum].CarDegree[0]));

                            if (nCD > 359)
                            {
                                nCD = 359;
                            }

                            newProOutData[nDataNum].CarDegree = nCD;
                        }

                        // [12] 가속도 (Km/sec2)
                        // [6] △Vx
                        byte bDeltaX = newProInData[nDataNum].AccelX;
                        string strDeltaX = "";
                        float gDeltaX;
                        if ((bDeltaX & 0x80) == 0x00)
                        {
                            gDeltaX = (float)(bDeltaX * 0.018 * 9.80665);
                            strDeltaX = "+";

                            if (gDeltaX < 1)
                                strDeltaX += "000";
                            else if (gDeltaX < 10)
                                strDeltaX += "00";
                            else if (gDeltaX < 100)
                                strDeltaX += "0";

                            strDeltaX += String.Format("{0:#.#}", gDeltaX);

                            if (strDeltaX.Length != 6)
                                strDeltaX += String.Format(".0");

                            newProOutData[nDataNum].CarAccelX = strDeltaX;
                        }
                        else
                        {
                            gDeltaX = (float)((256 - bDeltaX) * 0.018 * 9.80665);
                            strDeltaX = "-";
                            if (gDeltaX < 1)
                                strDeltaX += "000";
                            else if (gDeltaX < 10)
                                strDeltaX += "00";
                            else if (gDeltaX < 100)
                                strDeltaX += "0";

                            strDeltaX += String.Format("{0:#.#}", gDeltaX);

                            if (strDeltaX.Length != 6)
                                strDeltaX += String.Format(".0");

                            newProOutData[nDataNum].CarAccelX = strDeltaX;
                        }

                        // [6] △Vy
                        byte bDeltaY = newProInData[nDataNum].AccelY;
                        string strDeltaY = "";
                        float gDeltaY;
                        if ((bDeltaY & 0x80) == 0x00)
                        {
                            gDeltaY = (float)(bDeltaY * 0.018 * 9.80665);
                            strDeltaY = "+";
                            if (gDeltaY < 1)
                                strDeltaY += "000";
                            else if (gDeltaY < 10)
                                strDeltaY += "00";
                            else if (gDeltaY < 100)
                                strDeltaY += "0";

                            strDeltaY += String.Format("{0:#.#}", gDeltaY);

                            if (strDeltaY.Length != 6)
                                strDeltaY += String.Format(".0");

                            newProOutData[nDataNum].CarAccelY = strDeltaY;
                        }
                        else
                        {
                            gDeltaY = (float)((256 - bDeltaY) * 0.018 * 9.80665);
                            strDeltaY = "-";
                            if (gDeltaY < 1) strDeltaY += "000";
                            else if (gDeltaY < 10) strDeltaY += "00";
                            else if (gDeltaY < 100) strDeltaY += "0";

                            strDeltaY += String.Format("{0:#.#}", gDeltaY);

                            if (strDeltaY.Length != 6)
                                strDeltaY += String.Format(".0");

                            newProOutData[nDataNum].CarAccelY = strDeltaY;
                        }

                        // [2] 기기 및 통신 상태 코드 (백업 수집 주기 내)
                        byte bMS = 0;
                        ushort usMS = (ushort)((newProInData[nDataNum].MachineStatus[1] << 8) | (newProInData[nDataNum].MachineStatus[0]));

                        if ((usMS & 0x0001) == 0x0001)  // 11 위치추적장치 (GPS수신기) 이상
                        {
                            bMS = 0x11;
                        }
                        if ((usMS & 0x0002) == 0x0002)  // 12 속도센서 이상
                        {
                            bMS = 0x12;
                        }
                        if ((usMS & 0x0004) == 0x0004)  // 13 RPM 센서 이상
                        {
                            bMS = 0x13;
                        }
                        if ((usMS & 0x0008) == 0x0008)  // 14 브레이크 감지센서 이상
                        {
                            bMS = 0x14;
                        }
                        if ((usMS & 0x0010) == 0x0010)  // 21 센서 입력부 장치 이상
                        {
                            bMS = 0x21;
                        }
                        if ((usMS & 0x0020) == 0x0020)  // 22 센서 출력부 장치 이상
                        {
                            bMS = 0x22;
                        }
                        if ((usMS & 0x0040) == 0x0040)  // 31 데이터 출력부 장치 이상
                        {
                            bMS = 0x31;
                        }
                        if ((usMS & 0x0080) == 0x0080)  // 32 통신장치 이상
                        {
                            bMS = 0x32;
                        }
                        if ((usMS & 0x0100) == 0x0100)  // 41 운행거리 산정 이상
                        {
                            bMS = 0x41;
                        }
                        if ((usMS & 0x0200) == 0x0200)  // 99 전원공급 이상
                        {
                            bMS = 0x99;
                        }

                        ///
                        if ((newProInData[nDataNum].MachineStatus[0] == 0xFF) || (newProInData[nDataNum].MachineStatus[1] == 0xFF))
                        {
                            //bMS = 0x00;
                            if (nDataNum == 0)
                                bMS = 0x00;
                            else
                                bMS = newProOutData[nDataNum - 1].MachineStatus;
                        }

                        if ((newProOutData[nDataNum].CarGpsX == 0) || (newProOutData[nDataNum].CarGpsY == 0))
                            bMS = 0x11; // 위치추적장치 (GPS) 이상으로 강제화
                        ///

                        newProOutData[nDataNum].MachineStatus = bMS;
                        bIsSaveDataTrue = true;
                    }
                }
                #endregion // Transfer & store data

                //	if (arrNum > 0x7cf)
                //	{
                #region // Make Save File
                {
                    bool bIsStandAloneData = false;
                    nDataNum = 0;
                    int nC = DriverCodeTemp.Count;
                    if ((bIsSaveDataTrue) && (nC == 0))
                    {	// 저장할 데이터가 있는데, 운전자코드가 한번도 변경되지 않은 경우를 위한 루틴
                        nC = 1;
                        bIsStandAloneData = true;
                        bIsSaveDataTrue = false;
                    }

                    for (int driverN = 0; driverN < nC; driverN++)
                    {
                        string fileNameLeft, fileNameRight;
                        int tempHeadCnt;

                        #region // Make Save File Name
                        {
                            int nYYYYMMDD;
                            if (newProInHeader.TachoTransTime[0] == 0x00 || newProInHeader.TachoTransTime[0] == 0x00)
                            {
                                nYYYYMMDD = +((int)(transTime[0]) * 1000000)
                                           + ((int)(transTime[1]) * 10000)
                                           + ((int)(transTime[2]) * 100)
                                           + (int)(transTime[3]);
                            }
                            else
                            {


                                nYYYYMMDD = +((int)(newProInHeader.TachoTransTime[3]) * 1000000)
                                               + ((int)(newProInHeader.TachoTransTime[2]) * 10000)
                                               + ((int)(newProInHeader.TachoTransTime[1]) * 100)
                                               + (int)(newProInHeader.TachoTransTime[0]);
                            }

                            saveFname = String.Format("{0:D}", nYYYYMMDD);
                            saveFname += "-";
                            saveFname += newProOutHeader.CarRegistNum;
                            saveFname += "-";

                            if (arrNum > 2)
                            {
                                if ((newProInData[0].InforTime[0] == newProInData[1].InforTime[0])
                                    && (newProInData[0].InforTime[0] == newProInData[2].InforTime[0]))
                                {
                                    saveFname += "S";	// 1초 데이터
                                    bIs1sData = true;
                                }
                                else
                                {
                                    saveFname += "C";	// 0.01초 데이터
                                }
                            }
                            else
                            {
                                saveFname += "C";	// 0.01초 데이터
                            }
                            saveFname += "-";

                            fileNameLeft = saveFname;	// '11. 3. 2(추가)

                            if (saveYYMMDD != nYYYYMMDD)
                                headCnt = 1;
                            if (saveCarNum != newProOutHeader.CarRegistNum)
                                headCnt = 1;

                            tempHeadCnt = headCnt;

                            saveFname += String.Format("{0:D2}", headCnt);
                            saveFname += "-";

                            if (driverN == nC)
                                DriverCodeTemp.Add(newProOutHeader.DriverCode);

                            if (bIsStandAloneData)
                            {
                                saveFname += newProOutHeader.DriverCode.Substring(11, 7);
                                fileNameRight = "-" + newProOutHeader.DriverCode.Substring(11, 7) + ".TXT";	// '11. 3. 2(추가)
                            }
                            else
                            {
                                saveFname += DriverCodeTemp[driverN].Substring(11, 7);
                                fileNameRight = "-" + DriverCodeTemp[driverN].Substring(11, 7) + ".TXT";	// '11. 3. 2(추가)
                            }

                            saveFname += ".TXT";
                            saveFname2 = "_" + saveFname;

                            saveYYMMDD = nYYYYMMDD;
                            saveCarNum = newProOutHeader.CarRegistNum;
                        }
                        #endregion // Make Save File Name

                        #region Confirm save filename
                        {
                            // 불러들인 *.TMF 파일의 디렉토리
                            DirectoryInfo dir = new DirectoryInfo(dirName);

                            // *.TXT 파일명에서 .TXT를 제외한 실제 파일명 추출
                            char[] trimChars = { '.', 'T', 'X', 'T' };

                            bool bisLoopContinue = true;
                            do
                            {
                                string searchingfor = saveFname.TrimEnd(trimChars);

                                // Partial matches
                                // 저장할 파일명(.TXT 제외)과 같은 파일이 이미 있는지 조사
                                FileInfo[] partial = dir.GetFiles("*" + searchingfor + "*", SearchOption.TopDirectoryOnly);

                                if (partial.Length != 0)
                                {
                                    saveFname = fileNameLeft + String.Format("{0:D2}", ++tempHeadCnt) + fileNameRight;
                                    /*
                                    // 같은 파일이 이미 있다면, Dialog를 통해 사용자에게 저장할 파일명 입력받음
                                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                                    saveFileDialog1.Title = "Create Text File";
                                    saveFileDialog1.InitialDirectory = dirName;
                                    saveFileDialog1.FileName = saveFname;
                                    saveFileDialog1.Filter = "Text file(*.TXT)|*.TXT";

                                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                                    {
                                        saveFname = saveFileDialog1.FileName;
                                        //saveFname2 = saveFileDialog1.F
                                        saveFname2 = dirName + "\\_" + saveFname2;
                                    }
                                    */
                                }
                                else
                                {
                                    // 같은 파일이 없다면 불러들인 *.TMF 파일이 있는 디렉토리에 저장
                                    saveFname2 = dirName + "\\_" + saveFname;
                                    saveFname = dirName + "\\" + saveFname;
                                    bisLoopContinue = false;
                                }
                            } while (bisLoopContinue);
                        }
                        #endregion // Confirm save filename





                        //	if (newProInHeader.TachoTransTime[2] >= 0x0b)
                        //	{
                        #region Save file
                        {


                            //		if (TimeCheck == false)
                            //		{


                            Encoding euckr = Encoding.GetEncoding(51949);

                            using (StreamWriter sw = new StreamWriter(saveFname, false, euckr))
                            //	using (StreamWriter sw = new StreamWriter(saveFname, false, System.Text.Encoding.UTF8))										
                            {
                                nMakedFileNum++;	// 총 만들어진 파일 갯수를 알아보기 위한 변수																																

                                sw.Write(newProOutHeader.ModelName);
                                sw.Write(newProOutHeader.CarNum);
                                sw.Write("{0:X2}", newProOutHeader.CarType);
                                sw.Write(newProOutHeader.CarRegistNum);
                                sw.Write(newProOutHeader.OperatorRegistNum);


                                if (bIsStandAloneData)
                                    sw.Write(newProOutHeader.DriverCode);
                                else
                                    sw.Write(DriverCodeTemp[driverN]);

                                for (; nDataNum < arrNum; nDataNum++)
                                {

                                    	sw.Write("\n");
                                    if (newProOutData[nDataNum].CarBreak == 0x02)
                                    {

                                        // 운전자 코드가 변경된 시점
                                        // 현재 데이터를 버리고, 다음 데이터로 포인터를 옮긴 후, 현재 파일을 저장하고,
                                        // 새로운 파일을 만들기 위해 #region Make Save File 최상단 for 문으로 이동
                                        nDataNum++;
                                        //	MessageBox.Show("newProOutData[nDataNum].CarBreak == 0x02");
                                        break;
                                    }

                                    if (!newProInData[nDataNum].DataCheckSum)
                                    {
                                        //continue;
                                        //	MessageBox.Show("DataCheckSum error!!!!!!!!!");
                                    }
                                    /*	else
                                        {
                                            MessageBox.Show("error!!!!!!!!!");
                                        }*/

                                    sw.Write("{0:D4}", newProOutData[nDataNum].DayDriveDistance);
                                    sw.Write("{0:D7}", newProOutData[nDataNum].TotalDriveDistance);


                                    for (int a = 0; a < 7; a++)
                                    {
                                        sw.Write("{0:D2}", newProOutData[nDataNum].InforTime[a]);

                                    }

                                    sw.Write("{0:D3}", newProOutData[nDataNum].CarSpeed);
                                    sw.Write("{0:D4}", newProOutData[nDataNum].CarRPM);
                                    sw.Write("{0:D}", newProOutData[nDataNum].CarBreak);



                                    if (newProOutData[nDataNum].CarBreak == 2)
                                    {
                                        sw.Write("{0:X8}", newProOutData[nDataNum].CarGpsY);
                                        sw.Write("{0:X8}", newProOutData[nDataNum].CarGpsX);
                                        sw.Write("{0:X2}000", (newProOutData[nDataNum].CarDegree & 0xFF00) >> 8);


                                    }
                                    else
                                    {
                                        sw.Write("{0:D9}", newProOutData[nDataNum].CarGpsY);
                                        sw.Write("{0:D9}", newProOutData[nDataNum].CarGpsX);
                                        sw.Write("{0:D3}", newProOutData[nDataNum].CarDegree);

                                    }

                                    sw.Write(newProOutData[nDataNum].CarAccelX);
                                    sw.Write(newProOutData[nDataNum].CarAccelY);
                                    sw.Write("{0:X2}", newProOutData[nDataNum].MachineStatus);


                                }
                            }
                            //	}
                            //	TimeCheck = false;
                        }
                        #endregion // Save file
                        //	}
                    }
                }
                #endregion // Make Save File
                //	}

                headCnt++;

                if (fs.Position >= (fs.Length - 0x38))
                    break;

            } while (true);

            br.Close();
            fs.Close();
            bw.Close();
            /*	using (StreamWriter sw = new StreamWriter(strError, true, System.Text.Encoding.UTF8))
                {
                    sw.Write("=============================================\r\n");
                    string strETime = string.Format("Total Error Count: {0:D}\r\n\r\n", nTotalErrorCount);
                    sw.Write(strETime);
                }*/

            string strRe = String.Format("분석이 완료되었습니다.\r\n\r\n{0} 데이터: {1}개", (bIs1sData ? "1초" : "0.01초"), nMakedFileNum);
            MessageBox.Show(this, strRe, "결과");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
		
    }
}