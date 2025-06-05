using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Project_HMS;

namespace Project_HMS
{
    public partial class Login : Form
    {
        private DataAccess Da { get; set; }
        public Login()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.lblWrong.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userId = this.txtUserid.Text.Trim();
            string password = this.txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both User ID and Password.");
                return;
            }

            string sql = "SELECT * FROM login WHERE UId = @uid AND Password = @password";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, this.Da.Sqlcon);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@password", password);

                this.Da.Sqlcon.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Login successful!");

                    string role = dt.Rows[0]["Role"].ToString();

                    if (role == "Owner")
                    {
                        // OwnerDashboard od = new OwnerDashboard();
                        // od.Show();
                        MessageBox.Show("Welcome, Owner!");
                    }
                    else
                    {
                        ManagerDashboard md = new ManagerDashboard();
                        md.Show();
                    }

                    this.Hide();
                }
                else
                {
                    this.lblWrong.Visible = true;
                    MessageBox.Show("Login failed. Invalid credentials.");
                    this.txtUserid.Clear();
                    this.txtPassword.Clear();
                    this.txtUserid.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (this.Da.Sqlcon.State == ConnectionState.Open)
                    this.Da.Sqlcon.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtUserid.Clear();
            this.txtPassword.Clear();
            this.lblWrong.Visible = false;
        }
    }
}
