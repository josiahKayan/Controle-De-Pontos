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
        private static TimesheetContext db = new TimesheetContext();

        /**
        * Retorna uma lista de parceiros, exceto o admin.
        */
        public static List<Partners> GetParceirosSistema()
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




        public static List<Partners> GetAllParceiros()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var listaAll = db.Partners.Where(p => p.ENVIRONMENT == env && p.SHORTNAME.ToLower() != "admin").OrderBy(c => c.NAME).ToList();
            List<Partners> lista = new List<Partners>();
            foreach (var partner in listaAll)
            {
                
                db.Entry(partner).Reload();
                lista.Add(partner);
            }

            return lista;
        }

        public static Partners GetParceiroId(string id)
        {
            var parceiro = db.Partners.Find(int.Parse(id));
            return parceiro;
        }


        public static Partners GetParceiroName(string name)
        {
            var parceiro = db.Partners.Where( n => n.FIRSTNAME == name  ).FirstOrDefault();
            return parceiro;
        }

        /**
        * Retorna uma lista de parceiros, exceto o admin.
        */
        public static List<Partners> GetParceirosNaoUsuarios()
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

        /**
        * Retorna uma lista de datas, baseada no periodo passado como parametro.
        */
        public static List<Partners> GetEmpresas()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Partners.Where(p => p.ENVIRONMENT == env && p.ISUSER.ToUpper() == "N").OrderBy(p => p.SHORTNAME).ToList();
            return lista;
        }

        /**
      * Retorna o pais.
      */
        public static Partners GetPartnerByLogin(string login)
        {
            Users user = UsersDataAccess.GetUserByLogin(login);
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

        /**
        * Retorna uma empresa, pelo id
        */
        public static Partners GetEmpresa(int PartnerID)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var empresa = db.Partners.Find(PartnerID);
            return empresa;
        }

        /**
       * Retorna uma empresa, pelo id
       */
        public static Partners GetParceiro(int PartnerID)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var empresa = db.Partners.Find(PartnerID);
            return empresa;
        }

        public static Partners GetProjeto(int PartnerID)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var empresa = db.Partners.Find(PartnerID);
            return empresa;
        }


        public static  List<Partners> GetParceiroPorNome( string nome)
        {
           var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
           List<Partners> parceiros = db.Partners.Where( n => n.NAME.StartsWith(nome) ).ToList();
           return parceiros;
        }

      //    .Where(oh => oh.Hierarchy.Contains("/12/"))
      //You can also use.StartsWith() or.EndsWith().

      /**
      * Retorna gestores do sistrema.
      */
      public static List<Partners> GetGestores()
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


        

        /**
       * Retorna todo os paises.
       */
        public static List<Country> GetPaisesAll()
        {
            var pais = db.Countries.ToList();
            return pais;
        }

        /**
       * Retorna todas as cidades.
       */
        public static List<BrazilCity> GetCidadesAll()
        {
            var cidades = db.BrazilCity.ToList();
            return cidades;
        }


        /**
       * Retorna o pais.
       */
        public static Country GetPais(string COUNTRYID)
        {
            var pais = db.Countries.Find(COUNTRYID);
            return pais;
        }

        /**
       * Retorna o pais.
       */
        public static BrazilCity GetCidade(string CITYID)
        {
            var city = db.BrazilCity.Where(c => c.CITYCODE == CITYID).FirstOrDefault();
            return city;
        }

        /**
       * Retorna todos os estados.
       */
        public static List<BrazilState> GetEstadosAll()
        {
            var st = db.BrazilState.ToList();
            return st;
        }

        /**
       * Retorna o estado.
       */
        public static BrazilState GetEstado(string STATE)
        {
            var st = db.BrazilState.Where(s => s.UF == STATE).FirstOrDefault();
            return st;
        }

        /**
        * Verifica se ja tem o mesmo login cadastrado.
        */
        public static bool isLoginCadastrado(string login)
        {
            string tipoGestor = ((int)Constants.ProfileConstant.GESTOR).ToString();
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var count = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME == login).Count();
            return count > 0;
        }

        /**
        * Retorna o usuario do partner, se for do tipo usuario.
        */
        public static Users GetUsuario(Partners partner)
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



        /**
        * Retorna o usuario pelo id
        */
        public static Users GetUsuario(int id)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var usuario = db.Users.Where(u => u.ENVIRONMENT == env && u.USERID == id).FirstOrDefault();
            return usuario;
        }

        /**
        * Retorna o perfil pelo id
        */
        public static Perfil GetPerfil(int id)
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

        /**
        * Retorna o perfil pelo id
        */
        public static Permission GetPermission(int id)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var perfil = db.Permissions.Find(id);
            //db.Permissions.Where(u => u.ENVIRONMENT == env && u.PERMISSIONID == id).FirstOrDefault();
            return perfil;
        }

        /**
       * Retorna todos as permissoes do perfil.
       */
        public static List<Permission> GetPermissions(Perfil perfil)
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

        /**
      * Retorna todos os perfis do usuario.
      */
        public static List<Perfil> GetPerfisUsuario(Users usuario)
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

        /**
      * Retorna todos os perfis cadastrados.
      */
        public static List<Perfil> GetPerfisAll()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var list = from pf in db.Perfils where pf.ENVIRONMENT == env && pf.ENVIRONMENT == env && pf.CODE != "00" select pf;
            return list.ToList();
        }



        /**
        * Retorna todos as permissoes/objetos do usuario
        */
        public static List<Permission> GetPermissoesUsuario(Users usuario)
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

        /**
         * Percorre todos as permissoes e perfis do usuario, e verifica se tem permissao para o objeto.
         * Caso nao encontre o objeto, ele valida global, na regra antiga.
         */
        public static bool ValidaAcessoModulo(Users usuarioLogado, string ObjectCode)
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