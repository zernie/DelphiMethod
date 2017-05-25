using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Result : Form
    {
        public Result(double [,] data, List<double> sums, List<string> ranks, List<int> disabled, Config config)
        {
            InitializeComponent();

            try
            {
                Utils.InitResultDataGridView(dataGridView1, config, disabled);
                Utils.FillDataGridView(dataGridView1, data);
                Utils.CalculateGroupScoreSums(dataGridView1, sums);

                richTextBox1.Lines = ranks.ToArray();
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
