using ElevenNoteMVC.Models;
using ElevenNoteMVC.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNoteMVC.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            var model = service.GetNotes();
            return View(model);
        }

        //GET
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateNewService();
            if (service.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Note could not be created");
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateNewService();
            var model = svc.GetNoteById(id);
            return View(model);
        }

        private NoteService CreateNewService()
        {
            var userId = Guid.Parse(User.Identity?.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}