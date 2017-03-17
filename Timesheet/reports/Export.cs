using Apassos.Observer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Apassos.Models;

namespace Apassos.reports
{
    public static class Export
    {

        public static void ToExcel<T>(this IEnumerable<T> list, bool showColumName = false, string fileName = "migration.xls", bool hasTotalRow = false)
        {
            // Carrega lista de objetos em DataTable
            DataTable table = list.ToDataTable();

            //Nome da Tabela
            table.TableName = "Integration TW + TS";

            // Cria xls
            Excel xls = new Excel();

            // Cria a planilha
            ExcelWorksheet sheet = xls.Worksheets.Add(table.TableName);

            // Controla a linha corrente
            short currentHeaderLine = 0;

            // Colunas em um array
            DataColumn[] columns = table.Columns.Cast<DataColumn>().ToArray();

            // Configura header
            if (showColumName)
            {
                // Nome do campo
                sheet.SetHeader(++currentHeaderLine, 1, columns.Select(c => c.ColumnName).ToArray());
            }

            //Descrição do campo não tem

            // Preenche dados
            sheet.SetData(currentHeaderLine + 1, 1, table.Rows.Count, table.CreateDataReader());

            // Pinta fundo de toda planilha de branco
            sheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
            sheet.Cells.Style.Font.Size = 10;

            // Header
            sheet.Select(new ExcelAddress(currentHeaderLine, 1, currentHeaderLine, columns.Length));
            sheet.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            sheet.SelectedRange.Style.Font.Bold = true;
            sheet.SelectedRange.AutoFilter = true;

            // Configura formatação dos campos
            int columnIndex = 0;

            foreach (DataColumn column in columns)
            {
                // Incrementa índice da coluna
                columnIndex++;

                if (column.DataType == typeof(DateTime) | column.DataType == typeof(DateTimeOffset) |
                    column.DataType == typeof(DateTime?) | column.DataType == typeof(DateTimeOffset?))
                {
                    sheet.Select(new ExcelAddress(currentHeaderLine + 1, columnIndex, table.Rows.Count + 1, columnIndex));
                    sheet.SelectedRange.Style.Numberformat.Format = "dd/MM/yyyy";
                }
                else if (column.DataType == typeof(double) | column.DataType == typeof(decimal) |
                         column.DataType == typeof(double?) | column.DataType == typeof(decimal?))
                {
                    sheet.Select(new ExcelAddress(currentHeaderLine + 1, columnIndex, table.Rows.Count + 1, columnIndex));
                    sheet.SelectedRange.Style.Numberformat.Format = "#,##0.00;-#,##0.00";
                }
                else if (column.DataType == typeof(short) | column.DataType == typeof(int) | column.DataType == typeof(long) |
                         column.DataType == typeof(short?) | column.DataType == typeof(int?) | column.DataType == typeof(long?))
                {
                    sheet.Select(new ExcelAddress(currentHeaderLine + 1, columnIndex, table.Rows.Count + 1, columnIndex));
                    sheet.SelectedRange.Style.Numberformat.Format = "0";
                }

            }

            // Configura as bordas
            if (table.Rows.Count > 0)
            {
                sheet.Select(new ExcelAddress(1, 1, table.Rows.Count + currentHeaderLine, columns.Length));
            }
            else
            {
                sheet.Select(new ExcelAddress(1, 1, currentHeaderLine + 1, columns.Length));
            }

            sheet.SelectedRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.SelectedRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.SelectedRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.SelectedRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Se tem linha de total e registros, pinta a última linha de amarelo
            if (hasTotalRow & table.Rows.Count > 0)
            {
                sheet.Select(new ExcelAddress(table.Rows.Count + currentHeaderLine, 1, table.Rows.Count + currentHeaderLine, columns.Length));
                sheet.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                sheet.SelectedRange.Style.Font.Bold = true;
            }

            // Redimensiona células
            sheet.SelectedRange.AutoFitColumns();

            // Remove a seleção atual
            sheet.Select(new ExcelAddress(1, 1, 1, 1));

            // Salva XLS
            xls.SaveAs(fileName);

            // Limpa memória
            xls = null;




        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {

            Type elementType = typeof(T);
            PropertyInfo[] typeProperties = elementType.GetProperties();

            using (DataTable table = new DataTable())
            {

                foreach (PropertyInfo propInfo in typeProperties)
                {
                    Type propType = propInfo.PropertyType;
                    Type columnType = Nullable.GetUnderlyingType(propType) ?? propType;
                    table.Columns.Add(propInfo.Name, columnType);
                }

                DataRow row;

                foreach (T item in list)
                {
                    // Cria nova linha
                    row = table.NewRow();

                    foreach (PropertyInfo propInfo in typeProperties)
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                    }

                    // Adiciona linha
                    table.Rows.Add(row);
                }

                return table;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="initialColumn"></param>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        public static void SetHeader(this ExcelWorksheet sheet, int row, int initialColumn, params string[] columns)
        {
            // Controle da coluna
            int currentColumn = initialColumn;

            // Atribui as colunas passadas por parâmetro
            foreach (string column in columns)
            {
                sheet.Cells[row, currentColumn++].Value = column;
            }

        }


        public static void SetData(this ExcelWorksheet sheet, int initialRow, int initialColumn, int rowsCount, IDataReader data)
        {
            // Controla as linhas da planilha
            int currentRow = initialRow;

            // Retorna a planilha completa
            ExcelRange cells = sheet.Cells;

            while (data.Read())
            {
                // Preenche valores das colunas
                for (int currentColumn = initialColumn; currentColumn <= data.FieldCount; currentColumn++)
                {
                    cells[currentRow, currentColumn].Value = data[currentColumn - initialColumn];
                }

                currentRow++;

            }
        }

    }
}