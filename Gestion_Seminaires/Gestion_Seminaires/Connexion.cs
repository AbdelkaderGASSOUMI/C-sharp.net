using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
namespace Gestion_Seminaires
{
    class Connexion
    {
        public static SqlConnection getCon()
        {
         string cnx = Gestion_Seminaires.Properties.Settings.Default.cnx;
         SqlConnection con = new SqlConnection(cnx);
         return con;
        }
       


    }
}
