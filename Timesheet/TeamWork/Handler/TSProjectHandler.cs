using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeamWorkNet.Base;
using TeamWorkNet.Handler;
using TeamWorkNet.Response;

namespace Apassos.TeamWork.Handler
{
   public class TSProjectHandler : ProjectHandler
   {
      private readonly TeamWorkClient _client;

      public TSProjectHandler(TeamWorkClient client) : base(client)
      {
         _client = client;
      }

      /// <summary>
      ///   Returns a single Project, returns null if the user can not access the project
      /// </summary>
      /// <param name="projectID">the project id</param>
      /// <param name="includePeople">include people on project</param>
      /// <param name="includeTaskLists">include task lists</param>
      /// <param name="includeMilestones">include all milestones</param>
      /// <returns></returns>
      public async Task<ProjectResponse> GetProject(int projectId, string startDate = null)
      {
         using (var client = new AuthorizedHttpClient(_client))
         {
                var data = await client.GetAsync<ProjectResponse>("/projects/" + projectId + ".json", null);

            if (data.StatusCode == HttpStatusCode.OK)
            {
               var response = (ProjectResponse)data.ContentObj;

               StringBuilder url = new StringBuilder("projects/" + projectId + "/todo_lists.json?nestSubTasks=yes&false");
               if (startDate != null)
               {
                  url.Append("&createdAfterDate=");
                  url.Append(startDate);
               }

                    var tasks = await client.GetAsync<TaskListsResponse>(url.ToString(), null);
               if (tasks.StatusCode == HttpStatusCode.OK)
               {
                  var tasklist = (TaskListsResponse)tasks.ContentObj;
                  response.project.Tasklists = tasklist.TodoLists;
               }

               return new ProjectResponse { STATUS = "OK", project = response.project };
            }
         }
         return null;
      }


   }
}