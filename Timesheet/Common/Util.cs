using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Apassos.Common.Extensions;
using Apassos.Models;
using System.Globalization;
using System.Diagnostics;

namespace Apassos.Common
{
    public class Util
    {
        public static List<DateTime> GetDateListByRange(DateTime d1, DateTime d2)
        {
            List<DateTime> lstDates = new List<DateTime>();
            double diff = (d2 - d1).TotalDays;
            for (double cont = 0; cont <= diff; cont++)
            {
                lstDates.Add(d1.AddDays(cont));
            }
            return lstDates;
        }
        public static TimeSpan GetTotalHoras(TimeSpan timeInicio, TimeSpan timeFinal)
        {
            DateTime dtInicio = new DateTime().Add(timeInicio);
            DateTime dtFinal = new DateTime().Add(timeFinal);

            TimeSpan horaTotal = new TimeSpan(dtFinal.Ticks - dtInicio.Ticks);
            return horaTotal;
        }

        public static TimeSpan GetTotalHoras2(TimeSpan timeInicio, TimeSpan timeFinal)
        {
            DateTime dtInicio = new DateTime().Add(timeInicio);
            DateTime dtFinal = new DateTime().Add(timeFinal);

            TimeSpan horaTotal = new TimeSpan(dtFinal.Ticks - dtInicio.Ticks);
            TimeSpan result;

            if (horaTotal.Ticks < 0)
            {
                result = new TimeSpan(horaTotal.Ticks * (-1));
            }
            else
            {
                result = horaTotal;
            }

            return result;
        }

        public static TimeSpan GetTotalHoras(TimeSpan timeIn, TimeSpan timeOut, TimeSpan timeBreak)
        {
            DateTime dtInicio = new DateTime().Add(timeIn);
            DateTime dtFinal = new DateTime().Add(timeOut);
            DateTime dtBreak = new DateTime().Add(timeBreak);

            TimeSpan timeTotal = new TimeSpan((dtFinal.Ticks - dtInicio.Ticks) - dtBreak.Ticks);

            return timeTotal;
        }


        public static TimeSpan GetTotalHoras2(TimeSpan timeIn, TimeSpan timeOut, TimeSpan timeBreak)
        {
            DateTime dtInicio = new DateTime().Add(timeIn);
            DateTime dtFinal = new DateTime().Add(timeOut);
            DateTime dtBreak = new DateTime().Add(timeBreak);

            TimeSpan timeTotal = new TimeSpan((dtFinal.Ticks - dtInicio.Ticks) - dtBreak.Ticks);

            return timeTotal;
        }

        public static TimeSpan AddHoras(TimeSpan time, TimeSpan timeAdd)
        {
            TimeSpan timeTotal = time.Add(timeAdd);
            return timeTotal;
        }

        public static String AddHoras(TimeSpan time)
        {
            int days = time.Days;
            int hours = ((days * 24) + time.Hours);
            int minutes = time.Minutes;

            return hours.ToString().PadLeft(3, ' ') + ":" + minutes.ToString().PadLeft(2, '0');
        }

        public static DateTime GetPrimeiroDiaMes(int ano, int mes)
        {
            return new DateTime(ano, mes, 1);
        }

        public static DateTime GetUltimoDiaMes(int ano, int mes)
        {
            return new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
        }

        public static void EscreverLog(string message, object usuarioLogado)
        {
            try
            {
                string login = "System";

                if (usuarioLogado is Users)
                {
                    login = ((Users)usuarioLogado).USERNAME;
                }
                else
                {
                    login = (string)usuarioLogado;
                }


                var dirlog = ConfigurationManager.AppSettings["DIRLOG"].ToString();
                StreamWriter valor = new StreamWriter(dirlog + "messages.log", true, Encoding.ASCII);
                valor.WriteLine("[" + login + " - " + DateTime.Now.ToString() + "]" + message);
                valor.Close();
            }
            catch (AcessosException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
        }

        public static void EscreverLog(string message, object usuarioLogado, string filename)
        {
            try
            {
                string login = "System";

                if (usuarioLogado is Users)
                {
                    login = ((Users)usuarioLogado).USERNAME;
                }
                else
                {
                    login = (string)usuarioLogado;
                }


                var dirlog = ConfigurationManager.AppSettings["DIRLOG"].ToString();

                StreamWriter valor = new StreamWriter(dirlog + filename, true, Encoding.ASCII);
                valor.WriteLine("[" + login + " - " + DateTime.Now.ToString() + "]" + message);
                valor.Close();
            }
            catch (AcessosException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
        }


        public static void GetFirstLastDayofWeek(int ano, int mes, int dia)

        {

            DateTime data = new DateTime(ano, mes, dia);

            //Variáveis de controle dos dias.
            int numeroMenor = 1, numeroMaior = 7;

            var dataInicioSemana = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());

            var dataFimSemana = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());

        }

