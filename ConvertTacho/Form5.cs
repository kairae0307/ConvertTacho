using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace ConvertTacho
{
	public partial class Form5 : Form
	{
		public Form5()
		{
			InitializeComponent();

		}
		protected string ConnectionString
		{
			get
			{
				return "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\Tacho.mdb";
			}
		}
		public void FillData()
		{

			string DBstring = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + "\\Tacho.mdb";
			OleDbConnection Con = new OleDbConnection(@DBstring);
			OleDbCommand Cmd = new OleDbCommand();

			Cmd.CommandText = "SELECT * FROM Tacho ORDER BY ID";
			Cmd.Connection = Con;

			Con.Open();
			OleDbDataReader dataReader = Cmd.ExecuteReader();

			int articleCount = 0;

			while (dataReader.Read())
			{
				ListViewItem Myitem = new ListViewItem();
				articleCount++;

				Myitem = listView1.Items.Add(articleCount.ToString());
			//	Myitem.SubItems.Add(dataReader["ID"].ToString());
				Myitem.SubItems.Add(dataReader["File"].ToString());
			
				Myitem.Tag = dataReader["ID"].ToString();
			}

			dataReader.Close();
			Con.Close();

		//	btnModify.Enabled = false;
		//	btnDelete.Enabled = false;
		}
	}
	
}