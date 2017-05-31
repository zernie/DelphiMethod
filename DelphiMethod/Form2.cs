using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        private Config _config; // Исходные данные

        private int _tourNumber = 1; // Номер тура

        // Текущая матрица рангов
        private Matrix _currentRank
        {
            get => _ranks[_indicatorIndex];
            set => _ranks[_indicatorIndex] = value;
        }
        // Предыдущая матрица рангов
        private double[,] _previousRank
        {
            get => _previousRanks[_indicatorIndex];
            set => _previousRanks[_indicatorIndex] = value;
        }
        // Список матриц рангов
        private MatrixList _ranks;
        // Предыдущие матрицы оценок
        private List<double[,]> _previousRanks;

        // Текущий показатель
        private int _indicatorIndex => comboBox1.SelectedIndex;
        // Вес коэффициента
        private Indicator _indicator => _config.Indicators[_indicatorIndex];

        // Включить проверку введенных значений?
        private bool _disableTrigger;

        public Form2(MatrixList ranks, bool calculate = false)
        {
            InitializeComponent();

            _config = ranks.Configuration;
            _ranks = ranks;
            _previousRanks = ranks.Matrices.Select(rank => rank.X).ToList();

            // Заполняем показатели
            comboBox1.Items.AddRange(_config.Indicators.Select(x => x.Title).ToArray());
            comboBox1.SelectedIndex = 0;

            // Заполняем шкалу оценок
            ratingScaleTextBox.Text = _config.RatingScale.ToString();

            // Заполняем уровни значимости критерия α
            foreach (var alpha in _config.PearsonCorrelationTable.Alphas)
                alphaComboBox.Items.Add(alpha);
            alphaComboBox.SelectedIndex = 0;

            // Выводим матрицу
            Utils.InitInputDataGridView(dataGridView2, _config.ExpertsCount, _config.AlternativesCount);
            Utils.FillDataGridView(dataGridView2, _currentRank.X);

            if (calculate) Calculate();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            alphaComboBox.SelectedIndexChanged += alphaComboBox_SelectedIndexChanged;
        }

        // Переход на след.тур
        private void NextTour()
        {
            _previousRank = _currentRank.X;
            _currentRank = new Matrix(_currentRank.X, _indicator);
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";
        }

        // Проверить, достигнута ли согласованность
        private void CheckConsensus()
        {
            var isConsensusReached = _currentRank.IsConsensusReached(_config.PearsonCorrelationTable, alphaComboBox.SelectedIndex);
            isConsensusReachedLabel.Text = isConsensusReached ? "достигнута" : "не достигнута";
        }

        // Провести анализ
        private void Calculate()
        {
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
            Utils.FillGroupScores(dataGridView2, _currentRank);
            CheckConsensus();
        }

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Utils.ExportToFile(saveFileDialog1.FileName, _ranks);
            }
        }

        // Смена показателя
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            Utils.ClearCalculatedValues(dataGridView2);
            try
            {
                Calculate();
            }
            catch (ArithmeticException)
            {
                Utils.ClearCalculatedValues(dataGridView2);
                isConsensusReachedLabel.Text = "...";
            }
            _disableTrigger = false;
        }

        // Смена уровня значимости критерия α
        private void alphaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckConsensus();
        }

        // Ввод данных в ячейку
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                var value = Convert.ToDouble(dataGridView2[e.ColumnIndex, e.RowIndex].Value);

                if (_config.RatingScale.Includes(value))
                    _currentRank.X[e.RowIndex, e.ColumnIndex] = value;
                else
                    throw new FormatException("Оценка вышла за пределы шкалы");
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

        // След. тур
        private void nextTourButton_Click(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;
                Calculate();
                NextTour();
            }
            catch (ArithmeticException)
            {
                MessageBox.Show("Заполните матрицу рангов");
            }
            finally
            {
                _disableTrigger = false;
            }
        }

        // Посчитать
        private void calculateButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;
                Calculate();
                var analysisDone = _ranks.Matrices.All(rank => rank.IsConsensusReached(_config.PearsonCorrelationTable, alphaComboBox.SelectedIndex));
                if (analysisDone)
                {
                    MessageBox.Show("Мнения экспертов согласованы во всех показателях!");
                    nextTourButton.Enabled = false;
                }
            }
            catch (ArithmeticException)
            {
                MessageBox.Show("Заполните матрицу рангов");
            }
            finally
            {
                _disableTrigger = false;
            }
        }

        // Начать заново
        private void clearButton_Click(object sender, EventArgs e)
        {
            _disableTrigger = true;
            _currentRank.X = _previousRank;
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            Calculate();
            _disableTrigger = false;
        }

        // Заполнить случ. значениями
        private void button1_Click(object sender, EventArgs e)
        {
            _disableTrigger = true;
            _currentRank.FillWithRandomValues();
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            Calculate();
            _disableTrigger = false;
        }

        // Вывод результатов
        private void showResultButton_Click(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;

                var z = _ranks.GroupScores();
                var sums = _ranks.GroupScoresSums(z);
                var ranks = _ranks.Ranks(sums);
                var disabledRanks = _ranks.DisabledRanks(alphaComboBox.SelectedIndex);

                using (var form = new Result(z, sums, ranks, disabledRanks, _config))
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
    }
}
