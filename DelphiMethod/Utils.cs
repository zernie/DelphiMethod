using System;
using System.Windows.Forms;

namespace DelphiMethod
{
    class Utils
    {
        // Инициализировать таблицу заданным количеством строк и столбцов
        public static void InitDataGridView(DataGridView component, Config initialData)
        {
            component.Rows.Clear();
            component.Columns.Clear();
            for (var i = 0; i < initialData.ExpertsCount; i++)
            {
                component.Columns.Add(i.ToString(), $"Эксперт №{i + 1}");
            }

            for (var i = 0; i < initialData.AlternativesCount; i++)
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
