using AssessmentTool.Services;
using AssessmentTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentTool.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index(string search, int? page, int? items)
        {
            HomeViewModel model = new HomeViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Assessment Tool",
                PageDescription = ""
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 9;

            var quizzesSearch = QuizzesService.Instance.GetQuizzesForHomePage(model.searchTerm, model.pageNo, model.pageSize);

            model.Quizzes = quizzesSearch.Quizzes;
            model.TotalCount = quizzesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return View(model);
        }
    }
}