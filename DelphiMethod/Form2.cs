using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private InitialData _initialData; // Исходные данные

        private int _tourNumber; // Номер тура

        // Матрица оценок из таблицы
        private Matrix _evaluation;

        private List<Matrix> _evaluations;

        private bool _disableTrigger;

        public Form2(InitialData initialData)
        {
            InitializeComponent();
            _initialData = initialData;
            _evaluation = new Matrix(initialData);
            _evaluations = Enumerable.Repeat(_evaluation, initialData.IndicatorsCount).ToList();

            InitForm();
        }

        public Form2(InitialData initialData, Matrix evaluation)
        {
            InitializeComponent();
            _initialData = initialData;
            _evaluation = evaluation;
            _evaluations = Enumerable.Repeat(evaluation, initialData.IndicatorsCount).ToList();

            InitForm();
        }

        private void InitForm()
        {
            AddTourNumber();
            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _evaluation, _initialData);
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name= "groupEvaluation", HeaderText= "Групповая оценка", ReadOnly = true
            });
            ratingScaleTextBox.Text = _initialData.RatingScale.ToString();


            foreach (var weight in _initialData.WeightIndicators)
            {
                comboBox1.Items.Add($"Показатель с весом {weight}");
            }
            comboBox1.SelectedIndex = 0;

            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
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

                Utils.SaveAsCsv(_evaluation, saveFileDialog1.FileName);
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
            AddTourNumber();

            for (var i = 0; i < _initialData.AlternativesCount; i++)
            {
                dataGridView2["groupEvaluation", i].Value = _evaluation.GroupEvaluations[i];
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            _evaluation = _evaluations[comboBox1.SelectedIndex];
            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _evaluation, _initialData);

            dataGridView2.Columns.Add("groupEvaluation", "Групповая оценка");
            _disableTrigger = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                _evaluation = Utils.ExtractData(dataGridView2, _initialData);
                _evaluations[comboBox1.SelectedIndex] = _evaluation;
            }
            catch (FormatException exception)
            {
                _disableTrigger = true;
                dataGridView2.CurrentCell.Value = "0";
                MessageBox.Show($"'{dataGridView2.CurrentCell.Value}': {exception.Message}");
                _disableTrigger = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                var evaluation = Utils.ReadAsCsv(openFileDialog1.FileName);

                _evaluation = new Matrix(evaluation, _initialData);
                Utils.FillDataGridView(dataGridView2, _evaluation, _initialData);
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
