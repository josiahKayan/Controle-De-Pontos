using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Apassos.Common.Extensions;
using System.Diagnostics;

namespace Apassos.reports.classes
{
  public class BaseXLS
  {
    protected Period periodoAtual;
    protected Period periodoFinal;
    protected Period periodoInicial;
    protected string filename;
    protected IWorkbook wb;
    protected int indexRow = 0;
    protected ISheet sheet;
    protected ICellStyle styleDefault;
    protected ICellStyle styleDefaultHora;
    protected ICellStyle styleDefaultResolved;
    protected ICellStyle styleDefaultData;
    protected ICellStyle styleDefaultDataFeriadoFinalSemana;
    protected ICellStyle styleDefaultDiaFeriadoFinalSemana;



    protected readonly string WITH_PARTNER_CONST = "WITH_PARTNER";
    protected readonly string NO_PARTNER_CONST = "NO_PARTNER";
    protected readonly string BOTH_ITEMS_CONST = "BOTH_ITEMS";
    protected readonly string ONLY_PROJECT_CONST = "ONLY_PROJECT";
    protected readonly string ONLY_PARTNER_CONST = "ONLY_PARTNER";
    protected readonly string NO_PROJECT_CONST = "NO_PROJECT";

    protected List<List<string>> cellsIds = new List<List<string>>();

    public BaseXLS()
    {
    }


    public BaseXLS(string periodid)
    {
            PeriodDataAccess period = new PeriodDataAccess();
      this.periodoAtual = period.GetPeriodo(periodid);
      this.filename = "apontamentos_" + periodoAtual.YEAR + "_" + periodoAtual.MONTH + ".xlsx";
      this.wb = new XSSFWorkbook();
    }

    private void CriaAbas()
    {
    }


    public void ExecuteSaveFile()
    {
      var dir = ConfigurationManager.AppSettings["DIREXPORTSXLS"].ToString();

      using (var fileData = new FileStream(dir + filename, FileMode.Create))
      {
        this.wb.Write(fileData);

      }
    }

    public void Execute(HttpContextBase context)
    {
      HttpResponseBase response = context.Response;
      var dir = ConfigurationManager.AppSettings["DIREXPORTSXLS"].ToString();

      response.Clear();
      response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", this.filename));

      using (var f = File.Create(dir + filename))
      {
        this.wb.Write(f);
      }
      response.WriteFile(dir + filename);
      response.Flush();
      response.End();
    }


    protected void CriaCapa()
    {
      this.indexRow = 0;
      this.sheet = wb.CreateSheet("Capa");
      var row = sheet.CreateRow(this.indexRow);
      row.CreateCell(0).SetCellValue("RELATÓRIO DE APONTAMENTOS");

      this.indexRow++;
      row = sheet.CreateRow(this.indexRow);
      row.CreateCell(2).SetCellValue("PERIODO: " + periodoAtual.MONTH.ToString("00") + "/" + periodoAtual.YEAR.ToString());

    }

    protected void CriaAbaGeral(List<PartnersTimesheetHeaderAccess> listaConsultores)
    {
      this.indexRow = 0;
      this.sheet = wb.CreateSheet("Geral");

      IniciaStyleCellDefault();
      styleDefaultHora = GetIniciaStyleCellMask("hh:mm", HSSFColor.White.Index);
      //styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index); 
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      CriaCabecalho();
      foreach (PartnersTimesheetHeaderAccess consultor in listaConsultores)
      {
        AdicionaLinhasApontamentos(consultor.items, false);
      }
    }


    protected void CriaAbaGeralMensal(List<PartnersTimesheetHeaderAccess> listaConsultores)
    {
      this.indexRow = 0;
      this.sheet = wb.CreateSheet("Geral");

      IniciaStyleCellDefault();
      //styleDefaultHora = GetIniciaStyleCellMask("hh:mm", HSSFColor.White.Index);
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      CriaCabecalho();
      foreach (PartnersTimesheetHeaderAccess consultor in listaConsultores)
      {
        AdicionaLinhasApontamentosMensal(consultor.items, false);
      }
    }

