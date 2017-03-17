using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Apassos.reports
{
    /// <summary>
    /// Classe para geração de arquivos xls.
    /// </summary>
    public class Excel
    {
        /// <summary>
        /// Variável do compenente de manipulação de xls.
        /// </summary>
        private ExcelPackage _excel;

        /// <summary>
        /// Construtor default.
        /// </summary>
        public Excel()
        {
            // Instancia o componente
            this._excel = new ExcelPackage();
        }

        /// <summary>
        /// Construtor com o autor e o título do arquivo.
        /// </summary>
        public Excel(string author, string title)
          : this()
        {
            // Define autor e título do workbook
            this._excel.Workbook.Properties.Author = author;
            this._excel.Workbook.Properties.Title = title;
        }

        /// <summary>
        /// Propriedade que retorna o objeto workbook.
        /// </summary>
        public ExcelWorkbook Workbook { get { return this._excel.Workbook; } }

        /// <summary>
        /// Propriedade que retorna a lista de planilhas.
        /// </summary>
        public ExcelWorksheets Worksheets { get { return this._excel.Workbook.Worksheets; } }

        /// <summary>
        /// Abre uma planilha xls.
        /// </summary>
        /// <param name="fileName">Caminho completo do arquivo.</param>
        public void Open(string fileName)
        {
            // Valida a existência do arquivo
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("arquivo_nao_encontrado", fileName);
            }

            // Carrega o arquivo
            this._excel.Load(new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite));
        }

        /// <summary>
        /// Salva a planilha.
        /// </summary>
        public void Save()
        {
            this.SetProperties();
            this._excel.Save();
        }

        private void SetProperties()
        {
            this._excel.Workbook.Properties.Author = "APS";
            this._excel.Workbook.Properties.Company = "Apassos Consultoria & Software";
            this._excel.Workbook.Properties.Category = "APS";
            this._excel.Workbook.Properties.Comments = "Documento gerado pelo sistema APS";
            this._excel.Workbook.Properties.Keywords = "APS, SAP, EXCEL";
            this._excel.Workbook.Properties.Manager = "Apassos Consultoria & Software";
            this._excel.Workbook.Properties.Title = "Documento gerado pelo sistema APS";
        }

        /// <summary>
        /// Salvar planilha como ...
        /// </summary>
        /// <param name="fileName">Nome do arquivo.</param>
        public void SaveAs(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                this.SetProperties();
                this._excel.SaveAs(file);
                file.Flush();
                file.Close();
            }
        }

        /// <summary>
        /// Salva e Fecha pacote com a planilha xls.
        /// </summary>
        public void Close()
        {
            this._excel.Save();
        }

    }
}