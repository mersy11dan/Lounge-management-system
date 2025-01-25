using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoungeManagementApplication
{
    public  static class User
    {
        public  static string username { get; set; }
        public  static string password { get; set; }
        public   static string email{ get; set; }
        public static string phone { get; set; }

    }
        public  class Product
    {
        public int pid { get; set; }
        public int catid { get; set; }
        public int lgid { get; set; }
        public string pname { get; set; }
        public decimal pprice { get; set; }
        public byte[] pimage { get; set; }

        public Image GetImage()
        {
            if (pimage != null && pimage.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(pimage))
                {
                    return Image.FromStream(ms);
                }
            }
            return null;
        }

    }
    public class Lounge
    {
        public int lid { get; set; }
        public string lname { get; set; }
        public string location { get; set; }
        public byte[] limage { get; set; }

        public Image GetImage()
        {
            if (limage != null && limage.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(limage))
                {
                    return Image.FromStream(ms);
                }
            }
            return null;
        }

    }
    internal class MainClass
    {
        public static List<Product> products = new List<Product>();
        public static List<Lounge> lounges = new List<Lounge>();
        public static SqlConnection connection = new SqlConnection("Server=DESKTOP-2ONSC8K;Database=LMS_database;Trusted_Connection=True;");
                       
        public static void productsData()
         {
           
                try
                {
                 connection.Open();

                    using (SqlCommand command = new SqlCommand("GetProducts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    pid = (int)reader["productid"],
                                    catid = (int)reader["categoryid"],
                                    lgid = (int)reader["loungeid"],
                                    pname = reader["productname"].ToString(),
                                    pprice = (decimal)reader["productprice"],
                                    pimage = (byte[])reader["productimage"],
                                };

                                products.Add(product);
                            }
                        }
                        
                    }
                    connection.Close();
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }  
       
        public static void loungeData()
        {

             
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetLounges", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Lounge lounge = new Lounge
                                {
                                    lid = (int)reader["loungeid"],
                                    lname = reader["loungename"].ToString(),
                                    location = reader["location"].ToString(),
                                    limage = (byte[])reader["loungeimage"],
                                };

                                lounges.Add(lounge);
                            }
                        }
                       
                    }
                connection.Close();
            }
            catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

    }

