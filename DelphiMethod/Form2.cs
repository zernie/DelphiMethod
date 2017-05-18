using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private InitialData _initialData;

        private int _tourNumber;

        // Матрица оценок из таблицы
        public Matrix Evaluation => Utils.ExtractData(dataGridView2, _initialData);

        public Form2(InitialData initialData)
        {
            InitializeComponent();
            _initialData = initialData;

            InitForm(new Matrix(initialData));
        }

        public Form2(InitialData initialData, Matrix evaluation)
        {
            InitializeComponent();
            _initialData = initialData;

            InitForm(evaluation);
        }

        private void InitForm(Matrix data)
        {
            AddTourNumber();
            Utils.InitDataGridView(dataGridView2, _initialData.AlternativesCount, _initialData.ExpertsCount);
            Utils.FillDataGridView(dataGridView2, data, _initialData.AlternativesCount, _initialData.ExpertsCount);

            dataGridView2.Columns.Add("groupEvaluation", "Групповая оценка");
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

                for (var i = 0; i < _initialData.AlternativesCount; i++)
                {
                    dataGridView2["groupEvaluation", i].Value = Evaluation.GroupEvaluations[i];
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show($"'{exception.Data["value"]}': {exception.Message}");
            }
        }
    }
}
