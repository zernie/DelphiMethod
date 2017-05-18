using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private int
            _expertsCount, // количество экспертов
            _alternativesCount, // количество альтернатив
            _indicatorsCount, // количество альтернатив
            _tourNumber = 1; // номер текущего тура

        private List<decimal> _weightIndicators;

        // Матрица оценок из таблицы
        public Matrix Evaluation => Utils.ExtractData(dataGridView2,  _alternativesCount, _expertsCount);

        public Form2(int alternativesCount, int expertsCount, int indicatorsCount, List<decimal> weightIndicators)
        {
            InitializeComponent();

            _alternativesCount = alternativesCount;
            _expertsCount = expertsCount;
            _indicatorsCount = indicatorsCount;
            _weightIndicators = weightIndicators;
            
            InitForm(new Matrix(alternativesCount, expertsCount));
        }

        public Form2(Matrix data)
        {
            InitializeComponent();

            _expertsCount = data.Alternatives[0].Values.Count; 
            _alternativesCount = data.Alternatives.Count;

            InitForm(data);
        }

        private void InitForm(Matrix data)
        {
            AddTourNumber();
            Utils.InitDataGridView(dataGridView2,  _alternativesCount, _expertsCount);
            Utils.FillDataGridView(dataGridView2, data, _alternativesCount, _expertsCount);

            dataGridView2.Columns.Add("average", "Среднее арифметическое");
            dataGridView2.Columns.Add("median", "Медиана");
            dataGridView2.Columns.Add("group", "Групповая оценка");
        }

        // Увеличить счетчик тура
        private void AddTourNumber() => tourNumberLabel.Text = $"Номер тура: {_tourNumber++}";

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                Utils.SaveAsCsv(Evaluation, saveFileDialog1.FileName);
                MessageBox.Show("Файл сохранен.");
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Следующий тур
        private void calculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddTourNumber();

                for (var i = 0; i < _alternativesCount; i++)
                {
                    dataGridView2["median", i].Value = Evaluation.Medians[i];
                    dataGridView2["average", i].Value = Evaluation.Averages[i];
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show($"'{exception.Data["value"]}': {exception.Message}");
            }
        }
    }
}
