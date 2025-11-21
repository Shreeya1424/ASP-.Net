//using HMS.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using System.Data.SqlClient;

//namespace HMS.Controllers
//{
//    public class EmployeeController : Controller
//    {
//        private IConfiguration _configuration;
//        public EmployeeController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        #region selectall
//        public IActionResult EmployeeList()
//        {
//            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
//            SqlConnection connection = new SqlConnection(ConnectionString);
//            connection.Open();

//            SqlCommand command = connection.CreateCommand();
//            command.CommandType = System.Data.CommandType.StoredProcedure;
//            command.CommandText = "PR_employee_GetAll";
//            SqlDataReader reader = command.ExecuteReader();
//            DataTable table = new DataTable();
//            table.Load(reader);

//            return View(table);

//        }

//        #endregion

//        #region users add 
//        public IActionResult EmployeeAddEdit(EmployeeModel employeeModel)
//        {
//            ModelState.Remove("IsActive");
//            if (ModelState.IsValid)
//            {
//                string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
//                SqlConnection connection = new SqlConnection(ConnectionString);
//                connection.Open();

//                SqlCommand command = connection.CreateCommand();
//                command.CommandType = System.Data.CommandType.StoredProcedure;
//                if (employeeModel.EmployeeID == null || employeeModel.EmployeeID == 0)
//                {
//                    command.CommandText = "PR_employee_insert";
//                }
//                else
//                {
//                    command.CommandText = "PR_employee_update";
//                    command.Parameters.AddWithValue("@EmployeeID", SqlDbType.Int).Value = employeeModel.EmployeeID;
//                }

//                command.Parameters.AddWithValue("@FirstName", SqlDbType.VarChar).Value = employeeModel.FirstName;
//                command.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value = employeeModel.LastName;
//                command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value =    employeeModel.Email;
//                command.Parameters.AddWithValue("@DOB", SqlDbType.VarChar).Value = employeeModel.DOB;
//                command.Parameters.AddWithValue("@Gender", SqlDbType.VarChar).Value = employeeModel.Gender;

//                command.Parameters.AddWithValue("@HireDate", SqlDbType.VarChar).Value = employeeModel.HireDate;
//                command.Parameters.AddWithValue("@Jobtitle", SqlDbType.VarChar).Value = employeeModel.Jobtitle;
//                command.Parameters.AddWithValue("@Department", SqlDbType.VarChar).Value = employeeModel.Department;
//                command.Parameters.AddWithValue("@Salary", SqlDbType.VarChar).Value = employeeModel.Salary;


//                command.ExecuteNonQuery();

//                return RedirectToAction("EmployeeList");
//            }

//            return View(employeeModel);
//        }
//        #endregion

//        #region Edit
//        public IActionResult EmployeeEdit(int? EmployeeID)
//        {
//            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
//            SqlConnection connection = new SqlConnection(ConnectionString);
//            connection.Open();
//            SqlCommand command = connection.CreateCommand();
//            command.CommandType = System.Data.CommandType.StoredProcedure;
//            command.CommandText = "PR_employee_byid";
//            command.Parameters.AddWithValue("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
//            SqlDataReader reader = command.ExecuteReader();
//            DataTable table = new DataTable();
//            table.Load(reader);

//            EmployeeModel employeeModel = new EmployeeModel();
//            foreach (DataRow dr in table.Rows)
//            {
//                employeeModel.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
//                employeeModel.FirstName = dr["FirstName"].ToString();
//                employeeModel.LastName = dr["LastName"].ToString();
//                employeeModel.Email = dr["Email"].ToString();
//                employeeModel.DOB = Convert.ToDateTime(dr["DOB"]);
//                employeeModel.Gender= dr["Gender"].ToString();
//                employeeModel.HireDate = Convert.ToDateTime(dr["HireDate"]);
//                employeeModel.Jobtitle = dr["Jobtitle"].ToString();
//                employeeModel.Department = dr["Department"].ToString();
//                employeeModel.Salary = Convert.ToDecimal(dr["Salary"]);
//                employeeModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
//                employeeModel.Modified = Convert.ToDateTime(dr["Modified"]);

//            }

//            return View("EmployeeAddEdit", employeeModel);


//        }
//        #endregion

//        #region delete
//        public IActionResult DeleteEmployee(int EmployeeID)
//        {
//            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
//            SqlConnection connection = new SqlConnection(ConnectionString);
//            connection.Open();

//            SqlCommand command = connection.CreateCommand();
//            command.CommandType = System.Data.CommandType.StoredProcedure;
//            command.CommandText = "PR_employee_Delete";
//            command.Parameters.AddWithValue("@EmployeeID", SqlDbType.Int).Value = EmployeeID;

//            command.ExecuteNonQuery();

//            return RedirectToAction("EmployeeList");
//        }

//        #endregion
//    }
//}

