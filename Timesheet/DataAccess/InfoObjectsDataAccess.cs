using Apassos.Models;
using Apassos.TeamWork.JsonObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Apassos.DataAccess
{
    public class InfoObjectsDataAccess
    {
        private TimesheetContext db ;

        public void InsertObjects(List<InfoObjects> listObjects)
        {
            using (db = new TimesheetContext())
            {
                foreach (var item in listObjects)
                {

                    InfoObjects foundItem = db.InfoObjects.Where(x => x.PARTNERID == item.PARTNERID && x.DATE == x.DATE ).FirstOrDefault();

                    if (foundItem != null)
                    {
                        foundItem.DATE = item.DATE;
                        foundItem.CONTENT = item.CONTENT;
                        foundItem.HASH = item.HASH;
                        foundItem.PARTNERID = item.PARTNERID;
                        db.Entry(foundItem).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.InfoObjects.Add(item);
                        db.SaveChanges();
                    }

                }
            }

        }

    }
}