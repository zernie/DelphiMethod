﻿using System;
using System.Collections.Generic;
using System.IO;
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
            dataGridView1.Rows.Add("Надежность", 0.3);
            dataGridView1.Rows.Add("Удобство", 0.5);
        }

        private int AlternativesCount => (int)numericUpDown2.Value; // n, количество альтернатив
        private int ExpertsCount => (int)numericUpDown1.Value; // m, количество экспертов
        private int IndicatorsCount => (int)numericUpDown3.Value; // l, количество показателей

        // коэффициенты весов показателей
        private WeightIndicators WeightIndicators
        {
            get
            {
                var names = new List<string>(dataGridView1.RowCount);
                var values = new List<double>(dataGridView1.RowCount);

                for (var i = 0; i < dataGridView1.Rows.Count - 1; i++)
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
            try
            {
                var weightIndicators = WeightIndicators;
                if (weightIndicators.Values.Count != IndicatorsCount)
                {
                    MessageBox.Show($"Количество коэффициентов весов показателей равно {weightIndicators.Count}," +
                                    $" а должно равняться количеству показателей ({IndicatorsCount})");
                    return;
                }

                if (weightIndicators.Sum != 1.0)
                {
                    MessageBox.Show($"Сумма коэффициентов весов показателей = {weightIndicators.Sum}," +
                                    " а должна равняться единице");
                    return;
                }

                var initialData = new InitialData
                {
                    AlternativesCount = AlternativesCount,
                    ExpertsCount = ExpertsCount,
                    IndicatorsCount = IndicatorsCount,
                    RatingScale = RatingScale,
                    WeightIndicators = weightIndicators
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