using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace HMS.Controllers
{
    public class EmployeeController : Controller
    {
        private IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region selectall
        public IActionResult EmployeeList()
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_employee_GetAll";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    table.Load(reader);
                }
            }

            return View(table);

        }

        #endregion

        #region users add 
        [HttpGet]
        
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeAddEdit(EmployeeModel employeeModel)
        {
            // remove fields from validation if necessary
            ModelState.Remove("IsActive");

            if (!ModelState.IsValid)
                return View(employeeModel);

            // 1) convert uploaded file (if any) to bytes
            if (employeeModel.ProfileImageFile != null && employeeModel.ProfileImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    employeeModel.ProfileImageFile.CopyTo(ms);
                    employeeModel.ProfileImage = ms.ToArray();
                    employeeModel.ImageContentType = employeeModel.ProfileImageFile.ContentType;
                    employeeModel.ImageFileName = employeeModel.ProfileImageFile.FileName;
                }
            }

            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;

                if (employeeModel.EmployeeID == null || employeeModel.EmployeeID == 0)
                {
                    command.CommandText = "PR_employee_insert";
                }
                else
                {
                    command.CommandText = "PR_employee_update";
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeModel.EmployeeID;
                }

                // text columns
                command.Parameters.Add("@FirstName", SqlDbType.VarChar, 100).Value = (object)employeeModel.FirstName ?? DBNull.Value;
                command.Parameters.Add("@LastName", SqlDbType.VarChar, 100).Value = (object)employeeModel.LastName ?? DBNull.Value;
                command.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = (object)employeeModel.Email ?? DBNull.Value;

                // date/time columns - write DBNull if default
                command.Parameters.Add("@DOB", SqlDbType.DateTime).Value = (employeeModel.DOB == default(DateTime)) ? DBNull.Value : (object)employeeModel.DOB;
                command.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = (employeeModel.HireDate == default(DateTime)) ? DBNull.Value : (object)employeeModel.HireDate;

                command.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = (object)employeeModel.Gender ?? DBNull.Value;
                command.Parameters.Add("@Jobtitle", SqlDbType.VarChar, 100).Value = (object)employeeModel.Jobtitle ?? DBNull.Value;
                command.Parameters.Add("@Department", SqlDbType.VarChar, 100).Value = (object)employeeModel.Department ?? DBNull.Value;

                // salary (decimal) - set precision/scale
                var salaryParam = command.Parameters.Add("@Salary", SqlDbType.Decimal);
                salaryParam.Precision = 18;
                salaryParam.Scale = 2;
                salaryParam.Value = employeeModel.Salary == 0 ? (object)DBNull.Value : employeeModel.Salary;

                // Image parameters (varbinary)
                var imgParam = command.Parameters.Add("@ProfileImage", SqlDbType.VarBinary, -1);
                imgParam.Value = (object)employeeModel.ProfileImage ?? DBNull.Value;

                command.Parameters.Add("@ImageFileName", SqlDbType.VarChar, 250).Value = (object)employeeModel.ImageFileName ?? DBNull.Value;
                command.Parameters.Add("@ImageContentType", SqlDbType.VarChar, 100).Value = (object)employeeModel.ImageContentType ?? DBNull.Value;
                command.Parameters.Add("@RemoveImage", SqlDbType.Bit).Value = employeeModel.RemoveImage ? 1 : 0;

                command.ExecuteNonQuery();

            }

            return RedirectToAction("EmployeeList");
        }
        #endregion

        #region Edit
        [HttpPost]
        public IActionResult EmployeeEdit(int? EmployeeID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_employee_byid";
                command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID ?? 0;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    table.Load(reader);
                }
            }

            EmployeeModel employeeModel = new EmployeeModel();
            if (table.Rows.Count > 0)
            {
                var dr = table.Rows[0];
                employeeModel.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
                employeeModel.FirstName = dr["FirstName"].ToString();
                employeeModel.LastName = dr["LastName"].ToString();
                employeeModel.Email = dr["Email"].ToString();
                employeeModel.DOB = dr["DOB"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(dr["DOB"]);
                employeeModel.Gender = dr["Gender"].ToString();
                employeeModel.HireDate = dr["HireDate"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(dr["HireDate"]);
                employeeModel.Jobtitle = dr["Jobtitle"].ToString();
                employeeModel.Department = dr["Department"].ToString();
                employeeModel.Salary = dr["Salary"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Salary"]);
                employeeModel.IsActive = dr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsActive"]);
                employeeModel.Modified = dr["Modified"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(dr["Modified"]);

                // image columns
                employeeModel.ProfileImage = dr["ProfileImage"] == DBNull.Value ? null : (byte[])dr["ProfileImage"];
                employeeModel.ImageContentType = dr["ImageContentType"] == DBNull.Value ? null : dr["ImageContentType"].ToString();
                employeeModel.ImageFileName = dr["ImageFileName"] == DBNull.Value ? null : dr["ImageFileName"].ToString();
            }

            return View("EmployeeAddEdit", employeeModel);


        }
        #endregion

        #region delete
        public IActionResult DeleteEmployee(int EmployeeID)
        {
            string ConnectionString = this._configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_employee_Delete";
                command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID;

                command.ExecuteNonQuery();
            }

            return RedirectToAction("EmployeeList");
        }

        #endregion

        public IActionResult RemoveEmployeeImage(int EmployeeID)
        {
            string ConnectionString = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_employee_removeimage";
                command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                command.ExecuteNonQuery();
            }
            return RedirectToAction("EmployeeEdit", new { EmployeeID });
        }

    }
}
