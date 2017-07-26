using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.DataAccess;
using Apassos.Models;


namespace Apassos.Common
{
    public class AccessRules
    {

        /**
         * Valida o acesso no modulo passado pela string <modulo>.
         */
        public static bool ModuleValidate(Constants.ModulesConstant module, Users user)
        {
            Constants.ModulesConstant[] aRoot = { Constants.ModulesConstant.USERS, 
                Constants.ModulesConstant.PERFIL, 
                Constants.ModulesConstant.PROJECTS, 
                Constants.ModulesConstant.PARTNERS, 
                Constants.ModulesConstant.CONSULTANT, 
                Constants.ModulesConstant.PASSWORD, 
                Constants.ModulesConstant.CRYPTALL,
                Constants.ModulesConstant.REPORTS,
                Constants.ModulesConstant.INFOPROJETOS,
                Constants.ModulesConstant.INTEGRATION,
                Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT,
                Constants.ModulesConstant.TIMESHEET


                //,Constants.ModulesConstant.CHECKIN
            };
            Constants.ModulesConstant[] aGestor = { Constants.ModulesConstant.PROJECTS, 
                Constants.ModulesConstant.APPROVAL, 
                Constants.ModulesConstant.TIMESHEET, 
                Constants.ModulesConstant.PROJECTSCONSULTANT, 
                Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT,
                Constants.ModulesConstant.PASSWORD,
                Constants.ModulesConstant.REPORTS,
                Constants.ModulesConstant.INFOPROJETOS,
                Constants.ModulesConstant.INTEGRATION

                //,Constants.ModulesConstant.CHECKIN
            };
            Constants.ModulesConstant[] aAdmin = { Constants.ModulesConstant.PROJECTS, 
                Constants.ModulesConstant.APPROVAL, 
                Constants.ModulesConstant.TIMESHEET,
                Constants.ModulesConstant.PROJECTSCONSULTANT,
                Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT,
                Constants.ModulesConstant.PASSWORD,
                Constants.ModulesConstant.REPORTS,
                Constants.ModulesConstant.TIMESHEETINPERIODREPORT,
            Constants.ModulesConstant.INFOPROJETOS};

            Constants.ModulesConstant[] aConsult = { Constants.ModulesConstant.TIMESHEET, 
                Constants.ModulesConstant.PASSWORD,
                Constants.ModulesConstant.TIMESHEETINPERIODREPORT,
                Constants.ModulesConstant.INFOPROJETOS,
                Constants.ModulesConstant.INTEGRATION
                //,Constants.ModulesConstant.CHECKIN
            };
            switch (user.PROFILE)
            {
                case "0":
                    return Array.IndexOf(aRoot, module) >= 0;
                case "1":
                    return Array.IndexOf(aGestor, module) >= 0;
                case "2":
                    return Array.IndexOf(aAdmin, module) >= 0;
                case "3":
                    return Array.IndexOf(aConsult, module) >= 0;
            }
            return false;
        }


        /**
         * Valida o acesso no modulo passado pela string <modulo>.
         */
        public static List<string[]> ButtonsProfile(Users user)
        {
            var userperf = user.PerfilsList();
            if (userperf == null || userperf.Count() == 0)
            {
                return ButtonsProfileDefault(user.PROFILE);
            }
            else
            {
                List < String[] > list = ButtonsProfileByPerfil(user);
                return list;
            }
        }

