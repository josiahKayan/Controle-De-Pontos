using Apassos.TeamWork.Handler;
using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
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
        //private readonly string APIKEY = "dog671silk";
        private readonly string APIKEY = "memory897elbow";
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
            TimeResponse timeResponses = GetAllEntries();
            List<EntryTime> timeEntriesList = new List<EntryTime>();

            foreach (var item in timeResponses.TimeEntries)
            {
                timeEntriesList.Add(item);
            }

            return timeEntriesList;
        }

        public TimeResponse GetAllEntries()
        {
           
            string startDate = (new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).Date).ToString("yyyMMdd");
            int endDate = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
            string lastDayOfMonth = (new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, endDate).Date).ToString("yyyMMdd");

            var response = _tsTimeHandler.GetSingleTimeEntry(startDate, lastDayOfMonth).Result;

            return response;
        }

    }
}