    protected void CriaAbaGeralRelatorio(List<PartnersTimesheetHeaderAccess> listaConsultores)
    {
      this.indexRow = 0;
      this.sheet = wb.CreateSheet("Geral");

      IniciaStyleCellDefault();
      //styleDefaultHora = GetIniciaStyleCellMask("hh:mm", HSSFColor.White.Index);
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      CriaCabecalho();
      foreach (PartnersTimesheetHeaderAccess consultor in listaConsultores)
      {
        AdicionaLinhasApontamentos(consultor.items, false);
      }
    }


    protected void CriaAbaConsultor(PartnersTimesheetHeaderAccess consultorApontamentos)
    {
      this.sheet = wb.CreateSheet(consultorApontamentos.partner.SHORTNAME);
      this.indexRow = -1;

      IniciaStyleCellDefault();
      styleDefaultHora = GetIniciaStyleCellMask("hh:mm", HSSFColor.White.Index);
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      this.AdicionaLinhasApontamentos(consultorApontamentos.items, true);

    }

    protected void CriaAbaConsultorRelatorio(PartnersTimesheetHeaderAccess consultorApontamentos)
    {
      this.sheet = wb.CreateSheet(consultorApontamentos.partner.SHORTNAME);
      this.indexRow = -1;

      IniciaStyleCellDefault();
      styleDefaultHora = GetIniciaStyleCellMask("hh:mm", HSSFColor.White.Index);
      styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index);
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      this.AdicionaLinhasApontamentos(consultorApontamentos.items, true);

    }



