using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Apassos.Common
{
    public class Constants
    {
        public enum StatusAprovacaoConstant
        {
            [Description("Aberto")]
            Aberto = 0,
            [Description("Aprovado")]
            Aprovado = 1,
            [Description("Reprovado")]
            Reprovado = 2,
            [Description("Encerrado")]
            Encerrado = 3,
            [Description("Reaberto")]
            Reaberto = 4
        };

        public enum SelecaoGestorStatusAprovacaoConstant
        {
            [Description("Aberto")]
            Aberto = 0,
            [Description("Aprovado")]
            Aprovado = 1,
            [Description("Reprovado")]
            Reprovado = 2,
            [Description("Encerrado")]
            Encerrado = 3
        };

        public enum StatusAprovacaoGeralConstant
        {
            [Description("Concluído")]
            Concluido = 0,
            [Description("Pendente")]
            Pendente = 1
        };

        public enum StatusProjetoConstant
        {
            [Description("Aberto")]
            Aberto = 0,
            [Description("Iniciado")]
            Iniciado = 1,
            [Description("Parado")]
            Parado = 2,
            [Description("Encerrado")]
            Encerrado = 3,
            [Description("Cancelado")]
            Cancelado = 4
        };

        public enum TipoApontamentoConstant
        {
            [Description("Real")]
            Real = 0,
            [Description("Planejado")]
            Planejado = 1
        };

        public enum StatusLockConstant
        {
            [Description("Acesso ao Sistema")]
            Unlocked = 0,
            [Description("Bloqueado")]
            Locked = 1
        };

        public enum ProfileConstant
        {
            [Description("Root")]
            ROOT = 0,
            [Description("Consultor")]
            CONSULTOR = 3,
            [Description("Administrativo")]
            ADMINISTRATIVO = 2,
            [Description("Gestor")]
            GESTOR = 1
        }

        public enum ProfileShowConstant
        {
            [Description("Consultor")]
            CONSULTOR = 3,
            [Description("Administrativo")]
            ADMINISTRATIVO = 2,
            [Description("Gestor")]
            GESTOR = 1,
        }

        public enum TipoPessoaConstant
        {
            [Description("Física")]
            Fisica = 0,
            [Description("Jurídica")]
            Jurídica = 1
        };

        public enum SimNaoConstant
        {
            [Description("Sim")]
            Sim = 1,
            [Description("Não")]
            Nao = 0
        };



        public enum ModulesConstant
        {
            TIMESHEET = 0,
            CONSULTANT = 1,
            PARTNERS = 2,
            PROJECTS = 3,
            PERFIL = 4,
            APPROVAL = 5,
            PROJECTSCONSULTANT = 6,
            USERS = 7,
            PASSWORD = 8,
            PERIODS = 9,
            CRYPTALL = 10,
            ALLTIMESHEETINPERIODREPORT = 11,
            TIMESHEETINPERIODREPORT = 12,
            REPORTS=13,
            CHECKIN =14,
            INFOPROJETOS = 15,
            INTEGRATION = 16
        }

        public enum ButtonsConstant
        {
            [Description("Apontamentos")]
            TIMESHEET = 0,
            [Description("Usuários")]
            CONSULTANT = 1,
            [Description("Parceiros")]
            PARTNERS = 2,
            [Description("Projetos")]
            PROJECTS = 3,
            [Description("Perfis")]
            PERFIL = 4,
            [Description("Aprovação")]
            APPROVAL = 5,
            [Description("Relacionar Consultores")]
            PROJECTSCONSULTANT = 6,
            [Description("Usuários")]
            USERS = 7,
            [Description("Alterar senha")]
            PASSWORD = 8,
            [Description("Períodos")]
            PERIODS = 9,
            [Description("Criptografar Senhas")]
            CRYPTALL = 10,
            [Description("Relatório Apontamentos")]
            ALLTIMESHEETINPERIODREPORT = 11,
            [Description("Relatório Apontamentos")]
            TIMESHEETINPERIODREPORT = 12,
            [Description("Relatórios")]
            REPORTS = 13,
            [Description("Check-in")]
            CHECKIN = 14,
            [Description("Info-Projetos")]
            INFOPROJETOS = 15,
            [Description("IntegraçãoTW")]
            INTEGRATION = 16
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetDescricaoStatusProjeto( int status )
        {
            switch (status){
                case 0:
                    return "Aberto";
                case 1:
                    return "Iniciado";
                case 2:
                    return "Parado";
                case 3:
                    return "Encerrado";
                case 4:
                    return "Cancelado";
            }
            return "";
        }

        public static string GetTipoPessoaFJ(string tipo)
        {
            switch (tipo)
            {
                case "0":
                    return "F";
                case "1":
                    return "J";
            }
            return "";
        }

        public static string ConvertLockedSN(int locked)
        {
            switch (locked)
            {
                case 0:
                    return "N";
                case 1:
                    return "S";
            }
            return "";
        }

        
        public static int GetOrderPessoaFJ(string tipo)
        {
            switch (tipo)
            {
                case "F":
                    return ((int)TipoPessoaConstant.Fisica);
                case "J":
                    return ((int)TipoPessoaConstant.Jurídica);
            }
            return 99;
        }

        public static string GetDescricaoTipoApontamentos(string tipo)
        {
            switch (tipo)
            {
                case "P":
                    return GetEnumDescription(TipoApontamentoConstant.Planejado);
                case "R":
                    return GetEnumDescription(TipoApontamentoConstant.Real);
            }
            return "";
        }
    
    }
}