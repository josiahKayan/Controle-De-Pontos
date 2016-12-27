using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apassos.DataAccess
{

    public class RelatorioAccess
    {

        private static TimesheetContext db = new TimesheetContext();

        public static List<RelatorioHoras> GetListaRelatorioProjetoConsultorHoras(int ano, int mes)
        {
            List<RelatorioHoras> list = new List<RelatorioHoras>();

            list = db.Database.SqlQuery<RelatorioHoras>(GetRelatorioProjetoConsultorHorasQuery(mes, ano)).ToList();

            return list;
        }

        public static List<RelatorioHoras> GetListaRelatorioProjetoHoras(int ano, int mes, int projectId = 0, int partnerId = 0)
        {
            List<RelatorioHoras> list = new List<RelatorioHoras>();

            if (projectId == 0 && partnerId == 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(getRelatorioProjetoHorasQuery(ano, mes)).ToList();
            }
            else if (projectId != 0 && partnerId == 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportProjectAloneQuery(ano, mes, projectId)).ToList();
            }
            else if (projectId != 0 && partnerId != 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportProjectAndPartnerIdQuery(ano, mes, projectId, partnerId)).ToList();
            }
            else if (projectId == 0 && partnerId != 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportPartnerIdQuery(ano, mes, partnerId)).ToList();
            }

            return list;
        }
        
        private static string GetRelatorioProjetoConsultorHorasQuery(int ano, int mes)
        {
            string queryString = @"
          SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.SHORTNAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.YEAR = " + ano + " and per.MONTH = " + mes + ") AS foo) AS foo2) AS foo3 GROUP BY project_name, consultor_name;";
            return queryString;
        }

        private static string getRelatorioProjetoHorasQuery(int ano, int mes)
        {
            string queryString = @"
          SELECT project_name as ProjectName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name,
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.YEAR = " + ano + " and per.MONTH = " + mes + ") AS foo) AS foo2) AS foo3 GROUP BY project_name;";
            return queryString;
        }

        private static string GetReportProjectAloneQuery(int ano, int mes, int projectId)
        {
            string queryString = @"
        
            SELECT project_name as ProjectName,
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
                WHERE per.YEAR = " + ano + " and per.MONTH = " + mes + " and proj.PROJECTID = " + projectId + ")"
                + "AS foo) AS foo2) AS foo3 GROUP BY project_name;";
            return queryString;
        }

        private static string GetReportProjectAndPartnerIdQuery(int ano, int mes, int projectId, int partnetId)
        {
            string queryString = @"
        
            SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.NAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.YEAR = " + ano + " and per.MONTH = " + mes + " and proj.PROJECTID = "
               + projectId + " and consultor.PARTNERID = " + partnetId + ")"
                + "AS foo) AS foo2) AS foo3 GROUP BY project_name,consultor_name;";
            return queryString;
        }

        private static string GetReportPartnerIdQuery(int ano, int mes, int partnetId)
        {
            string queryString = @"
        
            SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.NAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.YEAR = " + ano + " and per.MONTH = " + mes + " and consultor.PARTNERID = " + partnetId + ")"
                + "AS foo) AS foo2) AS foo3 GROUP BY project_name,consultor_name;";
            return queryString;
        }




        //A partir daqui os novos construtores
        public static List<RelatorioHoras> GetListaRelatorioProjetoConsultorHoras(Period periodoInicial, Period periodoFinal)
        {
            List<RelatorioHoras> list = new List<RelatorioHoras>();
            string periodIncial = periodoInicial.TIMESHEETPERIODSTART.Value.ToString("yyyy-MM-dd");

            string periodFinal = periodoFinal.TIMESHEETPERIODFINISH.Value.ToString("yyyy-MM-dd");

            list = db.Database.SqlQuery<RelatorioHoras>(GetRelatorioProjetoConsultorHorasQueryy(periodIncial, periodFinal)).ToList();

            return list;
        }


        public static List<RelatorioHoras> GetListaRelatorioProjetoHoras(Period periodoInicial, Period periodoFinal, int projectId = 0, int partnerId = 0)
        {
            List<RelatorioHoras> list = new List<RelatorioHoras>();

            string initial = periodoInicial.TIMESHEETPERIODSTART.Value.ToString("yyyy-MM-dd");
            string final = periodoFinal.TIMESHEETPERIODFINISH.Value.ToString("yyyy-MM-dd");


            if (projectId == 0 && partnerId == 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(getRelatorioProjetoHorasQuery(initial, final)).ToList();
            }
            else if (projectId != 0 && partnerId == 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportProjectAloneQuery(initial, final, projectId)).ToList();
            }
            else if (projectId != 0 && partnerId != 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportProjectAndPartnerIdQuery(initial, final, projectId, partnerId)).ToList();
            }
            else if (projectId == 0 && partnerId != 0)
            {
                list = db.Database.SqlQuery<RelatorioHoras>(GetReportPartnerIdQuery(initial, final, partnerId)).ToList();
            }

            return list;
        }


        //A partir daqui
        private static string GetRelatorioProjetoConsultorHorasQueryy(string periodoInicial, string periodoFinal)
        {
            string queryString = @"
          SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.SHORTNAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.TIMESHEETPERIODSTART >= '"+periodoInicial+"'"+
               " and per.TIMESHEETPERIODFINISH <= '"+periodoFinal+"'"+
               ") AS foo) AS foo2) AS foo3 GROUP BY project_name, consultor_name;";
            return queryString;
        }


        private static string getRelatorioProjetoHorasQuery(string periodoInicial, string periodoFinal)
        {
            string queryString = @"
          SELECT project_name as ProjectName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name,
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.TIMESHEETPERIODSTART >= '" + periodoInicial + "'" +
               " and per.TIMESHEETPERIODFINISH <= '" + periodoFinal + "'" +
               ") AS foo) AS foo2) AS foo3 GROUP BY project_name;";
            return queryString;
        }

        private static string GetReportProjectAloneQuery(string periodoInicial, string periodoFinal, int projectId)
        {
            string queryString = @"
        
            SELECT project_name as ProjectName,
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.TIMESHEETPERIODSTART >= '" + periodoInicial + "'" +
               " and per.TIMESHEETPERIODFINISH <= '" + periodoFinal + "'" +
               " and proj.PROJECTID = " +projectId+ 
               ")AS foo) AS foo2) AS foo3 GROUP BY project_name;";
            return queryString;
        }


        private static string GetReportPartnerIdQuery(string periodoInicial, string periodoFinal, int partnetId)
        {
            string queryString = @"
        
            SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.NAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.TIMESHEETPERIODSTART >= '" + periodoInicial + "'" +
               " and per.TIMESHEETPERIODFINISH <= '" + periodoFinal + "'" +
               " and consultor.PARTNERID = " + partnetId + ")"
                + "AS foo) AS foo2) AS foo3 GROUP BY project_name,consultor_name;";
            return queryString;
        }

        private static string GetReportProjectAndPartnerIdQuery(string periodoInicial, string periodoFinal, int projectId, int partnetId)
        {

            string queryString = @"
        
            SELECT project_name as ProjectName, consultor_name as ConsultorName, 
             Cast( Floor(sum(total) / 60) as varchar) + ':' +
                  Right('00' + Cast( Floor((sum(total) % 60)) as varchar), 2) 
                  +':'+
                  Right('00' + Cast( Floor((sum(total) / 3600)) as varchar), 2)
                  AS TotalHours FROM
               (
              SELECT project_name, consultor_name, (horas_trabalhadas - intervalo_em_minutos) AS total FROM (
              SELECT project_name, consultor_name, 
               DATEDIFF(MINUTE,entrada,saida) AS horas_trabalhadas,
               DATEDIFF(MINUTE,'00:00:00',intervalo) AS intervalo_em_minutos FROM (
              select 
               proj.NAME AS project_name,
               consultor.NAME AS consultor_name,
               items.[IN] AS entrada,
               items.[OUT] AS saida,
               items.[BREAK] AS intervalo,
               items.[DATE] AS data
 
              from TimesheetItems items
               INNER join TimesheetHeaders headers on items.TimesheetHeader_TIMESHEETHEADERID = headers.TIMESHEETHEADERID
               INNER join Partners consultor on headers.Partner_PARTNERID = consultor.PARTNERID
               INNER join Projects proj on items.project_PROJECTID = proj.PROJECTID
               INNER join Partners empresa on proj.PARTNERID = empresa.PARTNERID
               INNER join Periods per on headers.Period_PERIODID = per.PERIODID
               WHERE per.TIMESHEETPERIODSTART >= '" + periodoInicial + "'" +
               " and per.TIMESHEETPERIODFINISH <= '" + periodoFinal + "'" +
               " and proj.PROJECTID = "
               + projectId + " and consultor.PARTNERID = " + partnetId + ")"
                + "AS foo) AS foo2) AS foo3 GROUP BY project_name,consultor_name;";
            return queryString;
        }

    }
}