        public static int GetWeekInMonth(DateTime date)
        {
            DateTime tempdate = date.AddDays(-date.Day + 1);
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNumStart = ciCurr.Calendar.GetWeekOfYear(tempdate, CalendarWeekRule.FirstFourDayWeek, ciCurr.DateTimeFormat.FirstDayOfWeek);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, ciCurr.DateTimeFormat.FirstDayOfWeek);
            return weekNum - weekNumStart + 1;
        }
        
        public static double GetTotalWeek(DateTime data, int numeroSemana)
        {
            int numeroMenor = 1, numeroMaior = 5;
            
            DateTime diaInicial, diaFinal;
            TimeSpan x = new TimeSpan();
            if (numeroSemana == 1)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 2)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                 x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 3)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                 x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 4)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                 x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 5)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                 x = diaFinal.Subtract(diaInicial);
            }

            return x.TotalHours;
        }


        public static double GetTotalWeekDone(DateTime data, int numeroSemana)
        {
            int numeroMenor = 1, numeroMaior = 5;
            DateTime diaInicial, diaFinal;
            TimeSpan x = new TimeSpan();
            if (numeroSemana == 1)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 2)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 3)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 4)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }
            else if (numeroSemana == 5)
            {
                diaInicial = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                diaFinal = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                x = diaFinal.Subtract(diaInicial);
            }

            return x.TotalHours;
        }


        /**
         * Captura o conteudo da primeira ocorrencia de uma tag.
         */
        public static string GetTagConteudo(string html, string tag)
        {
            var tagString = "<" + tag + ">";
            var endTagString = "</" + tag + ">";
            var fpos = html.IndexOf(tagString) + tagString.Length;
            var qtd = html.IndexOf(endTagString) - fpos;
            return html.Substring(fpos, qtd);
        }

        /**
         * Captura o conteudo da primeira ocorrencia de uma tag.
         */
        public static string ReplaceTagConteudo(string html, string tag, string replace)
        {
            string matchCodeTag = @"\[code\](.*?)\[/code\]";
            matchCodeTag = matchCodeTag.Replace("code", tag);
            string textToReplace = html;
            string replaceWith = replace;
            string output = Regex.Replace(textToReplace, matchCodeTag, replaceWith);
            return output;
        }


        public static bool DatesInRange(List<DateTime> listDates, DateTime dateIni, DateTime dateFin)
        {
            foreach (var dateToCompare in listDates)
            {
                if (dateToCompare.IsBetween(dateIni, dateFin))
                {
                    return true;
                }
            }
            return false;
        }

        public static string DiaDaSemana(DateTime data)
        {
            return DiaDaSemana(data, "dddd");
        }

        public static string DiaDaSemana(DateTime data, string mask)
        {
            string diaDaSemana = data.ToString(mask, new CultureInfo("pt-BR"));
            return char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);
        }

        public static string CorFeriadoFinalDeSemana(DateTime data, string corFDS, string corDefault)
        {
            string diaDaSemana = data.ToString("ddd", new CultureInfo("pt-BR"));
            if (diaDaSemana.ToLower().In("sab", "dom", "sáb"))
            {
                return corFDS;
            }
            return corDefault;
        }

        public static bool IsFeriadoFinalDeSemana(DateTime data)
        {
            string diaDaSemana = data.ToString("ddd", new CultureInfo("pt-BR"));
            return diaDaSemana.ToLower().In("sab", "dom", "sáb");
        }

        public static string Reverse(string oldString)
        {
            char[] charArray = oldString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

}