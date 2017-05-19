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
        private List<decimal> _weightIndicators => richTextBox1.Lines.Select(Convert.ToDecimal).ToList(); // коэффициенты весов показателей
        private decimal _weightIndicatorsSum => _weightIndicators.Sum();

        private Range _ratingScale =>
            radioButton1.Checked ? new Range(0M, 10M) : new Range(0, 100M); // Шкала оценок

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_weightIndicators.Count != _indicatorsCount)
                {
                    MessageBox.Show($"Количество коэффициентов весов показателей равно {_weightIndicators.Count}," +
                                    $" а должно равняться количеству показателей ({_indicatorsCount})");
                    return;
                }

                if (_weightIndicatorsSum != 1.0M)
                {
                    MessageBox.Show($"Сумма коэффициентов весов показателей = {_weightIndicatorsSum}," +
                                    " а должна равняться единице");
                    return;
                }

                var initialData = new InitialData
                {
                    AlternativesCount = _alternativesCount,
                    ExpertsCount = _expertsCount,
                    IndicatorsCount = _indicatorsCount,
                    RatingScale = _ratingScale,
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
            }
        }
    }
}
