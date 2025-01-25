using LoungeManagementApplication.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoungeManagementApplication
{
    public partial class FrmLogin : Form
    {
        bool isSeen=false;
        public FrmLogin()
        {
            InitializeComponent();
            
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmSignup frm = new FrmSignup();
            frm.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblerror.Text ="";
            if (txtusername.Text==""||txtpassword.Text=="")
            {
                lblerror.Text = "Enter all info";
                return;
            }
            try
            {
                MainClass.connection.Open();

                using (SqlCommand command = new SqlCommand("GetUsers", MainClass.connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                string usern = row["username"].ToString();
                                string passw = row["password"].ToString();
                                if (passw == txtpassword.Text && usern == txtusername.Text)
                                {
                                    User.username = txtusername.Text;
                                    MainClass.connection.Close();
                                    this.Hide();
                                    FrmUser frm = new FrmUser();
                                    frm.ShowDialog();
                                    

                                }
                                else
                                {
                                    lblerror.Text = "incorrect input";
                                }
                            }
                        }
                    }
                }
                MainClass.connection.Close();
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

            guna2Panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void icon_Click(object sender, EventArgs e)
        {
            if (isSeen == false)
            {

                txtpassword.PasswordChar = '\0';
                icon.Image = Resources.hide;
                isSeen = true;
            }
            else
            {

                txtpassword.PasswordChar = '●';
                icon.Image = Resources.view;
                isSeen = false;
            }
        }
    }
}
