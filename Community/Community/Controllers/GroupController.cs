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
using System.Diagnostics;
using Community.ViewModels;

namespace Community.Controllers
{
    /// <summary>
    /// Controlls actions regarding groups
    /// </summary>
    public class GroupController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Show list of all groups
        /// </summary>
        /// <returns>List of all groups</returns>
        public ActionResult Index()
        {
            List<Group> groups = db.Groups.ToList();
            List<GroupViewModel> viewmodels = new List<GroupViewModel>();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            foreach (Group group in groups)
            {
                GroupViewModel viewmodel = new GroupViewModel(group);
                if (group.Owner.Equals(user))
                {
                    viewmodel.isOwner = true;
                }
                if(group.Members.Contains(user)){
                    viewmodel.isMember=true;
                }
                else{
                    viewmodel.isMember=false;
                }
                viewmodels.Add(viewmodel);


            }


            return View(viewmodels);
        }

        /// <summary>
        /// Shows specific group
        /// </summary>
        /// <param name="id">ID of grup</param>
        /// <returns>Group data</returns>
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
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            GroupViewModel viewmodel = new GroupViewModel(group);
            if (group.Members.Contains(user) || group.Owner.Equals(user))
            {
                viewmodel.isMember = true;
            }
            return View(viewmodel);
        }

        /// <summary>
        /// Shows page to create group
        /// </summary>
        /// <returns>Create group page view</returns>
        public ActionResult Create()
        {
            return View();
        }

       /// <summary>
       /// Creates group
       /// </summary>
       /// <param name="groupViewModel">Viewmodel of Group</param>
       /// <returns>Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                Group group = new Group();
                group.Id = groupViewModel.Id;
                group.Name = groupViewModel.Name;
                group.Owner = db.Users.Find(User.Identity.GetUserId());
                group.AddMember(group.Owner);
                group.Description = groupViewModel.Description;
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupViewModel);
        }

        /// <summary>
        /// Shows group edit page if user is owner of the group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns></returns>
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

            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            if (!group.Owner.Equals(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(new GroupViewModel(group));
        }

        /// <summary>
        /// Edits group if user is owner of the group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                Group group = db.Groups.Find(groupViewModel.Id);
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (!group.Owner.Equals(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                group.Name = groupViewModel.Name;
                group.Description = groupViewModel.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupViewModel);
        }

        /// <summary>
        /// Shows group delete page if user is owner of the group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns></returns>
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
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (!group.Owner.Equals(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(new GroupViewModel(group));
        }

        /// <summary>
        /// Deletes group if user is owner of the group
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (!group.Owner.Equals(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Makes user join group if not already a member
        /// </summary>
        /// <param name="groupViewModel"></param>
        /// <returns></returns>
        public ActionResult JoinGroup(GroupViewModel groupViewModel)
        {
            if (groupViewModel== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(groupViewModel.Id);
            if (group == null)
            {
                return HttpNotFound();
            }
            Debug.WriteLine("Number of members in group before add: " + group.Members.Count);

            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            if (!group.Members.Contains(user))
            {
                group.AddMember(user);
                Debug.WriteLine("Number of members in group after add: " + group.Members.Count);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Makes user leave group if the user is member of the grup
        /// </summary>
        /// <param name="groupViewModel"></param>
        /// <returns></returns>
        public ActionResult LeaveGroup(GroupViewModel groupViewModel)
        {
            if (groupViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(groupViewModel.Id);
            if (group == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            if (group.Members.Contains(user))
            {
                group.RemoveMember(user);
                db.SaveChanges();
            }
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
