using System;
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
                var matrix = new Matrix(_config, i);
                _matrixList.Experts.Add(matrix);
            }
            ratingScaleTextBox.Text = _config.RatingScale.ToString();

            Utils.InitDataGridView(dataGridView2, _config);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        // Увеличить счетчик тура
        private void AddTourNumber() => tourNumberLabel.Text = $"Номер тура: {++_tourNumber}";

        private void Calculate()
        {
            Utils.CalculateAverageScores(dataGridView2, _currentRank);
            Utils.CalculateCoefficients(dataGridView2, _currentRank);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _disableTrigger = true;
            Utils.InitDataGridView(dataGridView2, _config);
            Utils.FillDataGridView(dataGridView2, _currentRank);
            Calculate();
            _disableTrigger = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_disableTrigger) return;
                _currentRank = new Matrix(Utils.ExtractData(dataGridView2, _config), _config, _indicator);
                _matrixList[comboBox1.SelectedIndex] = _currentRank;
            }
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
            AddTourNumber();
            Calculate();
        }

        private void calculateButton_Click_1(object sender, EventArgs e)
        {
            Calculate();
        }
    }
}
