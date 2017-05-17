using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private int 
            _expertsCount, // количество экспертов
            _alternativesCount, // количество альтернатив
            _tourNumber = 1; // номер текущего тура
        // Матрица оценок из таблицы
        public Matrix Evaluation => Utils.ExtractData(dataGridView2, _expertsCount, _alternativesCount);

        public Form2(int expertsCount, int alternativesCount)
        {

            InitializeComponent();

            _expertsCount = expertsCount;
            _alternativesCount = alternativesCount;
            InitForm(new Matrix(expertsCount, alternativesCount));
        }

        public Form2(Matrix data)
        {
            InitializeComponent();

            _expertsCount = data.Alternatives.Count;
            _alternativesCount = data.Alternatives[0].Values.Count;
            InitForm(data);
        }

        private void InitForm(Matrix data)
        {
            AddTourNumber();
            Utils.InitDataGridView(dataGridView2, _expertsCount, _alternativesCount);
            Utils.FillDataGridView(dataGridView2, data, _expertsCount, _alternativesCount);

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

                for (var i = 0; i < _expertsCount; i++)
                {
                    dataGridView2["median", i].Value = Evaluation.Medians[i];
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show($"'{exception.Data["value"]}': {exception.Message}");
            }
        }
    }
}
