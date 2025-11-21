using ClosedXML.Excel;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace HMS.Controllers
{
    public class AppointmentController : Controller
    {
        private IConfiguration _configuration;
        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //#region select all 
        //public IActionResult AppointmentList()
        //{

        //    ViewBag.BreadcrumbTitle = "Appointments";
        //    ViewBag.BreadcrumbPath = new string[] { "Home", "AppointmentList" };

        //    string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
        //    SqlConnection connection = new SqlConnection(ConnectionString);
        //    connection.Open();

        //    SqlCommand command = connection.CreateCommand();
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    command.CommandText = "PR_Appointments_GetAll";
        //    SqlDataReader reader = command.ExecuteReader();
        //    DataTable table = new DataTable();
        //    table.Load(reader);

        //    return View(table);
        //}

        //#endregion

        #region Add / Edit (POST)
        [HttpPost]
        public IActionResult AppointmentAddEdit(AppointmentModel appointmentModel)
        {
            ModelState.Remove("IsActive"); // if IsActive is not required

            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (appointmentModel.AppointmentID == 0) // ✅ int defaults to 0
                        {
                            command.CommandText = "PR_Appointment_Insert";
                            command.Parameters.AddWithValue("@UserID", appointmentModel.UserID);
                            TempData["SuccessMessage"] = "Appointment added successfully!";
                        }
                        else
                        {
                            command.CommandText = "PR_Appointment_Update";
                            command.Parameters.AddWithValue("@AppointmentID", appointmentModel.AppointmentID);
                            command.Parameters.AddWithValue("@UserID", appointmentModel.UserID);
                            TempData["UpdateSuccessMessage"] = "Appointment updated successfully!";
                        }

                        command.Parameters.AddWithValue("@DoctorID", appointmentModel.DoctorID);
                        command.Parameters.AddWithValue("@PatientID", appointmentModel.PatientID);
                        command.Parameters.AddWithValue("@AppointmentDate", appointmentModel.AppointmentDate);
                        command.Parameters.AddWithValue("@AppointmentStatus", appointmentModel.AppointmentStatus);
                        command.Parameters.AddWithValue("@Description", appointmentModel.Description);
                        command.Parameters.AddWithValue("@SpecialRemarks", appointmentModel.SpecialRemarks);
                        command.Parameters.AddWithValue("@TotalConsultedAmount", appointmentModel.TotalConsultedAmount);

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("AppointmentList");
            }

            // Reload dropdowns if validation fails
            LoadDropdowns();
            return View(appointmentModel);
        }

        [HttpGet]
        public IActionResult AppointmentAddEdit()
        {
            LoadDropdowns();
            return View(new AppointmentModel());
        }
        #endregion

        #region Edit (GET)
        public IActionResult AppointmentEdit(int AppointmentID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            AppointmentModel appointmentModel = new AppointmentModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Appointment_ByID";
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        if (table.Rows.Count > 0)
                        {
                            DataRow dr = table.Rows[0];
                            appointmentModel.AppointmentID = Convert.ToInt32(dr["AppointmentID"]);
                            appointmentModel.DoctorID = Convert.ToInt32(dr["DoctorID"]);
                            appointmentModel.PatientID = Convert.ToInt32(dr["PatientID"]);
                            appointmentModel.AppointmentDate = Convert.ToDateTime(dr["AppointmentDate"]);
                            appointmentModel.AppointmentStatus = dr["AppointmentStatus"].ToString();
                            appointmentModel.Description = dr["Description"].ToString();
                            appointmentModel.SpecialRemarks = dr["SpecialRemarks"].ToString();
                            appointmentModel.UserID = Convert.ToInt32(dr["UserID"]);
                            appointmentModel.TotalConsultedAmount = Convert.ToDecimal(dr["TotalConsultedAmount"]);
                        }
                    }
                }
            }

            LoadDropdowns(); // ✅ Load Doctor, Patient, User lists
            return View("AppointmentAddEdit", appointmentModel);
        }
        #endregion

        #region Helper Dropdowns
        private void LoadDropdowns()
        {
            string connStr = _configuration.GetConnectionString("ConnectionString");

            // 🔹 Doctors Dropdown
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
            ViewBag.Doctors = new SelectList(doctors, "DoctorID", "Name");

            // 🔹 Patients Dropdown
            List<PatientModel> patients = new List<PatientModel>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT PatientID, Name FROM Patient", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new PatientModel
                    {
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        Name = reader["Name"].ToString()
                    });
                }
            }
            ViewBag.Patients = new SelectList(patients, "PatientID", "Name");

            // 🔹 Users Dropdown
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
            ViewBag.Users = new SelectList(users, "UserID", "UserName");
        }
        #endregion


        #region delete
        public IActionResult DeleteAppointment(int AppointmentID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Appointment_Delete";
            command.Parameters.AddWithValue("@AppointmentID", SqlDbType.Int).Value = AppointmentID;

            command.ExecuteNonQuery();
            TempData["DeleteSuccessMessage"] = "User deleted successfully!";


            return RedirectToAction("AppointmentList");
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

                SqlCommand command = new SqlCommand("PR_Appointments_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cell(1, 1).Value = "AppointmentID";
                worksheet.Cell(1, 2).Value = "AppointmentDate";
                worksheet.Cell(1, 3).Value = "AppointmentStatus";
                worksheet.Cell(1, 4).Value = "Description";
                worksheet.Cell(1, 5).Value = "SpecialRemarks";
                worksheet.Cell(1, 6).Value = "TotalConsultedAmount";


                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["AppointmentID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["AppointmentDate"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["AppointmentStatus"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["Description"]?.ToString() ?? "";
                    worksheet.Cell(row, 5).Value = item["SpecialRemarks"]?.ToString() ?? "";
                    worksheet.Cell(row, 6).Value = item["TotalConsultedAmount"]?.ToString() ?? "";

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
        public IActionResult DeleteSelectedAppointments(List<int> selectedAppointmentIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedAppointmentIds != null && selectedAppointmentIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedAppointmentIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_Appointment_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@AppointmentID", id);
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
                TempData["DeleteSuccessMessage"] = $"{successCount} Appointment(s) deleted successfully.";

            if (failCount > 0)
                TempData["ErrorMessage"] = $"{failCount} Appointment(s) could not be deleted because they are referenced in other records.";

            return RedirectToAction("AppointmentList");
        }

        #endregion

        //#region search or filter
        //public IActionResult Index(AppointmentSearchModel searchModel)
        //{
        //    if (searchModel == null)
        //        searchModel = new AppointmentSearchModel();

        //    List<AppointmentModel> appointments = new List<AppointmentModel>();
        //    string connStr = _configuration.GetConnectionString("ConnectionString");

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    using (SqlCommand cmd = new SqlCommand("PR_Appointment_Filter", conn))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@AppointmentID", searchModel.AppointmentID == 0 ? DBNull.Value : (object)searchModel.AppointmentID);
        //        cmd.Parameters.AddWithValue("@DoctorID", searchModel.DoctorID == 0 ? DBNull.Value : (object)searchModel.DoctorID);
        //        cmd.Parameters.AddWithValue("@PatientID", searchModel.PatientID == 0 ? DBNull.Value : (object)searchModel.PatientID);
        //        cmd.Parameters.AddWithValue("@UserID", searchModel.UserID == 0 ? DBNull.Value : (object)searchModel.UserID);
        //        cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(searchModel.AppointmentStatus) ? DBNull.Value : (object)searchModel.AppointmentStatus);
        //        cmd.Parameters.AddWithValue("@FromDate", searchModel.FromDate == DateTime.MinValue ? DBNull.Value : (object)searchModel.FromDate);
        //        cmd.Parameters.AddWithValue("@ToDate", searchModel.ToDate == DateTime.MinValue ? DBNull.Value : (object)searchModel.ToDate);

        //        conn.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                appointments.Add(new AppointmentModel
        //                {
        //                    AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
        //                    DoctorID = Convert.ToInt32(reader["DoctorID"]),
        //                    PatientID = Convert.ToInt32(reader["PatientID"]),
        //                    UserID = Convert.ToInt32(reader["UserID"]),
        //                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
        //                    AppointmentStatus = reader["AppointmentStatus"].ToString(),
        //                    Description = reader["Description"].ToString(),
        //                    SpecialRemarks = reader["SpecialRemarks"].ToString(),
        //                    TotalConsultedAmount = reader["TotalConsultedAmount"] == DBNull.Value ? null : (decimal?)reader["TotalConsultedAmount"],
        //                    Created = Convert.ToDateTime(reader["Created"]),
        //                    Modified = Convert.ToDateTime(reader["Modified"])
        //                });
        //            }
        //        }
        //    }

        //    ViewBag.SearchModel = searchModel;
        //    return View(appointments);
        //}

        //#endregion

        public IActionResult AppointmentList(int? AppointmentID, int? DoctorID, int? PatientID, int? UserID,
                                     string AppointmentStatus, DateTime? FromDate, DateTime? ToDate)
        {
            DataTable dt = new DataTable();
            string connStr = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("PR_Appointment_Filter", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PatientID", PatientID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UserID", UserID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(AppointmentStatus) ? (object)DBNull.Value : AppointmentStatus);
                cmd.Parameters.AddWithValue("@FromDate", FromDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", ToDate ?? (object)DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // Preserve search values back to view
            ViewBag.AppointmentID = AppointmentID;
            ViewBag.DoctorID = DoctorID;
            ViewBag.PatientID = PatientID;
            ViewBag.UserID = UserID;
            ViewBag.AppointmentStatus = AppointmentStatus;
            ViewBag.FromDate = FromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = ToDate?.ToString("yyyy-MM-dd");

            return View(dt);
        }

    }
}
