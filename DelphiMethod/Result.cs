using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Result : Form
    {
        public Result(double [,] data, Config config)
        {
            InitializeComponent();

            Utils.InitResultDataGridView(dataGridView1, config.IndicatorsCount, config.AlternativesCount);
            Utils.FillDataGridView(dataGridView1, data);

            var sums = new List<double>(config.AlternativesCount);
            for (var i = 0; i < config.AlternativesCount; i++)
            {
                var temp = new List<double>(config.IndicatorsCount);
                for (var j = 0; j < config.IndicatorsCount; j++)
                {
                    temp.Add(data[i, j]);
                }
                dataGridView1["sum", i].Value = Math.Round(temp.Sum(),3);
            }
        }
    }
}
