using ClosedXML.Excel;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace HMS.Controllers
{
    public class PatientController : Controller
    {
        private IConfiguration _configuration;
        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region selectall
        public IActionResult PatientList()
        {

            ViewBag.BreadcrumbTitle = "Patients";
            ViewBag.BreadcrumbPath = new string[] { "Home", "PatientList" };
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Patients_GetAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
            
        }

        #endregion

        #region Add / Edit (POST)
        [HttpPost]
        public IActionResult PatientAddEdit(PatientModel patientModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = _configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            if (patientModel.PatientID == 0) // ✅ int default = 0
                            {
                                command.CommandText = "PR_Patient_Insert";
                                command.Parameters.AddWithValue("@UserID", patientModel.UserID);
                                TempData["SuccessMessage"] = "Patient added successfully!";
                            }
                            else
                            {
                                command.CommandText = "PR_Patient_Update";
                                command.Parameters.AddWithValue("@PatientID", patientModel.PatientID);
                                command.Parameters.AddWithValue("@UserID", patientModel.UserID);
                                TempData["UpdateSuccessMessage"] = "Patient updated successfully!";
                            }

                            command.Parameters.AddWithValue("@Name", patientModel.Name);
                            command.Parameters.AddWithValue("@DateOfBirth", patientModel.DateOfBirth);
                            command.Parameters.AddWithValue("@Gender", patientModel.Gender);
                            command.Parameters.AddWithValue("@Email", patientModel.Email);
                            command.Parameters.AddWithValue("@Phone", patientModel.Phone);
                            command.Parameters.AddWithValue("@Address", patientModel.Address);
                            command.Parameters.AddWithValue("@City", patientModel.City);
                            command.Parameters.AddWithValue("@State", patientModel.State);

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("PatientList");
                }

                // Reload dropdowns if validation fails
                LoadUserDropdown();
                LoadCityDropdown();
                LoadStateDropdown();
                return View("PatientAddEdit", patientModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                ViewBag.ErrorMessage = "Something went wrong while saving.";
                LoadUserDropdown();
                LoadCityDropdown();
                LoadStateDropdown();
                return View("PatientAddEdit", patientModel);
            }
        }

        [HttpGet]
        public IActionResult PatientAddEdit()
        {
            LoadUserDropdown();
            LoadCityDropdown();
            LoadStateDropdown();
            return View(new PatientModel());
        }
        #endregion

        #region Edit (GET)
        public IActionResult PatientEdit(int PatientID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            PatientModel patientModel = new PatientModel();

            // Load dropdown before returning view
            LoadUserDropdown();
            LoadCityDropdown();
            LoadStateDropdown();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Patient_ByID";
                    command.Parameters.AddWithValue("@PatientID", PatientID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        if (table.Rows.Count > 0)
                        {
                            DataRow dr = table.Rows[0];
                            patientModel.PatientID = Convert.ToInt32(dr["PatientID"]);
                            patientModel.Name = dr["Name"].ToString();
                            patientModel.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                            patientModel.Gender = dr["Gender"].ToString();
                            patientModel.Email = dr["Email"].ToString();
                            patientModel.Phone = dr["Phone"].ToString();
                            patientModel.Address = dr["Address"].ToString();
                            patientModel.City = dr["City"].ToString();
                            patientModel.State = dr["State"].ToString();
                            patientModel.UserID = Convert.ToInt32(dr["UserID"]);
                        }
                    }
                }
            }

            return View("PatientAddEdit", patientModel);
        }
        #endregion

        #region Helper
        private void LoadUserDropdown()
        {
            string conn = _configuration.GetConnectionString("ConnectionString");
            List<UsersModel> userList = new List<UsersModel>();

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM Users", connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(new UsersModel
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            UserName = reader["UserName"].ToString()
                        });
                    }
                }
            }

            ViewBag.Users = new SelectList(userList, "UserID", "UserName");
        }
        private void LoadCityDropdown()
        {
            // You can replace this with DB query if you have a City table
            var cities = new List<string> { "Ahmedabad", "Surat", "Mumbai", "Delhi", "Bangalore" };
            ViewBag.Cities = new SelectList(cities);
        }

        private void LoadStateDropdown()
        {
            // You can replace this with DB query if you have a State table
            var states = new List<string> { "Gujarat", "Maharashtra", "Karnataka", "Rajasthan", "Punjab" };
            ViewBag.States = new SelectList(states);
        }

        private void LoadDropdowns()
        {
            LoadUserDropdown();
            LoadCityDropdown();
            LoadStateDropdown();
        }
        #endregion


        #region delete
        public IActionResult DeletePatient(int PatientID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Patient_Delete";
            command.Parameters.AddWithValue("@PatientID", SqlDbType.Int).Value = PatientID;

            command.ExecuteNonQuery();
            TempData["DeleteSuccessMessage"] = "Patient deleted successfully!";

            return RedirectToAction("PatientList");
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Patients_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cell(1, 1).Value = "PatientID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Email";
                worksheet.Cell(1, 6).Value = "Phone";
                worksheet.Cell(1, 7).Value = "Address";
                worksheet.Cell(1, 8).Value = "City";
                worksheet.Cell(1, 9).Value = "State";
                worksheet.Cell(1, 10).Value = "IsActive";



                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["PatientID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["Name"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["DateOfBirth"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["Gender"]?.ToString() ?? "";
                    worksheet.Cell(row, 5).Value = item["Email"]?.ToString() ?? "";
                    worksheet.Cell(row, 6).Value = item["Phone"]?.ToString() ?? "";
                    worksheet.Cell(row, 7).Value = item["Address"]?.ToString() ?? "";
                    worksheet.Cell(row, 8).Value = item["City"]?.ToString() ?? "";
                    worksheet.Cell(row, 9).Value = item["State"]?.ToString() ?? "";
                    worksheet.Cell(row, 10).Value = item["IsActive"]?.ToString() ?? "";

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                // ✅ DO NOT dispose stream before returning
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"Users-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region delete all
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSelectedPatients(List<int> selectedPatientIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedPatientIds != null && selectedPatientIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedPatientIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_Patient_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PatientID", id);
                            cmd.ExecuteNonQuery();
                            successCount++;
                        }
                        catch (SqlException ex)
                        {
                            // Check if it's a foreign key conflict
                            if (ex.Number == 547) // FK violation error number
                            {
                                failCount++;
                                // Optional: log it or collect failed IDs
                            }
                            else
                            {
                                throw; // rethrow unknown errors
                            }
                        }
                    }
                }
            }

            if (successCount > 0)
                TempData["DeleteSuccessMessage"] = $"{successCount} Patient(s) deleted successfully.";

            if (failCount > 0)
                TempData["ErrorMessage"] = $"{failCount} Patient(s) could not be deleted because they are referenced in other records.";

            return RedirectToAction("PatientList");
        }

        #endregion

    }
}
