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
        private Matrix _currentRank => _ranks[_indicatorIndex];

        // Список матриц рангов
        private MatrixList _ranks;
        // Предыдущие матрицы оценок
        private MatrixList _initialRanks;

        // Текущий показатель
        private int _indicatorIndex => indicatorComboBox.SelectedIndex;
        // Вес коэффициента
        private Indicator _indicator => _config.Indicators[_indicatorIndex];

        // Включить проверку введенных значений?
        private bool _disableTrigger;

        public Form2(MatrixList ranks, bool calculate = false)
        {
            InitializeComponent();

            _config = ranks.Configuration;
            _ranks = ranks;
            _initialRanks = (MatrixList)ranks.Clone();

            // Заполняем показатели
            indicatorComboBox.Items.AddRange(_config.Indicators.Select(x => x.Title).ToArray());
            indicatorComboBox.SelectedIndex = 0;

            // Заполняем шкалу оценок
            ratingScaleTextBox.Text = _config.RatingScale.ToString();

            // Выводим матрицу
            Utils.InitInputDataGridView(dataGridView2, _config);
            Utils.FillDataGridView(dataGridView2, _currentRank.X);

            if (calculate) Calculate();

            indicatorComboBox.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        // Провести анализ
        private void Calculate()
        {
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
            Utils.FillGroupScores(dataGridView2, _currentRank);

            // Проверить, достигнута ли согласованность
            var isConsensusReached =
                _currentRank.IsConsensusReached(_config.PearsonCorrelationTable, _config.AlphaIndex);
            isConsensusReachedLabel.Text = isConsensusReached ? "достигнута" : "не достигнута";

            if (isConsensusReached)
            {
                fillWithRandomValuesButton.Enabled = false;
                dataGridView2.Enabled = false;
            }

            if (_ranks.IsAnalysisDone)
            {
                MessageBox.Show("Мнения экспертов согласованы во всех показателях!");
                nextTourButton.Enabled = false;
            }
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
            fillWithRandomValuesButton.Enabled = true;
            dataGridView2.Enabled = true;
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
                dataGridView2.CurrentCell.Value = "0";
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
                _ranks.ClearWhereConsensusIsReached(_indicator);

                Utils.FillDataGridView(dataGridView2, _currentRank.X);

                fillWithRandomValuesButton.Enabled = true;
                dataGridView2.Enabled = true;
                tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";
            }
            catch (ArithmeticException)
            {
                MessageBox.Show("Заполните матрицу рангов");
            }
            _disableTrigger = false;
        }

        // Посчитать
        private void calculateButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;
                Calculate();
            }
            catch (ArithmeticException)
            {
                MessageBox.Show("Заполните матрицу рангов");
            }
            _disableTrigger = false;
        }

        // Начать заново
        private void clearButton_Click(object sender, EventArgs e)
        {
            _disableTrigger = true;
            _ranks = _initialRanks;
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            Calculate();
            _tourNumber = 1;
            tourNumberLabel.Text = $"Номер тура: {_tourNumber}";
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

                using (var form = new Result(_ranks))
                {
                    form.ShowDialog();
                }
            }
            catch (ArithmeticException exception)
            {
                MessageBox.Show(exception.Message);
            }
            _disableTrigger = false;
        }
    }
}
