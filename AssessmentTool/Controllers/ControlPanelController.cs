using AssessmentTool.Data;
using AssessmentTool.Services;
using AssessmentTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AssessmentTool.Entities;
using AssessmentTool.Commons;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace AssessmentTool.Controllers
{
    [CustomAuthorize(Roles = "Administrator")]
    public class ControlPanelController : BaseController
    {
        public ActionResult Index(bool? isPartial = false)
        {
            ControlPanelViewModel model = new ControlPanelViewModel();
            
            model.PageInfo = new PageInfo()
            {
                PageTitle = "Control Panel",
                PageDescription = "Manage AssessmentTool."
            };

            return View(model);
        }

        public ActionResult Users(string search, int? page, int? items)
        {
            UsersListingViewModel model = new UsersListingViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Users",
                PageDescription = "AssessmentTool Users."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var usersSearch = UsersService.Instance.GetUsersWithRoles(model.searchTerm, model.pageNo, model.pageSize);

            model.Users = usersSearch.Users;
            model.TotalCount = usersSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return PartialView("_Users", model);
        }
        
        public ActionResult UserDetails(string ID)
        {
            UserDetailsViewModel model = new UserDetailsViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "User Details",
                PageDescription = "User Details"
            };
            
            model.User = UsersService.Instance.GetUserWithRolesByID(ID);
            model.AvailableRoles = ControlPanelService.Instance.GetAllRoles();

            //remove roles from dropdown which are already with the user 
            foreach (var userRole in model.User.Roles)
            {
                var availableRole = model.AvailableRoles.Where(x => x.Id.Equals(userRole.Id)).FirstOrDefault();

                if(availableRole != null)
                {
                    model.AvailableRoles.Remove(availableRole);
                }
            }

            return PartialView("_UserDetails", model);
        }

        public ActionResult Roles(string search, int? page, int? items)
        {
            RolesListingViewModel model = new RolesListingViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Roles",
                PageDescription = "AssessmentTool Roles."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var rolesSearch = ControlPanelService.Instance.GetRoles(model.searchTerm, model.pageNo, model.pageSize);

            model.Roles = rolesSearch.Roles;
            model.TotalCount = rolesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return PartialView("_Roles", model);
        }

        public ActionResult NewRole(string ID)
        {
            NewRoleViewModel model = new NewRoleViewModel();

            return PartialView("_NewRole", model);
        }

        public ActionResult NewUser(string ID)
        {
            NewUserViewModel model = new NewUserViewModel();

            return PartialView("_NewUser", model);
        }

        private AssessmentToolSignInManager _signInManager;
        private AssessmentToolUserManager _userManager;

        public AssessmentToolSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AssessmentToolSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AssessmentToolUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AssessmentToolUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewUser(NewUserViewModel model)
        {
            string pss = CreateRandomPassword();
            sendmail(model.Name, pss);
           
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (!ModelState.IsValid)
            {
                var Errors = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Errors.Add(error.ErrorMessage.ToString());
                    }
                }

                result.Data = new { Success = false, Errors = Errors };

                return result;
            }
            else
            {
                try
                {
                    var user = new AssessmentToolUser { UserName = model.Name, Email = model.Email, RegisteredOn = DateTime.Now };

                  result.Data = new { Success = await UserManager.CreateAsync(user, pss) };
                }
                catch (Exception ex)
                {
                    result.Data = new { Success = false, Errors = ex.InnerException.Message };
                }
            }

            return result;
        }


        public void sendmail(string To, string pss)
        {
            try
            {
                using (MailMessage mm = new MailMessage())
                {
                    var verifyUrl = "https://localhost:44394/attempt-quiz/6";
                    mm.Subject = "Innova Online Skill test";
                    //mm.Body = "hi";
                    MailAddress fromaddress = new MailAddress("sabilasha50@gmail.com");
                    mm.From = fromaddress;
                    //Adding email id of receiver link
                    mm.To.Add("hanuabilasha@gmail.com");
                    //mm.IsBodyHtml = false;
                    //             mm.Body = "<br/><br/>We are excited to tell you that your account is" +
                    //" successfully created. Please click on the below link to verify your account" +
                    //" <br/><br/><a href='" + verifyUrl + "'>" + verifyUrl + "</a> ";
                   
                    //string body = "Dear " + "Abilasha" + ",";
                    string body = "Dear " + To + ",";
                    body += "<br /><br />Thank you for applying Innova";
                    body += "<br />You’re invited to complete an online skill assessment as part of our hiring process. Click the link to start the test";
                    body += "'" + verifyUrl + "' ";
                    body += "<br />UserID : " + To + "";
                    body += "<br />Password : " + pss+" ";
                    body += "<br /><br />Regards,";
                    body += "<br />Innova Recruitment team";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("sabilasha50@gmail.com", "Lordshiva@70");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        ViewBag.Message = "Email sent.";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static string CreateRandomPassword(int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        [HttpPost]
        public async Task<JsonResult> NewRole(NewRoleViewModel model)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!ModelState.IsValid)
            {
                var Errors = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Errors.Add(error.ErrorMessage.ToString());
                    }
                }

                result.Data = new { Success = false, Errors = Errors };

                return result;
            }
            else
            {
                try
                {
                    result.Data = new { Success = await ControlPanelService.Instance.NewRole(new IdentityRole() { Name = model.Name })};
                }
                catch (Exception ex)
                {
                    result.Data = new { Success = false, Errors = ex.InnerException.Message };
                }
            }

            return result;
        }

        [HttpPost]
        public async Task<JsonResult> AddUserRole(string userID, string roleID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                result.Data = new { Success = await ControlPanelService.Instance.AddUserRole(userID, roleID) };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Errors = ex.InnerException.Message };
            }

            return result;
        }

        [HttpPost]
        public async Task<JsonResult> RemoveUserRole(string userID, string roleID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                result.Data = new { Success = await ControlPanelService.Instance.RemoveUserRole(userID, roleID) };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Errors = ex.InnerException.Message };
            }

            return result;
        }

        public ActionResult RoleDetails(string ID)
        {
            RoleDetailsViewModel model = new RoleDetailsViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Role Details",
                PageDescription = "Role Details"
            };
            
            model.Role = ControlPanelService.Instance.GetRoleByID(ID);

            return PartialView("_RoleDetails", model);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRole(UpdateRoleViewModel model)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!ModelState.IsValid)
            {
                var Errors = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Errors.Add(error.ErrorMessage.ToString());
                    }
                }

                result.Data = new { Success = false, Errors = Errors };

                return result;
            }
            else
            {
                try
                {
                    await ControlPanelService.Instance.UpdateRole(new IdentityRole() { Id = model.ID, Name = model.Name });

                    result.Data = new { Success = true };
                }
                catch (Exception ex)
                {
                    result.Data = new { Success = false, Errors = ex.InnerException };
                }
            }

            return result;
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRole(string ID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (string.IsNullOrEmpty(ID))
            {
                result.Data = new { Success = false, Errors = "Role can not be identified." };

                return result;
            }
            else
            {
                try
                {
                    await ControlPanelService.Instance.DeleteRole(ID);

                    result.Data = new { Success = true };
                }
                catch (Exception ex)
                {
                    result.Data = new { Success = false, Errors = ex.Message };
                }
            }

            return result;
        }


        public ActionResult Quizzes(string search, int? page, int? items)
        {
            QuizListViewModel model = new QuizListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Quizzes",
                PageDescription = "List of Quizzes."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var quizzesSearch = QuizzesService.Instance.GetQuizzes(model.searchTerm, model.pageNo, model.pageSize);

            model.Quizzes = quizzesSearch.Quizzes;
            model.TotalCount = quizzesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);
            
            return PartialView("_Quizzes", model);
        }

        public ActionResult Attempts(string search, int? page, int? items)
        {
            QuizAttemptsListViewModel model = new QuizAttemptsListViewModel();
            
            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var resultsSearch = StudentQuizzesService.Instance.GetQuizAttempts(model.searchTerm, model.pageNo, model.pageSize);

            model.StudentQuizzes = resultsSearch.StudentQuizzes;
            model.TotalCount = resultsSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return PartialView("_Attempts", model);
        }

        public ActionResult CandidateRegistraion(string search, int? page, int? items)
        {
            QuizListViewModel model = new QuizListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Quizzes",
                PageDescription = "List of Quizzes."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var quizzesSearch = QuizzesService.Instance.GetQuizzes(model.searchTerm, model.pageNo, model.pageSize);

            model.Quizzes = quizzesSearch.Quizzes;
            model.TotalCount = quizzesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return PartialView("_CandidateRegistraion", model);
        }
    }
}