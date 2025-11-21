using ClosedXML.Excel;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace HMS.Controllers
{
    public class DoctorDepartmentController : Controller
    {

        private IConfiguration _configuration;
        public DoctorDepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //#region select all
        //public IActionResult DoctorDepartmentList()
        //{
        //    ViewBag.BreadcrumbTitle = "DoctorDepartment";
        //    ViewBag.BreadcrumbPath = new string[] { "Home", "DoctorDeptList" };

        //    string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
        //    SqlConnection connection = new SqlConnection(ConnectionString);
        //    connection.Open();

        //    SqlCommand command = connection.CreateCommand();
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    command.CommandText = "PR_DoctorDepartments_GetAll";
        //    SqlDataReader reader = command.ExecuteReader();
        //    DataTable table = new DataTable();
        //    table.Load(reader);

        //    return View(table);
        //}

        //#endregion

        #region List
        public IActionResult DoctorDepartmentList()
        {
            ViewBag.BreadcrumbTitle = "DoctorDepartment";
            ViewBag.BreadcrumbPath = new string[] { "Home", "DoctorDeptList" };

            string connectionString = _configuration.GetConnectionString("ConnectionString");
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("PR_DoctorDepartments_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }
        #endregion

        //#region Add / Edit (POST)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DoctorDepartmentAddEdit(DoctorDepartmentModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string connStr = _configuration.GetConnectionString("ConnectionString");
        //            using SqlConnection conn = new SqlConnection(connStr);
        //            conn.Open();

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            if (model.DoctorDepartmentID == 0) // ✅ Only check for 0
        //            {
        //                cmd.CommandText = "PR_DoctorDepartment_Insert";
        //                cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
        //                cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
        //                cmd.Parameters.AddWithValue("@UserID", model.UserID);
        //                TempData["SuccessMessage"] = "Doctor Department added successfully!";
        //            }
        //            else
        //            {
        //                cmd.CommandText = "PR_DoctorDepartment_Update";
        //                cmd.Parameters.AddWithValue("@DoctorDepartmentID", model.DoctorDepartmentID);
        //                cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
        //                cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
        //                cmd.Parameters.AddWithValue("@UserID", model.UserID);
        //                TempData["UpdateSuccessMessage"] = "Doctor Department updated successfully!";
        //            }

        //            cmd.ExecuteNonQuery();
        //            return RedirectToAction("DoctorDepartmentList");
        //        }

        //        // Reload dropdowns if validation fails
        //        LoadDropdowns();
        //        return View("DoctorDepartmentAddEdit", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("❌ Error in AddEdit: " + ex.Message);
        //        ViewBag.ErrorMessage = "Something went wrong while saving.";
        //        LoadDropdowns();
        //        return View("DoctorDepartmentAddEdit", model);
        //    }
        //}

        //public IActionResult DoctorDepartmentAddEdit()
        //{
        //    LoadDropdowns();
        //    return View(new DoctorDepartmentModel());
        //}
        //#endregion

        //#region Edit (GET)
        //public IActionResult DoctorDepartmentEdit(int DoctorDepartmentID)
        //{
        //    string connStr = _configuration.GetConnectionString("ConnectionString");
        //    DoctorDepartmentModel model = new DoctorDepartmentModel();

        //    LoadDropdowns();

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "PR_DoctorDepartment_ByID";
        //        cmd.Parameters.AddWithValue("@DoctorDepartmentID", DoctorDepartmentID);

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        DataTable table = new DataTable();
        //        table.Load(reader);

        //        if (table.Rows.Count > 0)
        //        {
        //            DataRow row = table.Rows[0];
        //            model.DoctorDepartmentID = Convert.ToInt32(row["DoctorDepartmentID"]);
        //            model.DoctorID = Convert.ToInt32(row["DoctorID"]);
        //            model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
        //            model.UserID = Convert.ToInt32(row["UserID"]);
        //        }
        //    }

        //    return View("DoctorDepartmentAddEdit", model);
        //}
        //#endregion

        #region Add / Edit (POST)
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DoctorDepartmentAddEdit(DoctorDepartmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connStr = _configuration.GetConnectionString("ConnectionString");
                    using SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (model.DoctorDepartmentID == 0) // ✅ Insert
                    {
                        cmd.CommandText = "PR_DoctorDepartment_Insert";
                        cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
                        cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                        cmd.Parameters.AddWithValue("@UserID", model.UserID);
                        cmd.ExecuteNonQuery();

                        TempData["SuccessMessage"] = "Doctor Department added successfully!";
                    }
                    else // ✅ Update
                    {
                        cmd.CommandText = "PR_DoctorDepartment_Update";
                        cmd.Parameters.AddWithValue("@DoctorDepartmentID", model.DoctorDepartmentID);
                        cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
                        cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                        cmd.Parameters.AddWithValue("@UserID", model.UserID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            TempData["UpdateSuccessMessage"] = "Doctor Department updated successfully!";
                        else
                            TempData["ErrorMessage"] = "No record was updated. Please check your stored procedure.";
                    }

                    return RedirectToAction("DoctorDepartmentList");
                }

                // Validation failed
                LoadDropdowns(model.DoctorID, model.DepartmentID, model.UserID);
                return View("DoctorDepartmentAddEdit", model);

            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in AddEdit: " + ex.Message);
                TempData["ErrorMessage"] = "Something went wrong while saving.";
                LoadDropdowns();
                return View("DoctorDepartmentAddEdit", model);
            }
        }

        public IActionResult DoctorDepartmentAddEdit()
        {
            LoadDropdowns();
            return View(new DoctorDepartmentModel());

        }
        #endregion


        #region Edit (GET)
        public IActionResult DoctorDepartmentEdit(int DoctorDepartmentID)
        {
            string connStr = _configuration.GetConnectionString("ConnectionString");
            DoctorDepartmentModel model = new DoctorDepartmentModel();

            


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_DoctorDepartment_ByID";
                cmd.Parameters.AddWithValue("@DoctorDepartmentID", DoctorDepartmentID);

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    model.DoctorDepartmentID = Convert.ToInt32(row["DoctorDepartmentID"]);
                    model.DoctorID = Convert.ToInt32(row["DoctorID"]);
                    model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                    model.UserID = Convert.ToInt32(row["UserID"]);
                }
            }

            //return View("DoctorDepartmentAddEdit", model);
            LoadDropdowns(model.DoctorID, model.DepartmentID, model.UserID);
            return View("DoctorDepartmentAddEdit", model);
        }
        #endregion


        #region Dropdown Loader
        private void LoadDropdowns(int? selectedDoctorID = null, int? selectedDepartmentID = null, int? selectedUserID = null)
        {
            string connStr = _configuration.GetConnectionString("ConnectionString");

            // Doctors
            List<DoctorModel> doctors = new List<DoctorModel>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DoctorID, Name FROM Doctor", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new DoctorModel
                    {
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        Name = reader["Name"].ToString()
                    });
                }
            }
            // Insert "Select Doctor"
            doctors.Insert(0, new DoctorModel { DoctorID = 0, Name = "-- Select Doctor --" });
            ViewBag.Doctors = new SelectList(doctors, "DoctorID", "Name", selectedDoctorID);

            // Departments
            List<DepartmentModel> departments = new List<DepartmentModel>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DepartmentID, DepartmentName FROM Department", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new DepartmentModel
                    {
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        DepartmentName = reader["DepartmentName"].ToString()
                    });
                }
            }
            departments.Insert(0, new DepartmentModel { DepartmentID = 0, DepartmentName = "-- Select Department --" });
            ViewBag.Departments = new SelectList(departments, "DepartmentID", "DepartmentName", selectedDepartmentID);

            // Users
            List<UsersModel> users = new List<UsersModel>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM Users", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UsersModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }
            users.Insert(0, new UsersModel { UserID = 0, UserName = "-- Select User --" });
            ViewBag.Users = new SelectList(users, "UserID", "UserName", selectedUserID);
        }

        #endregion

        #region delete
        public IActionResult DeleteDoctorDepartment(int DoctorDepartmentID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_DoctorDepartment_Delete";
            command.Parameters.AddWithValue("@DoctorDepartmentID", SqlDbType.Int).Value = DoctorDepartmentID;

            command.ExecuteNonQuery();
            TempData["DeleteSuccessMessage"] = "DoctorDepartment deleted successfully!";

            return RedirectToAction("DoctorDepartmentList");
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

                SqlCommand command = new SqlCommand("PR_DoctorDepartments_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cell(1, 1).Value = "DoctorDepartmentID";
                worksheet.Cell(1, 2).Value = "DoctorID";
                worksheet.Cell(1, 3).Value = "DepartmentID";
                worksheet.Cell(1, 4).Value = "UserID";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["UserID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["DoctorID"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["DepartmentID"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["UserID"]?.ToString() ?? "";
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
        public IActionResult DeleteSelectedDoctorDepartments(List<int> selectedDoctorDepartmentIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedDoctorDepartmentIds != null && selectedDoctorDepartmentIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedDoctorDepartmentIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_DoctorDepartment_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@DoctorDepartmentID", id);
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
                TempData["DeleteSuccessMessage"] = $"{successCount} DoctorDepartment(s) deleted successfully.";

            if (failCount > 0)
                TempData["ErrorMessage"] = $"{failCount} DoctorDepartment(s) could not be deleted because they are referenced in other records.";

            return RedirectToAction("DoctorDepartmentList");
        }

        #endregion
    }
}
