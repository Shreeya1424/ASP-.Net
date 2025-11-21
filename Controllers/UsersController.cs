using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Api.Ads.AdWords.v201809;
using HMS.Helper;
using HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using SixLabors.ImageSharp;
using System.Data;
using System.Data.SqlClient;


namespace HMS.Controllers
{
    
    public class UsersController : Controller
    {
        #region Config
        private IConfiguration _configuration;
        private object configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region GetAll
        [CheckAccess]
        public IActionResult UserList()
        {
            ViewBag.BreadcrumbTitle = "Users";
            ViewBag.BreadcrumbPath = new string[] { "Home", "UserList" };

            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Users_GetAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }
        #endregion


        #region users add 
        public IActionResult UsersAddEdit(UsersModel usersModel)
        {

            ModelState.Remove("IsActive");
            if (ModelState.IsValid)
            {
                string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (usersModel.UserID == null || usersModel.UserID == 0)
                {
                    command.CommandText = "PR_Users_insert";
                    TempData["SuccessMessage"] = "User added successfully!";

                }
                else
                {
                    command.CommandText = "PR_Users_update";
                    command.Parameters.AddWithValue("@UserID", SqlDbType.Int).Value = usersModel.UserID;
                    TempData["UpdateSuccessMessage"] = "User updated successfully!";


                }

                command.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = usersModel.UserName;
                command.Parameters.AddWithValue("@Password", SqlDbType.VarChar).Value = usersModel.Password;
                command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = usersModel.Email;
                command.Parameters.AddWithValue("@MobileNo", SqlDbType.VarChar).Value = usersModel.MobileNo;
                command.ExecuteNonQuery();

                return RedirectToAction("UserList");
            }
           
            return View(usersModel);
        }
        #endregion


        #region delete
        public IActionResult DeleteUser(string UserID)
        {
            int decryptedUserID = Convert.ToInt32(UrlEncryptor.Decrypt(UserID));

            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Users_Delete";
            command.Parameters.AddWithValue("@UserID", SqlDbType.Int).Value = decryptedUserID;

            command.ExecuteNonQuery();
            TempData["DeleteSuccessMessage"] = "User deleted successfully!";

            return RedirectToAction("UserList");
        }

        #endregion

        #region Edit

        public IActionResult UsersEdit(string? EncUserID)
        {
            int decryptedUserID =String.IsNullOrEmpty(EncUserID)?0: Convert.ToInt32(UrlEncryptor.Decrypt(EncUserID));

            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            UsersModel usersModel = new UsersModel();
           if (decryptedUserID > 0) { 
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_UserS_ByID";
            command.Parameters.AddWithValue("@UserID", SqlDbType.Int).Value = decryptedUserID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            
            foreach (DataRow dr in table.Rows)
            {
                usersModel.UserID = Convert.ToInt32(dr["UserID"]);
                usersModel.UserName = dr["UserName"].ToString();
                usersModel.Password = dr["Password"].ToString();
                usersModel.Email = dr["Email"].ToString();
                usersModel.MobileNo = dr["MobileNo"].ToString();
                usersModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                usersModel.Modified = Convert.ToDateTime(dr["Modified"]);

            }

        }

            return View("UsersAddEdit", usersModel);


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

                SqlCommand command = new SqlCommand("PR_Users_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cell(1, 1).Value = "UserID";
                worksheet.Cell(1, 2).Value = "UserName";
                //worksheet.Cell(1, 3).Value = "Password";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "MobileNo";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["UserID"]?.ToString() ?? "";
                    worksheet.Cell(row, 2).Value = item["UserName"]?.ToString() ?? "";
                    //worksheet.Cell(row, 3).Value = item["Password"]?.ToString() ?? "";
                    worksheet.Cell(row, 3).Value = item["Email"]?.ToString() ?? "";
                    worksheet.Cell(row, 4).Value = item["MobileNo"]?.ToString() ?? "";
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
        public IActionResult DeleteSelectedUsers(List<int> selectedUserIds)
        {
            int successCount = 0;
            int failCount = 0;

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (int id in selectedUserIds)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("PR_Users_Delete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserID", id);
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

            return RedirectToAction("UserList");
        }

        #endregion

        #region register
        // GET: Show the registration form
        [AllowAnonymous]
        [HttpGet]
        public IActionResult UserRegister()
        {
            // explicitly load Register.cshtml
            return View("Register");
        }

        // POST: Handle registration form submission
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "PR_User_Register";
                        sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                        sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                        sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                        sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                        sqlCommand.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = userRegisterModel.IsActive;
                        sqlCommand.ExecuteNonQuery();
                    }

                    // ✅ Success → go to login
                    TempData["SuccessMessage"] = "Account created successfully. Please login.";
                    return RedirectToAction("Login", "Users");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            // ❌ If invalid → reload register with entered data
            return View("Register", userRegisterModel);
        }


        #endregion

        #region login
        [AllowAnonymous]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandText = "PR_User_Login";
                        sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                        sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(sqlDataReader);

                        if (dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                                HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            }

                            return RedirectToAction("UserList", "Users");
                        }
                        else
                        {
                            // 🔹 Incorrect username or password
                            TempData["ErrorMessage"] = "Invalid username or password!";
                            return RedirectToAction("Login", "Users");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        #endregion


        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

    }
}
