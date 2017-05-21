using System;
using System.Collections.Generic;
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
            get => _matrixList[_indicator];
            set => _matrixList[_indicator] = value;
        }

        // Список матриц оценок
        private MatrixList _matrixList;
        // Текущий показатель
        private int _indicator => comboBox1.SelectedIndex;

        private bool _disableTrigger;

        public Form2(InitialData initialData)
        {
            InitializeComponent();
            _initialData = initialData;
            _matrixList = new MatrixList(initialData);
            AddTourNumber();

            foreach (var weight in _initialData.WeightIndicators)
            {
                comboBox1.Items.Add($"с весом {weight}");
            }

            comboBox1.SelectedIndex = 0;
            for (var i = 0; i < initialData.IndicatorsCount; i++)
            {
                var matrix = new Matrix(initialData, i);
                _matrixList.Experts.Add(matrix);
            }

            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _currentRank);

            ratingScaleTextBox.Text = _initialData.RatingScale.ToString();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        public Form2(MatrixList matrixList)
        {
            _matrixList = matrixList;
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

        private void Calculate()
        {
            Utils.CalculateAverageScores(dataGridView2, _currentRank);
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
        }

        // Следующий тур
        private void calculateButton_Click(object sender, EventArgs e)
        {
            AddTourNumber();
            Calculate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.InitDataGridView(dataGridView2, _initialData);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();
             _disableTrigger = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                _currentRank = new Matrix(Utils.ExtractData(dataGridView2, _initialData), _initialData, _indicator);
                _matrixList[comboBox1.SelectedIndex] = _currentRank;
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
            var rank = new double[0, 0];
            try
            {
                _disableTrigger = true;
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                rank = Utils.ReadAsCsv(openFileDialog1.FileName);

                _currentRank = new Matrix(rank, _initialData, _indicator);
                Utils.FillDataGridView(dataGridView2, _currentRank);
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message, "Ошибка чтения");
            }
            catch (IndexOutOfRangeException)
            {
                var width = rank.GetLength(1);
                var height = rank.GetLength(0);
                MessageBox.Show(
                    $"Размеры прочитанной матрицы: {height}x{width}," +
                    $"а ожидалось: {_initialData.AlternativesCount}x{_initialData.ExpertsCount}",
                    "Ошибка чтения");
            }
            finally
            {
                _disableTrigger = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Calculate();
        }
    }
}
