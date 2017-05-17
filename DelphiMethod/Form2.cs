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
        public decimal[,] Evaluation => Utils.ExtractData(dataGridView2, _expertsCount, _alternativesCount);

        public Form2(int expertsCount, int alternativesCount)
        {

            InitializeComponent();

            _expertsCount = expertsCount;
            _alternativesCount = alternativesCount;
            InitForm(new decimal[alternativesCount, expertsCount]);
        }

        public Form2(decimal[,] data)
        {
            InitializeComponent();

            _expertsCount = data.GetLength(0);
            _alternativesCount = data.GetLength(1);
            InitForm(data);
        }

        private void InitForm(decimal[,] data)
        {
            AddTourNumber();
            Utils.InitDataGridView(dataGridView2, _alternativesCount, _expertsCount);
            Utils.FillDataGridView(dataGridView2, data);

            dataGridView2.Columns.Add("average", "Среднее");
            dataGridView2.Columns.Add("median", "Медиана");
            dataGridView2.Columns.Add("groupEvaluation", "Групповая оценка");
        }

        // Вычислить и вывести средние
        private void CalcAverages(DataGridView component, string colname)
        {
            var averages = Utils.CalcAverages(Evaluation);
            Utils.FillColumn(component, averages, colname);
        }

        // Вычислить и вывести медиану
        private void CalcMedians(DataGridView component, string colname)
        {
            var medians = Utils.CalcMedians(Evaluation);
            Utils.FillColumn(component, medians, colname);
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
                CalcAverages(dataGridView2, "average");
                CalcMedians(dataGridView2, "median");
            }
            catch (FormatException exception)
            {
                MessageBox.Show($"'{exception.Data["value"]}': {exception.Message}");
            }
        }
    }
}
