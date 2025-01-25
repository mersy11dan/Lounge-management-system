using Guna.UI2.WinForms;
using LoungeManagementApplication.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LoungeManagementApplication
{
    public partial class FrmUser : Form
    {
        public int lid;
        public int catid;
        bool load = false;
        bool payment = false;
        public string orderType="";
        public FrmUser()
        {
             
            InitializeComponent(); 
            MainClass.productsData(); 
            MainClass.loungeData(); 
            displayLounges();
            lbltotal.Text = "0";


        }

        private void ucLounge_Click(object sender, EventArgs e )
        {
            ucLounge clickedLounge = (ucLounge)sender;
            lid= clickedLounge.loungeid;
            userPanel.Visible = true;  
            addCategory(); 
            displayProducts();


        }
        private void ucProduct_Click(object sender, EventArgs e)
        {
            var clickedproduct = (ucProduct)sender;
            double total= 0;
            bool productExists = false;

            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                if (Convert.ToInt32(item.Cells["dgvid"].Value) == clickedproduct.pid)
                {
                     
                    item.Cells["dgvqty"].Value = int.Parse(item.Cells["dgvqty"].Value.ToString()) + 1;
                    item.Cells["dgvamount"].Value = int.Parse(item.Cells["dgvqty"].Value.ToString()) *
                                                double.Parse(item.Cells["dgvprice"].Value.ToString());
                    productExists = true;
                     
                    break;
                }
              
            }

            if (!productExists)
            {
                guna2DataGridView1.Rows.Add(new object[] { clickedproduct.pid, clickedproduct.productname, 1, clickedproduct.pprice, clickedproduct.pprice });

            }

            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                total += double.Parse(item.Cells["dgvamount"].Value.ToString());
            }
                lbltotal.Text = total.ToString();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }
        private void displayProducts()
        {

            if (load == false)
            {
                foreach (Product product in MainClass.products)
                {
                  
                    ucProduct lCard = new ucProduct();
                    lCard.pid = product.pid;
                    lCard.Catid = product.catid;
                    lCard.pprice = product.pprice;                    
                    lCard.productname = product.pname;
                    lCard.pimage = product.GetImage();                     
                    lCard.lid = product.lgid;                   
                    lCard.Click += ucProduct_Click;
                    load = true;
                    if (lCard.lid == lid)
                    {
                        productPanel.Controls.Add(lCard);
                        lCard.Show();

                    }
                }
            }
        }
        private void displayLounges()
        {
           
            foreach (Lounge lounge in MainClass.lounges)
            {
                
                ucLounge lCard = new ucLounge();
                lCard.loungename = lounge.lname;
                lCard.loungelocation = lounge.location;
                lCard.limage = lounge.GetImage();
                lCard.loungeid =lounge.lid;
                lCard.Click += ucLounge_Click;
                loungePanel.Controls.Add(lCard);
                lCard.Show();
                 
            } 
        }

      

        private void FrmUser_Load(object sender, EventArgs e)
        { 
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void wellcome_page_Paint(object sender, PaintEventArgs e)
        { 
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void userPanel_Paint(object sender, PaintEventArgs e)
        { 
        }

        private void CategoryPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void addCategory()
        {
            catpanel.Controls.Clear();
            Guna.UI2.WinForms.Guna2Button ba = new Guna.UI2.WinForms.Guna2Button();
            ba.FillColor = Color.LightSeaGreen;
            ba.Size = new Size(302, 52);
            ba.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            ba.Click += new EventHandler(filterInCatagory);
            ba.Text = "All Category";
            ba.HoverState.FillColor = Color.LightSeaGreen;
            catpanel.Controls.Add(ba);

            try
            {
                MainClass.connection.Open(); 
                using (SqlCommand command = new SqlCommand("GetCategory", MainClass.connection))
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

                                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                                b.FillColor = Color.LightSeaGreen;
                                b.Size = new Size(302, 52);
                                b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                                b.Click += new EventHandler( filterInCatagory);
                                b.Text = row["categoryname"].ToString();
                                b.HoverState.FillColor = Color.LightSeaGreen;
                                int lgid = Convert.ToInt16( row["loungeid"]);
                                
                                if (lid==lgid){

                                    catpanel.Controls.Add(b);
                                }

                            }
                        }
                    }
                 MainClass.connection.Close();
                }
                }catch (SqlException ex){
                MessageBox.Show(ex.Message);
            }
        }
        private void filterInCatagory (object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button clickedbutton = (Guna.UI2.WinForms.Guna2Button)sender;
            catid=GetCategoryId(clickedbutton.Text);

            productPanel.Controls.Clear();
            if(clickedbutton.Text=="All Category")
            {
                load = false;                 
                displayProducts();
                return;
            }
            foreach (Product product in MainClass.products)
            {

                ucProduct lCard = new ucProduct();
                lCard.productname = product.pname;
                lCard.pimage = product.GetImage();
                lCard.Catid = product.catid;
                lCard.lid = product.lgid;
                lCard.Click += ucProduct_Click;
                
                if (lCard.lid == lid && lCard.Catid==catid)
                {                    
                    productPanel.Controls.Add(lCard);
                    lCard.Show();

                }
            }

        }

        public int GetCategoryId(string categoryname)
        {
           
            try
            {
                MainClass.connection.Open();

                int categoryId = 0;
                using (SqlCommand command = new SqlCommand("GetCategoryId", MainClass.connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Categoryname", categoryname);
                      
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        categoryId = Convert.ToInt32(result);
                    }
                    MainClass.connection.Close();
                    return categoryId;
                    
                }
                  
            }
            catch (SqlException ex) { 
                MessageBox.Show(ex.Message);
            }
            return 0;

        }



        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            foreach(Control item in productPanel.Controls)
            {
                if (item is ucProduct pro)
                { 
                    pro.Visible = pro.Name.ToLower().Contains(txtSearch.Text.Trim().ToLower());
                }
            }
        }

        private void guna2TileButton5_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();
            payment = false;
            lbltotal.Text ="0";
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            orderType = "Delivery";
            dininbtn.FillColor = Color.LightSeaGreen;
            deliverybtn.FillColor = Color.DarkOliveGreen;
            tackawaybtn.FillColor = Color.LightSeaGreen;
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            orderType = "Take Away";
            dininbtn.FillColor = Color.LightSeaGreen;
            deliverybtn.FillColor = Color.LightSeaGreen;
            tackawaybtn.FillColor = Color.DarkOliveGreen;
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            orderType = "Din In";
            dininbtn.FillColor = Color.DarkOliveGreen;
            deliverybtn.FillColor = Color.LightSeaGreen; 
            tackawaybtn.FillColor = Color.LightSeaGreen;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton2_Click_1(object sender, EventArgs e)
        {
             
                if (this.orderType=="")
                { 
                    warningmessage.Show("Please choose order type");
                    return;
                }
                else if (this.payment == false)
                {
                warningmessage.Show("Please pay the money");
                return;
                }
               else
                { 
                    PlaceOrder(User.username, this.orderType, guna2DataGridView1);
                    
                }
              
           
        }


        public void PlaceOrder(string username, string ordertype, DataGridView dataGridView)
        {
                 
           
                // Create a DataTable to hold the product details
                DataTable productDetails = new DataTable();
                productDetails.Columns.Add("productid", typeof(int));
                productDetails.Columns.Add("quantity", typeof(int));

                // Loop through the DataGridView rows and add them to the DataTable
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder
                    int productId = Convert.ToInt32(row.Cells["dgvid"].Value);
                    int quantity = Convert.ToInt32(row.Cells["dgvqty"].Value);
                    productDetails.Rows.Add(productId, quantity);
                }

            try
            {
                MainClass.connection.Close();
                MainClass.connection.Open();
                using (SqlCommand command = new SqlCommand("PlaceOrder", MainClass.connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@ordertype", ordertype);

                    // Add the DataTable as a parameter
                    SqlParameter productDetailsParam = command.Parameters.AddWithValue("@productDetails", productDetails);
                    productDetailsParam.SqlDbType = SqlDbType.Structured; // Specify that it's a structured type

                    int orderId = (int)command.ExecuteScalar();  

                    infomessage.Show($"Order placed successfully! Order ID: {orderId}");
                    orderType = "";
                }
                MainClass.connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {

        }

        private void paybtn_Click(object sender, EventArgs e)
        {
             this.payment= true;
            infomessage.Show("payment successful!");
        }
    }
}
