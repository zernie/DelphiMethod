using System;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Форма вывода результатов
    public partial class Result : Form
    {
        public Result(MatrixList matrices)
        {
            InitializeComponent();
            try
            {
                var z = matrices.GroupScores();
                var sums = matrices.xj(z);
                var ranksStrings = matrices.Ranks(sums);
                var disabledRanks = matrices.ConsensusReachedMatrices();

                Utils.InitResultDataGridView(dataGridView1, matrices, disabledRanks);
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
