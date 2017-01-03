
using Apassos.DataAccess;
using Apassos.IoC;
using Apassos.Models;
using Apassos.TeamWork.Parsers;
using Apassos.TeamWork.Services;
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
        private ITimesheetParser _parser;

        [Inject]
        private ITeamWorkService _service;

        private readonly IKernel _kernel;

        public TeamWorkJob()
        {
            _kernel = new StandardKernel(new TimesheetNinjectModule());
            _parser = _kernel.Get<ITimesheetParser>();
            _service = _kernel.Get<ITeamWorkService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            List<TimesheetTeamWorkItem> items = _parser.GetItems();
            TimesheetDataAccess.SaveTimesheetItems(items);

            Erros.ErroMensage erros = new Erros.ErroMensage();
            List<TeamworkLogTraces> listLogs = erros.RetornaErros();


            //Emails.Email email = new Emails.Email();
            //try
            //{
            //    email.EnviaMensagemEmail(listLogs);
            //}
            //catch (Exception e)
            //{

            //}

        }
    }

}