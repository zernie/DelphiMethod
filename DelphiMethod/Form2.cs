using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        // Текущая матрица оценок
        private Matrix _currentRank
        {
            get => _ranks[_indicator];
            set => _ranks[_indicator] = value;
        }

        // Список матриц оценок
        private List<Matrix> _ranks = new List<Matrix>();
        // Текущий показатель
        private int _indicator => comboBox1.SelectedIndex;

        private bool _disableTrigger;

        public Form2(InitialData initialData)
        {
            InitializeComponent();
            _initialData = initialData;
            AddTourNumber();

            foreach (var weight in _initialData.WeightIndicators)
            {
                comboBox1.Items.Add($"Показатель с весом {weight}");
            }

            comboBox1.SelectedIndex = 0;

            for (var i = 0; i < initialData.IndicatorsCount; i++)
            {
                var matrix = new Matrix(initialData, i);
                _ranks.Add(matrix);
            }

            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            dataGridView2.Columns.Add("groupEvaluation", "Групповая оценка");
            ratingScaleTextBox.Text = _initialData.RatingScale.ToString();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        // Увеличить счетчик тура
        private void AddTourNumber() => tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                Utils.SaveAsCsv(_currentRank, saveFileDialog1.FileName);
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
                dataGridView2["groupEvaluation", i].Value = _currentRank.GroupEvaluations[i];
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _currentRank);

            dataGridView2.Columns.Add("groupEvaluation", "Групповая оценка");
            _disableTrigger = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                _currentRank = new Matrix(Utils.ExtractData(dataGridView2, _initialData), _initialData, _indicator);
                _ranks[comboBox1.SelectedIndex] = _currentRank;
            }
            catch (FormatException exception)
            {
                _disableTrigger = true;
                MessageBox.Show($"'{dataGridView2.CurrentCell.Value}': {exception.Message}");
                dataGridView2.CurrentCell.Value = _initialData.RatingScale.Start;
                _disableTrigger = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                var rank = Utils.ReadAsCsv(openFileDialog1.FileName);

                _currentRank = new Matrix(rank, _initialData, _indicator);
                Utils.FillDataGridView(dataGridView2, _currentRank);
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
