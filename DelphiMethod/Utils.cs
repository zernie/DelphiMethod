using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            component.Rows.Add(new DataGridViewRow
            {
                HeaderCell = new DataGridViewRowHeaderCell
                {
                    Value = "Коэффициент компетентности"
                }
            });
        }

        // Извлечь матрицу из таблицы
        public static double[,] ExtractData(DataGridView component, Config initialData)
        {
            var width = initialData.ExpertsCount;
            var height = initialData.AlternativesCount;
            var data = new double[height, width];

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var value = Convert.ToDouble(component.Rows[i].Cells[j].Value);

                    if (!initialData.RatingScale.Includes(value))
                    {
                        throw new FormatException("Оценка вышла за пределы шкалы");
                    }

                    data[i, j] = value;
                }
            }

            return data;
        }

        // Заполнить таблицу матрицей
        public static void FillDataGridView(DataGridView component, Matrix data)
        {
            for (var i = 0; i < data.Height; i++)
            {
                for (var j = 0; j < data.Width; j++)
                {
                    component.Rows[i].Cells[j].Value = data[i, j];
                }
            }
        }

        public static void CalculateAverageScores(DataGridView component, Matrix data)
        {
            for (var i = 0; i < data.Height; i++)
            {
                var groupEvaluation = data.AverageScores(data.InitialCompetenceCoefficient[i]);
                component["averageScores", i].Value = Math.Round(groupEvaluation[i], 3);
            }
        }

        public static void CalculateCoefficients(DataGridView component, Matrix data)
        {
            var coefficients = data.CompetenceCoefficients();

            for (var i = 0; i < data.Width; i++)
            {
                component[i, data.Height].Value = Math.Round(coefficients[i], 3);
            }
        }
    }
}
