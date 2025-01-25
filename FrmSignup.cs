using LoungeManagementApplication.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
 

namespace LoungeManagementApplication
{
    public partial class FrmSignup : Form
    {
        bool isSeen = false;
        public FrmSignup()
        {
            InitializeComponent();
        }
 

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string emailpattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string phonepattern = @"^(09|07)\d{8}$";
            
            lblerror.Text =" ";

            if (txtusername.Text == "" || txtpassword.Text == "" || txtphone.Text == "" || txtconfirm.Text == "" || txtphone.Text == "")
            {
                lblerror.Text = "Enter all info";
                return;
            }
           
            try
            {
                MainClass.connection.Open();
        
                 SqlCommand command = new SqlCommand("GetUsers", MainClass.connection);
                    command.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("in");
                        foreach (DataRow row in dt.Rows)
                        {
                            MessageBox.Show("in");
                            string usernam = row["username"].ToString();
                            string usereml = row["email"].ToString();
                            if (usernam == txtusername.Text)
                            {
                                lblerror.Text = "there is account in this username!";
                                MainClass.connection.Close();
                                return;
                            }
                            else if (usereml == txtemail.Text)
                            {
                                lblerror.Text = "there is account in this email!";
                                MainClass.connection.Close();
                                return ;
                            }
                        }
                    }
                    Regex regex1 = new Regex(emailpattern);
                    Regex regex2 = new Regex(phonepattern);
                    if (!regex1.IsMatch(txtemail.Text))
                    {
                        MessageBox.Show("here"+ txtemail.Text);
                       lblerror.Text = "Invalid email format. try again";
                        MainClass.connection.Close();
                        return;
                    }
                    if (!regex2.IsMatch(txtphone.Text))
                    { 
                        lblerror.Text = "Invalid phone format! please begin with 09 or 07";
                        MainClass.connection.Close();
                        return;
                    }
                    else if (txtpassword.Text != txtconfirm.Text)
                    {
                        lblerror.Text = "password confirmation failed!";
                        MainClass.connection.Close();
                        return;
                    }
                    else
                    {
                        MainClass.connection.Close();
                        storeUser(txtusername.Text, txtpassword.Text, txtemail.Text, txtphone.Text);
                        this.Hide();
                        FrmUser frm = new FrmUser();
                        frm.ShowDialog();
                        
                      
                    }
                }
          
               
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }



        private void storeUser(string username, string password, string email, string phone)
        {
            try
            {
                MainClass.connection.Open();

                using (SqlCommand command = new SqlCommand("StoreUser", MainClass.connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.ExecuteNonQuery();
                     
                }
                MainClass.connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

}
        private void Loginbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmLogin frm = new FrmLogin();
            frm.ShowDialog();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmSignup_Load(object sender, EventArgs e)
        {
            guna2Panel3.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void picon_Click(object sender, EventArgs e)
        {
            if (isSeen == false)
            {

                txtpassword.PasswordChar = '\0';
                picon.Image = Resources.hide;
                isSeen = true;
            }
            else
            {

                txtpassword.PasswordChar = '●';
                picon.Image = Resources.view;
                isSeen = false;
            }
         
        }

        private void cicon_Click(object sender, EventArgs e)
        {
            if (isSeen == false)
            {
                txtconfirm.PasswordChar = '\0';
                cicon.Image = Resources.hide;
                isSeen = true;
               
            }
            else
            {
                 txtconfirm.PasswordChar = '●';
                cicon.Image = Resources.view;
                isSeen = false;
               
            }
 
        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
