using System;
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
                component.Rows.Add();
                component.Rows[i].HeaderCell.Value = $"Альтернатива №{i + 1}";
            }

            component.Columns.Add("averageScores", "Средние оценки");
            component.Columns.Add("groupScores", "Групповые оценки");
            component.Rows.Add(new DataGridViewRow
            {
                HeaderCell = new DataGridViewRowHeaderCell
                {
                    Value = "Коэффициент компетентности"
                }
            });
        }

        // Инициализировать таблицу групповых оценок заданным количеством строк и столбцов
        public static void InitResultDataGridView(DataGridView component, int width, int count)
        {
            for (var i = 0; i < width; i++)
            {
                component.Columns.Add(i.ToString(), $"z{i + 1}");
            }

            for (var i = 0; i < count; i++)
            {
                component.Rows.Add();
                component.Rows[i].HeaderCell.Value = $"x{i + 1}";
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

        public static void CalculateGroupScores(DataGridView component, Matrix data)
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

        public static void CalculateAverageScores(DataGridView component, Matrix data)
        {
            var averageScores = data.AverageScores(data.InitialCompetenceCoefficient);

            for (var i = 0; i < data.Height; i++)
            {
                component["averageScores", i].Value = Math.Round(averageScores[i], 3);
            }
        }

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
    }
}
