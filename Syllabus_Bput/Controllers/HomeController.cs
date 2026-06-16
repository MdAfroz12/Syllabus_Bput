using StudentRepo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Syllabus_Bput.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)] 
    public class HomeController : Controller
    {

        private static string connectionString = "Data Source=AFROZ;Initial Catalog=Bput_Syllabus;Integrated Security=True;";

        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        public static bool isFormSubmitted; 
        public static string BatchId = "";

        public ActionResult Index()
        {
            try
            {

                //Course
                BindCourse();

                var courseItems = HttpContext.Session["courseItems"] as List<SelectListItem>;
                var Cour = HttpContext.Session["Cour"] as string;
               

                if (courseItems == null)
                {
                    courseItems = new List<SelectListItem>();
                }

                courseItems = courseItems
                    .Select(item => new SelectListItem
                    {
                        Value = item.Value,
                        Text = item.Text
                    })
                    .ToList();

                ViewBag.CourseList = courseItems;
                ViewBag.selectedCourse = Cour;

                foreach (var item in courseItems)
                {
                    if (item.Value == ViewBag.selectedCourse)
                    {
                        item.Selected = true;
                        break;
                    }
                }

                BindSyllabus(Cour);
                
                //Syllabus

                var syllabusItems = HttpContext.Session["syllabusItems"] as List<SelectListItem>;
                var BatchId = HttpContext.Session["BatchId"] as string;
                
                if (syllabusItems == null)
                {
                    syllabusItems = new List<SelectListItem>();
                }

                syllabusItems = syllabusItems
                    .Select(item => new SelectListItem
                    {
                        Value = item.Value,
                        Text = item.Text
                    })
                    .ToList();

                ViewBag.syllabusList = syllabusItems;
                ViewBag.selectedItemId = BatchId;


                foreach (var item in syllabusItems)
                {
                    if (item.Value == ViewBag.selectedItemId)
                    {
                        item.Selected = true;
                        break;
                    }
                }

                BindBranch(BatchId);

                //Branch
                var BranchItems = HttpContext.Session["BranchItems"] as List<SelectListItem>;
                var Branch = HttpContext.Session["Bran"] as string;
                if (BranchItems == null)
                {
                    BranchItems = new List<SelectListItem>();
                }


                BranchItems = BranchItems
                    .Select(item => new SelectListItem
                    {
                        Value = item.Value,
                        Text = item.Text
                    })
                    .ToList();

                ViewBag.BranchList = BranchItems;
                ViewBag.selectedBranch = Branch;

                foreach (var item in BranchItems)
                {
                    if (item.Value == ViewBag.selectedBranch)
                    {
                        item.Selected = true;
                        break;
                    }
                }

                BindSemester(Branch);

                //Semester
                var semesterItems = HttpContext.Session["SemesterItems"] as List<SelectListItem>;
                var SemId = HttpContext.Session["SemId"] as string;


                if (semesterItems == null)
                {
                    semesterItems = new List<SelectListItem>();
                }

                semesterItems = semesterItems
                    .Select(item => new SelectListItem
                    {
                        Value = item.Value,
                        Text = "Semester " + item.Text
                    })
                    .ToList();

                ViewBag.SemesterList = semesterItems;
                ViewBag.SelectedSemId = SemId;

                foreach (var item in semesterItems)
                {
                    if (item.Value == ViewBag.SelectedSemId)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

            }

            var studentInfoList = HttpContext.Session["studentInfoList"];

            if (studentInfoList == null)
            {
                return View();
            }
            return View(studentInfoList);
        }


        #region Course
        public ActionResult BindCourse()
        {
            var BatchId = HttpContext.Session["BatchId"] as string;
            DataTable dt = new DataTable();

            if (BatchId == null)
            {
                BatchId = "0";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Course", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    DataTable CourseList = dt;

                    List<SelectListItem> courseItems = CourseList.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Course"].ToString(),
                        Text = row["Course"].ToString()
                    }).ToList();
                    ViewBag.CourseList = courseItems;
                    HttpContext.Session["courseItems"] = courseItems;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return RedirectToAction("Index");
            //return dt;
        }
        #endregion

        #region Syllabus Year
        public ActionResult BindSyllabus(string Cour)
        {


            if (Cour == null)
            {
                Cour = "";
            }


            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Syllabus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cour", Cour);
                        con.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);

                        }
                    }

                    DataTable SyllabusList = dt;

                    List<SelectListItem> syllabusItems = SyllabusList.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Batchfrom"].ToString(),
                        Text = row["Batchfrom"].ToString()
                    }).ToList();

                    ViewBag.syllabusList = syllabusItems;


                    HttpContext.Session["syllabusItems"] = syllabusItems;
                    HttpContext.Session["Cour"] = Cour;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Branch
        public ActionResult BindBranch(string BatchId)
        {
           
            var Cour = HttpContext.Session["Cour"];


            if (Cour == null)
            {
                Cour = "";
            }

            if (BatchId == null)
            {
                BatchId = "0";
            }

          

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Branch", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.AddWithValue("@Cour", Cour);
                        cmd.Parameters.AddWithValue("@BatchId", BatchId);
                        con.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    
                    DataTable BranchList = dt;

                    List<SelectListItem> branchItems = BranchList.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Branch"].ToString(),
                        Text = row["Branch"].ToString()
                    }).ToList();

                    ViewBag.BranchList = branchItems;

                    HttpContext.Session["BranchItems"] = branchItems;
                    HttpContext.Session["BatchId"] = BatchId;


                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("Index");

        }
        #endregion

        #region Semester
        public ActionResult BindSemester(string Branch)
        {
            var Cour = HttpContext.Session["Cour"];
            var BatchId = HttpContext.Session["BatchId"];
            
            DataTable dt = new DataTable();

            if (Cour == null)
            {
                Cour = "";
            }

            if (BatchId == null)
            {
                BatchId = "0";
            }

            if (Branch == null)
            {
                Branch = "";
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Semester", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cour", Cour);
                        cmd.Parameters.AddWithValue("@BatchId", BatchId);
                        cmd.Parameters.AddWithValue("@Branch", Branch);
                        con.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                 
                    DataTable SemesterList = dt;

                    List<SelectListItem> semesterItems = SemesterList.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Semester"].ToString(),
                        Text = row["Semester"].ToString()
                    })
                    .ToList();

                    ViewBag.SemesterList = semesterItems;

                    HttpContext.Session["SemesterItems"] = semesterItems;

                    HttpContext.Session["Bran"] = Branch;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
        #endregion


        #region Branch
        public ActionResult GetSemester(string SemId)
        {

            HttpContext.Session["SemId"] = SemId;

            return RedirectToAction("Index");

        }
        #endregion

        [HttpPost]
        public ActionResult SubmitForm()
        {
            DataTable dt = new DataTable();

            var BatchId = HttpContext.Session["BatchId"] as string;
            var SemId = HttpContext.Session["SemId"] as string;
            var Cour = HttpContext.Session["Cour"] as string;
            var Branch = HttpContext.Session["Bran"] as string;

            if (BatchId == null)
            {
                BatchId = "";
            }
            if (Cour == null)
            {
                Cour = "";
            }

            if (Branch == null)
            {
                Branch = "";
            }

            if (SemId == null)
            {
                SemId = "";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_repStudent", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cour", Cour);
                        cmd.Parameters.AddWithValue("@BatchId", BatchId);
                        cmd.Parameters.AddWithValue("@Branch", Branch);
                        cmd.Parameters.AddWithValue("@SemId", SemId);
                        con.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    HttpContext.Session["studentInfoList"] = MapDataToStudentInfo(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        private List<StudentInfo> MapDataToStudentInfo(DataTable dt)
        {
            List<StudentInfo> studentInfoList = new List<StudentInfo>();
            DataTable studentInfoTbl = dt;

            foreach (DataRow row in studentInfoTbl.Rows)
            {
                StudentInfo studentInfo = new StudentInfo
                {
                    BR_TITLE = row["BR_TITLE"].ToString(),
                    Semester = row["Semester"].ToString(),
                    SubjectCODE = row["SubjectCODE"].ToString(),
                    SubjectTP = row["SubjectTP"].ToString(),
                    Reg_Le_Code = row["Reg_Le_Code"].ToString(),
                    SubType = row["SubType"].ToString(),
                    Credits = row["Credits"].ToString(),
                    MaxInternalMarks = row["MaxInternalMarks"].ToString(),
                    InternalPassMarks = row["InternalPassMarks"].ToString(),
                    MaxExternalMarks = row["MaxExternalMarks"].ToString(),
                    ExternalPassMarks = row["ExternalPassMarks"].ToString(),
                    MaxPracticalMarks = row["MaxPracticalMarks"].ToString(),
                    PracticalPassMarks = row["PracticalPassMarks"].ToString(),
                    PracticalCredits = row["PracticalCredits"].ToString(),
                    TotalCredits = row["TotalCredits"].ToString(),
                    BatchFrom = row["BatchFrom"].ToString(),
                    SubjectName = row["SubjectName"].ToString(),
                };

                studentInfoList.Add(studentInfo);
            }

            return studentInfoList;
        }

        public ActionResult ResetFunc()
        {
            ModelState.Clear();
            ClearSession();
            return RedirectToAction("Index");
        }
        private void ClearSession()
        {
            HttpContext.Session["BatchId"] = null;
            HttpContext.Session["SemId"] = null;
            HttpContext.Session["Cour"] = null;
            HttpContext.Session["Bran"] = null;
            HttpContext.Session["studentInfoList"] = null;
        }
    }
}