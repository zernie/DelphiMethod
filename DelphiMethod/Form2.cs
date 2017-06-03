using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form2 : Form
    {
        public Config Config; // Исходные данные

        public int TourNumber = 1; // Номер тура

        // Текущая матрица рангов
        private Matrix CurrentMatrix => Matrices[IndicatorIndex];

        // Список матриц рангов
        public MatrixList Matrices;

        // Текущий показатель
        public int IndicatorIndex => indicatorComboBox.SelectedIndex;
        // Вес коэффициента текущего показателя
        public Indicator Indicator => Config.Indicators[IndicatorIndex];

        // Достигнута ли согласованность значений в текущем показателе?
        public bool IsConsensusReached => CurrentMatrix.IsConsensusReached(Config.PearsonCorrelationTable, Config.AlphaIndex);

        // Включить проверку введенных значений?
        private bool _disableTrigger;

        public Form2(MatrixList matrices, bool calculate = false)
        {
            InitializeComponent();

            Config = matrices.Configuration;
            Matrices = matrices;

            // Заполняем показатели
            indicatorComboBox.Items.AddRange(Config.Indicators.Select(x => x.Title).ToArray());
            indicatorComboBox.SelectedIndex = 0;

            // Заполняем шкалу оценок
            ratingScaleTextBox.Text = Config.RatingScale.ToString();

            // Выводим матрицу
            Utils.InitInputDataGridView(dataGridView2, Config);
            Utils.FillDataGridView(dataGridView2, CurrentMatrix.X);

            if (calculate) Calculate();

            indicatorComboBox.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        // Провести анализ
        private void Calculate()
        {
            Utils.CalculateCoefficients(dataGridView2, CurrentMatrix);
            concordLabel.Text = Math.Round(CurrentMatrix.W(), 3).ToString();

            isConsensusReachedLabel.Text = IsConsensusReached ? "достигнута" : "не достигнута";

        }

        // Экспорт из таблицы в файл
        private void exportButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Utils.ExportToFile(saveFileDialog1.FileName, Matrices);
            }
        }

        // Смена показателя
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.FillDataGridView(dataGridView2, CurrentMatrix.X);
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
                concordLabel.Text = "...";
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

                if (Config.RatingScale.Includes(value))
                    CurrentMatrix.X[e.RowIndex, e.ColumnIndex] = value;
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
                Matrices.ClearWhereConsensusIsNotReached(Indicator);

                Utils.FillDataGridView(dataGridView2, CurrentMatrix.X);

                // Проверить, достигнута ли согласованность

                if (IsConsensusReached)
                {
                    fillWithRandomValuesButton.Enabled = false;
                    dataGridView2.Enabled = false;
                }

                if (Matrices.IsAnalysisDone)
                {
                    MessageBox.Show("Мнения экспертов согласованы во всех показателях!");
                    nextTourButton.Enabled = false;
                }

                fillWithRandomValuesButton.Enabled = true;
                dataGridView2.Enabled = true;
                tourNumberLabel.Text = $"Номер тура: {++TourNumber}";
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
            //!!!
            Matrices.ReturnToInitialValues();
            Utils.FillDataGridView(dataGridView2, CurrentMatrix.X);
            Utils.ClearCalculatedValues(dataGridView2);
            TourNumber = 1;
            tourNumberLabel.Text = $"Номер тура: {TourNumber}";
            _disableTrigger = false;
        }

        // Заполнить случ. значениями
        private void button1_Click(object sender, EventArgs e)
        {
            _disableTrigger = true;
            CurrentMatrix.FillWithRandomValues();
            Utils.FillDataGridView(dataGridView2, CurrentMatrix.X);
            Calculate();
            _disableTrigger = false;
        }

        // Вывод результатов
        private void showResultButton_Click(object sender, EventArgs e)
        {
            try
            {
                _disableTrigger = true;

                using (var form = new Result(Matrices))
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
