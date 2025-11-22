using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ukkFauzanRizaldy
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnMasuk_Click(object sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Username dan Password wajib diisi!");
                return;
            }

            using (SqlConnection conn = Connection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT nama, kontak FROM dbo.users WHERE username=@user AND password=@pass";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Form1.namaUser = reader["nama"].ToString();
                        Form1.kontakUser = reader["kontak"].ToString();
                        Form1.isLoggedIn = true;

                        MessageBox.Show("Login berhasil!");

                        this.Hide();
                        Application.OpenForms["Form1"].Show();
                        ((Form1)Application.OpenForms["Form1"]).UpdateUI();
                    }
                    else
                    {
                        MessageBox.Show("Username atau password salah!");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
