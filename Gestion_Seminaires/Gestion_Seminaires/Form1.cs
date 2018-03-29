using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Gestion_Seminaires
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string cnx = Gestion_Seminaires.Properties.Settings.Default.cnx;
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
           string req = "select * from Connexion where login='" + Logintxt.Text + "' and password='" + pwdtxt.Text+"'";
            SqlConnection con = new SqlConnection(cnx);
            con.Open();
            SqlCommand cmd = new SqlCommand(req, con);
       
            int l = cmd.ExecuteNonQuery();
            if (l != 0)
            {
                //Seminaire s = new Seminaire();
                Sem s = new Sem();
              //  s.Visible = true;
                s.Show();
                
            }
            else
                MessageBox.Show("erreur");

           
            con.Close();
          //  Sem s = new Sem();
        //    s.Visible = true;
          //  s.Show();
        //    this.Visible = false;
          //  this.Hide();
            
        }
    }
}
