using Microsoft.Data.SqlClient;
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
    public partial class Produk : Form
    {
        string imagePath = "";
        int selectedId = -1;
        int userId;

        public Produk()
        {
            InitializeComponent();
        }
        public Produk(int uid)
        {
            InitializeComponent();
            userId = uid;
            LoadData();
        }

        private void LoadData(string keyword = "")
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query = "SELECT id_produk, nama_produk, harga, stok FROM dbo.produk";

                if (!string.IsNullOrEmpty(keyword))
                    query += " WHERE nama_produk LIKE @keyword";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(keyword))
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridViewProduk.DataSource = dt;
                dataGridViewProduk.Columns["id_produk"].Visible = false;
            }
        }
        private bool IsInputEmpty()
        {
            return string.IsNullOrWhiteSpace(tbNamaProduk.Text) ||
                   string.IsNullOrWhiteSpace(tbHarga.Text) ||
                   string.IsNullOrWhiteSpace(tbStok.Text) ||
                   string.IsNullOrWhiteSpace(tbDeskripsi.Text);
        }
        private void ClearForm()
        {
            tbNamaProduk.Clear();
            tbHarga.Clear();
            tbStok.Clear();
            tbDeskripsi.Clear();
            pbGambar.Image = null;
            imagePath = "";
            selectedId = -1;
        }
        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNamaProduk.Text) ||
                string.IsNullOrWhiteSpace(tbHarga.Text) ||
                string.IsNullOrWhiteSpace(tbStok.Text) ||
                string.IsNullOrWhiteSpace(tbDeskripsi.Text))
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string getId = "SELECT ISNULL(MAX(id_produk), 0) + 1 FROM produk";
                SqlCommand getIdCmd = new SqlCommand(getId, conn);
                int newId = Convert.ToInt32(getIdCmd.ExecuteScalar());

                string query =
                    @"INSERT INTO produk (id_produk, id_user, nama_produk, harga, stok, deskripsi, gambar_produk, tanggal)
              VALUES (@id, @iduser, @nama, @harga, @stok, @desk, @gambar, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", newId);
                cmd.Parameters.AddWithValue("@iduser", userId);
                cmd.Parameters.AddWithValue("@nama", tbNamaProduk.Text);
                cmd.Parameters.AddWithValue("@harga", tbHarga.Text);
                cmd.Parameters.AddWithValue("@stok", tbStok.Text);
                cmd.Parameters.AddWithValue("@desk", tbDeskripsi.Text);

                if (imagePath != "")
                    cmd.Parameters.Add("@gambar", SqlDbType.VarBinary).Value = File.ReadAllBytes(imagePath);
                else
                    cmd.Parameters.Add("@gambar", SqlDbType.VarBinary).Value = DBNull.Value;

                cmd.ExecuteNonQuery();

                MessageBox.Show("Produk berhasil ditambahkan!");

                ClearForm();
                LoadData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih produk terlebih dahulu!");
                return;
            }

            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query =
                    @"UPDATE produk SET
                    nama_produk=@nama,
                    harga=@harga,
                    stok=@stok,
                    deskripsi=@desk,
                    gambar_produk=@gambar
                    WHERE id_produk=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.Parameters.AddWithValue("@nama", tbNamaProduk.Text);
                cmd.Parameters.AddWithValue("@harga", tbHarga.Text);
                cmd.Parameters.AddWithValue("@stok", tbStok.Text);
                cmd.Parameters.AddWithValue("@desk", tbDeskripsi.Text);

                if (imagePath != "")
                    cmd.Parameters.Add("@gambar", SqlDbType.VarBinary).Value = File.ReadAllBytes(imagePath);
                else
                    cmd.Parameters.Add("@gambar", SqlDbType.VarBinary).Value = DBNull.Value;

                cmd.ExecuteNonQuery();

                MessageBox.Show("Produk berhasil diupdate!");

                ClearForm();
                LoadData();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih produk terlebih dahulu!");
                return;
            }

            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM produk WHERE id_produk=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Produk berhasil dihapus!");

                ClearForm();
                LoadData();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePath = ofd.FileName;
                pbGambar.Image = Image.FromFile(imagePath);
            }
        }

        private void pbGambar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewProduk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            selectedId = Convert.ToInt32(dataGridViewProduk.Rows[e.RowIndex].Cells["id_produk"].Value);

            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM produk WHERE id_produk=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tbNamaProduk.Text = dr["nama_produk"].ToString();
                    tbHarga.Text = dr["harga"].ToString();
                    tbStok.Text = dr["stok"].ToString();
                    tbDeskripsi.Text = dr["deskripsi"].ToString();

                    if (dr["gambar_produk"] != DBNull.Value)
                    {
                        byte[] img = (byte[])dr["gambar_produk"];
                        MemoryStream ms = new MemoryStream(img);
                        pbGambar.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        pbGambar.Image = null;
                    }

                    imagePath = ""; // reset supaya edit tidak memaksa upload
                }
            }
        }

        private void tbCari_TextChanged(object sender, EventArgs e)
        {
            LoadData(tbCari.Text.Trim());
        }

        private void dataGridViewProduk_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
