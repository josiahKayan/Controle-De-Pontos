using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Apassos.Common;
using Apassos.Models;
using Apassos.Common.Extensions;
using System.Data;
using System.Data.Entity;

namespace Apassos.DataAccess
{
    public class UsersDataAccess
    {
        static private TimesheetContext db = new TimesheetContext();


    public static void SalvarUsuario(Users usuario, string checkPerfil, string idParceiro, Users usuarioLogado, string locked, string isalterpwd)

        {
            var id = usuario.USERID;
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

            if (id == 0) //inserir
            {
                Partners partner = db.Partners.Find(int.Parse(idParceiro));
                partner.ISUSER = "S";
                db.Entry(partner).State = EntityState.Modified;

                usuario.ENVIRONMENT = env;
                usuario.LOCKED = Constants.ConvertLockedSN(int.Parse(locked));
                usuario.ISALTERPWD = isalterpwd;
                usuario.PASSWORD = "@passos";
                usuario.PROFILE = "3";
                usuario.PARTNERID = partner.PARTNERID;
                usuario.USERNAME = usuario.USERNAME.ToUpper();
                usuario.CREATEDBY = usuarioLogado.USERNAME;
                usuario.CREATIONDATE = DateTime.Now;
                usuario.CHANGEDBY = usuarioLogado.USERNAME;
                usuario.CHANGEDATE = DateTime.Now;
                
                db.Users.Add(usuario);
                db.SaveChanges();

                var idsperfis = checkPerfil.Split(',');
                foreach (var idp in idsperfis)
                {
                    UserPerfil itemSalvar = new UserPerfil
                    {
                        ENVIRONMENT = env,
                        PERFILID = int.Parse(idp),
                        USERID = usuario.USERID,
                        CREATEDBY = usuarioLogado.USERNAME,
                        CREATIONDATE = DateTime.Now,
                        CHANGEDBY = usuarioLogado.USERNAME,
                        CHANGEDATE = DateTime.Now
                    };
                    db.UserPerfils.Add(itemSalvar);
                }

            }
            else //atualizar
            {
                Users usuarioEdit = db.Users.Find(usuario.USERID);
                string perfisIds = checkPerfil;
                perfisIds = "_" + perfisIds.Replace(",", "_") + "_";
                string parceirosInserts = "";

                usuarioEdit.PARTNERID = int.Parse(idParceiro);
                usuarioEdit.LOCKED = Constants.ConvertLockedSN(int.Parse(locked));
                usuarioEdit.ISALTERPWD = isalterpwd;
                usuarioEdit.VALIDFROM = usuario.VALIDFROM;
                usuarioEdit.VALIDTO = usuario.VALIDTO;
                usuarioEdit.CHANGEDBY = usuarioLogado.USERNAME;
                usuarioEdit.CHANGEDATE = DateTime.Now;
                db.Entry(usuarioEdit).State = EntityState.Modified;

                //remove os perfis nao selecionados
                var listPerfisOld = usuario.UserPerfilsList();
                foreach (UserPerfil upf in listPerfisOld)
                {
                    if (perfisIds.IndexOf("_" + upf.PERFILID + "_") >= 0)
                    {
                        parceirosInserts = parceirosInserts + "_" + upf.PERFILID + "_";
                    }
                    else
                    {
                        db.UserPerfils.Remove(upf);
                    }
                }

                var idsperfis = checkPerfil.Split(',');
                foreach (string idp in idsperfis)
                {
                    if (parceirosInserts.IndexOf("_" + idp + "_") < 0)
                    {
                        UserPerfil itemSalvar = new UserPerfil
                        {
                            ENVIRONMENT = env,
                            PERFILID = int.Parse(idp),
                            USERID = usuario.USERID,
                            CREATEDBY = usuarioLogado.USERNAME,
                            CREATIONDATE = DateTime.Now,
                            CHANGEDBY = usuarioLogado.USERNAME,
                            CHANGEDATE = DateTime.Now
                        };
                        db.UserPerfils.Add(itemSalvar);
                    }
                }
            }

            db.SaveChanges();

            Users usuarioUpdate = db.Users.Find(usuario.USERID);
            db.Entry(usuarioUpdate).Reload();
           

        }

        /**
      * Retorna todos os perfis do usuario.
      */
        public static List<UserPerfil> GetUserPerfisUsuario(Users usuario)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var perfisUsuario = from upf in db.UserPerfils where upf.ENVIRONMENT == env && upf.USERID == usuario.USERID select upf;
            List<Perfil> list = new List<Perfil>();
            return perfisUsuario.ToList();
        }

        /**
      * Retorna usuario pelo login.
      */
        public static Users GetUserByLogin(string login)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var usuario = from upf in db.Users where upf.ENVIRONMENT == env && upf.USERNAME == login select upf;
            if ( usuario != null && usuario.Count() > 0 ) {
                return usuario.ToList()[0];
            }
            return null;
        }



        public static bool VerificaPerfil(Users usuario, Perfil perfil)
        {
            var perfillist = usuario.PerfilsList();

            var perfilExiste = perfillist.Where(p => p.PERFILID == perfil.PERFILID).FirstOrDefault();
            return perfilExiste != null;
        }

        public static Users GetUsuario(int id)
        {
            return db.Users.Find(id);
        }



        public static string CreateLogin(Partners partner)
        {
            //busca se tem um login igual a PrimeiraLetraDoNome+UltimoSobrenome
            //se encontrar adiciona um contador para o usuario
            short contador = 0;
            string login = partner.FIRSTNAME.Substring(0, 1).ToUpper() + partner.LASTNAME.ToUpper();
            while (true)
            {
                var encontrou = db.Users.Where(u => u.USERNAME == login).FirstOrDefault();
                if ( encontrou == null ) {
                    return login;
                }
                contador++;
                login = login + contador;
            }
        }

        public static string SenhaDefault(Users usuario)
        {
            return (usuario.USERNAME.ToLower() + ":" + "@passos".ToLower()).sysPassEncrypt();
        }
        public static string SenhaDefault(String login)
        {
            return (login.ToLower() + ":" + "@passos".ToLower()).sysPassEncrypt();
        }
        public static string ConcatSenhaAcesso(string login, string senha, bool cript)
        {
            string senhaCript = login.ToLower() + ":" + senha.ToLower();
            if ( cript ) {
                senhaCript = senhaCript.sysPassEncrypt();
            }
            return senhaCript;
        }

        public static bool isLogin(string loginid, string password)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var userLoginPass = (loginid.ToLower() + ":" + password.ToLower()).sysPassEncrypt();
            var user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(userLoginPass.Trim().ToLower())).FirstOrDefault();
            if (user == null)
            {
                user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                    Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(password.Trim().ToLower())).FirstOrDefault();
            }
            if (user == null)
            {
                throw new AcessosException("login e senha não encontrados. Verifique os dados informados e tente novamente!");
            }
            return true;
        }
    }
}