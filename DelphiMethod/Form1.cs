using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            indicatorsDataGridView.Rows.Add("Полнота поиска", 0.3);
            indicatorsDataGridView.Rows.Add("Точность поиска", 0.2);
            indicatorsDataGridView.Rows.Add("Усилия, затрачиваемые на формулирование запросов", 0.3);
            indicatorsDataGridView.Rows.Add("Форма представления найденной информации", 0.1);
            indicatorsDataGridView.Rows.Add("Полнота информационного массива", 0.1);
            var lines = ReadPearsonCorrelationFromFile();
            if (lines == null) Application.Exit();
            PearsonCorrelationTable = new PearsonCorrelation(lines);
            alternativesCountNumericUpDown.Maximum = PearsonCorrelationTable.Length;

            // Заполняем уровни значимости критерия α
            foreach (var alpha in PearsonCorrelationTable.Alphas)
                alphaComboBox.Items.Add(alpha);
            alphaComboBox.SelectedIndex = PearsonCorrelationTable.Alphas.Count / 2;


            indicatorsDataGridView.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private List<string> Alternatives => alternativesRichTextBox.Lines.ToList();
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

        // Таблица корреляции
        private PearsonCorrelation PearsonCorrelationTable;

        // Шкала оценок
        private Range RatingScale => new Range(0.0, radioButton1.Checked ? 10.0 : 100.0);

        // Сумма весов коэффициентов
        private double IndicatorsWeightSum => Indicators().Sum(x => x.Weight);


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

                if (!CheckInput()) return;

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

        // Проверка на верность введенных данных
        private bool CheckInput()
        {
            if (IndicatorsWeightSum != 1.0)
            {
                MessageBox.Show($"Сумма коэффициентов весов показателей = {IndicatorsWeightSum}, а должна равняться 1");
                return false;
            }

            if (Alternatives.Count < 2)
            {
                MessageBox.Show($"Пожалуйста, введите не менее 2 альтернатив");
                return false;

            }

            if (Experts.Count < 3)
            {
                MessageBox.Show($"Пожалуйста, введите не менее 3 экспертов");
                return false;

            }
            return true;
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
                var path = Path.Combine(Path.GetFullPath(@"..\..\"), "P.csv");
                return File.ReadAllLines(path);
            }
            catch (IOException e)
            {
                MessageBox.Show($"Не удалось загрузить таблицу критических значений корреляции Пирсона: {e.Message}");
            }
            return null;
        }

        private void UpdateCounter(TextBoxBase component,NumericUpDown counter)
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
            var lines = alternativesRichTextBox.Lines;

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

        private void alternativesRichTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCounter((RichTextBox)sender, alternativesCountNumericUpDown);
        }

        private void expertsRichTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCounter((RichTextBox)sender, expertsCountNumericUpDown);
        }

        private void alternativesCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBox(alternativesRichTextBox, (NumericUpDown) sender, "Альтернатива x");
        }

        private void expertsCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBox(expertsRichTextBox, (NumericUpDown)sender, "Эксперт ");
        }
    }
}
