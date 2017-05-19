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
        public static List<Alternative> ReadAsCsv(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return lines
                .Select(line => new Alternative(line.Split(' ').Select(Convert.ToDecimal).ToList()))
                .ToList();
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
        }

        // Извлечь матрицу из таблицы
        public static List<Alternative> ExtractData(DataGridView component, InitialData initialData)
        {
            var alternatives = new List<Alternative>();

            for (var i = 0; i < initialData.AlternativesCount; i++)
            {
                var values = new List<decimal>();
                for (var j = 0; j < initialData.ExpertsCount; j++)
                {
                    var value = Convert.ToDecimal(component.Rows[i].Cells[j].Value);

                    if (!initialData.RatingScale.Includes(value))
                    {
                        throw new FormatException("Оценка вышла за пределы шкалы");
                    }

                    values.Add(value);
                }
                alternatives.Add(new Alternative(values));
            }

            return alternatives;
        }

        // Заполнить таблицу матрицей
        public static void FillDataGridView(DataGridView component, Matrix data)
        {
            for (var i = 0; i < data.InitialData.AlternativesCount; i++)
            {
                for (var j = 0; j < data.InitialData.ExpertsCount; j++)
                {
                    component.Rows[i].Cells[j].Value = data[i, j];
                }
            }
        }
    }
}
