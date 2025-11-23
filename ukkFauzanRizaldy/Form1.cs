using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ukkFauzanRizaldy
{
    public partial class Form1 : Form
    {
        public static bool isLoggedIn = false;
        public static string namaUser = "";
        public static string kontakUser = "";
        public static int userId;

        public Form1()
        {
            InitializeComponent();
        }
        public void UpdateUI()
        {
            if (isLoggedIn)
            {
                labelNama.Text = namaUser;
                labelKontak.Text = kontakUser;

                label7.Visible = true;
                label8.Visible = true;

                btnLogout.Visible = true;
                btnLogin.Visible = false;
                btnKelola.Visible = true;
            }
            else
            {
                labelNama.Text = "";
                labelKontak.Text = "";

                label7.Visible = false;
                label8.Visible = false;

                btnLogout.Visible = false;
                btnLogin.Visible = true;
                btnKelola.Visible = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts(textBox1.Text.Trim());
            LoadProducts();
            UpdateUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isLoggedIn = false;
            namaUser = "";
            kontakUser = "";

            UpdateUI();

            MessageBox.Show("Logout berhasil!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void LoadProducts(string keyword = "")
        {
            DataTable dt = ProdukSearch.GetProducts(keyword);

            dataGridViewProduk.DataSource = dt;

            // tampilkan hanya kolom tertentu
            dataGridViewProduk.Columns["id_produk"].Visible = false;
            dataGridViewProduk.Columns["deskripsi"].Visible = false;
            dataGridViewProduk.Columns["gambar_produk"].Visible = false;
            dataGridViewProduk.Columns["tanggal"].Visible = false;

            dataGridViewProduk.Columns["nama_produk"].HeaderText = "Nama Produk";
            dataGridViewProduk.Columns["harga"].HeaderText = "Harga";
            dataGridViewProduk.Columns["stok"].HeaderText = "Stok";
        }
        public static DataTable GetProducts(string keyword)
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query =
                    "SELECT id_produk, nama_produk, harga, stok, deskripsi, gambar_produk, tanggal FROM produk WHERE id_user=@id";

                if (!string.IsNullOrEmpty(keyword))
                    query += " AND nama_produk LIKE @keyword";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Form1.userId);

                if (!string.IsNullOrEmpty(keyword))
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        private void dataGridViewProduk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dataGridViewProduk.Rows[e.RowIndex].Cells["id_produk"].Value);

            DataRow row = ProdukSearch.GetProductDetail(id);

            if (row != null)
            {
                tbNamaProduk.Text = row["nama_produk"].ToString();
                tbHarga.Text = row["harga"].ToString();
                tbStok.Text = row["stok"].ToString();
                tbDeskripsi.Text = row["deskripsi"].ToString();

                dtProduk.Value = Convert.ToDateTime(row["tanggal"]);

                // jika gambar ada
                if (row["gambar_produk"] != DBNull.Value)
                {
                    byte[] imgData = (byte[])row["gambar_produk"];
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        pbGambar.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pbGambar.Image = null;
                }
            }
        }

        private void dtProduk_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            Produk p = new Produk(Form1.userId);
            p.Show();
        }

        private void dataGridViewProduk_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadProducts(textBox1.Text.Trim());
        }
    }
}
