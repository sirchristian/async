using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Asynk.Library;

namespace SampleApplication
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Timer for use in this sample application
        /// </summary>
        private Stopwatch timer = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAsynk_Click(object sender, EventArgs e)
        {
            textBoxStatus.Text = "Starting Timer..." + Environment.NewLine; 
            timer.Reset();
            timer.Start();
            Asynker.Process(new Display(), "DisplayBox");
            textBoxStatus.Text += "Finished call in " + timer.Elapsed.ToString() + Environment.NewLine;
        }

        private void buttonSynk_Click(object sender, EventArgs e)
        {
            textBoxStatus.Text = "Starting Timer..." + Environment.NewLine;
            timer.Reset();
            timer.Start();
            new Display().DisplayBox();
            textBoxStatus.Text += "Finished call in " + timer.Elapsed.ToString() + Environment.NewLine;
        }

        [Serializable]
        public class Display
        {
            [AsynkCallable]
            public void DisplayBox()
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));
                MessageBox.Show("Done");
            }
        }
    }
}
