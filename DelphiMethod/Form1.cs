using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form1 : Form
    {
        // Альтернативы
        private List<string> Alternatives => alternativesRichTextBox.Lines.ToList();
        // Эксперты
        private List<string> Experts => expertsRichTextBox.Lines.ToList();

        // Показатели, их названия и веса q^k
        private List<Indicator> Indicators()
        {
            var indicators = new List<Indicator>(indicatorsDataGridView.RowCount);

            for (var i = 0; i < indicatorsDataGridView.RowCount - 1; i++)
            {
                var title = Convert.ToString(indicatorsDataGridView["Title", i].Value);
                var weight = Convert.ToDouble(indicatorsDataGridView["Weight", i].Value);
                indicators.Add(new Indicator(title, weight));
            }

            return indicators;
        }

        // Таблица критических значений x2
        private PearsonCorrelation PearsonCorrelationTable;

        // Шкала оценок
        private Range RatingScale => new Range(0.0, radioButton1.Checked ? 10.0 : 100.0);

        // Сумма весов коэффициентов
        private double IndicatorsWeightSum => Indicators().Sum(x => x.Weight);

        public Form1()
        {
            InitializeComponent();
            // Кол-во показателей
            const int indicatorsCount = 3;
            for (var i = 1; i <= indicatorsCount; i++)
            {
                indicatorsDataGridView.Rows.Add($"Показатель z{i}", Math.Round(1.0 / indicatorsCount, 3));
            }
            PearsonCorrelationTable = new PearsonCorrelation(ReadPearsonCorrelationFromFile());

            // Заполняем уровни значимости критерия α
            foreach (var alpha in PearsonCorrelationTable.Alphas)
                alphaComboBox.Items.Add(alpha);
            alphaComboBox.SelectedIndex = (PearsonCorrelationTable.Alphas.Count / 2); ;

            // Заполняем экспертов и альтернативы
            alternativesCountNumericUpDown.Maximum = PearsonCorrelationTable.Length;
            alternativesCountNumericUpDown.Value = 10;
            expertsCountNumericUpDown.Value = 10;

            indicatorsDataGridView.CellValueChanged += dataGridView1_CellValueChanged;
        }




        // Пуск
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var configuration = new Config
                {
                    Alternatives = Alternatives,
                    Experts = Experts,
                    RatingScale = RatingScale,
                    Indicators = Indicators(),
                    PearsonCorrelationTable = PearsonCorrelationTable,
                    AlphaIndex = alphaComboBox.SelectedIndex,
                };

                if (Math.Abs(IndicatorsWeightSum - 1.0) > 0.01)
                {
                    MessageBox.Show($"Сумма коэффициентов весов показателей = {IndicatorsWeightSum}, а должна равняться 1");
                    return;
                }

                var matrixList = new MatrixList(configuration);

                Hide();
                var form = new Form2(matrixList);
                if (form.ShowDialog() == DialogResult.Cancel) Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        // Импорт из файла
        private void importButton_Click(object sender, EventArgs e)
        {
            if (configOpenFileDialog.ShowDialog() == DialogResult.Cancel) return;

            var matrixList = Utils.ImportFromFile(configOpenFileDialog.FileName);
            if (matrixList == null) return;

            Hide();
            var form = new Form2(matrixList, true);
            if (form.ShowDialog() == DialogResult.Cancel) Show();
        }

        // Удаление показателя из таблицы
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (indicatorsDataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 &&
                e.RowIndex != indicatorsDataGridView.NewRowIndex &&
                indicatorsDataGridView.RowCount > 2
                )
            {
                indicatorsDataGridView.Rows.RemoveAt(e.RowIndex);
            }
        }

        // Проверка введенного веса показателя
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var value = indicatorsDataGridView.CurrentCell.Value;
            if (indicatorsDataGridView.CurrentCell.ColumnIndex != 1 && value != null) return;

            try
            {
                var doubleValue = Convert.ToDouble(value);

                if (value == null || doubleValue < 0.0 || doubleValue > 1.0)
                    throw new FormatException();
            }
            catch (FormatException)
            {
                indicatorsDataGridView.CurrentCell.Value = 0.0;
                MessageBox.Show("Введите число от 0 до 1");
            }
        }

        // Чтение таблицы критических значений корреляции пирсона из файла
        private static string[] ReadPearsonCorrelationFromFile()
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, "P.csv");
                if (!File.Exists(path))
                    path = Path.Combine(Path.GetFullPath(@"..\..\"), "P.csv");

                return File.ReadAllLines(path);
            }
            catch (IOException e)
            {
                MessageBox.Show($"Не удалось загрузить таблицу критических значений корреляции Пирсона: {e.Message}");
                Environment.Exit(1);
            }
            return null;
        }

        
        // Изменение названий альтернатив
        private void alternativesRichTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCounter((RichTextBox)sender, alternativesCountNumericUpDown);
        }

        // Изменение имен экспертов
        private void expertsRichTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCounter((RichTextBox)sender, expertsCountNumericUpDown);
        }

        // Изменение кол-ва альтернатив
        private void alternativesCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBox(alternativesRichTextBox, (NumericUpDown)sender, "Альтернатива x");
        }

        // Изменение кол-ва экспертов
        private void expertsCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBox(expertsRichTextBox, (NumericUpDown)sender, "Эксперт ");
        }

        private void UpdateCounter(TextBoxBase component, NumericUpDown counter)
        {
            var length = component.Lines.Length;
            var min = counter.Minimum;
            var max = counter.Maximum;
            if (length >= min && length <= max)
                counter.Value = length;
            else
            {
                component.Undo();
                MessageBox.Show($"Введите не менее {min} и не более {max} значений.");
            }
        }

        private void UpdateTextBox(TextBoxBase component, NumericUpDown counter, string text)
        {
            var value = (int)counter.Value;
            var lines = component.Lines;

            if (value > component.Lines.Length)
            {
                var newLines = new List<string>();
                for (var i = 1; i <= value - lines.Length; i++)
                {
                    newLines.Add($"{text}{lines.Length + i}");
                }

                component.Lines = lines.Concat(newLines).ToArray();
            }
            else
            {
                component.Lines = lines.Take(value).ToArray();
            }
        }
    }
}
