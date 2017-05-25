using System;
using System.Linq;
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
            get => _matrixList[_indicatorIndex];
            set => _matrixList[_indicatorIndex] = value;
        }

        // Список матриц оценок
        private MatrixList _matrixList;
        // Текущий показатель
        private int _indicatorIndex => comboBox1.SelectedIndex;
        // Вес коэффициента
        private Indicator _indicator => _config.Indicators[_indicatorIndex];

        // Включить проверку введенных значений?
        private bool _disableTrigger;

        public Form2(MatrixList matrixList, bool calculate = false)
        {
            InitializeComponent();

            _config = matrixList.Configuration;
            _matrixList = matrixList;

            AddTourNumber();

            comboBox1.Items.AddRange(_config.Indicators.Select(x => x.Title).ToArray());
            comboBox1.SelectedIndex = 0;

            ratingScaleTextBox.Text = _config.RatingScale.ToString();
            foreach (var alpha in _config.PearsonCorrelationTable.Alphas)
                alphaComboBox.Items.Add(alpha);
            alphaComboBox.SelectedIndex = 0;

            Utils.InitInputDataGridView(dataGridView2, _config.ExpertsCount, _config.AlternativesCount);
            Utils.FillDataGridView(dataGridView2, _currentRank.X);


            if (calculate) Calculate();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            alphaComboBox.SelectedIndexChanged += alphaComboBox_SelectedIndexChanged;
        }

        // Увеличить счетчик тура
        private void AddTourNumber() => tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";


        // Проверить, достигнут ли консенсус
        private void checkConsensus()
        {
            var isConsensusReached = _currentRank.IsConsensusReached(_config.PearsonCorrelationTable, alphaComboBox.SelectedIndex);
            isConsensusReachedLabel.Text = isConsensusReached ? "достигнута" : "не достигнута";
        }

        // Провести анализ
        private void Calculate()
        {
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
            Utils.FillGroupScores(dataGridView2, _currentRank);
            checkConsensus();
        }

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Utils.ExportToFile(saveFileDialog1.FileName, _matrixList);
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
            checkConsensus();
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

        // Следующий тур
        private void nextTourButton_Click(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;

                var z = _matrixList.GroupScores();
                var sums = _matrixList.GroupScoresSums(z);
                var ranks = _matrixList.Ranks(sums);

                using (var form = new Result(z, sums, ranks, _config))
                {
                    form.ShowDialog();
                }
                AddTourNumber();
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
            try
            {
                _disableTrigger = true;
                Calculate();
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

        // Очистить таблицу
        private void clearButton_Click(object sender, EventArgs e)
        {
            _disableTrigger = true;
            _currentRank = new Matrix(_config.ExpertsCount, _config.AlternativesCount, _indicator);
            Utils.FillDataGridView(dataGridView2, _currentRank.X);
            Utils.ClearCalculatedValues(dataGridView2);
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
    }
}
