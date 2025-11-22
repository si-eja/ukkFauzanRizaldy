using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            }
            else
            {
                labelNama.Text = "";
                labelKontak.Text = "";

                label7.Visible = false;
                label8.Visible = false;

                btnLogout.Visible = false;
                btnLogin.Visible = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts(tbCari.Text.Trim());
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
    }
}
