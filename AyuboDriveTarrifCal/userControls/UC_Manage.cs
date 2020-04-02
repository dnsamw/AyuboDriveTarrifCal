using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AyuboDriveTarrifCal.userControls
{
    public partial class UC_Manage : UserControl
    {

        SqlConnection sqlConnection;
        
        public UC_Manage()
        {
            InitializeComponent();
            PopulateListBox();
            fetchDriverInfo();
            //PackageDetails();
            populatePackages();
        }

        private void listBoxVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            clickChange();
        }

        //fatching driver info to textBoxes from the db
        private void fetchDriverInfo()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AyuboConnectionString"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                string query = "select * from AyuboDriverFees";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    txtBoxDriverPerDayFee.Text = dataTable.Rows[0][1].ToString();
                    txtBoxDriverPerKmFee.Text = dataTable.Rows[0][2].ToString();
                    txtBoxExtraHour.Text = dataTable.Rows[0][3].ToString();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        //populating the vehicle types listBox from the db
        private void PopulateListBox()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AyuboConnectionString"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                string query = "select * from AyuboVehicles";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    // listBoxVehicles.DataSource = dataTable;
                    listBoxVehicles.DisplayMember = "vehicle_type";
                    listBoxVehicles.ValueMember = "Id";
                    //cmbBoxVehicleType.ValueMember = "Id";
                    listBoxVehicles.DataSource = dataTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }
       
        //vehicle types listBox ClickChange Event
        private void clickChange()
        {
            listBoxVehicles.Select();
            string query = "select * from AyuboVehicles where Id = @SelectedId";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@SelectedId", listBoxVehicles.SelectedValue);
                    DataTable dataTable2 = new DataTable();

                    sqlDataAdapter.Fill(dataTable2);
                    txtBoxType.Text = dataTable2.Rows[0][1].ToString();
                    txtBoxPerDay.Text = dataTable2.Rows[0][2].ToString();
                    txtBoxPerKm.Text = dataTable2.Rows[0][3].ToString();
                }
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }
        }

        //add vehicle types to the db
        private void btnAddType_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "insert into AyuboVehicles(vehicle_type,per_day,per_km) values (@VehicleType,@PerDay,@PerKm)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@VehicleType", txtBoxType.Text);
                sqlCommand.Parameters.AddWithValue("@PerDay", txtBoxPerDay.Text);
                sqlCommand.Parameters.AddWithValue("@PerKm", txtBoxPerKm.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            finally
            {
                sqlConnection.Close();
                PopulateListBox();
            }
        }

        //delete vehicle types from the db
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "delete from AyuboVehicles where Id = @SelectedId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@SelectedId", listBoxVehicles.SelectedValue);
                sqlCommand.ExecuteScalar();

                MessageBox.Show(listBoxVehicles.Text + " is Deleted! ");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            finally
            {
                sqlConnection.Close();
                PopulateListBox();
            }

        }

        private void PackageDetails()
        {
            string query = "select * from AyuboPackages where pkg_name = @SelectedPackage";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@SelectedPackage", cBoxPkgName.Text);
                    DataTable dataTable2 = new DataTable();
                    sqlDataAdapter.Fill(dataTable2);
                    txtBoxMaxKm.Text = dataTable2.Rows[0][2].ToString();
                    txtBoxMaxHrs.Text = dataTable2.Rows[0][3].ToString();
                    txtBoxXtrHrs.Text = dataTable2.Rows[0][4].ToString();
                    txtBoxXtrKm.Text = dataTable2.Rows[0][5].ToString();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void populatePackages()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AyuboConnectionString"].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
                string query = "select * from AyuboPackages";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    cBoxPkgName.DisplayMember = "pkg_name";
                    cBoxPkgName.DataSource = dataTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void cBoxPkgName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackageDetails();
        }

        //Update package info
        private void btnUpdatePkgInfo_Click(object sender, EventArgs e)
        {
            string query = "update AyuboPackages set pkg_max_km = @PkgMaxKm," +
                "pkg_max_hrs = @PkgMaxHrs, pkg_extra_hr_charge = @PkgExtraHrCharge," +
                "pkg_extra_km_charge = @PkgExtraKmCharge where pkg_name = @PkgName ";
            try
            {
                //string query = "insert into AyuboVehicles(vehicle_type,per_day,per_km) values (@VehicleType,@PerDay,@PerKm)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@PkgName", cBoxPkgName.Text);//string
                sqlCommand.Parameters.AddWithValue("@PkgMaxKm", Int32.Parse(txtBoxMaxKm.Text));//int
                sqlCommand.Parameters.AddWithValue("@PkgMaxHrs", Int32.Parse(txtBoxMaxHrs.Text));//int
                sqlCommand.Parameters.AddWithValue("@PkgExtraHrCharge", Int32.Parse(txtBoxXtrHrs.Text));//int
                sqlCommand.Parameters.AddWithValue("@PkgExtraKmCharge", Int32.Parse(txtBoxXtrKm.Text));//int
                sqlCommand.ExecuteScalar();

                MessageBox.Show("Package rates successfully updated!");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        
        //Update driver charges info
        private void btnUpdateDriverInfo_Click(object sender, EventArgs e)
        {
            string query = "update AyuboDriverFees set per_day_charge = @PerDayCharge," +
                "per_km_charge = @PerKmCharge, extra_hour_charge = @PerHourCharge where Id = @dId ";
            try
            {
                //string query = "insert into AyuboVehicles(vehicle_type,per_day,per_km) values (@VehicleType,@PerDay,@PerKm)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@dId", 1);//int

                sqlCommand.Parameters.AddWithValue("@PerDayCharge", Int32.Parse(txtBoxDriverPerDayFee.Text));//int
                sqlCommand.Parameters.AddWithValue("@PerKmCharge", Int32.Parse(txtBoxDriverPerKmFee.Text));//int
                sqlCommand.Parameters.AddWithValue("@PerHourCharge", Int32.Parse(txtBoxExtraHour.Text));//int
                sqlCommand.ExecuteScalar();

                MessageBox.Show("Driver Charges successfully updated!");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        //Update vehicle type info
        private void btnUpdateVehicleInfo_Click(object sender, EventArgs e)
        {
            string query = "update AyuboVehicle" +
                "s set per_day = @PerDay," +
               "per_km = @PerKm where vehicle_type = @VehicleType ";
            try
            {
                //string query = "insert into AyuboVehicles(vehicle_type,per_day,per_km) values (@VehicleType,@PerDay,@PerKm)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@VehicleType", txtBoxType.Text);//string
                sqlCommand.Parameters.AddWithValue("@PerDay", Int32.Parse(txtBoxPerDay.Text));//int
                sqlCommand.Parameters.AddWithValue("@PerKm", Int32.Parse(txtBoxPerKm.Text));//int
                sqlCommand.ExecuteScalar();

                MessageBox.Show("Vehicle type info successfully updated!");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        //TODO : Implement Add package and delete package.
    }
    }

