using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Apassos.Tools
{
    public class Conexao
    {
        string StringConexao = getConfigTagByName("UserDBContext");

        public static string getConfigTagByName(string Name)
        {
            return ConfigurationManager.AppSettings[Name].ToString();
        }

        public bool UsuarioExistente(string login, string password)
        {
            SqlConnection conexao = new SqlConnection("UserDBContext");
            var comando = "select * from USERS where LOGINID = '"+login+"' and PASSWORD = '"+password+"'";
            SqlCommand cmd = new SqlCommand(comando, conexao);
            conexao.Open();
            cmd.ExecuteNonQuery();
            conexao.Close();
            return false;
        }
    }
}