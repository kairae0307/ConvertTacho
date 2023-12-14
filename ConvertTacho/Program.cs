using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;

namespace ConvertTacho
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
	public class Dummy
	{

	}

	class IpInfo
	{
		public string Ip;
		public int Port;
		public TcpState State;
		public string ProcessName;

		public string CountryName;
		public string RegionName;
		public string City;
		public double Latitude;
		public double Longitude;
		public DateTime CacheTime;

		public DateTime StatusTime;
		public bool TracePoint;
	}

	struct IpStatus
	{
		private string countryName;
		public string CountryName
		{
			get
			{
				return countryName;
			}
			set
			{
				countryName = value;
			}
		}

		private int connectionsCount;
		public int ConnectionsCount
		{
			get
			{
				return connectionsCount;
			}
			set
			{
				connectionsCount = value;
			}
		}
	}

	class DescendingComparer : IComparer<IpStatus>
	{
		public bool SortOnlyCountryName = false;

		public int Compare(IpStatus x, IpStatus y)
		{
			int r = 0;

			if (!SortOnlyCountryName)
			{
				r = y.ConnectionsCount.CompareTo(x.ConnectionsCount);
			}

			if (r == 0)
			{
				return x.CountryName.CompareTo(y.CountryName);
			}
			return r;
		}
	}

	class TraceRoute
	{
		readonly static string Data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
		readonly static byte[] DataBuffer;
		readonly static int timeout = 8888;

		static TraceRoute()
		{
			DataBuffer = Encoding.ASCII.GetBytes(Data);
		}

	/*	public static List<PingReply> GetTraceRoute(string hostNameOrAddress)
		{
			var ret = GetTraceRoute(hostNameOrAddress, 1);

			return ret;
		}*/

		private static List<PingReply> GetTraceRoute(string hostNameOrAddress, int ttl)
		{
			List<PingReply> result = new List<PingReply>();

			using (Ping pinger = new Ping())
			{
				PingOptions pingerOptions = new PingOptions(ttl, true);

				PingReply reply = pinger.Send(hostNameOrAddress, timeout, DataBuffer, pingerOptions);

				//Debug.WriteLine("GetTraceRoute[" + hostNameOrAddress + "]: " + reply.RoundtripTime + "ms " + reply.Address + " -> " + reply.Status);

				if (reply.Status == IPStatus.Success)
				{
					result.Add(reply);
				}
				else if (reply.Status == IPStatus.TtlExpired)
				{
					// add the currently returned address
					result.Add(reply);

					// recurse to get the next address...
					result.AddRange(GetTraceRoute(hostNameOrAddress, ttl + 1));
				}
				else
				{
					Debug.WriteLine("GetTraceRoute: " + hostNameOrAddress + " - " + reply.Status);
				}
			}

			return result;
		}
	}

}