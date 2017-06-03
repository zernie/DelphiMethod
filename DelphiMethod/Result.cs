using System;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Result : Form
    {
        public Result(MatrixList ranks)
        {
            InitializeComponent();
            try
            {
                var z = ranks.GroupScores();
                var sums = ranks.xj(z);
                var ranksStrings = ranks.Ranks(sums);
                var disabledRanks = ranks.ConsensusReachedMatrices();

                Utils.InitResultDataGridView(dataGridView1, ranks.Configuration, disabledRanks);
                Utils.FillDataGridView(dataGridView1, z);
                Utils.CalculateGroupScoreSums(dataGridView1, sums);

                richTextBox1.Lines = ranksStrings.ToArray();
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
