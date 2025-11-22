using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ukkFauzanRizaldy
{
    public class ProdukSearch
    {
        public static DataTable GetProducts(string keyword = "")
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query =
                    @"SELECT id_produk, nama_produk, harga, stok, deskripsi, gambar_produk, tanggal
                  FROM dbo.products";

                if (!string.IsNullOrEmpty(keyword))
                {
                    query += " WHERE nama_produk LIKE @keyword";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(keyword))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public static DataRow GetProductDetail(int id)
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                string query =
                    @"SELECT * FROM dbo.products WHERE id_produk=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];

                return null;
            }
        }
    }
}