    // Cria o Excel para Horas Mensal por projeto
    protected void CriaAbaProjetoConsultorHoras(List<RelatorioHoras> relatorioHoras, string exportToExcel)
    {

      if (exportToExcel.Equals(WITH_PARTNER_CONST) || exportToExcel.Equals(BOTH_ITEMS_CONST)
          || exportToExcel.Equals(ONLY_PARTNER_CONST))
      {
        this.sheet = wb.CreateSheet("Horas Total Consultor");
      }
      else
      {
        this.sheet = wb.CreateSheet("Horas Total por Projeto");
      }


      this.indexRow = -1;

      IniciaStyleCellDefault();
      //styleDefaultHora = GetIniciaStyleCellHora("hh:mm", HSSFColor.White.Index);
      styleDefaultData = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.White.Index);
      styleDefaultResolved = CellMask("[h]:mm:ss", HSSFColor.White.Index);
      styleDefaultDataFeriadoFinalSemana = GetIniciaStyleCellMask("dd/mm/yyyy", HSSFColor.Red.Index);
      styleDefaultDiaFeriadoFinalSemana = GetIniciaStyleCellMask("", HSSFColor.Red.Index);
      this.AdicionaLinhasApontamentosProjetoConsultorHoras(relatorioHoras, true, exportToExcel);

    }

    private void IniciaStyleCellDefault()
    {
      IFont font = wb.CreateFont();
      font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
      this.styleDefault = wb.CreateCellStyle();
      this.styleDefault.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
      this.styleDefault.FillPattern = FillPattern.SolidForeground;
      this.styleDefault.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
      this.styleDefault.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
      this.styleDefault.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
      this.styleDefault.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
      this.styleDefault.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
      this.styleDefault.SetFont(font);
      this.styleDefault.WrapText = true;
    }



    private ICellStyle GetIniciaStyleCellHora(string mask, short color)
    {
      ICellStyle styleDefaultLocal = wb.CreateCellStyle();
      IFont font = wb.CreateFont();
      font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
      styleDefaultLocal.FillForegroundColor = color;
      styleDefaultLocal.FillPattern = FillPattern.SolidForeground;
      styleDefaultLocal.FillBackgroundColor = color;
      styleDefaultLocal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.SetFont(font);
      styleDefaultLocal.WrapText = true;




      styleDefaultLocal.Alignment = HorizontalAlignment.Right;
      if (mask != "")
      {
        IDataFormat df = wb.CreateDataFormat();
        styleDefaultLocal.DataFormat = df.GetFormat(mask);
      }
      return styleDefaultLocal;
    }

    private ICellStyle GetIniciaStyleCellMask(string mask, short color)
    {
      ICellStyle styleDefaultLocal = wb.CreateCellStyle();
      IFont font = wb.CreateFont();
      font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
      styleDefaultLocal.FillForegroundColor = color;
      styleDefaultLocal.FillPattern = FillPattern.SolidForeground;
      styleDefaultLocal.FillBackgroundColor = color;
      styleDefaultLocal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.SetFont(font);
      styleDefaultLocal.WrapText = true;

      if (mask != "")
      {
        IDataFormat df = wb.CreateDataFormat();
        styleDefaultLocal.DataFormat = df.GetFormat(mask);
      }
      return styleDefaultLocal;
    }


    private ICellStyle CellMask(string mask, short color)
    {
      ICellStyle styleDefaultLocal = wb.CreateCellStyle();
      IFont font = wb.CreateFont();
      font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
      styleDefaultLocal.FillForegroundColor = color;
      styleDefaultLocal.FillPattern = FillPattern.SolidForeground;
      styleDefaultLocal.FillBackgroundColor = color;
      styleDefaultLocal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
      styleDefaultLocal.SetFont(font);
      styleDefaultLocal.WrapText = true;

      if (mask != "")
      {
        IDataFormat df = wb.CreateDataFormat();
        styleDefaultLocal.DataFormat = df.GetFormat(mask);
      }
      return styleDefaultLocal;
    }



    public void SetStyleRegion(short forecolor, short bgcolor, int row, int col, int rowEnd, int colEnd)
    {
      IFont font = wb.CreateFont();
      font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
      for (int rowY = row; rowY <= rowEnd; rowY++)
      {
        for (int colX = col; colX <= colEnd; colX++)
        {
          SetBackgroundColor(forecolor, bgcolor, rowY, colX);
          ICellStyle style1 = getLinha(rowY).GetCell(colX).CellStyle;
          style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
          style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
          style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
          style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
          style1.SetFont(font);

        }
      }
    }

    public void SetBackgroundColor(short forecolor, short bgcolor, int row, int col)
    {
      //fill background
      ICellStyle style1 = wb.CreateCellStyle();
      style1.FillForegroundColor = forecolor;
      style1.FillPattern = FillPattern.SolidForeground;
      style1.FillBackgroundColor = bgcolor;
      getLinha(row).GetCell(col).CellStyle = style1;
    }

    public void WidthCell(int col, int wid)
    {
      this.sheet.SetColumnWidth(col, wid * 256);
    }

    public void AutoWidthCell(int col)
    {
      ICellStyle cellstyle1 = wb.CreateCellStyle();
      cellstyle1.ShrinkToFit = true;
      var cell1 = getLinha(indexRow).GetCell(col);
      cell1.CellStyle = cellstyle1;
    }

    public void HeightCell(IRow row1, short hei)
    {
      row1.Height = (short)(hei * 20);
    }

    public ICell novaColuna(IRow row, Object value)
    {
      return novaColuna(row, value, 0, "", 0);
    }

    public ICell novaColuna(IRow row, Object value, string style, int cellsToMerge)
    {
      return novaColuna(row, value, 0, style, cellsToMerge);
    }

    public ICell novaColuna(IRow row, Object value, short width)
    {
      return novaColuna(row, value, width, "", 0);
    }

    public ICell novaColuna(IRow row, Object value, string style)
    {
      return novaColuna(row, value, 0, style, 0);
    }

    public ICell novaColuna(IRow row, Object value, short width, string style, int cellsToMerge)
    {

      short col = row.LastCellNum;
      if (col < 0)
      {
        col = 0;
      }

      if (cellsToMerge > 0)
      {
        int nrow = row.RowNum;
        var cellRange = new CellRangeAddress(nrow, nrow + (cellsToMerge - 1), col, col);
        sheet.AddMergedRegion(cellRange);
      }

      if (value is DateTime)
      {
        row.CreateCell(col).SetCellValue((DateTime)value);
      }
      else if (value is bool)
      {
        row.CreateCell(col).SetCellValue((bool)value);
      }

      else if (value is IRichTextString)
      {
        row.CreateCell(col).SetCellValue((IRichTextString)value);
      }
      else if (value is string)
      {
        row.CreateCell(col).SetCellValue((string)value);
      }
      else if (value is double)
      {
        row.CreateCell(col).SetCellValue((double)value);
      }

      if (width > 0)
      {
        WidthCell(col, width);
      }

      if (style == "dd/mm/yyyy")
      {
        DateTime dfds = DateTime.FromOADate((double)value);
        if (Util.IsFeriadoFinalDeSemana(dfds))
        {
          row.GetCell(col).CellStyle = styleDefaultDataFeriadoFinalSemana;
        }
        else
        {
          row.GetCell(col).CellStyle = styleDefaultData;

        }
      }
      else if (style == "hh:mm")
      {
        row.GetCell(col).CellStyle = styleDefaultHora;
      }

      else if (style == "[h]:mm:ss")
      {
        row.GetCell(col).CellStyle = styleDefaultResolved;
      }
      

      else if (style == "diasemana")
      {
        var diasemana = (string)value;
        if (diasemana.ToLower().In("sab", "sáb", "dom", "feriado"))
        {
          row.GetCell(col).CellStyle = styleDefaultDiaFeriadoFinalSemana;
        }
        else
        {
          row.GetCell(col).CellStyle = styleDefault;
        }
      }
      else
      {
        row.GetCell(col).CellStyle = styleDefault;

      }

      return row.GetCell(col);
    }

    public IRow novaLinha()
    {
      this.indexRow++;
      var row = sheet.CreateRow(this.indexRow);
      return row;
    }

    public IRow getLinha(int linha)
    {
      var row = sheet.GetRow(linha);
      return row;
    }


    protected void AdicionaLinhasApontamentos(List<TimesheetItem> apontamentos, bool adicionaCabecalho)
    {
      IRow row;
      if (adicionaCabecalho)
      {
        CriaCabecalho();
      }

      string dataApontamento = string.Empty;
      foreach (TimesheetItem item in apontamentos)
      {
        row = novaLinha();
        string grupo = item.TimesheetHeader.Partner.USERGROUP;
        if (grupo == null)
        {
          grupo = "";
        }

        novaColuna(row, item.TimesheetHeader.Period.YEAR.ToString("0000"));
        novaColuna(row, item.TimesheetHeader.Period.MONTH.ToString("00"));
        novaColuna(row, grupo);
        novaColuna(row, item.TimesheetHeader.Partner.SHORTNAME);
        novaColuna(row, Common.Constants.GetEnumDescription((Common.Constants.SelecaoGestorStatusAprovacaoConstant)item.STATUS));

        ICell cell = novaColuna(row, item.DATE.ToOADate(), "dd/mm/yyyy");

        novaColuna(row, Util.DiaDaSemana(item.DATE, "ddd"), "diasemana");
        novaColuna(row, item.project.Partner.SHORTNAME);
        novaColuna(row, item.project.NAME);
        novaColuna(row, Constants.GetDescricaoTipoApontamentos(item.TYPE));

        cell = novaColuna(row, new DateTime(item.DATE.Year, item.DATE.Month, item.DATE.Day, item.IN.Hours, item.IN.Minutes, 0).ToOADate(), "hh:mm");

        ICell cellOut = novaColuna(row, new DateTime(item.DATE.Year, item.DATE.Month, item.DATE.Day, item.OUT.Hours, item.OUT.Minutes, 0).ToOADate(), "hh:mm");

        ICell cellBreak = novaColuna(row, new DateTime(1900, 1, 1, item.BREAK.Hours, item.BREAK.Minutes, 0).ToOADate(), "hh:mm");
        cellBreak.SetCellType(CellType.Formula);
        cellBreak.SetCellFormula("time(" + item.BREAK.Hours + "," + item.BREAK.Minutes + "," + item.BREAK.Seconds + ")");

        try
        {
          ICell cellTotal = novaColuna(row, new DateTime(1900, 1, 1, item.TotalHours.Hours, item.TotalHours.Minutes, 0).ToOADate(), "hh:mm");
          cellTotal.SetCellType(CellType.Formula);
          cellTotal.SetCellFormula("time(" + item.TotalHours.Hours + "," + item.TotalHours.Minutes + "," + item.TotalHours.Seconds + ")");
        }
        catch (Exception ex)
        {
          ICell cellTotal = novaColuna(row, "Erro");
          Util.EscreverLog(ex.Message + ". Total negativo: " + item.TotalHours.Hours + "id:" + item.TIMESHEETITEMID, item.TimesheetHeader.Partner.PARTNERID.ToString());
        }

        int totalDays = 0;
        if (!item.DATE.ToString("yyyy/MM/dd").Equals(dataApontamento))
        {
          dataApontamento = item.DATE.ToString("yyyy/MM/dd");
          List<TimesheetItem> items = apontamentos.FindAll(x => x.DATE.ToString("yyyy/MM/dd") == dataApontamento);

          totalDays = items.Count();
          var ticks = items.Sum(x => x.TotalHours.Ticks);

          item.TotalHoursDay = TimeSpan.FromTicks(ticks);
        }

        ICell cellHorasTotal = novaColuna(row, new DateTime(1900, 1, 1, item.TotalHoursDay.Hours, item.TotalHoursDay.Minutes, 0).ToOADate(), "hh:mm", totalDays);
        cellHorasTotal.SetCellType(CellType.Formula);
        cellHorasTotal.SetCellFormula("time(" + item.TotalHoursDay.Hours + "," + item.TotalHoursDay.Minutes + "," + item.TotalHoursDay.Seconds + ")");

        novaColuna(row, item.DESCRIPTION);
        novaColuna(row, item.NOTE);
      }
    }

    protected void AdicionaLinhasApontamentosMensal(List<TimesheetItem> apontamentos, bool adicionaCabecalho)
    {
      IRow row;
      if (adicionaCabecalho)
      {
        CriaCabecalho();
      }

      string dataApontamento = string.Empty;
      foreach (TimesheetItem item in apontamentos)
      {
        row = novaLinha();
        string grupo = item.TimesheetHeader.Partner.USERGROUP;
        if (grupo == null)
        {
          grupo = "";
        }

        novaColuna(row, item.TimesheetHeader.Period.YEAR.ToString("0000"));
        novaColuna(row, item.TimesheetHeader.Period.MONTH.ToString("00"));
        novaColuna(row, grupo);
        novaColuna(row, item.TimesheetHeader.Partner.SHORTNAME);
        novaColuna(row, Common.Constants.GetEnumDescription((Common.Constants.SelecaoGestorStatusAprovacaoConstant)item.STATUS));

        ICell cell = novaColuna(row, item.DATE.ToOADate(), "dd/mm/yyyy");

        novaColuna(row, Util.DiaDaSemana(item.DATE, "ddd"), "diasemana");
        novaColuna(row, item.project.Partner.SHORTNAME);
        novaColuna(row, item.project.NAME);
        novaColuna(row, Constants.GetDescricaoTipoApontamentos(item.TYPE));

        cell = novaColuna(row, new DateTime(item.DATE.Year, item.DATE.Month, item.DATE.Day, item.IN.Hours, item.IN.Minutes, 0).ToOADate(), "hh:mm");

        ICell cellOut = novaColuna(row, new DateTime(item.DATE.Year, item.DATE.Month, item.DATE.Day, item.OUT.Hours, item.OUT.Minutes, 0).ToOADate(), "hh:mm");

        ICell cellBreak = novaColuna(row, new DateTime(1900, 1, 1, item.BREAK.Hours, item.BREAK.Minutes, 0).ToOADate(), "hh:mm");
        cellBreak.SetCellType(CellType.Formula);
        cellBreak.SetCellFormula("time(" + item.BREAK.Hours + "," + item.BREAK.Minutes + "," + item.BREAK.Seconds + ")");

        try
        {
          ICell cellTotal = novaColuna(row, new DateTime(1900, 1, 1, item.TotalHours.Hours, item.TotalHours.Minutes, 0).ToOADate(), "hh:mm");
          cellTotal.SetCellType(CellType.Formula);
          cellTotal.SetCellFormula("time(" + item.TotalHours.Hours + "," + item.TotalHours.Minutes + "," + item.TotalHours.Seconds + ")");
        }
        catch (Exception ex)
        {
          ICell cellTotal = novaColuna(row, "Erro");
          Util.EscreverLog(ex.Message + ". Total negativo: " + item.TotalHours.Hours + "id:" + item.TIMESHEETITEMID, item.TimesheetHeader.Partner.PARTNERID.ToString());
        }

        int totalDays = 0;
        if (!item.DATE.ToString("yyyy/MM/dd").Equals(dataApontamento))
        {
          dataApontamento = item.DATE.ToString("yyyy/MM/dd");
          List<TimesheetItem> items = apontamentos.FindAll(x => x.DATE.ToString("yyyy/MM/dd") == dataApontamento);
          totalDays = items.Count();
          var ticks = items.Sum(x => x.TotalHours.Ticks);
          item.TotalHoursDay = TimeSpan.FromTicks(ticks);
        }

        novaColuna(row, new DateTime(1900, 1, 1, item.TotalHoursDay.Hours, item.TotalHoursDay.Minutes, 0).ToOADate(), "hh:mm", totalDays);
        novaColuna(row, item.DESCRIPTION);
        novaColuna(row, item.NOTE);
      }
    }

    protected void AdicionaLinhasApontamentosProjetoConsultorHoras(List<RelatorioHoras> listaRelatorio, bool adicionaCabecalho, string exportToExcel)
    {
      IRow row;
      if (adicionaCabecalho)
      {
        CriaCabecalhoProjetoConsultorHoras(exportToExcel);
      }
      foreach (RelatorioHoras item in listaRelatorio)
      {
        row = novaLinha();

        novaColuna(row, item.ProjectName);
        if (exportToExcel.Equals(WITH_PARTNER_CONST) || exportToExcel.Equals(BOTH_ITEMS_CONST) || exportToExcel.Equals(ONLY_PARTNER_CONST))
        {
          novaColuna(row, item.ConsultorName);
        }

        //novaColuna(row, item.TotalHours, 0, "hh:mm", 0);

        //string[] t = item.TotalHours.Replace(':', "");



        //TimeSpan ts = new /*TimeSpan*/(int.Parse(t[0]), int.Parse(t[1]), int.Parse(t[2]));

        //string tt = string.Format("{0:hh\\:mm\\:ss}",ts);

        //novaColuna(row, item.TotalHours);

        //novaColuna(row, new T(int.Parse(t[0]), int.Parse(t[1]), 0).ToOADate(), "[h]:mm:ss");

        string []t = item.TotalHours.Split(':');
        string x = t[0] +","+ t[1] ;
        double g = double.Parse(x) / 24;
        novaColuna(row, g, "[h]:mm:ss");

        //cellTotal.SetCellType(CellType.Formula);
        //cellTotal.SetCellFormula("time(" + item.TotalHours.Hours + "," + item.TotalHours.Minutes + "," + item.TotalHours.Seconds + ")");

      }
    }

    private IRow CriaCabecalho()
    {
      indexRow = -1;
      var row = novaLinha();
      novaColuna(row, "ANO");
      novaColuna(row, "MES");
      novaColuna(row, "GRUPO");
      novaColuna(row, "CONSULTOR");
      novaColuna(row, "STATUS");
      novaColuna(row, "DATA");
      novaColuna(row, "DIA");
      novaColuna(row, "CLIENTE");
      novaColuna(row, "PROJETO");
      novaColuna(row, "TIPO");
      novaColuna(row, "INICIO");
      novaColuna(row, "TERMINO", 12);
      novaColuna(row, "INTERVALO");
      novaColuna(row, "TOTAL");
      novaColuna(row, "TOTAL DIA");
      novaColuna(row, "OBSERVACAO", 25);
      novaColuna(row, "NOTA DO GESTOR", 25);

      SetStyleRegion(HSSFColor.Yellow.Index, HSSFColor.Yellow.Index, indexRow, 0, indexRow, 16);

      sheet.SetAutoFilter(new CellRangeAddress(indexRow, indexRow, 0, 16));

      return row;
    }



    private IRow CriaCabecalhoProjetoConsultorHoras(string exportToExcel)
    {
      indexRow = -1;

      var row = novaLinha();

      int column = 0;
      if (exportToExcel.Equals(WITH_PARTNER_CONST) || exportToExcel.Equals(BOTH_ITEMS_CONST) || exportToExcel.Equals(ONLY_PARTNER_CONST))
      {

        column = 2;
        novaColuna(row, "PROJETO");
        novaColuna(row, "CONSULTOR");
        novaColuna(row, "TOTAL DE HORAS");
      }
      else
      {
        column = 1;
        novaColuna(row, "PROJETO");
        novaColuna(row, "TOTAL DE HORAS");
        sheet.SetAutoFilter(new CellRangeAddress(indexRow, indexRow, 0, column));
      }

      SetStyleRegion(NPOI.HSSF.Util.HSSFColor.Yellow.Index, NPOI.HSSF.Util.HSSFColor.Yellow.Index, indexRow, 0, indexRow, column);

      sheet.SetAutoFilter(new CellRangeAddress(indexRow, indexRow, 0, column));

      return row;
    }

    public void AutoSizeColumn()
    {
      for (int col = 0; col < 16; col++)
      {
        sheet.AutoSizeColumn(col);
        try
        {
          sheet.SetColumnWidth(col, sheet.GetColumnWidth(col) + (5 * 256));
        }
        catch (Exception ex)
        {
          string stackTrace = ex.StackTrace;
          Debug.WriteLine(stackTrace);
          //sheet.SetColumnWidth(col, sheet.GetColumnWidth(col) + (5 * 100));
        }
        switch (col)
        {
        case 2:
          if (sheet.GetColumnWidth(col) > (20 * 256))
          {
            sheet.SetColumnWidth(col, (20 * 256));
          }
          break;
        case 3:
          if (sheet.GetColumnWidth(col) > (16 * 256))
          {
            sheet.SetColumnWidth(col, (16 * 256));
          }
          break;
        case 6:
          if (sheet.GetColumnWidth(col) > (22 * 256))
          {
            sheet.SetColumnWidth(col, (22 * 256));
          }
          break;
        case 8:
          if (sheet.GetColumnWidth(col) > (36 * 256))
          {
            sheet.SetColumnWidth(col, (36 * 256));
          }
          break;
        case 14:
          if (sheet.GetColumnWidth(col) > (56 * 256))
          {
            sheet.SetColumnWidth(col, (56 * 256));
          }
          break;
        case 15:
          if (sheet.GetColumnWidth(col) > (56 * 256))
          {
            sheet.SetColumnWidth(col, (56 * 256));
          }
          break;
        }
      }
      //zoom 80%
      this.sheet.SetZoom(4, 5);
      this.sheet.DisplayGridlines = false;

    }

  }
}