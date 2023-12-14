using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvertTacho
{
	public partial class FormLoad : Form
	{
		public FormLoad()
		{
			InitializeComponent();

         // PictureBox 투명하게 만들기 
             pictureBox2.BackColor = Color.Transparent;
		//	 pictureBox1.Parent = pictureBox2;
		//	 pictureBox1.Location= new Point(0, 0);

		}
	}
}