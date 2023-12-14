using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvertTacho
{
    public partial class DlgForm : Form
    {
        public DlgForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //Dispose(true);
        }

        private void DlgForm_Load(object sender, EventArgs e)
        {

        }
    }
}