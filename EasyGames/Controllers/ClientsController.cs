using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class ClientsController : Controller
    {
        private EasyGamesDatabase _context = new EasyGamesDatabase();

        // GET: Clients
        public ActionResult Index()
        {
            return View(_context.Clients.ToList());
        }

        public ActionResult Transactions(int clientId)
        {
            var transactions = _context.Transactions
                .Where(t => t.ClientID == clientId)
                .ToList();

            ViewBag.ClientId = clientId;  // Pass ClientId to the view for editing purposes
            return View(transactions);
        }

        [HttpPost]
        public ActionResult EditComment(int transactionId, string newComment)
        {
            // Find the transaction by ID
            var transaction = _context.Transactions.Find(transactionId);

            if (transaction == null)
            {
                // Redirect to an error page or show an error message if transaction is not found
                return RedirectToAction("Error", new { message = "Transaction not found." });
            }

            if (!string.IsNullOrWhiteSpace(newComment))
            {
                // Update the transaction comment
                transaction.Comment = newComment;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the transaction list for the selected client
                return RedirectToAction("Transactions", new { clientId = transaction.ClientID });
            }

            // If comment is invalid, reload the same page with an error
            TempData["ErrorMessage"] = "Comment cannot be empty.";
            return RedirectToAction("EditTransaction", new { transactionId = transactionId });
        }


        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = _context.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientID,Name,Surname,ClientBalance")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(clients);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clients);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = _context.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientID,Name,Surname,ClientBalance")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(clients).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clients);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = _context.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clients clients = _context.Clients.Find(id);
            _context.Clients.Remove(clients);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
