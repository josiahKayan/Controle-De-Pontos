
using Apassos.DataAccess;
using Apassos.IoC;
using Apassos.Models;
using Apassos.TeamWork.Handler;
using Apassos.TeamWork.JsonObject;
using Apassos.TeamWork.Parsers;
using Apassos.TeamWork.Services;
using Apassos.TeamWork.WebServices;
using Ninject;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Apassos.TeamWork.Jobs
{

    public class TeamWorkJob : IJob
    {

        [Inject]
        private ITimeEntryService _parser;

        [Inject]
        private ITeamWorkWebService _service;

        private readonly IKernel _kernel;

        public TeamWorkJob()
        {
            _kernel = new StandardKernel(new TimesheetNinjectModule());
            //TimesheetPaerser
            _parser = _kernel.Get<ITimeEntryService>();
            //TeamWorkService
            _service = _kernel.Get<ITeamWorkWebService>();
        }

        public void Execute(IJobExecutionContext context)
     {

            var items = _parser.GetItems();

            TimesheetManager tsManager = new TimesheetManager();

            List<InfoObjects> listTimesheetItems =  tsManager.InsertData(items);

            //Emails.Email email = new Emails.Email();
            //try
            //{
            //    email.EnviaMensagemEmail(listTimesheetItems);
            //}
            //catch (Exception e)
            //{

            //}

        }
    }

}