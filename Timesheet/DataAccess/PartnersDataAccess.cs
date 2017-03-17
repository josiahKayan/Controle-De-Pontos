using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Apassos.Common;
using Apassos.Models;
using System.Diagnostics;

namespace Apassos.DataAccess
{
    /**
     * Classe para conter os dados dos consultores e apontamentos, de um periodo.
     */
    public class PartnerDataAccess
    {
        /**
        * Retorna uma lista de parceiros, exceto o admin.
        */
        public List<Partners> GetParceirosSistema()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaX = db.Partners.Where(p => p.ENVIRONMENT == env && p.PARTNERID != 1).OrderBy(c => c.NAME).ToList();

                List<Partners> lista = new List<Partners>();
                foreach (var item in listaX)
                {
                    db.Entry(item).Reload();
                    lista.Add(item);
                }
                return lista;
            }
        }


        public List<Partners> GetParceirosSistemaSemRepeticao()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var lista = db.Partners.Where(p => p.ENVIRONMENT == env && p.PARTNERID != 1).OrderBy(c => c.NAME).Distinct().ToList();
                return lista;
            }
        }

        public List<Partners> GetAllParceiros()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaAll = db.Partners.Where(p => p.ENVIRONMENT == env && p.SHORTNAME.ToLower() != "admin").OrderBy(c => c.NAME).ToList();
                List<Partners> lista = new List<Partners>();
                foreach (var partner in listaAll)
                {

                    //db.Entry(partner).Reload();
                    lista.Add(partner);
                }

                return lista;
            }
        }


        public List<Partners> GetAllParceirosSemRepeticao()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<Partners> lista = db.Partners.Where(p => p.ENVIRONMENT == env  ).Distinct().ToList();
                return lista;
            }
        }


        public Partners GetParceiroId(string id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                Partners parceiro = db.Partners.Find(int.Parse(id));
                return parceiro;
            }
        }

        public Partners GetParceirosById(int? id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                Partners partner = db.Partners.Where(p => p.ENVIRONMENT == env && p.PARTNERID == id).FirstOrDefault();
                return partner;
            }
        }


        public Partners GetParceiroName(string name)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var parceiro = db.Partners.Where(n => n.FIRSTNAME == name).FirstOrDefault();
                return parceiro;
            }
        }

        /**
        * Retorna uma lista de parceiros, exceto o admin.
        */
        public List<Partners> GetParceirosNaoUsuarios()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaAll = db.Partners.Where(p => p.ENVIRONMENT == env && p.SHORTNAME.ToLower() != "admin").OrderBy(c => c.NAME).ToList();
                List<Partners> lista = new List<Partners>();
                foreach (var partner in listaAll)
                {
                    if (!partner.IsUserForced)
                    {
                        db.Entry(partner).Reload();
                        lista.Add(partner);
                    }
                }

                return lista;
            }
        }

        /**
        * Retorna uma lista de datas, baseada no periodo passado como parametro.
        */
        public List<Partners> GetEmpresas()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var lista = db.Partners.Where(p => p.ENVIRONMENT == env && p.ISUSER.ToUpper() == "N").OrderBy(p => p.SHORTNAME).ToList();
                return lista;
            }
        }

        /**
      * Retorna o pais.
      */
        public Partners GetPartnerByLogin(string login)
        {
            UsersDataAccess userData = new UsersDataAccess();
            using (TimesheetContext db = new TimesheetContext())
            {
                Users user = userData.GetUserByLogin(login);
                int userid = user.USERID;
                Partners partner = null;
                foreach (Partners p in db.Partners)
                {
                    if (p.user.USERID == userid)
                    {
                        return p;
                    }
                }
                return partner;
            }
        }

        /**
        * Retorna uma empresa, pelo id
        */
        public Partners GetEmpresa(int PartnerID)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var empresa = db.Partners.Find(PartnerID);
                return empresa;
            }
        }

        /**
       * Retorna uma empresa, pelo id
       */
        public Partners GetParceiro(int PartnerID)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var empresa = db.Partners.Find(PartnerID);
                return empresa;
            }
        }

        public Partners GetProjeto(int PartnerID)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var empresa = db.Partners.Find(PartnerID);
                return empresa;
            }
        }


        public List<Partners> GetParceiroPorNome(string nome)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<Partners> parceiros = db.Partners.Where(n => n.NAME.StartsWith(nome)).ToList();
                return parceiros;
            }
        }

        public Partners GetParceiroPorPrimeiroNome(string nome)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                Partners parceiros = db.Partners.Where(n => n.FIRSTNAME == nome).FirstOrDefault();
                return parceiros;
            }
        }


        public Partners GetParceiroPorUltimoNome(string nome)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                Partners parceiros = db.Partners.Where(n => n.LASTNAME == nome).FirstOrDefault();
                return parceiros;
            }
        }



        //    .Where(oh => oh.Hierarchy.Contains("/12/"))
        //You can also use.StartsWith() or.EndsWith().

        /**
        * Retorna gestores do sistrema.
        */
        public List<Partners> GetGestores()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                string tipoGestor = ((int)Constants.ProfileConstant.GESTOR).ToString();
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaUsers = db.Users.Where(u => u.ENVIRONMENT == env && u.PROFILE == tipoGestor).ToList();
                List<Partners> lista = new List<Partners>();

                foreach (var item in listaUsers)
                {
                    lista.Add(item.Partner);
                }

                return lista;
            }
        }




        /**
       * Retorna todo os paises.
       */
        public static List<Country> GetPaisesAll()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var pais = db.Countries.ToList();
                return pais;
            }
        }

        /**
       * Retorna todas as cidades.
       */
        public static List<BrazilCity> GetCidadesAll()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var cidades = db.BrazilCity.ToList();
                return cidades;
            }
        }


        /**
       * Retorna o pais.
       */
        public static Country GetPais(string COUNTRYID)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var pais = db.Countries.Find(COUNTRYID);
                return pais;
            }
        }

        /**
       * Retorna o pais.
       */
        public static BrazilCity GetCidade(string CITYID)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var city = db.BrazilCity.Where(c => c.CITYCODE == CITYID).FirstOrDefault();
                return city;
            }
        }

        /**
       * Retorna todos os estados.
       */
        public static List<BrazilState> GetEstadosAll()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var st = db.BrazilState.ToList();
                return st;
            }
        }

        /**
       * Retorna o estado.
       */
        public static BrazilState GetEstado(string STATE)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var st = db.BrazilState.Where(s => s.UF == STATE).FirstOrDefault();
                return st;
            }
        }

        /**
        * Verifica se ja tem o mesmo login cadastrado.
        */
        public static bool isLoginCadastrado(string login)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                string tipoGestor = ((int)Constants.ProfileConstant.GESTOR).ToString();
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var count = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME == login).Count();
                return count > 0;
            }
        }

        /**
        * Retorna o usuario do partner, se for do tipo usuario.
        */
        public static Users GetUsuario(Partners partner)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaUsuarios = db.Users.Where(u => u.ENVIRONMENT == env);
                foreach (var usuario in listaUsuarios)
                {
                    if (usuario.Partner.PARTNERID == partner.PARTNERID)
                    {
                        return usuario;
                    }
                }
                return null;
            }
        }



        /**
        * Retorna o usuario pelo id
        */
        public static Users GetUsuario(int id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var usuario = db.Users.Where(u => u.ENVIRONMENT == env && u.USERID == id).FirstOrDefault();
                return usuario;
            }
        }

        /**
        * Retorna o perfil pelo id
        */
        public static Perfil GetPerfil(int id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                //
                var lista = from upf in db.Perfils where upf.ENVIRONMENT == env && upf.PERFILID == id select upf;
                //var lista = db.Perfils.ToList();
                //foreach(var item in lista) {
                //    if ( item.PERFILID == id ) {
                //        return item;
                //    }
                //}
                if (lista != null)
                {
                    var listaX = lista.ToList();
                    return listaX[0];
                }
                return null;
            }
        }

        /**
        * Retorna o perfil pelo id
        */
        public static Permission GetPermission(int id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var perfil = db.Permissions.Find(id);
                //db.Permissions.Where(u => u.ENVIRONMENT == env && u.PERMISSIONID == id).FirstOrDefault();
                return perfil;
            }
        }

        /**
       * Retorna todos as permissoes do perfil.
       */
        public static List<Permission> GetPermissions(Perfil perfil)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var listaPP = db.PerfilPermissions.Where(p => p.ENVIRONMENT == env && p.PERFILID == perfil.PERFILID);
                List<Permission> listaPerm = new List<Permission>();

                foreach (var p in listaPP)
                {
                    listaPerm.Add(p.permission);
                }

                //var st = from upf in db.PerfilPermissions where upf.ENVIRONMENT == env select upf.permission; 

                return listaPerm;
            }
        }

        /**
      * Retorna todos os perfis do usuario.
      */
        public static List<Perfil> GetPerfisUsuario(Users usuario)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var perfisUsuario = from upf in db.UserPerfils where upf.ENVIRONMENT == env && upf.USERID == usuario.USERID select upf;
                List<Perfil> list = new List<Perfil>();
                foreach (UserPerfil p in perfisUsuario)
                {
                    list.Add(p.perfil);
                }
                return list;
            }
        }

        /**
      * Retorna todos os perfis cadastrados.
      */
        public static List<Perfil> GetPerfisAll()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var list = from pf in db.Perfils where pf.ENVIRONMENT == env && pf.ENVIRONMENT == env && pf.CODE != "00" select pf;
                return list.ToList();
            }
        }



        /**
        * Retorna todos as permissoes/objetos do usuario
        */
        public static List<Permission> GetPermissoesUsuario(Users usuario)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<Permission> listPermissions = new List<Permission>();
                foreach (var perfil in usuario.PerfilsList())
                {
                    foreach (var permission in perfil.GetPermissions())
                    {
                        listPermissions.Add(permission);
                    }
                }
                //var permissoes = from perfil in usuario.PerfilsList
                //            from permission in perfil.GetPermissions()
                //            select permission;
                //return permissoes.Distinct().ToList();
                return listPermissions.Distinct().ToList();
            }
        }

        /**
         * Percorre todos as permissoes e perfis do usuario, e verifica se tem permissao para o objeto.
         * Caso nao encontre o objeto, ele valida global, na regra antiga.
         */
        public static bool ValidaAcessoModulo(Users usuarioLogado, string ObjectCode)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                //busca todas permissoes do usuario
                var listUserPermissions = GetPermissoesUsuario(usuarioLogado);

                if (listUserPermissions != null && listUserPermissions.Count() > 0)
                {
                    return listUserPermissions.Where(p => p.CODE == ObjectCode).Count() > 0;
                }

                return false;
            }
        }


    }
}