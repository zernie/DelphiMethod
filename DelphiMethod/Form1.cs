using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var expertsCount = (int)numericUpDown1.Value;
            var alternativesCount = (int)numericUpDown2.Value;

            using (var form = new Form2(alternativesCount, expertsCount))
            {
                form.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                var evaluation = Utils.ReadAsCsv(openFileDialog1.FileName);

                using (var form = new Form2(evaluation))
                {
                    form.ShowDialog();
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
