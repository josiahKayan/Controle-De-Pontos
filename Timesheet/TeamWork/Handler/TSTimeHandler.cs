
using Apassos.TeamWork.Response;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private string apiKey ;
        private string domain ; //.teamwork.com
        private string endpoint ; //eg projects.json , milestones.json etc

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

            domain = _client.BaseUrl;
            apiKey = _client.APiKey;

            
            using (var client = new HttpClient { BaseAddress = new Uri(domain) })
            {

                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiKey, "x"))));
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "/time_entries.json?sortorder=ASC&fromdate=" + startDate + "&todate=" + endDate + "&page=" + i;

                try
                {

                    var data = await client.GetAsync(url);
                    using (Stream responseStream = await data.Content.ReadAsStreamAsync())
                    {
                        if (data.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await data.Content.ReadAsStringAsync();

                            try
                            {
                                var timeResponseList = JsonConvert.DeserializeObject<TimeResponse>(result);
                                return timeResponseList;
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    return null;
                }
                return null;
            }
        }

















        /// <summary>
        ///   Returns a single Time Entry, returns null if the user can not access the project
        /// </summary>
        /// <param name="id">Time Entry ID</param>
        /// <returns></returns>
        //public async Task<TimeResponse> GetSingleTimeEntry(string startDate, string endDate, int i)
        //{
        //    using (var client = new AuthorizedHttpClient(_client))
        //    {
        //        string url = "/time_entries.json?sortorder=ASC&fromdate=" + startDate + "&todate=" + endDate + "&page=" + i;
        //        var data = await client.GetAsync<TimeResponse>(url, null);
        //        if (data.StatusCode == HttpStatusCode.OK)
        //        {
        //            TimeResponse response = (TimeResponse)data.ContentObj;
        //            return response;
        //        }
        //    }
        //    return null;
        //}

    }
}

