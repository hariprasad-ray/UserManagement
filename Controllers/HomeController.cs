using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
          
            var data = new List<UserDetail>();
            var userDetails = new UserDetail();

            try
            {
                data = _context.UserDetails.ToList();

                if (data != null)
                {                    
                    userDetails = data.Where(p => p.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }          

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
            var data = new List<UserDetail>();

            try
            {
                data = _context.UserDetails.ToList(); //linq queries                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(data);
            #endregion
        }


        [HttpPost]
        public JsonResult Update(UserDetail userDetail)
        {
            bool status = false;

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

                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(new { status = status });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            //LINQ - Database, object, XML, entities

            var searchText = "Ram";

            //Select * from UserDetails where FirstName = 'Ram' AND LastName = 'M' Orderby LastName desc
            var data = _context.UserDetails.Where(p => p.FirstName == "Ram" && p.LastName == "M").ToList();

            //Ramraj- contains
            var data2 = _context.UserDetails.Where(p => searchText.Contains(p.FirstName)).ToList();

            //Orderby
            var data3 = _context.UserDetails.Where(p => p.IsActive == true).OrderByDescending(p => p.LastName).ToList();

            //Group by
            var results = from p in _context.UserDetails
                          group p.IsActive by p.Id into g
                          select new { UserId = g.Key, Details = g.ToList() };

            //Joins
            var id = 1;

            List<StudentDetails> studentDetails = new List<StudentDetails>() { new StudentDetails { StudentId = 1, StudentName = "Ram" } };
            List<StudentAddress> StudentAddress = new List<StudentAddress>() { new StudentAddress { StudentId = 1, Address = "HYD" } };

            var query =
               (from post in studentDetails
                join meta in StudentAddress on post.StudentId equals meta.StudentId
                where post.StudentId == id
                select new 
                {
                    Post = post,
                    Address = meta.Address

                }).ToList();


            //select new { Post = post, Meta = meta };



            //C# objects
            List<string> errors = new List<string>() { "Ram", "Raj" };
            var data4 = errors.Where(p => p == "Ram").OrderByDescending(p => p);

            List<ErrorViewModel> errors2 = new List<ErrorViewModel>();
            errors2.Where(p => p.RequestId == "").ToList();




            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
