
using System;
using System.Collections.Generic;
using TeamWorkNet.Model;
using TeamWorkNet.Response;

namespace Apassos.TeamWork.Services
{
    public interface ITeamWorkService
    {

        /**
         * Retrieve total of hours from a project id
         */
        TimeTotalsResponse GetTotalHoursProject(int projectId);

        /**
         * Retrieve all people from Teamwork database
         */
        PeopleResponse GetAllPeople();

        /**
         * Get All todo items
         */
        List<TodoItem> GetAllTodoItems();

        /**
         * Get single TimeEntry by todo item id
         */
        TimeEntriesResponse GetSingleTimeEntry(int id);
    }

}
