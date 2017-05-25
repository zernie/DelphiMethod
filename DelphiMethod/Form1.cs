using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Add("Стоимость", 0.2);
            dataGridView1.Rows.Add("Надежность", 0.2);
            dataGridView1.Rows.Add("Удобство", 0.4);
            dataGridView1.Rows.Add("Популярность", 0.1);
            dataGridView1.Rows.Add("Наличие", 0.1);
        }

        private Config Configuration;

        private int AlternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        private int ExpertsCount => (int)numericUpDown1.Value; // m, количество экспертов
        private int IndicatorsCount => Indicators.Count; // l, количество показателей

        // коэффициенты весов показателей q^k
        private List<Indicator> Indicators
        {
            get
            {
                var indicators = new List<Indicator>(dataGridView1.RowCount);

                for (var i = 0; i < dataGridView1.RowCount -1; i++)
                {
                    var title = Convert.ToString(dataGridView1["Title", i].Value);
                    var weight = Convert.ToDouble(dataGridView1["Weight", i].Value);
                    indicators.Add(new Indicator(title, weight));
                }

                return indicators;
            }
        }

        // Шкала оценок
        private Range RatingScale => new Range(0.0, radioButton1.Checked ? 10.0 : 100.0);

        // Пуск
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration = new Config
            {
                AlternativesCount = AlternativesCount,
                ExpertsCount = ExpertsCount,
                IndicatorsCount = IndicatorsCount,
                RatingScale = RatingScale,
                Indicators = Indicators
            };

            if (!ValidWeightIndicators(Configuration.Indicators)) return;

            var matrixList = new MatrixList(Configuration);

            using (var form = new Form2(matrixList))
            {
                form.ShowDialog();
            }
        }

        // Импорт из файла
        private void importButton_Click(object sender, EventArgs e)
        {
            if (configOpenFileDialog.ShowDialog() == DialogResult.Cancel) return;
            
            var matrixList = Utils.ImportFromFile(configOpenFileDialog.FileName);
            if (matrixList == null) return;

            using (var form = new Form2(matrixList, true))
            {
                form.ShowDialog();
            }
        }

        // Проверка на верность введенных данных
        private bool ValidWeightIndicators(List<Indicator> indicators)
        {
            if (indicators.Count != IndicatorsCount)
            {
                MessageBox.Show($"Количество коэффициентов весов показателей равно {indicators.Count}," +
                                $" а должно равняться количеству показателей ({IndicatorsCount})");
                return false;
            }

            var sumIndicatorsWeight = indicators.Select(x => x.Weight).Sum();

            if (sumIndicatorsWeight != 1.0)
            {
                MessageBox.Show($"Сумма коэффициентов весов показателей = {sumIndicatorsWeight}," +
                                " а должна равняться единице");
                return false;
            }
            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 &&
                e.RowIndex != dataGridView1.NewRowIndex &&
                dataGridView1.RowCount > 2
                )
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }
    }
}
