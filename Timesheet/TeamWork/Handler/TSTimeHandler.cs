﻿
using Apassos.TeamWork.Response;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TeamWorkNet.Base;
using TeamWorkNet.Handler;
using TeamWorkNet.Model;
using TeamWorkNet.Response;

namespace Apassos.TeamWork.Handler
{
    public class TSTimeHandler : TimeHandler
    {
        private readonly TeamWorkClient _client;

        public TSTimeHandler(TeamWorkClient client) : base(client)
        {
            _client = client;
        }

        /// <summary>
        ///   Returns a single Time Entry, returns null if the user can not access the project
        /// </summary>
        /// <param name="id">Time Entry ID</param>
        /// <returns></returns>
        public async Task<TimeResponse> GetSingleTimeEntry(string startDate , string endDate, int i)
        {
            using (var client = new AuthorizedHttpClient(_client))
            {
                string url = "/time_entries.json?sortorder=ASC&fromdate="+startDate+"&todate="+endDate+"&page="+i;
                var data = await client.GetAsync<TimeResponse>(url, null);
                if (data.StatusCode == HttpStatusCode.OK)
                {
                    TimeResponse response = (TimeResponse)data.ContentObj;
                    return response;
                }
            }
            return null;
        }


    }
}

