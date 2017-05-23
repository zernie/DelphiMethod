﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private Config _config; // Исходные данные

        private int _tourNumber; // Номер тура

        // Текущая матрица оценок
        private Matrix _currentRank
        {
            get => _matrixList[_indicator];
            set => _matrixList[_indicator] = value;
        }

        // Список матриц оценок
        private MatrixList _matrixList;
        // Текущий показатель
        private int _indicator => comboBox1.SelectedIndex;
        private double _weightIndicator => _config.WeightIndicators[_indicator];

        private bool _disableTrigger;

        public Form2(MatrixList matrixList)
        {
            _config = matrixList.Configuration;
            _matrixList = matrixList;
            InitForm();
        }

        public Form2(Config config)
        {
            _config = config;
            _matrixList = new MatrixList(config);
            InitForm();
        }

        private void InitForm()
        {
            InitializeComponent();
            AddTourNumber();

            comboBox1.Items.AddRange(_config.WeightIndicators.Titles.ToArray());
            comboBox1.SelectedIndex = 0;
            for (var i = 0; i < _config.IndicatorsCount; i++)
            {
                var matrix = new Matrix(_config.ExpertsCount, _config.AlternativesCount, _config.WeightIndicators[i]);
                _matrixList.Matrices.Add(matrix);
            }
            ratingScaleTextBox.Text = _config.RatingScale.ToString();

            Utils.InitInputDataGridView(dataGridView2, _config.ExpertsCount, _config.AlternativesCount);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        // Увеличить счетчик тура
        private void AddTourNumber() => tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";

        // Провести анализ
        private void Calculate()
        {
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
            Utils.FillGroupScores(dataGridView2, _currentRank);
        }

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                var x = new BinaryFormatter();

                using (var fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate))
                {
                    x.Serialize(fs, _matrixList);
                }
                MessageBox.Show("Файл сохранен.");
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Смена показателя
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();
            _disableTrigger = false;
        }

        // Ввод данных в ячейку
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                var data = Utils.ExtractData(dataGridView2, _config);
                _currentRank = new Matrix(data, _weightIndicator);
                _matrixList[comboBox1.SelectedIndex] = _currentRank;
            }
            // в случае ввода нечисловых данных выдаем ошибку
            catch (FormatException exception)
            {
                _disableTrigger = true;
                MessageBox.Show($"'{dataGridView2.CurrentCell.Value}': {exception.Message}");
                dataGridView2.CurrentCell.Value = _config.RatingScale.Start;
                _disableTrigger = false;

            }
        }

        private void nextTourButton_Click(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;

                var z = _matrixList.GroupScores();
                var sums = _matrixList.Sums();
                AddTourNumber();

                using (var form = new Result(z, sums, _config))
                {
                    form.ShowDialog();
                }
            }
            catch (ArithmeticException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                _disableTrigger = false;
            }
        }

        // Высчитать групповые коэффициенты
        private void calculateButton_Click_1(object sender, EventArgs e)
        {
            Calculate();
        }

        // Очистить таблицу
        private void clearButton_Click(object sender, EventArgs e)
        {
            _currentRank = new Matrix(_config.ExpertsCount, _config.AlternativesCount, _weightIndicator);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();
        }
    }
}
