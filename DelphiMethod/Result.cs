using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Result : Form
    {
        public Result(double [,] data, List<double> sums, string[] ranks, Config config)
        {
            InitializeComponent();

            try
            {
                Utils.InitResultDataGridView(dataGridView1, config);
                Utils.FillDataGridView(dataGridView1, data);
                Utils.CalculateGroupScoreSums(dataGridView1, sums);

                richTextBox1.Lines = ranks;
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
