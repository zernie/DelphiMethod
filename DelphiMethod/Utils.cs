using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelphiMethod
{
    class Utils
    {
        // Считать матрицу из файла
        public static decimal[,] ReadAsCSV(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var firstLineColsCount = lines[0].Split(' ').Length;
            var data = new decimal[lines.Length, firstLineColsCount];

            var i = 0;

            foreach (var line in lines)
            {
                var j = 0;
                foreach (var row in line.Split(' '))
                {
                    data[i, j] = Convert.ToDecimal(row);
                    j++;
                }
                i++;
            }

            return data;
        }

        // Сохранить матрицу в файл
        public static void SaveAsCsv(decimal[,] data, string filename)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append((j == 0 ? "" : " ") + data[i, j]);
                }
                sb.AppendLine();
            }

            File.WriteAllText(filename, sb.ToString());
        }

        // Вычислить среднее арифметическое матрицы
        public static decimal[] CalcAverages(decimal[,] data)
        {
            var width = data.GetLength(1);
            var height = data.GetLength(0);

            var averages = new decimal[height];
            for (var i = 0; i < height; i++)
            {
                var sum = 0.0M;
                for (var j = 0; j < width; j++)
                {
                    sum += data[i, j];
                }

                averages[i] = Math.Round(sum / width, 2);
            }

            return averages;
        }

        // Вычислить медианы матрицы 
        public static decimal[] CalcMedians(decimal[,] data)
        {
            var width = data.GetLength(1);
            var height = data.GetLength(0);

            var medians = new decimal[height];

            for (var i = 0; i < height; i++)
            {
                var temp = new decimal[width];
                for (var j = 0; j < width; j++)
                {
                    temp[j] = data[i, j];
                }
                Array.Sort(temp);

                medians[i] = temp[temp.Length / 2];
            }

            return medians;
        }

        // Инициализировать таблицу заданным количеством строк и столбцов
        public static void InitDataGridView(DataGridView component, int rows, int cols)
        {
            component.Rows.Clear();
            component.Columns.Clear();
            for (var i = 1; i <= rows; i++)
            {
                component.Columns.Add(i.ToString(), $"Эксперт №{i}");
            }
            component.Rows.Add(cols);
        }
        
        // Извлечь матрицу из таблицы
        public static decimal[,] ExtractData(DataGridView component, int width, int height)
        {
            var value = "";

            try
            {
                var data = new decimal[width, height];

                for (var i = 0; i < width; i++)
                {
                    var row = component.Rows[i];

                    for (var j = 0; j < height; j++)
                    {
                        value = row.Cells[j].Value.ToString();
                        data[i, j] = Convert.ToDecimal(value);
                    }
                }

                return data;
            }
            catch (FormatException e)
            {
                e.Data.Add("value", value);
                throw;
            }
        }

        // Заполнить столбец массивом
        public static void FillColumn(DataGridView component, decimal[] data, string colname)
        {
            var index = component.Columns[colname].Index;
            var i = 0;

            foreach (DataGridViewRow row in component.Rows)
            {
                row.Cells[index].Value = data[i++];
            }

        }

        // Заполнить таблицу матрицей
        public static void FillDataGridView(DataGridView component, decimal[,] data)
        {
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    component.Rows[i].Cells[j].Value = data[i, j];
                }
            }
        }
    }
}
