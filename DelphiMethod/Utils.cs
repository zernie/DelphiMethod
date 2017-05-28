using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Вспомогательный класс для вывода значений на форму
    class Utils
    {
        // Цифр после запятой
        public const int DigitsAfterPoint = 3;

        // Инициализировать таблицу рангов заданным количеством строк и столбцов
        public static void InitInputDataGridView(DataGridView component, int width, int count)
        {
            for (var i = 0; i < width; i++)
            {
                component.Columns.Add(i.ToString(), $"Эксперт №{i + 1}");
            }

            for (var i = 0; i < count; i++)
            {
                component.Rows.Add(new DataGridViewRow
                {
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = $"Альтернатива x{i + 1}"
                    },
                });
            }

            component.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "groupScore",
                HeaderText = "Групповые оценки",
                ReadOnly = true,
                DefaultCellStyle =
                {
                    BackColor = Color.Gold,
                }
            });
            component.Rows.Add(new DataGridViewRow
            {
                ReadOnly = true,
                HeaderCell = new DataGridViewRowHeaderCell
                {
                    Value = "Коэффициент компетентности",
                },
                DefaultCellStyle =
                {
                    BackColor = Color.Gold,
                }
            });
        }

        // Инициализировать таблицу групповых оценок заданным количеством строк и столбцов
        public static void InitResultDataGridView(DataGridView component, Config configuration)
        {
            for (var i = 0; i < configuration.IndicatorsCount; i++)
            {
                var indicator = configuration.Indicators[i].Title;
                component.Columns.Add(i.ToString(), $"z{i + 1} ({indicator})");
            }

            for (var i = 0; i < configuration.AlternativesCount; i++)
            {
                component.Rows.Add(new DataGridViewRow
                {
                    ReadOnly = true,
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = $"Групповая оценка x{i + 1}"
                    }
                });
            }

            component.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "sum",
                HeaderText = "Сумма",
                ReadOnly = true,
                DefaultCellStyle =
                {
                    BackColor = Color.Gold,
                }
            });
        }

        // Заполнить матрицу данными
        public static void FillDataGridView(DataGridView component, double[,] x)
        {
            for (var i = 0; i < x.GetLength(0); i++)
            {
                for (var j = 0; j < x.GetLength(1); j++)
                {
                    component[j, i].Value = Math.Round(x[i, j], DigitsAfterPoint).ToString();
                }
            }
        }

        // Очистить коэффициенты компетентности и групповые оценки
        public static void ClearCalculatedValues(DataGridView component)
        {
            for (var i = 0; i < component.RowCount; i++)
            {
                component["groupScore", i].Value = "";
            }

            for (var i = 0; i < component.ColumnCount; i++)
            {
                component[i, component.RowCount-1].Value = "";
            }
        }

        // Вычислить групповые оценки и вывести их на форму
        public static void FillGroupScores(DataGridView component, Matrix x)
        {
            var groupScores = x.GroupScores(x.CompetenceCoefficients());

            for (var i = 0; i < x.N; i++)
            {
                component["groupScore", i].Value = Math.Round(groupScores[i], DigitsAfterPoint);
            }
        }

        // Вычислить коэффициенты компетентности и вывести их на форму
        public static void CalculateCoefficients(DataGridView component, Matrix x)
        {
            var coefficients = x.CompetenceCoefficients();

            for (var i = 0; i < x.M; i++)
            {
                component[i, x.N].Value = Math.Round(coefficients[i], 3);
            }
        }

        // Вычислить коэффициенты компетентности и вывести их на форму результатов
        public static void CalculateGroupScoreSums(DataGridView component, List<double> x)
        {
            for (var i = 0; i < x.Count; i++)
            {
                component["sum", i].Value = Math.Round(x[i], DigitsAfterPoint);
            }
        }

        // Импорт из файла
        public static MatrixList ImportFromFile(string filename)
        {
            try
            {
                var serializer = new BinaryFormatter();
                MatrixList matrixList;

                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    matrixList = (MatrixList)serializer.Deserialize(fs);
                }

                return matrixList;
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        // Экспорт в файл
        public static void ExportToFile(string filename, MatrixList matrixList)
        {
            try
            {
                var x = new BinaryFormatter();

                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    x.Serialize(fs, matrixList);
                }
                MessageBox.Show("Файл сохранен.");
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
