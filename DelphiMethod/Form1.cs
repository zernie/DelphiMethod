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

        private int _alternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        private int _expertsCount => (int)numericUpDown1.Value; // m, количество экспертов
        private int _indicatorsCount => (int)numericUpDown3.Value; // l, количество показателей
        private List<decimal> _weightIndicators => textBox1.Lines.Select(Convert.ToDecimal).ToList(); // коэффициенты весов показателей
        private decimal _weightIndicatorsSum => _weightIndicators.Sum();
        private decimal _maxEvaluation => radioButton1.Checked ? 10.0M : 100.0M; // Максимум шкалы 

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_weightIndicators.Count != _indicatorsCount)
                {
                    MessageBox.Show($"Количество коэффициентов весов показателей должно равняться количеству показателей({_indicatorsCount})");
                    return;
                }

                if (_weightIndicatorsSum != 1.0M)
                {
                    MessageBox.Show($"Сумма коэффициентов весов показателей = {_weightIndicatorsSum}, а должна равняться единице");
                    return;
                }

                var initialData = new InitialData()
                {
                    AlternativesCount = _alternativesCount,
                    ExpertsCount = _expertsCount,
                    IndicatorsCount = _indicatorsCount,
                    MaxEvaluation = _maxEvaluation,
                    WeightIndicators = _weightIndicators
                };

                using (var form = new Form2(initialData))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                var evaluation = Utils.ReadAsCsv(openFileDialog1.FileName);

                var initialData = new InitialData
                {
                    AlternativesCount = _alternativesCount,
                    ExpertsCount = _expertsCount,
                    IndicatorsCount = _indicatorsCount,
                    MaxEvaluation = _maxEvaluation,
                    WeightIndicators = _weightIndicators
                };

                using (var form = new Form2(initialData, new Matrix(evaluation, initialData)))
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
            textBox1.AppendText($"\n {1.0M - _weightIndicatorsSum}");
        }
    }
}
