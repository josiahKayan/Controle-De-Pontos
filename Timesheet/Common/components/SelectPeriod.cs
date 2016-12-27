using Apassos.DataAccess;
using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Common.Components
{
    public class SelectPeriod
    {
        private string onchange;
        private string idSelect;
        private string style;
        private bool selected;
        private Period period;

        public SelectPeriod() {
        }
        public SelectPeriod(string idSelect)
        {
            this.period = PeriodDataAccess.GetPeriodoAtual();
            this.onchange = "";
            this.idSelect = idSelect;
            this.style = "width:150px;";
            this.selected = false;
        }
        public SelectPeriod( string idSelect, Period period, string onchange, string style, bool selected ) {
            this.period = period;
            this.onchange = onchange;
            this.idSelect = idSelect;
            this.style = style;
            this.selected = selected;
        }

        public SelectPeriod SetSelected(bool selected)
        {
            this.selected = selected;
            return this;
        }

        public SelectPeriod SetPeriod(Period period)
        {
            this.period = period;
            return this;
        }

        public SelectPeriod SetStyle(string style)
        {
            this.style = style;
            return this;
        }

        public SelectPeriod SetOnChange(string onchange)
        {
            this.onchange = onchange;
            return this;
        }

        public string EchoSelectPeriod()
        {
            var listPeriod = PeriodDataAccess.GetPeriodoAll();
            string htmlSelect = "<select id='"+this.idSelect+"' name='"+this.idSelect+"' onchange='"+this.onchange+"' style='"+this.style+"'>";
            htmlSelect = htmlSelect + "<option value=''>Todos os períodos</option>";
            foreach (var periodItem in listPeriod)
            {
                string strselect = "";
                if ( this.selected ) {
                    if ( this.period != null && periodItem.PERIODID == this.period.PERIODID)
                    {
                        strselect = "selected";
                    }
                }
                htmlSelect = htmlSelect + "<option value='" + periodItem.PERIODID + "' " + strselect + ">" + periodItem.MONTH.ToString("00") + "/" + periodItem.YEAR + "</option>";
            }
            htmlSelect = htmlSelect + "</select>";

            return htmlSelect;

        }

    }
}