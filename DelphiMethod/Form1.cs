using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        public int AlternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        public int ExpertsCount => (int)numericUpDown1.Value; // m, количество экспертов
        public int IndicatorsCount => (int)numericUpDown3.Value; // l, количество показателей
        // коэффициенты веса показателей
        public List<decimal> WeightIndicators =>
           richTextBox1
                .Lines
                .Select(Convert.ToDecimal)
                .ToList();
        public decimal WeightIndicatorsSum => WeightIndicators.Sum();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (WeightIndicatorsSum != 1.0M)
                {
                    MessageBox.Show($"Сумма коэффициенты весов показателей = {WeightIndicatorsSum}, а должна равняться единице");
                    return;
                }
                if (WeightIndicators.Count != IndicatorsCount)
                {
                    MessageBox.Show($"Количество коэффициенты весов показателей должно равняться количеству показателей({IndicatorsCount})");
                    return;
                }

                using (var form = new Form2(AlternativesCount, ExpertsCount, IndicatorsCount, WeightIndicators))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
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

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            richTextBox1.AppendText($"\n {1.0M - WeightIndicatorsSum}");
        }
    }
}
