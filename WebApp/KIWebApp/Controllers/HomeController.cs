using KIWebApp.Classes;
using KIWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KIWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IDAL dal;

        public HomeController()
        {
            dal = new DAL();
        }

        public HomeController(IDAL dal)
        {
            this.dal = dal;
        }

        public ActionResult Index(string returnUrl)
        {
            return View();
        }

        public ActionResult Search(SearchModel model)
        {
            return View(dal.GetSearchResults(model.Query));
        }

        public ActionResult LearnMore()
        {
            return View();
        }

        public ActionResult PlayerLeaderboards()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NavigationExpanded(bool state)
        {
            Session["NavExpanded"] = state;
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult SearchServers(SearchModel model)
        {
            return PartialView("_ServerSearchResults", dal.GetServerSearchResults(model.Query));
        }

        [HttpGet]
        public ActionResult SearchPlayers(SearchModel model)
        {
            return PartialView("_PlayerSearchResults", dal.GetPlayerSearchResults(model.Query));
        }
    }
}