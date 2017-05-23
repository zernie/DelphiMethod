using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        private int AlternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        private int ExpertsCount => (int)numericUpDown1.Value; // m, количество экспертов
        private int IndicatorsCount => (int)numericUpDown3.Value; // l, количество показателей
        private Config Configuration;

        // коэффициенты весов показателей
        private WeightIndicators WeightIndicators
        {
            get
            {
                var names = new List<string>(dataGridView1.RowCount);
                var values = new List<double>(dataGridView1.RowCount);

                for (var i = 0; i < dataGridView1.RowCount; i++)
                {
                    names.Add(Convert.ToString(dataGridView1["Title", i].Value));
                    values.Add(Convert.ToDouble(dataGridView1["Value", i].Value));
                }

                return new WeightIndicators(names, values);
            }
        }

        private Range RatingScale =>
            radioButton1.Checked ? new Range(0.0, 10.0) : new Range(0, 100.0); // Шкала оценок

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration = new Config
            {
                AlternativesCount = AlternativesCount,
                ExpertsCount = ExpertsCount,
                IndicatorsCount = IndicatorsCount,
                RatingScale = RatingScale,
                WeightIndicators = WeightIndicators
            };

            if (!ValidWeightIndicators(Configuration.WeightIndicators)) return;

            using (var form = new Form2(Configuration))
            {
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            if (configOpenFileDialog.ShowDialog() == DialogResult.Cancel) return;

            var serializer = new BinaryFormatter();
            MatrixList matrixList;

            using (var fs = new FileStream(configOpenFileDialog.FileName, FileMode.OpenOrCreate))
            {
                matrixList = (MatrixList)serializer.Deserialize(fs);
            }

            using (var form = new Form2(matrixList))
            {
                form.ShowDialog();
            }
        }

        private bool ValidWeightIndicators(WeightIndicators weightIndicators)
        {
            if (weightIndicators.Count != IndicatorsCount)
            {
                MessageBox.Show($"Количество коэффициентов весов показателей равно {weightIndicators.Count}," +
                                $" а должно равняться количеству показателей ({IndicatorsCount})");
                return false;
            }

            if (weightIndicators.Sum != 1.0)
            {
                MessageBox.Show($"Сумма коэффициентов весов показателей = {weightIndicators.Sum}," +
                                " а должна равняться единице");
                return false;
            }
            return true;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.RowCount = (int)numericUpDown3.Value;
        }
    }
}
