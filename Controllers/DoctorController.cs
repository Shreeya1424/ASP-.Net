using ClosedXML.Excel;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace HMS.Controllers
{
    public class DoctorController : Controller
    {
        private IConfiguration _configuration;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region get all
        public IActionResult DoctorList()
        {

            ViewBag.BreadcrumbTitle = "Doctors";
            ViewBag.BreadcrumbPath = new string[] { "Home", "DoctorList" };
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Doctors_GetAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }
        #endregion

        #region Add / Edit
        [HttpPost]
        public IActionResult DoctorAddEdit(DoctorModel doctorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            if (doctorModel.DoctorID == null || doctorModel.DoctorID == 0)
                            {
                                command.CommandText = "PR_Doctor_Insert";
                                command.Parameters.AddWithValue("@UserID", doctorModel.UserID);

                                TempData["SuccessMessage"] = "Doctor added successfully!";
                            }
                            else
                            {
                                command.CommandText = "PR_Doctor_Update";
                                command.Parameters.AddWithValue("@DoctorID", doctorModel.DoctorID);
                                TempData["SuccessMessage"] = "Doctor updated successfully!";
                            }

                            command.Parameters.AddWithValue("@Name", doctorModel.Name);
                            command.Parameters.AddWithValue("@Phone", doctorModel.Phone);
                            command.Parameters.AddWithValue("@Email", doctorModel.Email);
                            command.Parameters.AddWithValue("@Qualification", doctorModel.Qualification);
                            command.Parameters.AddWithValue("@Specialization", doctorModel.Specialization);

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("DoctorList");
                }

                // Reload UserList if validation fails
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

                return View("DoctorAddEdit", doctorModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                ViewBag.ErrorMessage = "Something went wrong while saving.";
                return View(doctorModel);
            }
        }

        public IActionResult DoctorAddEdit()
        {
            // Load User dropdown for first-time GET
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

            return View(new DoctorModel());
        }
        #endregion

        #region Edit
        public IActionResult DoctorEdit(int? DoctorID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            DoctorModel doctorModel = new DoctorModel();

            // Load User List for Dropdown
            List<UsersModel> userList = new List<UsersModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM Users", conn))
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

            // Fetch Doctor Data by ID
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Doctor_ByID";
                    command.Parameters.AddWithValue("@DoctorID", DoctorID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        foreach (DataRow row in table.Rows)
                        {
                            doctorModel.DoctorID = Convert.ToInt32(row["DoctorID"]);
                            doctorModel.Name = row["Name"].ToString();
                            doctorModel.Phone = row["Phone"].ToString();
                            doctorModel.Email = row["Email"].ToString();
                            doctorModel.Qualification = row["Qualification"].ToString();
                            doctorModel.Specialization = row["Specialization"].ToString();
                            doctorModel.UserID = Convert.ToInt32(row["UserID"]);
                        }
                    }
                }
            }

            return View("DoctorAddEdit", doctorModel);
        }
        #endregion


        #region delete
        public IActionResult DeleteDoctor(int DoctorID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Doctor_Delete";
            command.Parameters.AddWithValue("@DoctorID", SqlDbType.Int).Value = DoctorID;

            command.ExecuteNonQuery();
            TempData["DeleteSuccessMessage"] = "User deleted successfully!";

            return RedirectToAction("DoctorList");
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

                SqlCommand command = new SqlCommand("PR_Doctors_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cell(1, 1).Value = "DoctorID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Phone";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Qualification";
                worksheet.Cell(1, 6).Value = "Specialization";
                worksheet.Cell(1, 7).Value = "IsActive";


                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["DoctorID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["Name"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["Phone"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["Email"]?.ToString() ?? "";
                    worksheet.Cell(row, 5).Value = item["Qualification"]?.ToString() ?? "";
                    worksheet.Cell(row, 6).Value = item["Specialization"]?.ToString() ?? "";
                    worksheet.Cell(row, 7).Value = item["IsActive"]?.ToString() ?? "";

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
        public IActionResult DeleteSelectedDoctors(List<int> selectedDoctorIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedDoctorIds != null && selectedDoctorIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedDoctorIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_Doctor_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@DoctorID", id);
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
                TempData["DeleteSuccessMessage"] = $"{successCount} Doctor(s) deleted successfully.";

            if (failCount > 0)
                TempData["ErrorMessage"] = $"{failCount} DoctorID(s) could not be deleted because they are referenced in other records.";

            return RedirectToAction("DoctorList");
        }

        #endregion
    }
}
