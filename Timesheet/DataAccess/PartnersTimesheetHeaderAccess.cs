using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Apassos.Common;
using Apassos.Models;

namespace Apassos.DataAccess
{
    /**
     * Classe para conter os dados dos consultores e apontamentos, de um periodo.
     */
    public class PartnersTimesheetHeaderAccess
    {
        private TimesheetContext db = new TimesheetContext();

        public Partners partner;
        public Period period;
        public Period periodInicial;
        public Period periodFinal;
        public TimesheetHeader header;
        public List<TimesheetItem> items;
        public List<TimesheetItem> timeItems;
        public Constants.StatusAprovacaoConstant generalStatus;
        public int hash;

        public PartnersTimesheetHeaderAccess(Partners partner, Period period, Project project )
        {
            using (TimesheetContext db = new TimesheetContext())
            {

                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;

                header = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env
                    && th.Period.PERIODID == period.PERIODID && th.Partner.PARTNERID == partner.PARTNERID).ToList().FirstOrDefault();

                if (header != null)
                {
                    if (project == null)
                    {
                        items = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID).
                            OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList();
                    }
                    else
                    {
                        items = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID
                            && ti.project.PROJECTID == project.PROJECTID).OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList();
                    }

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }
            }
        }


        public PartnersTimesheetHeaderAccess(Partners partner, bool x, Period periodInicial = null, Period periodFinal = null, Project project = null )
        {

            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;
                List<TimesheetHeader> listHeaderX = new List<TimesheetHeader>();
                List<TimesheetHeader> listHeader = new List<TimesheetHeader>();

                if (periodInicial != null && periodFinal != null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;
                    
                    listHeader = db.TimesheetHeaders.Include("Period").Include("Partner").Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                        th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH
                    ).ToList();

                }
                else if (periodInicial != null)
                {
                    this.periodInicial = periodInicial;
                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                       th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART
                    ).ToList();
                }
                else if (periodFinal != null)
                {
                    this.periodFinal = periodFinal;
                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                       th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH
                    ).ToList();
                }

                else if (periodInicial == null && periodFinal == null && project == null)
                {

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env
                    ).ToList();

                }
                else if (periodInicial == null && periodFinal == null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID
                    ).ToList();

                }

                items = new List<TimesheetItem>();
                if (listHeader != null)
                {
                    if (project == null)
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID).
                            OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }
                    else
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Include("project").Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID &&
                            ti.project.PROJECTID == project.PROJECTID).OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }

            }
        }


        public PartnersTimesheetHeaderAccess(Partners partner , Period periodInicial = null, Period periodFinal =null, Project project =null)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;
                List<TimesheetHeader> listHeaderX = new List<TimesheetHeader>();
                List<TimesheetHeader> listHeader = new List<TimesheetHeader>();


                if (periodInicial != null && periodFinal != null && project != null && partner == null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env &&
                        th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH
                    ).ToList();

                }

                else if (periodInicial != null && periodFinal != null && project == null && partner != null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Include("Period").Include("Partner").Where(th => th.ENVIRONMENT == env &&
                        th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH &&
                        th.Partner.PARTNERID == partner.PARTNERID
                    ).ToList();

                }

                else if (periodInicial != null && periodFinal != null && project == null && partner == null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env
                    //th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH 
                    ).ToList();

                }

                else if (periodInicial != null && periodFinal != null && project != null && partner != null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Include("Period").Include("Partner").Where(th => th.ENVIRONMENT == env &&
                        th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH &&
                        th.Partner.PARTNERID == partner.PARTNERID
                    ).ToList();

                }

                else if (periodInicial != null && periodFinal != null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                        th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART && th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH
                    ).ToList();

                }

                else if (periodInicial != null)
                {
                    this.periodInicial = periodInicial;
                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                       th.Period.TIMESHEETPERIODSTART >= periodInicial.TIMESHEETPERIODSTART
                    ).ToList();
                }
                else if (periodFinal != null)
                {
                    this.periodFinal = periodFinal;
                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID &&
                       th.Period.TIMESHEETPERIODFINISH <= periodFinal.TIMESHEETPERIODFINISH
                    ).ToList();
                }

                else if (periodInicial == null && periodFinal == null)
                {
                    this.periodInicial = periodInicial;
                    this.periodFinal = periodFinal;

                    listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID
                    ).ToList();

                }

                items = new List<TimesheetItem>();
                if (listHeader != null)
                {
                    if (project == null)
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Include("project").Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID).
                            OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }
                    else
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Include("project").Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID &&
                            ti.project.PROJECTID == project.PROJECTID).OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }
            }
        }


        public PartnersTimesheetHeaderAccess(Partners partner, Project project)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;
                List<TimesheetHeader> listHeaderX = new List<TimesheetHeader>();
                List<TimesheetHeader> listHeader = new List<TimesheetHeader>();

                listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID).ToList();

                items = new List<TimesheetItem>();
                if (listHeader != null)
                {
                    if (project == null)
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID).
                            OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }
                    else
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID &&
                            ti.project.PROJECTID == project.PROJECTID).OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                    }

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }
            }
        }


        public PartnersTimesheetHeaderAccess(Partners partner, Period periodFinal, Project project, bool x)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;
                this.periodFinal = periodFinal;

                List<TimesheetHeader> listHeader = new List<TimesheetHeader>();

                listHeader = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env
                   && (th.Period.MONTH <= periodFinal.MONTH
                   && th.Period.YEAR <= periodFinal.YEAR)
                   && th.Partner.PARTNERID == partner.PARTNERID
                ).ToList();

                items = new List<TimesheetItem>();


                if (listHeader != null)
                {
                    if (project == null)
                    {
                        items.AddRange(db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID).
                            OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                    }
                    else
                    {
                        foreach (var h in listHeader)
                        {
                            items.AddRange(db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == h.TIMESHEETHEADERID &&
                            ti.project.PROJECTID == project.PROJECTID).OrderBy(ts => ts.DATE).ThenBy(ts => ts.IN).ToList());
                        }
                        //items.AddRange(db.TimesheetItems.Contains(listHeader/)
                    }

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }
            }
        }


        public PartnersTimesheetHeaderAccess(Partners partner, Period period, bool irrelevante)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                this.partner = partner;
                this.period = period;

                header = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Period.PERIODID == period.PERIODID
                           && th.Partner.PARTNERID == partner.PARTNERID).FirstOrDefault();

                if (header != null)
                {

                    var query = from it in db.TimesheetItems
                                where it.ENVIRONMENT == env && it.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID
                                orderby it.DATE
                                select it;

                    items = query.ToList();

                    var aprovCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Aprovado)).Count();
                    var encCount = items.Where(i => i.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    if (aprovCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aprovado;
                    }
                    else if (encCount == items.Count())
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Encerrado;
                    }
                    else
                    {
                        generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                    }
                }
                else
                {
                    header = new TimesheetHeader();
                    items = new List<TimesheetItem>();
                    generalStatus = Constants.StatusAprovacaoConstant.Aberto;
                }
            }
        }


        public TimeSpan TotalHours()
        {
            TimeSpan total = new TimeSpan();
            foreach (TimesheetItem item in items)
            {
                total = Util.AddHoras(total, item.TotalHours);
            }
            return total;
        }

        public TimeSpan AprovTotalHours()
        {
            TimeSpan total = new TimeSpan();
            foreach (TimesheetItem item in items)
            {
                if (((int)Common.Constants.StatusAprovacaoConstant.Aprovado) == item.STATUS)
                {
                    total = Util.AddHoras(total, item.TotalHours);
                }
            }
            return total;
        }
        public TimeSpan ReprovTotalHours()
        {
            TimeSpan total = new TimeSpan();
            foreach (TimesheetItem item in items)
            {
                if (((int)Common.Constants.StatusAprovacaoConstant.Reprovado) == item.STATUS)
                {
                    total = Util.AddHoras(total, item.TotalHours);
                }
            }
            return total;
        }


        public TimeSpan TotalHoursReal()
        {
            TimeSpan total = new TimeSpan();
            foreach (TimesheetItem item in items)
            {
                if (item.TYPE == "R")
                {
                    total = Util.AddHoras(total, item.TotalHours);
                }
            }
            return total;
        }
        public TimeSpan TotalHoursPlan()
        {
            TimeSpan total = new TimeSpan();
            foreach (TimesheetItem item in items)
            {
                if (item.TYPE == "P")
                {
                    total = Util.AddHoras(total, item.TotalHours);
                }
            }
            return total;
        }

    }

    /**
     * Classe para conter os dados dos consultores e apontamentos, de um periodo.
     */
    public class ListPartnersTimesheetHeaderPeriod
    {

        public List<PartnersTimesheetHeaderAccess> list;

        public TimeSpan totalHours;


        public ListPartnersTimesheetHeaderPeriod(Period period)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.Period.PERIODID == period.PERIODID).
                Select(x => x.TimesheetHeader.Partner).ToList();
            list = new List<PartnersTimesheetHeaderAccess>();
            foreach (Partners partner in listPartners)
            {
                list.Add(new PartnersTimesheetHeaderAccess(partner, period));
            }
        }
        }

    }



}