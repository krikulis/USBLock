using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBLock
{
    public partial class MainForm : Form
    {
        private USBStorageStatus status;
        private bool nextStatus;
        public MainForm()
        {
            status = new USBStorageStatus();
            InitializeComponent();
        }
        private void updateStatus()
        {
            bool currentStatus = status.getUSBStorageStatus();       
            if (currentStatus == true)
            {
                this.statusDescription.Text = "Atļautas";
                this.pictureBox1.Image = USBLock.Properties.Resources.export_ok;
                this.toggleButton.Text = "Bloķēt";
                this.nextStatus = false;
            } else
            {
                this.statusDescription.Text = "Bloķētas";
                this.pictureBox1.Image = USBLock.Properties.Resources.export_no;
                this.toggleButton.Text = "Atļaut";
                this.nextStatus = true;


            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.updateStatus();
        }


        private void toggleButton_Click(object sender, EventArgs e)
        {
            this.status.toggleStatus(this.nextStatus);
            this.updateStatus();
        }
    }
}