        public static List<String[]> ButtonsProfileByPerfil(Users user)
        {
            List<string[]> aPermissions = new List<string[]>();
            Dictionary<string, string[]> map = GetMapPermissions();
            var list = user.PermissionsList();

            foreach (Permission item in list)
            {
                var btn = map.ContainsKey(item.CODE);
                if ( btn ) {
                    aPermissions.Add(map[item.CODE]);
                }
            }
            if ( user.ISALTERPWD == "1" ) {
                aPermissions.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PASSWORD), "btSenha", "Login", "Senha" });
            }


            return aPermissions;
        }

        public static Dictionary<string, string[]> GetMapPermissions()
        {
            Dictionary<string, string[]> permissionsMap = new Dictionary<string, string[]>();

            permissionsMap.Add(Constants.ButtonsConstant.PARTNERS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PARTNERS), "btCliente", "Cliente", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.PROJECTS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTS), "btProjetos", "Projetos", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.CONSULTANT.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CONSULTANT), "btUsuarios", "Usuarios", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.USERS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.USERS), "btUsuarios", "Usuarios", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.PERIODS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PERIODS), "btPeriodo", "Period", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.PROJECTSCONSULTANT.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTSCONSULTANT), "btProjetoConsultores", "ProjetosConsultores", "SelectionProjectConsultant" });
            permissionsMap.Add(Constants.ButtonsConstant.APPROVAL.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.APPROVAL), "btAprovacao", "Gestor", "Aprovacao" });
            permissionsMap.Add(Constants.ButtonsConstant.TIMESHEET.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btApontamentos", "Apontamentos", "Index" });
            permissionsMap.Add(Constants.ButtonsConstant.REPORTS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.REPORTS), "btRelatorios", "Relatorios", "Relatorios" });
            //permissionsMap.Add(Constants.ButtonsConstant.CHECKIN.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CHECKIN), "btCheckin", "Checkin", "Checkin" });
            permissionsMap.Add(Constants.ButtonsConstant.INFOPROJETOS.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INFOPROJETOS), "btInfoProjetos", "InfoProjetos", "InfoProjetos" });
            permissionsMap.Add(Constants.ButtonsConstant.INTEGRATION.ToString(), new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INTEGRATION), "btIntegration", "Integration", "Integration" });

            return permissionsMap;
        }




        public static List<string[]> ButtonsProfileDefault(string profile)
        {
            List<string[]> aRoot = new List<string[]>();
            //aRoot.Add(new string[]{ Constants.GetEnumDescription( Constants.ButtonsConstant.USERS), "btUsuarios","Users","Index" });
            //aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PERFIL), "btPerfil", "Perfil", "Index" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PARTNERS), "btCliente", "Cliente", "Index" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTS), "btProjetos", "Projetos", "Index" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CONSULTANT), "btUsuarios", "Usuarios", "Index" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PERIODS), "btPeriodo", "Period", "Index" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PASSWORD), "btSenha", "Login", "Senha" });
            //aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CRYPTALL), "btCriptSenha", "Login", "Cript" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INFOPROJETOS), "btInfoProjetos", "InfoProjetos", "InfoProjetos" });
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INTEGRATION), "btIntegration", "Integration", "Integration" });


            List<string[]> aGestor = new List<string[]>();
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTS), "btProjetos", "Projetos", "Index" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTSCONSULTANT), "btProjetoConsultores", "ProjetosConsultores", "SelectionProjectConsultant" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.APPROVAL), "btAprovacao", "Gestor", "Aprovacao" });
            //aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.ALLTIMESHEETINPERIODREPORT), "btTodosApontRelat", "Rel. Apontamentos", "Index" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PASSWORD), "btSenha", "Login", "Senha" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.REPORTS), "btRelatorio", "Relatorios", "Relatorios" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INFOPROJETOS), "btInfoProjetos", "InfoProjetos", "InfoProjetos" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btApontamentos", "Apontamentos", "Index" });
            //aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CHECKIN), "btCheckin", "Checkin", "Checkin" });
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INTEGRATION), "btIntegration", "Integration", "Integration" });



            List<string[]> aAdmin = new List<string[]>();
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTS), "btProjetos", "Projetos", "Index" });
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PROJECTSCONSULTANT), "btProjetoConsultores", "ProjetosConsultores", "SelectionProjectConsultant" });
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.APPROVAL), "btAprovacao", "Gestor", "Aprovacao" });
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btApontamentos", "Apontamentos", "Index" });
            //aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btTodosApontRelat", "Rel. Apontamentos", "Index" });
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PASSWORD), "btSenha", "Login", "Senha" });
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INFOPROJETOS), "btInfoProjetos", "InfoProjetos", "InfoProjetos" });
            //aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CHECKIN), "btCheckin", "Checkin", "Checkin" });


            List<string[]> aConsult = new List<string[]>();
            aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btApontamentos", "Apontamentos", "Index" });
            aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.PASSWORD), "btSenha", "Login", "Senha" });
            aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INFOPROJETOS), "btInfoProjetos", "InfoProjetos", "InfoProjetos" });
            //aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.CHECKIN), "btCheckin", "Checkin", "Checkin" });
            aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.INTEGRATION), "btIntegration", "Integration", "Integration" });


            switch (profile)
            {
                case "0":
                    return aRoot;
                case "1":
                    return aGestor;
                case "2":
                    return aAdmin;
                case "3":
                    return aConsult;
            }
            return null;
        }


        /**
         * Valida o acesso no modulo passado pela string <modulo>.
         */
        public static List<string[]> DefaultProfile(Users user)
        {
            List<string[]> aRoot = new List<string[]>();
            aRoot.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.USERS), "btCliente", "Cliente", "Index" });

            List<string[]> aGestor = new List<string[]>();
            aGestor.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.APPROVAL), "btAprovacao", "Gestor", "Aprovacao" });

            List<string[]> aAdmin = new List<string[]>();
            aAdmin.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.APPROVAL), "btAprovacao", "Gestor", "Aprovacao" });

            List<string[]> aConsult = new List<string[]>();
            aConsult.Add(new string[] { Constants.GetEnumDescription(Constants.ButtonsConstant.TIMESHEET), "btApontamentos", "Apontamentos", "Index" });

            switch (user.PROFILE)
            {
                case "0":
                    return aRoot;
                case "1":
                    return aGestor;
                case "2":
                    return aAdmin;
                case "3":
                    return aConsult;
            }
            return null;
        }

    }
}