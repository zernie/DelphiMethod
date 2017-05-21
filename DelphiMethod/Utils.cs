using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    class Utils
    {
        // Считать матрицу из файла
        public static double[,] ReadAsCsv(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var width = lines[0].Split(' ').Length;
            var height = lines.Length;

            var data = new double[height, width];

            for (var i = 0; i < height; i++)
            {
                var line = lines[i].Split(' ');
                for (var j = 0; j < width; j++)
                {
                   data[i,j] = Convert.ToDouble(line[j]);
                }
            }

            return data;
        }

        // Сохранить матрицу в файл
        public static void SaveAsCsv(Matrix data, string filename)
        {
            File.WriteAllText(filename, data.ToString());
        }

        // Инициализировать таблицу заданным количеством строк и столбцов
        public static void InitDataGridView(DataGridView component, InitialData initialData)
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

            component.Columns.Add("groupEvaluation", "Групповая оценка");
            component.Rows.Add(new DataGridViewRow
            {
                HeaderCell = new DataGridViewRowHeaderCell
                {
                    Value = "Коэффициент компетентности"
                }
            });
        }

        // Извлечь матрицу из таблицы
        public static double[,] ExtractData(DataGridView component, InitialData initialData)
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


        public static void CalculateGroupEvaluation(DataGridView component, Matrix data)
        {
            for (var i = 0; i < data.Height; i++)
            {
                component["groupEvaluation", i].Value = data.GroupEvaluations(data.InitialCompetenceCoefficient)[i];
            }
        }

        public static void CalculateCoefficients(DataGridView component, Matrix data)
        {
            var competenceCoefficients = data.CompetenceCoefficients();

            for (var i = 0; i < data.Width; i++)
            {
                component[i, data.Height].Value = competenceCoefficients[i];
            }
        }
    }
}
