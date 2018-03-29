using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Seminaires
{
    public partial class Sem : Form
    {
        public Sem()
        {
            InitializeComponent();
        }

        public string id;
        string id_ch;
        string id_org;
        private void button2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            button4.Text = "Modifier";

        }
        public static void remplire_grid(string req, DataGridView gd)
        {

            SqlConnection cn = new SqlConnection();
            cn = Connexion.getCon();
            cn.Open();

            SqlDataAdapter da = new SqlDataAdapter(req, cn);
            DataSet ds = new DataSet();
            da.Fill(ds, "table");
            DataTable dtable = ds.Tables["table"];

            gd.DataSource = dtable;
            // gd.DataBind();
            //dr.Close();
            cn.Close();


        }

       

        private void label3_Click(object sender, EventArgs e)
        {

        }
     public bool test()
        { string txt=textBox2.Text;
         int c;
            if ((textBox1.Text == string.Empty) || (textBox2.Text == string.Empty) || (textBox3.Text == string.Empty))
                return false;
            if (int.TryParse(txt, out c))
            
               return true;
            
            else
                return false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
          if (test()==true)
            {
                if (button4.Text == "Ajouter")
                {



                    string req = "insert into SEMINAIRE(DateS, DureeS, LieuS) values (" + "CAST('" + textBox1.Text + "' AS DATETIME)" + ","
                        +textBox2.Text + ",'" + textBox3.Text + "')";
                    SqlConnection con;
                    con = Connexion.getCon();
                    con.Open();
                    SqlCommand cmd = new SqlCommand(req, con);
                    int ex = cmd.ExecuteNonQuery();
                    if (ex == 0)
                        MessageBox.Show(this, "Erreur de connexion", "msg", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    string req1 = "select * from SEMINAIRE";
                    remplire_grid(req1, dataGridView1);

                }
                else if (button4.Text == "Modifier")
                {
                    string req = "update SEMINAIRE set dateS= CAST('" + textBox1.Text + "' AS DATETIME)," + " DureeS=CAST('" + textBox2.Text + "' AS FLOAT)," + "LieuS='" + textBox3.Text + "' where CodeS=" + Convert.ToInt32(id);
                    SqlConnection cn;
                    cn = Connexion.getCon();
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(req, cn);
                    int res = cmd.ExecuteNonQuery();
                    if (res == 0)
                        MessageBox.Show("erreur");
                    else
                    {

                        string req1 = "select * from SEMINAIRE";
                        remplire_grid(req1, dataGridView1);

                    }
                }
            }
            else
                MessageBox.Show("verifier les types des champs");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.Red;
            id = dataGridView1.Rows[e.RowIndex].Cells["CodeS"].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["dateS"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["DureeS"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["LieuS"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            button4.Text = "Ajouter";
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string req = "delete from SEMINAIRE where CodeS=" + Convert.ToInt32(id);
            SqlConnection cn;
            cn = Connexion.getCon();
            cn.Open();
            SqlCommand cmd = new SqlCommand(req, cn);
            int res = cmd.ExecuteNonQuery();
            if (res == 0)
                MessageBox.Show("suppression echou");
            else
            {
                string req1 = "select * from SEMINAIRE";
                remplire_grid(req1, dataGridView1);
            }
        }

        private void Sem_Load(object sender, EventArgs e)
        {

            tabControl1.TabPages.Remove(tabPage3); ;//employer invisible....................................................
            //visibiliter de button consulter .................................................

            string req1 = "select e.nom_emp, e.prenom_emp,s.LieuS, s.dateS, p.etat from Emp e, SEMINAIRE s, Present p where s.CodeS=p.CodeS and e.id_emp=p.id_emp";

            string req = "select * from SEMINAIRE";
            string req2 = "select * from CHERCHEUR";
            string req3 = "select o.id, c.NomCH, s.dateS, o.Bonus  from ORGANISATION o, CHERCHEUR c, SEMINAIRE s where c.MatriculeCH=o.MatriculeCH and s.CodeS=o.CodeS";
            remplire_grid(req1, dataGridView4);
            remplire_grid(req2, dataGridView2);
            remplire_grid(req, dataGridView1);
            remplire_grid(req3, dataGridView3);
            dataGridView3.Columns["id"].Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
          
            remplir_combo_cherch();
            remplir_combo_sem();
            
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        public static DataTable rempli_cobo(string req, ComboBox combo)
        {
            SqlConnection cn = Connexion.getCon();
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter(req, cn);
            
            DataSet ds = new DataSet();
            da.Fill(ds, "table");
            DataTable dtable = ds.Tables["table"];
            DataTable ddtable = ds.Tables["table"];//ajouter par moi
            return ddtable;
          //  return dtable;



        }
        public void remplir_combo_cherch()
        {
            comboBox1.SelectedIndexChanged -= new EventHandler(comboBox1_SelectedIndexChanged);
            string req2 = "select distinct MatriculeCH, NomCH, PrenomCH from CHERCHEUR  ";//T, MM_Facture M where T.ID=M.ID_Tiers";
            DataTable dtable = rempli_cobo(req2, comboBox1);
            comboBox1.DataSource = dtable;
       
            comboBox1.DisplayMember = "NomCH";
            comboBox1.ValueMember = "MatriculeCH";
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }
        //---------------------------------------------------------------------
        public void remplir_combo_sem()//seminaire 
        {
            comboBox2.SelectedIndexChanged -= new EventHandler(comboBox2_SelectedIndexChanged_1);
            string reqq = "select CodeS, dateS FROM SEMINAIRE";
            DataTable ddtable = rempli_cobo(reqq, comboBox2);
            comboBox2.DataSource = ddtable;
            comboBox2.DisplayMember = "dateS";
            comboBox2.ValueMember = "CodeS";
            comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged_1);
        }
         
        //-------------------------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("id=" + comboBox1.Text);
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
          //  MessageBox.Show("CodeS=" + comboBox2.SelectedValue);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button8.Text = "Ajouter";
            groupBox3.Visible = true;
            
            


        }

        private void button6_Click(object sender, EventArgs e)
        {
            button8.Text = "Modifier";
            groupBox3.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string req = "delete from CHERCHEUR where MatriculeCH=" + Convert.ToInt32(id_ch);
            SqlConnection cn;
            cn = Connexion.getCon();
            cn.Open();
            SqlCommand cmd = new SqlCommand(req, cn);
            int res = cmd.ExecuteNonQuery();
            if (res == 0)
                MessageBox.Show("suppression echou");
            else
            {
                string req1 = "select * from CHERCHEUR";
                remplire_grid(req1, dataGridView2);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string req1 = "select * from CHERCHEUR";
            if (button8.Text == "Ajouter")
            {
                if ((textBox4.Text == string.Empty) || (textBox5.Text == string.Empty))
                { MessageBox.Show("verifier les champs vides "); }
                else
                {

                    string req = "insert into CHERCHEUR(NomCH, PrenomCH) values ('"+ textBox4.Text + "','"+textBox5.Text+ "')";
                    SqlConnection con;
                    con = Connexion.getCon();
                    con.Open();
                    SqlCommand cmd = new SqlCommand(req, con);
                    int ex = cmd.ExecuteNonQuery();
                    if (ex == 0)
                        MessageBox.Show(this, "Erreur de connexion", "msg", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                   
                    remplire_grid(req1, dataGridView2);
                    textBox4.Text = string.Empty;//pour vider la zone du text..................................................
                    textBox5.Text = string.Empty;//pour vider la zone du text..................................................
                }
            }
            else if(button8.Text == "Modifier")
            {
                string req = "update CHERCHEUR set NomCH='" + textBox4.Text + "', PrenomCH='" + textBox5.Text + "' where MatriculeCH=" + Convert.ToInt32(id_ch);
                SqlConnection cn;
                cn = Connexion.getCon();
                cn.Open();
                SqlCommand cmd = new SqlCommand(req, cn);
                int res = cmd.ExecuteNonQuery();
                if (res == 0)
                    MessageBox.Show("erreur");
                else
                {

                   
                    remplire_grid(req1, dataGridView2);
                    textBox4.Text = string.Empty;//pour vider la zone du text..................................................
                    textBox5.Text = string.Empty;//pour vider la zone du text..................................................
               

                }
            }
  

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.Red;
            id_ch = dataGridView2.Rows[e.RowIndex].Cells["MatriculeCH"].Value.ToString();
            textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells["NomCH"].Value.ToString();
            textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells["PrenomCH"].Value.ToString();
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime tm = dateTimePicker1.Value;
            textBox1.Text = tm.ToString(); 
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            string req1 = "select o.id, c.NomCH, s.dateS, o.Bonus  from ORGANISATION o, CHERCHEUR c, SEMINAIRE s where c.MatriculeCH=o.MatriculeCH and s.CodeS=o.CodeS";
            if (button12.Text == "Ajouter")
            {
               // if ((textBox4.Text == string.Empty) || (textBox5.Text == string.Empty))
              //  { MessageBox.Show("verifier les champs vides "); }
              //  else
               // {

                    string req = "insert into ORGANISATION (MatriculeCH, CodeS, Bonus) values ('" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "', "+textBox6.Text+")";
           
                    SqlConnection con;
                    con = Connexion.getCon();
                    con.Open();
                    SqlCommand cmd = new SqlCommand(req, con);
                    int ex = cmd.ExecuteNonQuery();
                    if (ex == 0)
                        MessageBox.Show(this, "Erreur de connexion", "msg", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);


                    remplire_grid(req1, dataGridView3);
               // }
            }
            else if (button8.Text == "Modifier")
            {
                string req = "update CHERCHEUR set MatriculeCH='" + comboBox1.SelectedValue + "', CodeS='" + comboBox2.SelectedValue + "', Bonus='"+textBox6.Text+"' where id=" + Convert.ToInt32(id_org);
                SqlConnection cn;
                cn = Connexion.getCon();
                cn.Open();
                SqlCommand cmd = new SqlCommand(req, cn);
                int res = cmd.ExecuteNonQuery();
                if (res == 0)
                    MessageBox.Show("erreur");
                else
                {


                    remplire_grid(req1, dataGridView3);

                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button12.Text = "Ajouter";
            groupBox4.Visible = true;
            textBox6.Text = string.Empty;
        
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            button12.Text = "Modifier";

            groupBox4.Visible = true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
           //button13.Visible = true;  //visibiliter de button consulter ...................................
        
        }
        
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.RowsDefaultCellStyle.SelectionBackColor = Color.Red;
            id_org = dataGridView3.Rows[e.RowIndex].Cells["id"].Value.ToString();
            //afiché la valeur selectinner de datagridview en combobox1 
            comboBox1.Text = dataGridView3.Rows[e.RowIndex].Cells["Nomch"].Value.ToString();

            comboBox2.Text = dataGridView3.Rows[e.RowIndex].Cells["dateS"].Value.ToString();
            textBox6.Text = dataGridView3.Rows[e.RowIndex].Cells["Bonus"].Value.ToString();

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click_1(object sender, EventArgs e)
        {
            
        }
        bool btclick = false;
        private void button13_Click(object sender, EventArgs e)
        {
            if (btclick == false)
            {
                tabControl1.TabPages.Add(tabPage3);
                tabControl1.SelectTab(3);
            }

        btclick = true;
            
            //visibilité d'employer ................
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        
    }
}
