using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace AyuboDriveTarrifCal.userControls
{
    public partial class UC_CompletedTrips : UserControl
    {
       SqlConnection sqlConnection;

        public UC_CompletedTrips()
        {
            InitializeComponent();
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AyuboConnectionString"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                string query = "select * from AyuboJobs";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    foreach (DataRow item in dataTable.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["Id"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["customer_name"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["contact_no"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["rent_date"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["rent_period"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["start_km_read"].ToString();
                        dataGridView1.Rows[n].Cells[6].Value = item["vehicle_type"].ToString();
                        dataGridView1.Rows[n].Cells[7].Value = item["is_driver_incl"].ToString();
                        dataGridView1.Rows[n].Cells[8].Value = item["package_name"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
