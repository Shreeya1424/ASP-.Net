using HMS.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace HMS.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            string connStr = _configuration.GetConnectionString("ConnectionString");

            int deptCount = 0, doctorCount = 0, patientCount = 0, pendingCount = 0, completedCount = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Department", conn))
                    deptCount = Convert.ToInt32(cmd.ExecuteScalar());

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Doctor", conn))
                    doctorCount = Convert.ToInt32(cmd.ExecuteScalar());

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Patient", conn))
                    patientCount = Convert.ToInt32(cmd.ExecuteScalar());

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Appointment WHERE AppointmentStatus = 'Pending'", conn))
                    pendingCount = Convert.ToInt32(cmd.ExecuteScalar());

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Appointment WHERE AppointmentStatus = 'Completed'", conn))
                    completedCount = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // Assign ViewBag values
            ViewBag.Departments = deptCount;
            ViewBag.Doctors = doctorCount;
            ViewBag.Patients = patientCount;
            ViewBag.PendingAppointments = pendingCount;
            ViewBag.CompletedAppointments = completedCount;

            return View(); // ✅ FIXED
        }
    }
}
