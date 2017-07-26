using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Exceptions
{
    public class GenericExceptions : Exception
    {

        public void PeriodIsClosed( string status )
        {
            if (status.Equals("f"))
            {
                throw new Exception("O período está Fechado. Entre em contato com o Administrador");
            }
        }

        public void SheetIsApproved(int status)
        {
            if (status == 1)
            {
                throw new Exception("Atividade não migrada. O apontamento já foi aprovado pelo Gestor");
            }
        }

    }
}