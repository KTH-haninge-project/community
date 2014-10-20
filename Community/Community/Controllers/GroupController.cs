using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Group
        public ActionResult Index()
        {
            List<Group> groups = db.Groups.ToList();
            List<GroupViewModel> viewmodels = new List<GroupViewModel>();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            foreach (Group group in groups)
            {
                GroupViewModel viewmodel = new GroupViewModel(group);
                if(group.Members.Contains(user)){
                    viewmodel.isMember=true;
                }
                else if(group.God.Equals(user)){
                    viewmodel.isMember=true;
                }
                else{
                    viewmodel.isMember=false;
                }
                viewmodels.Add(viewmodel);


            }


            return View(viewmodels);
        }

        // GET: Group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupViewModel viewmodel = new GroupViewModel(group);
            viewmodel.Members.Add(group.God.Email);
            viewmodel.Members.Add("Knatte");
            viewmodel.Members.Add("Fnatte");
            viewmodel.Members.Add("Tjatte");

            return View(viewmodel);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                Group group = new Group();
                group.Id = groupViewModel.Id;
                group.Name = groupViewModel.Name;
                group.God = db.Users.Find(User.Identity.GetUserId());
                group.Description = groupViewModel.Description;
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupViewModel);
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(new GroupViewModel(group));
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                Group group = db.Groups.Find(groupViewModel.Id);
                group.Name = groupViewModel.Name;
                group.Description = groupViewModel.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupViewModel);
        }

        // GET: Group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(new GroupViewModel(group));
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
