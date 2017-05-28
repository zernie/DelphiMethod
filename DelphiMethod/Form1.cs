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
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private Config Configuration;

        private int AlternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        private int ExpertsCount => (int)numericUpDown1.Value; // m, количество экспертов

        // Показатели, их названия и веса q^k
        private List<Indicator> Indicators()
        {
            var indicators = new List<Indicator>(dataGridView1.RowCount);

            for (var i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                var title = Convert.ToString(dataGridView1["Title", i].Value);
                var weight = Convert.ToDouble(dataGridView1["Weight", i].Value);
                indicators.Add(new Indicator(title, weight));
            }

            return indicators;
        }

        // Шкала оценок
        private Range RatingScale => new Range(0.0, radioButton1.Checked ? 10.0 : 100.0);

        // Сумма весов коэффициентов
        private double IndicatorsWeightSum => Indicators().Sum(x => x.Weight);



        // Пуск
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration = new Config
                {
                    AlternativesCount = AlternativesCount,
                    ExpertsCount = ExpertsCount,
                    RatingScale = RatingScale,
                    Indicators = Indicators()
                };

                if (!CheckWeightIndicators()) return;

                var matrixList = new MatrixList(Configuration);

                Hide();
                var form = new Form2(matrixList);
                if (form.ShowDialog() == DialogResult.Cancel) Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Импорт из файла
        private void importButton_Click(object sender, EventArgs e)
        {
            if (configOpenFileDialog.ShowDialog() == DialogResult.Cancel) return;

            var matrixList = Utils.ImportFromFile(configOpenFileDialog.FileName);
            if (matrixList == null) return;

            Hide();
            var form = new Form2(matrixList, true);
            if (form.ShowDialog() == DialogResult.Cancel) Show();
        }

        // Проверка на верность введенных данных
        private bool CheckWeightIndicators()
        {
            if (IndicatorsWeightSum == 1.0) return true;

            MessageBox.Show($"Сумма коэффициентов весов показателей = {IndicatorsWeightSum}, а должна равняться 1");
            return false;
        }

        // Удаление показателя из таблицы
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

        // Проверка введенного веса показателя
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var value = dataGridView1.CurrentCell.Value;
            if (dataGridView1.CurrentCell.ColumnIndex != 1 && value != null) return;

            try
            {
                var doubleValue = Convert.ToDouble(value);

                if (value == null || doubleValue < 0.0 || doubleValue > 1.0)
                    throw new FormatException();
            }
            catch (FormatException)
            {
                dataGridView1.CurrentCell.Value = 0.0;
                MessageBox.Show("Введите число от 0 до 1");
            }
        }
    }
}
