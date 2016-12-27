using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Apassos.DataAccess
{
   public class CheckinDataAccess
   {

      static private TimesheetContext db = new TimesheetContext();

      public static void SalvarCheckin(string _id, string _data, string _projectid, string _type, string _in, string _out, string _break, string _description, string _timesheetheaderid, string _consultorid, string _periodoid, string _dataDescription)
      {
         //valida as entradas
         if (_data == string.Empty || _projectid == string.Empty || _type == string.Empty || _in == string.Empty || _out == string.Empty)
         {
            return;
         }

         if (_in.Trim() == string.Empty)
         {
            _in = "00:00";
         }
         if (_out.Trim() == string.Empty)
         {
            _out = "00:00";
         }
         if (_break.Trim() == string.Empty)
         {
            _break = "00:00";
         }
         if (_description.Trim() == string.Empty)
         {
            _description = ".";
         }



         if (_dataDescription.Equals("Dia da semana"))
         {
            _dataDescription = "Ds";
         }
         else if (_dataDescription.Equals("Fim de Semana"))
         {
            _dataDescription = "Fs";
         }
         else if (_dataDescription.Equals("Feriado"))
         {
            _dataDescription = "Fe";
         }



         if (_id == null) //inserir
         {
            Users user = PartnerDataAccess.GetUsuario(PartnerDataAccess.GetParceiroId(_consultorid));
            List<TimeSpan> horasApontamento = TimesheetDataAccess.GetTotalHorasApontamentoDia(_periodoid, _data, user.Partner.PARTNERID.ToString());
            TimeSpan ts = new TimeSpan();
            foreach (var x in horasApontamento)
            {
               ts = ts + x;
            }

            TimeSpan total = (Apassos.Common.Util.GetTotalHoras2(TimeSpan.Parse(_in), TimeSpan.Parse(_out), TimeSpan.Parse(_break)));




            Checkins checkin = new Checkins
            {
               ENTRY = TimeSpan.Parse(_in),//  8
               SAIDA = TimeSpan.Parse(_out),// 19
               LUNCH = TimeSpan.Parse(_break),//1
               DATA = DateTime.Parse(_data),
               APONTAMENTO = ts,//8
               DESCRIPTION = _description,
               TOTAL = total,//19 - 8 - 1
               HORASNAOAPONTADAS = Apassos.Common.Util.GetTotalHoras2(ts, total),// 19 - 8 -1    -  8   
               USERID = user.USERID,
               ISFERIADO = _dataDescription,
            };
            db.Checkins.Add(checkin);
         }
         else
         {


            Users user = PartnerDataAccess.GetUsuario(PartnerDataAccess.GetParceiroId(_consultorid));
            List<TimeSpan> horasApontamento = TimesheetDataAccess.GetTotalHorasApontamentoDia(_periodoid, _data, user.Partner.PARTNERID.ToString());
            TimeSpan ts = new TimeSpan(0, 0, 0);
            foreach (var x in horasApontamento)
            {
               ts = ts + x;
            }
            TimeSpan total = (Apassos.Common.Util.GetTotalHoras2(TimeSpan.Parse(_in), TimeSpan.Parse(_out), TimeSpan.Parse(_break)));
            Checkins checkin = db.Checkins.Find(int.Parse(_id));

            checkin.ENTRY = TimeSpan.Parse(_in);
            checkin.SAIDA = TimeSpan.Parse(_out);
            checkin.LUNCH = TimeSpan.Parse(_break);
            checkin.DATA = DateTime.Parse(_data);
            checkin.APONTAMENTO = ts;
            checkin.DESCRIPTION = _description;
            checkin.TOTAL = total;
            checkin.HORASNAOAPONTADAS = Apassos.Common.Util.GetTotalHoras2(ts, total);
            checkin.USERID = user.USERID;
            checkin.ISFERIADO = _dataDescription;

         }
         db.SaveChanges();
      }

      public static void Excluir(string id)
      {
         if (id != null && id != string.Empty)
         {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var checkin = db.Checkins.Find(int.Parse(id));
            db.Checkins.Remove(checkin);
         }
      }

      public static List<Checkins> getListaCheckinsPeriodoUser(Partners partner, Period period)
      {
         var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

         //List<Users> listaUser = db.Users.Where(u => u.USERID == partner.user.USERID).ToList();



         List<Checkins> lista = db.Checkins.Where(c => c.USERID == partner.user.USERID && c.DATA.Month == period.MONTH && c.DATA.Year == period.YEAR).ToList();

         return lista;
      }






      public static void ExcluirCheckin(string id)
      {
         if (id != null && id != string.Empty)
         {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            Checkins checkin = db.Checkins.Find(int.Parse(id));
            db.Checkins.Remove(checkin);
            db.SaveChanges();
         }
      }



   }
}