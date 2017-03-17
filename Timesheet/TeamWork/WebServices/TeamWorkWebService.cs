using Apassos.TeamWork.Handler;
using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TeamWorkNet.Base;
using TeamWorkNet.Handler;
using TeamWorkNet.Model;
using TeamWorkNet.Response;

namespace Apassos.TeamWork.WebServices
{
    public class TeamWorkWebService : ITeamWorkWebService
    {

        //Paulo Administrator Key
        private readonly string APIKEY = "dog671silk";
        //private readonly string APIKEY = "memory897elbow";
        private readonly string DOMAIN = "apassosconsultingsoftware";

        private readonly TeamWorkClient _teamWorkClient;
        private readonly TSTimeHandler _tsTimeHandler;
        private readonly TaskHandler _taskHandler;

        public TeamWorkWebService()
        {
            _teamWorkClient = new TeamWorkClient();
            _teamWorkClient.Init(APIKEY, DOMAIN);
            _taskHandler = new TaskHandler(_teamWorkClient);
            _tsTimeHandler = new TSTimeHandler(_teamWorkClient);
        }


        public List<EntryTime> GetAllTimeEntries()
        {
            int weekInMonth = 12;

            List<TimeResponse> responseList = new List<TimeResponse>();
            for (int i = 1; i <= weekInMonth; i++)
            {
                TimeResponse timeResponses = GetAllEntries(i);
                if (timeResponses.TimeEntries.Count() > 0 )
                {
                    responseList.Add(timeResponses);
                }
            }

            List<EntryTime> timeEntriesList = new List<EntryTime>();


            foreach ( TimeResponse timeResponse in responseList)
            {
                foreach (var item in timeResponse.TimeEntries)
                {
                    timeEntriesList.Add(item);
                }
            }

            return timeEntriesList;
        }

        public TimeResponse GetAllEntries(int i)
        {
            //Pega o primeiro dia do Mês
            string startDate = (new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).Date).ToString("yyyMMdd");

            int endDate = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);

            //Pega o último dia do Mês
            string lastDayOfMonth = (new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, endDate).Date).ToString("yyyMMdd");

            var response = _tsTimeHandler.GetSingleTimeEntry(startDate, lastDayOfMonth,i).Result;

            return response;
        }


        private void GetFirstLastDayofWeek(int ano, int mes, int dia)
        {
            DateTime data = new DateTime(ano, mes, dia);
            //Variáveis de controle dos dias.
            int numeroMenor = 1, numeroMaior = 7;
            var dataInicioSemana = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
            var dataFimSemana = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());

        }
    }
}