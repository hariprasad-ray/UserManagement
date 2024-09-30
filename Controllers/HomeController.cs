using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManagementContext _context;

        public HomeController(ILogger<HomeController> logger, UserManagementContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(UserDetail userDetail)
        {
            bool status = false;

            try
            {
                _context.UserDetails.Add(userDetail);
                _context.SaveChanges();

                status = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(status);
        }

        public IActionResult Details(int Id)
        {
            int totalCount = 0, activeCount = 0, inActiveCount = 0;
            var data = new List<UserDetail>();
            var userDetails = new UserDetail();

            try
            {
                data = _context.UserDetails.ToList();

                if (data != null)
                {
                    totalCount = data.Count;
                    activeCount = data.Where(p => p.IsActive == true).Count();
                    inActiveCount = data.Where(p => p.IsActive == false).Count();

                    userDetails = data.Where(p => p.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            ViewBag.TotalUserCount = totalCount;
            ViewBag.ActiveUserCount = activeCount;
            ViewBag.InActiveUserCount = inActiveCount;


            return View(userDetails);
        }

        public IActionResult List()
        {
            #region sample ADO.Net code
            try
            {
                //string ConnectionString = @"Data Source=DESKTOP-MDKAEQ4\SQLEXPRESS;Initial Catalog=UserManagement; User Id=sa; Password=Ind_Sql@sa2024;TrustServerCertificate=True;";
                //using (SqlConnection connection = new SqlConnection(ConnectionString))
                //{
                //    connection.Open();
                //    //Create the SqlDataAdapter instance by specifying the command text and connection object
                //    SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from UserDetails;", connection);

                //    //Creating DataSet Object
                //    DataSet dataSet = new DataSet();

                //    //Filling the DataSet using the Fill Method of SqlDataAdapter object
                //    //Here, we have not specified the data table name and the data table will be created at index position 0
                //    dataAdapter.Fill(dataSet);

                //    //Iterating through the DataSet 
                //    //First fetch the Datatable from the dataset and then fetch the rows using the Rows property of Datatable
                //    foreach (DataRow row in dataSet.Tables[0].Rows)
                //    {                       
                //        //Accessing the Data using the integer index position as key
                //        //Console.WriteLine(row["Id"] + ",  " + row["Name"] + ",  " + row["Mobile"]);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
            #endregion


            #region EF 
            int totalCount = 0, activeCount = 0, inActiveCount = 0;
            var data = new List<UserDetail>();

            try
            {
                data = _context.UserDetails.ToList();

                if (data != null)
                {
                    totalCount = data.Count;
                    activeCount = data.Where(p => p.IsActive == true).Count();
                    inActiveCount = data.Where(p => p.IsActive == false).Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            ViewBag.TotalUserCount = totalCount;
            ViewBag.ActiveUserCount = activeCount;
            ViewBag.InActiveUserCount = inActiveCount;

            return View(data);
            #endregion
        }


        [HttpPost]
        public JsonResult Update(UserDetail userDetail)
        {
            bool status = false;
            var activeCount = 0;
            var inActiveCount = 0;

            try
            { 
                if (userDetail != null && userDetail.Id > 0)
                {
                    var user = _context.UserDetails.Where(p => p.Id == userDetail.Id).FirstOrDefault();
                    if (user != null)
                    {
                        user.IsActive = userDetail.IsActive;
                    }
                    _context.SaveChanges();

                     activeCount = _context.UserDetails.Where(p => p.IsActive == true).Count();
                     inActiveCount = _context.UserDetails.Where(p => p.IsActive == false).Count();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(new { status = status, activecount = activeCount, inactivecount = inActiveCount });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool status = false;
            var activeCount = 0;
            var inActiveCount = 0;

            try
            {
                var user = _context.UserDetails.Where(p => p.Id == id).FirstOrDefault();
                if (user != null)
                {
                    _context.UserDetails.Remove(user);
                    _context.SaveChanges();

                    activeCount = _context.UserDetails.Where(p => p.IsActive == true).Count();
                    inActiveCount = _context.UserDetails.Where(p => p.IsActive == false).Count();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(new { status = status, activecount = activeCount, inactivecount = inActiveCount });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
