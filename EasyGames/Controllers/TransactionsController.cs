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
    public class TransactionsController : Controller
    {
        private readonly EasyGamesDatabase _context = new EasyGamesDatabase();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = _context.Transactions.Include(t => t.Client).Include(t => t.TransactionType);
            return View(transactions.ToList());
        }

        public ActionResult AddTransaction(int clientId)
        {
            // Pass TransactionTypes to ViewBag for dropdown
            ViewBag.TransactionTypes = _context.TransactionTypes.ToList();

            var transaction = new Transaction { ClientID = clientId };
            return View(transaction);
        }


        [HttpPost]
        public ActionResult AddTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Add the transaction to the database
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                // Update the client's balance
                var client = _context.Clients.Find(transaction.ClientID);
                if (client != null)
                {
                    // Assuming TransactionTypeID 1 is Debit and 2 is Credit
                    if (transaction.TransactionTypeID == 1) // Debit
                    {
                        client.ClientBalance -= transaction.Amount;
                    }
                    else if (transaction.TransactionTypeID == 2) // Credit
                    {
                        client.ClientBalance += transaction.Amount;
                    }
                    _context.SaveChanges(); // Save the updated balance
                }

                return RedirectToAction("Transactions", new { clientId = transaction.ClientID });
            }

            // If model state is invalid, return to the view
            ViewBag.TransactionTypes = _context.TransactionTypes.ToList();
            return View(transaction);
        }

        public JsonResult GetClientBalance(int clientId)
        {
            var client = _context.Clients.Find(clientId);
            if (client != null)
            {
                return Json(new { newBalance = client.ClientBalance }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { newBalance = 0 }, JsonRequestBehavior.AllowGet);
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(_context.Clients, "ClientID", "Name");
            ViewBag.TransactionTypeID = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionID,ClientID,Amount,Comment,TransactionTypeID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(_context.Clients, "ClientID", "Name", transaction.ClientID);
            ViewBag.TransactionTypeID = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName", transaction.TransactionTypeID);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(_context.Clients, "ClientID", "Name", transaction.ClientID);
            ViewBag.TransactionTypeID = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName", transaction.TransactionTypeID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionID,ClientID,Amount,Comment,TransactionTypeID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(_context.Clients, "ClientID", "Name", transaction.ClientID);
            ViewBag.TransactionTypeID = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName", transaction.TransactionTypeID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = _context.Transactions.Find(id);
            _context.Transactions.Remove(transaction);
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
