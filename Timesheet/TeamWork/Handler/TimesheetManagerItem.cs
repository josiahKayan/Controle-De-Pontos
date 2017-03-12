using Apassos.Models;
using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Handler
{
    public class TimesheetManagerItem
    {

        private readonly string DEFAULT_BREAK_TIME = "00:00:00";
        private readonly string DEFAULT_NOTE = ".";
        private readonly string DEFAULT_TYPE = "R";
        private readonly string DEFAULT_TIME_PATTERN = "HH:mm:ss";

        private readonly int MINUTES_OF_A_HOUR = 60;

        public TimesheetItem CreateTimesheetItem(Period period, Partners partner, Project projectTodoItem, EntryTime entry )
        {
            TimesheetManagerHeader tsManagerHeader = new TimesheetManagerHeader();
            TimesheetHeader header = new TimesheetHeader();
            header = tsManagerHeader.GetHeader(period, partner);

            DateTime initialDate = DateTime.Parse(entry.Date);
            int totalMinutes = (int.Parse(entry.Hours) * MINUTES_OF_A_HOUR) + int.Parse(entry.Minutes);
            DateTime finalDate = initialDate.Add(new TimeSpan(0, totalMinutes, 0));
            string initialDateAsString = initialDate.ToString(DEFAULT_TIME_PATTERN);
            string finalDateAsString = finalDate.ToString(DEFAULT_TIME_PATTERN);

            TimesheetItem item = new TimesheetItem();
            item.ENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString(); 
            item.TimesheetHeader = header;
            item.project = projectTodoItem;
            item.COUNTER = 1;
            item.DESCRIPTION = "TW: " + entry.Description;
            item.TYPE = DEFAULT_TYPE;
            item.NOTE = DEFAULT_NOTE;
            item.BREAK = TimeSpan.Parse(DEFAULT_BREAK_TIME);
            item.STATUS = 0;
            item.DATE = DateTime.Parse(entry.Date);
            item.IN = TimeSpan.Parse(initialDateAsString);
            item.OUT = TimeSpan.Parse(finalDateAsString);
            item.CreationDate = DateTime.Now.Date;
            return item;
        }


    }
}