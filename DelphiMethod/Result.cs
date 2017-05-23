using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Result : Form
    {
        public Result(double [,] data, List<double> sums, Config config)
        {
            InitializeComponent();

            try
            {
                Utils.InitResultDataGridView(dataGridView1, config.IndicatorsCount, config.AlternativesCount);
                Utils.FillDataGridView(dataGridView1, data);
                Utils.CalculateGroupScoreSums(dataGridView1, sums);
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
