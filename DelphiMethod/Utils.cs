using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Вспомогательный класс для вывода значений на форму
    class Utils
    {
        // Инициализировать таблицу рангов заданным количеством строк и столбцов
        public static void InitInputDataGridView(DataGridView component, int width, int count)
        {
            component.Rows.Clear();
            component.Columns.Clear();
            for (var i = 0; i < width; i++)
            {
                component.Columns.Add(i.ToString(), $"Эксперт №{i + 1}");
            }

            for (var i = 0; i < count; i++)
            {
                component.Rows.Add(new DataGridViewRow()
                {
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = $"Альтернатива №{i + 1}"
                    },
                });
            }

            component.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "groupScores",
                HeaderText = "Групповые оценки",
                ReadOnly = true,
            });
            component.Rows.Add(new DataGridViewRow
            {
                ReadOnly = true,
                HeaderCell = new DataGridViewRowHeaderCell
                {
                    Value = "Коэффициент компетентности",
                }
            });
        }

        // Инициализировать таблицу групповых оценок заданным количеством строк и столбцов
        public static void InitResultDataGridView(DataGridView component, int width, int count)
        {
            for (var i = 0; i < width; i++)
            {
                component.Columns.Add(i.ToString(), $"Показатель z{i + 1}");
            }

            for (var i = 0; i < count; i++)
            {
                component.Rows.Add(new DataGridViewRow()
                {
                    ReadOnly = true,
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = $"Групповая оценка x{i + 1}"
                    }
                });
            }

            component.Columns.Add("sum", "Сумма");
        }

        // Заполнить таблицу матрицей
        public static void FillDataGridView(DataGridView component, Matrix data)
        {
            for (var i = 0; i < data.Height; i++)
            {
                for (var j = 0; j < data.Width; j++)
                {
                    component[j, i].Value = data[i, j];
                }
            }
        }

        public static void FillDataGridView(DataGridView component, double[,] data)
        {
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    component[j, i].Value = Math.Round(data[i, j], 3);
                }
            }
        }


        // Извлечь матрицу из таблицы
        public static double[,] ExtractData(DataGridView component, Config config)
        {
            var width = config.ExpertsCount;
            var height = config.AlternativesCount;
            var data = new double[height, width];

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var value = Convert.ToDouble(component[j, i].Value);

                    if (!config.RatingScale.Includes(value))
                    {
                        throw new FormatException("Оценка вышла за пределы шкалы");
                    }

                    data[i, j] = value;
                }
            }

            return data;
        }

        // Вычислить групповые оценки и вывести их на форму
        public static void FillGroupScores(DataGridView component, Matrix data)
        {
            var groupScores = data.GroupScores(data.CompetenceCoefficients());

            for (var i = 0; i < data.Height; i++)
            {
                if (double.IsNaN(groupScores[i]))
                {
                    component["groupScores", i].Value = "-";
                    continue;
                }
                component["groupScores", i].Value = Math.Round(groupScores[i], 3);
            }
        }

        // Вычислить коэффициенты компетентности и вывести их на форму
        public static void CalculateCoefficients(DataGridView component, Matrix data)
        {
            var coefficients = data.CompetenceCoefficients();

            for (var i = 0; i < data.Width; i++)
            {
                var value = coefficients[i];
                if (double.IsNaN(value))
                {
                    component[i, data.Height].Value = "-";
                    continue;
                }

                component[i, data.Height].Value = Math.Round(value, 3);
            }
        }

        // Вычислить коэффициенты компетентности и вывести их на форму
        public static void CalculateGroupScoreSums(DataGridView component, List<double> data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                component["sum", i].Value = Math.Round(data[i], 3);
            }
        }

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

        public static void ExportToFile(string filename, MatrixList matrixList)
        {
            try { 
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
