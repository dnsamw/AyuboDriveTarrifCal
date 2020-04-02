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
    public partial class UC_HireRent : UserControl
    {
        SqlConnection sqlConnection;
        public UC_HireRent()
        {
            InitializeComponent();
            txtBoxRentDate.Text = DateTime.Now.ToShortDateString();
            populateData();
            radioButton1.Checked = true;
            TotalCal();
        }

        //TODO: pupulate comboBox from the db
        private void populateData()
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
                    //cmbBoxVehicleType.DataSource = dataTable;
                    cmbBoxVehicleType.DisplayMember = "vehicle_type";
                    //cmbBoxVehicleType.ValueMember = "Id";
                    cmbBoxVehicleType.DataSource = dataTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
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
                    cBoxPackage.DisplayMember = "pkg_name";
                    cBoxPackage.DataSource = dataTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        //TODO: calculate Fee
        private void FeeCal()
        {
            string query = "select * from AyuboVehicles av, AyuboDriverFees where av.vehicle_type = @SelectedType";
            //string query = "select per_day from AyuboVehicles where vehicle_type = @SelectedType";
            

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("SelectedType", cmbBoxVehicleType.Text);
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    lblPerDayCharge.Text = dataTable.Rows[0][2].ToString();
                    cBoxDriver.Text = dataTable.Rows[0][5].ToString()+" Rs/day";


                    bool cbIsChecked;
                    int df = Int32.Parse(dataTable.Rows[0][5].ToString());

                    if (cBoxDriver.Checked)
                    {
                        cbIsChecked = true;
                        //MessageBox.Show("true");
                        lblDriverIncl.Text = "Driver Included : Yes";
                    }
                    else
                    {
                        cbIsChecked = false;
                        //MessageBox.Show("false");
                        lblDriverIncl.Text = "Driver Included : No";
                    }

                    int holdPerDay = Int32.Parse(dataTable.Rows[0][2].ToString());
                    Vehicle vehicle = new Vehicle(cmbBoxVehicleType.Text, holdPerDay, cbIsChecked,df);
                    lblPerDayCharge.Text = vehicle.PerDay().ToString();
                    lblPerWeekCharge.Text = vehicle.PerWeek().ToString();
                    lblPerMonthCharge.Text = vehicle.PerMonth().ToString();
                }
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }
        }

        //package details clickChange
       
        private void PackageDetails()
        {
            //string query = "select * from AyuboPackages where pkg_name = @SelectedPackage";
            string query = "select * from AyuboPackages av,AyuboDriverFees where av.pkg_name = @SelectedPackage";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@SelectedPackage", cBoxPackage.Text);
                    DataTable dataTable2 = new DataTable();
                    sqlDataAdapter.Fill(dataTable2);
                    lblKmLimit.Text = dataTable2.Rows[0][2].ToString();
                    lblTimeLimit.Text = dataTable2.Rows[0][3].ToString();
                    //dc = dataTable2.Rows[0][3].ToString();

                   string dc = dataTable2.Rows[0][4].ToString();//package vehicle charge



                    //bool cbIsChecked;

                    //if (cBoxDriver.Checked)
                    //{
                    //    cbIsChecked = true;
                    //    //MessageBox.Show("true");
                    //    lblDriverIncl.Text = "Driver Included : Yes";
                    //}
                    //else
                    //{
                    //    cbIsChecked = false;
                    //    //MessageBox.Show("false");
                    //    lblDriverIncl.Text = "Driver Included : No";
                    //}

                    //int holdPerDay = Int32.Parse(dataTable2.Rows[0][0].ToString());
                    //Vehicle vehicle = new Vehicle(cmbBoxVehicleType.Text, holdPerDay, cbIsChecked);
                    //lblPerDayCharge.Text = vehicle.PerDay();
                    //lblPerWeekCharge.Text = vehicle.PerWeek();
                    //lblPerMonthCharge.Text = vehicle.PerMonth();
                }
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }
        }

        //calculate tootal befor saving job
        private void TotalCal()
        {
            string vehicleType = cmbBoxVehicleType.Text;

            if (radioButton1.Checked)//Rent - No package
            {
                bool IsParsed = int.TryParse(txtBoxDays.Text,out int Days);
                if (IsParsed)
                {
                    Vehicle vehicle = new Vehicle(Int32.Parse(lblPerDayCharge.Text), Days);
                    if (cBoxDriver.Checked)
                    {
                        //MessageBox.Show("Rent Radio Button is checked! : Driver Included");
                        //IsDiverIncluded = true;
                        
                        lblTotal.Text = vehicle.StandardRent().ToString();

                    }
                    else
                    {
                        //MessageBox.Show("Rent Radio Button is checked! : Driver Not-Included");
                        lblTotal.Text = vehicle.StandardRent().ToString();
                        //IsDiverIncluded = false;
                    }
                }
                else
                {
                    MessageBox.Show("Days Cannot be empty!");
                }  
            }
            else if (radioButton2.Checked)//Hire - No Days
            {
                
                if (Int32.Parse(lblKmLimit.Text)>100)
                {
                    double x = ((Convert.ToDouble(lblPerDayCharge.Text) / 24) * Convert.ToDouble(lblTimeLimit.Text) * Convert.ToDouble(lblKmLimit.Text) / 100);
                    lblTotal.Text = (Math.Round(x, 2)).ToString();
                }
                else
                {
                    double x = ((Convert.ToDouble(lblPerDayCharge.Text) / 24) * Convert.ToDouble(lblTimeLimit.Text));
                    lblTotal.Text = (Math.Round(x, 2)).ToString();
                }
            }

        }


        //Radio button events
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxDays.Enabled = true;
            txtBoxDays.Text = "1";
            cBoxDriver.Enabled = true;
            cBoxDriver.Checked = false;

            cBoxPackage.Enabled = false;

            lblPackage.Text = "";
            cBoxPackage.Text = "";
            lblTotal.Text = "00000";

            label15.ForeColor = System.Drawing.Color.Gray;
            label16.ForeColor = System.Drawing.Color.Gray;
            label17.ForeColor = System.Drawing.Color.Gray;
            label13.ForeColor = System.Drawing.Color.Black;

            lblPkgType.Text = "Package Type : Rent";
            lblDriverIncl.Text = "Driver Included : No";
            TotalCal();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxDays.Enabled = false;
            cBoxDriver.Enabled = false;
            cBoxDriver.Checked = true;

            cBoxPackage.Enabled = true;

            lblKmRead.Text = "";
            lblTotal.Text = "00000";

            //txtBoxDays.Clear();
            label13.ForeColor = System.Drawing.Color.Gray;
            label15.ForeColor = System.Drawing.Color.Black;
            label16.ForeColor = System.Drawing.Color.Black;
            label17.ForeColor = System.Drawing.Color.Black;

            lblKmRead.Text = txtBoxStartKmR.Text;
            lblPkgType.Text = "Package Type : Hire";
            lblDriverIncl.Text = "Driver Included : Yes";

            populatePackages();
            TotalCal();
        }

        //Text box events
        private void txtBoxDays_TextChanged(object sender, EventArgs e)
        {
            lblTDays.Text = txtBoxDays.Text;
            TotalCal();

        }

        private void txtBoxStartKmR_TextChanged(object sender, EventArgs e)
        {
            lblKmRead.Text = txtBoxStartKmR.Text;
        }

        private void txtBoxStartKmR_TextChanged_1(object sender, EventArgs e)
        {
                lblKmRead.Text = txtBoxStartKmR.Text;
        }

        private void txtBoxStartKmR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)&&!char.IsDigit(e.KeyChar)&&(e.KeyChar !='.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxCustomerContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtBoxDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        //Combo box events
        private void cBoxPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackageDetails();
            lblPackage.Text = cBoxPackage.Text;
            TotalCal();
        }

        private void cmbBoxVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TotalCal();
        }

        private void cmbBoxVehicleType_SelectedValueChanged(object sender, EventArgs e)
        {
            FeeCal();
            txtBoxStartKmR.Clear();
        }

        private void cBoxDriver_CheckedChanged(object sender, EventArgs e)
        {
            FeeCal();
            TotalCal();

        }

        //button click events
        //TODO: check if phone number has 10 digits.
        private void btnSaveJob_Click(object sender, EventArgs e)
        {
            if (textBoxCustomerName.Text == "" || textBoxCustomerContactNo.Text == "" || txtBoxStartKmR.Text == "")
            {
                MessageBox.Show("Empty fields : Please Complete all the required fields before saving the job.");
            }
            else
            {
                byte IsDriverIn;
                try
                {
                    string query = "insert into AyuboJobs(customer_name,contact_no,rent_date,rent_period,start_km_read,vehicle_type,is_driver_incl,package_name) values " +
                        "(@CustomerName,@ContactNo,@RentDate,@RentPeriod,@StartKmRead,@VehicleType,@IsDriverIncl,@PackageName)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@CustomerName", textBoxCustomerName.Text);
                    sqlCommand.Parameters.AddWithValue("@ContactNo", Int32.Parse(textBoxCustomerContactNo.Text));
                    sqlCommand.Parameters.AddWithValue("@RentDate", DateTime.Now.ToShortDateString());
                    sqlCommand.Parameters.AddWithValue("@RentPeriod", Int32.Parse(txtBoxDays.Text));
                    sqlCommand.Parameters.AddWithValue("@StartKmRead", Int32.Parse(txtBoxStartKmR.Text));
                    sqlCommand.Parameters.AddWithValue("@VehicleType", cmbBoxVehicleType.Text);
                    if (cBoxDriver.Checked)
                    {
                        IsDriverIn = 1;
                        sqlCommand.Parameters.AddWithValue("@IsDriverIncl", IsDriverIn);//byte data type
                    }
                    else
                    {
                        IsDriverIn = 0;
                        sqlCommand.Parameters.AddWithValue("@IsDriverIncl", IsDriverIn);//byte data type
                    }
                    sqlCommand.Parameters.AddWithValue("@PackageName", cBoxPackage.Text);
                    sqlCommand.ExecuteScalar();

                    MessageBox.Show("Job Successfully Saved to database!");
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString());
                }
            }
            //TotalCal();
        }
    }
}
