﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    class Utils
    {
        // Считать матрицу из файла
        public static Matrix ReadAsCsv(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var data = new List<Alternative>();

            foreach (var line in lines)
            {
                var rows = line.Split(' ').ToList().Select(Convert.ToDecimal).ToList();

                data.Add(new Alternative(rows));
            }

            return new Matrix(data);
        }

        // Сохранить матрицу в файл
        public static void SaveAsCsv(Matrix data, string filename)
        {
            File.WriteAllText(filename, data.ToString());
        }

        // Инициализировать таблицу заданным количеством строк и столбцов
        public static void InitDataGridView(DataGridView component, int rows, int cols)
        {
            component.Rows.Clear();
            component.Columns.Clear();
            for (var i = 0; i < cols; i++)
            {
                component.Columns.Add(i.ToString(), $"Альтернатива №{i + 1}");
            }

            for (var i = 0; i < rows; i++)
            {
                component.Rows.Add();
                component.Rows[i].HeaderCell.Value = $"Эксперт №{i + 1}";
            }
        }

        // Извлечь матрицу из таблицы
        public static Matrix ExtractData(DataGridView component, int rows, int cols)
        {
            var value = "";

            try
            {
                var matrix = new Matrix();

                for (var i = 0; i < rows; i++)
                {
                    var values = new List<decimal>();
                    for (var j = 0; j < cols; j++)
                    {
                        value = component[j, i].Value.ToString();
                        values.Add(Convert.ToDecimal(value));
                    }
                    matrix.Alternatives.Add(new Alternative(values));
                }

                return matrix;
            }
            catch (FormatException e)
            {
                e.Data.Add("value", value);
                throw;
            }
        }

        // Заполнить таблицу матрицей
        public static void FillDataGridView(DataGridView component, Matrix data, int rows, int cols)
        {
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    component[j, i].Value = data[i, j].ToString();
                }
            }
        }
    }
}