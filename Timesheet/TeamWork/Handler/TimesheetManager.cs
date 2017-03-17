using Apassos.DataAccess;
using Apassos.Erros;
using Apassos.Models;
using Apassos.Observer;
using Apassos.TeamWork.JsonObject;
using Apassos.TeamWork.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Handler
{
    public class TimesheetManager
    {

        private readonly string DEFAULT_BREAK_TIME = "00:00:00";
        private readonly string DEFAULT_NOTE = ".";
        private readonly string DEFAULT_TYPE = "R";
        private readonly string DEFAULT_TAG_PREFIX = "#:";
        private readonly string DEFAULT_TIME_PATTERN = "HH:mm:ss";

        private readonly int MINUTES_OF_A_HOUR = 60;
        private readonly string STATUS_COMPLETED = "completed";

        public List<InfoObjects> InsertData( List<EntryTime> entryTimeList)
        {
            TimesheetManagerItem tsManagerItem = new TimesheetManagerItem();
            Period period ;
            Project project ;
            Partners partner ;
            List<InfoObjects> listlogTrace = new List<InfoObjects>();

            List<Logs> listLog = new List<Logs>();
            Logs log ;

            foreach (var item in entryTimeList)
            {
                //Inicializa o log
                log = new Logs();

                //Pega a lista de Tags
                project = GetProject(item.Tags);

                //Pega o Parceiro
                partner = GetPartnerByEntry(item.PersonFirstName, item.PersonLastName);

                if (partner == null)
                {
                    partner = GetPartnerByFirstName(item.PersonFirstName);
                }

                if (partner == null)
                {
                    partner = GetPartnerByLastName(item.PersonLastName);
                }

                //Pega o período
                period = GetPeriodByEntry(item.Date);



                //Cria o Header e o Item( O Apontamento em si )
                tsManagerItem.CreateTimesheetItem(period, partner, project, item,log);

                if (project != null)
                {

                    //Cria um objeto para erro
                    Erros.Erros noTag = new Erros.Erros();
                    noTag.Consultor = partner.SHORTNAME;
                    noTag.ConsultorID = ""+partner.PARTNERID;
                    noTag.Content = item.Description;

                    var newDate = item.Date.Split('T');
                    var tdate = newDate[0].Split('-');
                    var newDatePt = tdate[2] + "" + tdate[1] + "" + tdate[0];
                    noTag.Date = newDatePt;
                    noTag.TWProject = item.ProjectName;
                    noTag.ProblemDescription = "O Apontamento não contém Tag";
                    if (!(partner.EMAIL.Equals("") || partner.EMAIL == null))
                    {
                        noTag.EmailPartner = partner.EMAIL;
                    }
                    else
                    {
                        noTag.EmailPartner = "paulo.palmeira@apassos.com.br";
                        noTag.Consultor = partner.SHORTNAME;
                        noTag.ConsultorID = "" + partner.PARTNERID;
                        if (item.Description.Equals(string.Empty) || item.Description == null)
                        {
                            item.Description = " Sem nome ";
                        }
                        noTag.Content = "O consultor " + noTag.Consultor + " não colocou a Tag para o projeto " + noTag.TWProject +"  no lançamento "+item.Description;
                        noTag.Date = item.Date;
                        noTag.TWProject = item.ProjectName;
                    }
                    InfoObjects obj = new InfoObjects();
                    obj.Date = DateTime.UtcNow.Date;
                    obj.Erro = noTag;
                    listlogTrace.Add(obj);
                }
            }


            return listlogTrace;
        }

        private Project GetProject(List<Tag> listTag)
        {
            try
            {
                int idByTag = GetTagById(listTag);

                ProjectDataAccess projectDataAccess = new ProjectDataAccess();

                Project project = projectDataAccess.GetProjeto(idByTag.ToString());
                return project;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        private int GetTagById(List<Tag> listTag)
        {
            int idByTag = -1;

            foreach (var tag in listTag)
            {
                if (tag.Name.Contains(DEFAULT_TAG_PREFIX))
                {
                    int indexTag = tag.Name.IndexOf(':');
                    string tagName = tag.Name.Substring(indexTag + 1);

                    int index = tagName.IndexOf('-');

                    if (index >= 0)
                    {
                        string newTag = tagName.Substring(0, index);
                        try
                        {
                            idByTag = int.Parse(newTag.Trim());
                        }
                        catch (Exception e)
                        {
                            Debug.Write(e.StackTrace);
                        }
                    }
                }
            }

            return idByTag;
        }

        private Period GetPeriodByEntry(string entryDate)
        {
            DateTime date = DateTime.Parse(entryDate);
            PeriodDataAccess _periods = new PeriodDataAccess();
            return _periods.GetPeriodoAtual();
        }


        private Partners GetPartnerByEntry(string FirstName, string LastName)
        {
            PartnerDataAccess p = new PartnerDataAccess();
            List<Partners> _partners = p.GetAllParceiros();

            if (FirstName.Contains('á') || LastName.Contains('á'))
            {
                FirstName = FirstName.Replace('á', 'a');
                LastName = LastName.Replace('á', 'a');
                int number = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();
                if (number >= 1)
                {
                    return _partners.Find(x => x.FIRSTNAME.Replace('á', 'a').ToUpper().Trim().Equals(FirstName.ToUpper().Trim()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));
                }
            }

            if (FirstName.Contains('é') || LastName.Contains('é'))
            {
                FirstName = FirstName.Replace('é', 'e');
                LastName = LastName.Replace('é', 'e');
                int number = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();
                if (number >= 1)
                {
                    return _partners.Find(x => x.FIRSTNAME.Replace('é', 'e').ToUpper().Trim().Equals(FirstName.ToUpper().Trim()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));
                }
            }

            if (FirstName.Contains('í') || LastName.Contains('í'))
            {
                FirstName = FirstName.Replace('í', 'i');
                LastName = LastName.Replace('í', 'i');
                int number = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();
                if (number >= 1)
                {
                    return _partners.Find(x => x.FIRSTNAME.Replace('í', 'i').ToUpper().Trim().Equals(FirstName.ToUpper().Trim()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));
                }
            }

            int numberPartner = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();


            if (numberPartner > 1)
            {
                return _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).FirstOrDefault();

            }

            //return _partners.Find(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));

            return _partners.Find(x =>  x.FIRSTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));


        }


        private Partners GetPartnerByFirstName(string firstName)
        {
            PartnerDataAccess p = new PartnerDataAccess();
            return p.GetParceiroPorPrimeiroNome(firstName.ToUpper().Trim());
        }

        private Partners GetPartnerByLastName(string lastName)
        {
            PartnerDataAccess p = new PartnerDataAccess();
            return p.GetParceiroPorUltimoNome(lastName.ToUpper().Trim());
        }

        private string SerializeObject(Erros.Erros tsItem)
        {
            string tsJson = JsonConvert.SerializeObject(tsItem);
            return tsJson;
        }

        private void InsertObjectJson(List<InfoObjects> listInfoObject)
        {
            InfoObjectsDataAccess infoDataAccess = new InfoObjectsDataAccess();
            infoDataAccess.InsertObjects(listInfoObject);
        }

    }
}