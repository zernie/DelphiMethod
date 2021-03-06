﻿using System;
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
        public static void InitInputDataGridView(DataGridView component, Config config)
        {
            for (var i = 0; i < config.m; i++)
            {
                component.Columns.Add(i.ToString(), config.Experts[i]);
            }

            for (var i = 0; i < config.n; i++)
            {
                component.Rows.Add(new DataGridViewRow
                {
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = config.Alternatives[i]
                    },
                });
            }

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
        public static void InitResultDataGridView(DataGridView component, MatrixList matrices, List<Matrix> disabled)
        {
            var config = matrices.Configuration;

            for (var i = 0; i < config.l; i++)
            {
                var indicator = config.Indicators[i].Title;
                component.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = indicator,
                    ReadOnly = true,
                    DefaultCellStyle =
                    {
                        BackColor = disabled.Contains(matrices[i])? Color.LimeGreen : Color.White,
                    }
                });
            }

            for (var i = 0; i < config.n; i++)
            {
                component.Rows.Add(new DataGridViewRow
                {
                    ReadOnly = true,
                    HeaderCell = new DataGridViewRowHeaderCell
                    {
                        Value = config.Alternatives[i]
                    }
                });
            }

            component.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "sum",
                HeaderText = "xj",
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
            for (var i = 0; i < component.ColumnCount; i++)
            {
                component[i, component.RowCount - 1].Value = "";
            }
        }

        // Вычислить коэффициенты компетентности и вывести их на форму
        public static void CalculateCoefficients(DataGridView component, Matrix x)
        {
            var coefficients = x.Ki();

            for (var i = 0; i < x.m; i++)
            {
                component[i, x.n].Value = Math.Round(coefficients[i], 3);
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
                MatrixList ranks;

                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    ranks = (MatrixList)serializer.Deserialize(fs);
                }

                return ranks;
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        // Экспорт в файл
        public static void ExportToFile(string filename, MatrixList ranks)
        {
            try
            {
                var x = new BinaryFormatter();

                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    x.Serialize(fs, ranks);
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
