using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient; 

namespace HMS.Controllers
{

    public class DepartmentController : Controller
    {
        private IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Select All
        public IActionResult DepartmentList()
        {
            ViewBag.BreadcrumbTitle = "Department";
            ViewBag.BreadcrumbPath = new string[] { "Home", "DepartmentList" };

            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Departments_GetAll";
           using SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }
        #endregion



        #region Add / Edit
        [HttpPost]
        public IActionResult DepartmentAddEdit(DepartmentModel departmentModel)
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

                            if (departmentModel.DepartmentID == null || departmentModel.DepartmentID == 0)
                            {
                                command.CommandText = "PR_Department_insert";
                                command.Parameters.AddWithValue("@UserID", departmentModel.UserID);

                                TempData["SuccessMessage"] = "Department added successfully!";
                            }
                            else
                            {
                                command.CommandText = "PR_Department_Update";
                                command.Parameters.AddWithValue("@DepartmentID", departmentModel.DepartmentID);
                                TempData["SuccessMessage"] = "Department updated successfully!";
                            }

                            command.Parameters.AddWithValue("@DepartmentName", departmentModel.DepartmentName);
                            command.Parameters.AddWithValue("@Description", departmentModel.Description);

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("DepartmentList");
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
                // ✅ FIXED: use ViewBag.Users (not UserList)
                ViewBag.Users = new SelectList(userList, "UserID", "UserName");

                return View("DepartmentAddEdit", departmentModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                ViewBag.ErrorMessage = "Something went wrong while saving.";
                return View(departmentModel);
            }
        }

        public IActionResult DepartmentAddEdit()
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

            return View(new DepartmentModel());
        }
        #endregion

        #region Edit
        public IActionResult DepartmentEdit(int? DepartmentID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            DepartmentModel departmentModel = new DepartmentModel();

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
            // ✅ FIXED: use ViewBag.Users
            ViewBag.Users = new SelectList(userList, "UserID", "UserName");

            // Fetch Department Data by ID
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_DepartmentByID";
                    command.Parameters.AddWithValue("@DepartmentID", DepartmentID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        foreach (DataRow row in table.Rows)
                        {
                            departmentModel.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                            departmentModel.DepartmentName = row["DepartmentName"].ToString();
                            departmentModel.Description = row["Description"].ToString();
                            departmentModel.UserID = Convert.ToInt32(row["UserID"]);
                        }
                    }
                }
            }

            return View("DepartmentAddEdit", departmentModel);
        }
        #endregion

        #region Delete
        public IActionResult DeleteDepartment(int DepartmentID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Department_Delete";
            command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
            command.ExecuteNonQuery();

            TempData["DeleteSuccessMessage"] = "Department deleted successfully!";
            return RedirectToAction("DepartmentList");
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

                SqlCommand command = new SqlCommand("PR_Departments_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                worksheet.Cell(1, 1).Value = "DepartmentID";
                worksheet.Cell(1, 2).Value = "DepartmentName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "IsActive";

                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["DepartmentID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["DepartmentName"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["Description"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["IsActive"]?.ToString() ?? "";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

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
        public IActionResult DeleteSelectedDepartments(List<int> selectedDepartmentIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedDepartmentIds != null && selectedDepartmentIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedDepartmentIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_Department_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@DepartmentID", id);
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
                TempData["DeleteSuccessMessage"] = $"{successCount} user(s) deleted successfully.";

            if (failCount > 0)
                TempData["ErrorMessage"] = $"{failCount} user(s) could not be deleted because they are referenced in other records.";

            return RedirectToAction("DepartmentList");
        }

        #endregion
    }
}
