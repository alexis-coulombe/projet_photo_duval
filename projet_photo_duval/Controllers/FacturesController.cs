﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projet_photo_duval.Models;
using PagedList;
using projet_photo_duval.ViewModels;
using static projet_photo_duval.MetaData.Facture;
using System.ComponentModel.DataAnnotations;
using projet_photo_duval.DAL;

namespace projet_photo_duval.Controllers
{
    [MetadataType(typeof(FactureMetaData))]
    public partial class FacturesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Factures
        public ActionResult Index(string ordreTri, string dateFiltre, string filtreCourantNom, string filtreCourantDate, int? page, string MessageError)
        {
            ViewBag.TriStatut = string.IsNullOrEmpty(ordreTri) ? "statut_desc" : "";
            ViewBag.TriDate = ordreTri == "date" ? "date_desc" : "date";
            ViewBag.MessageError = "";
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            if (dateFiltre != null)
            {
                page = 1;
            }
            else
            {
                dateFiltre = filtreCourantDate;
            }

            ViewBag.triCourant = ordreTri;
            ViewBag.filtreCourantDate = dateFiltre;

            var facture = unitOfWork.FactureRepository.Get(includeProperties: "Seance", filter: x => x.DateFacturation.Year == currentYear && x.DateFacturation.Month == currentMonth);

            if (!string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    facture = facture.Where(x => x.DateFacturation.Year == date.Year && x.DateFacturation.Month == date.Month);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas valide";
                }
            }

            switch (ordreTri)
            {
                case "statut_desc":
                    facture = facture.OrderByDescending(x => x.EstPayee);
                    break;
                case "date":
                    facture = facture.OrderBy(x => x.DateFacturation);
                    break;
                case "date_desc":
                    facture = facture.OrderByDescending(x => x.DateFacturation);
                    break;
                default:
                    facture = facture.OrderBy(x => x.EstPayee);
                    break;
            }

            int pageNo = page ?? 1;
            int taillePage = 5;

            return View(facture.ToPagedList(pageNo, taillePage));
        }

        // GET: Factures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var res = db.Facture.Find(id);

            //var res = from facture in db.Facture
            //          join agent in db.Agent on facture.Seance.Agent_ID equals agent.Agent_ID
            //          join photographe in db.Photographe on facture.Seance.Photographe_ID equals photographe.Photographe_ID
            //          where facture.Facture_ID == id
            //          select new ViewModelDetailsFacture { nomAgent = agent.Nom + ", " + agent.Prenom, nomPhotographe = photographe.Nom + ", " + photographe.Prenom, prix = facture.Prix, estPayee = facture.EstPayee, adresse = facture.Seance.Adresse, date = facture.DateFacturation };

            var res = unitOfWork.FactureRepository.Get(includeProperties:"Seance")
                      .SingleOrDefault(x => x.Facture_ID == id);

            ViewModelDetailsFacture details = new ViewModelDetailsFacture { nomAgent = res.Seance.Agent.Nom + ", " + res.Seance.Agent.Prenom, nomPhotographe = res.Seance.Photographe.Nom + ", " + res.Seance.Photographe.Prenom, prix = (decimal)res.Prix, estPayee = res.EstPayee, adresse = res.Seance.Adresse, date = res.Seance.DateSeance };

            if (res == null)
            {
                return HttpNotFound();
            }

            return View(details);

        //if (id == null)
        //{
        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}
        //Facture facture = db.Facture.Find(id);
        //if (facture == null)
        //{
        //    return HttpNotFound();
        //}
        //return View(facture);
       }

        // GET: Factures/Create
        public ActionResult Create()
        {
            ViewBag.Seance_ID = new SelectList(unitOfWork.SeanceRepository.Get(), "Seance_ID", "Adresse");
            return View();
        }

        // POST: Factures/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Facture_ID,Seance_ID,Prix,EstPayee,DateFacturation,Forfait,Commentaire")] Facture facture)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FactureRepository.Insert(facture);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Seance_ID = new SelectList(unitOfWork.SeanceRepository.Get(), "Seance_ID", "Adresse", facture.Seance_ID);
            return View(facture);
        }

        // GET: Factures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facture facture = unitOfWork.FactureRepository.GetByID(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            ViewBag.Seance_ID = new SelectList(unitOfWork.SeanceRepository.Get(), "Seance_ID", "Adresse", facture.Seance_ID);
            return View(facture);
        }

        // POST: Factures/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Facture_ID,Seance_ID,Prix,EstPayee,DateFacturation,Forfait,Commentaire")] Facture facture)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FactureRepository.UpdateEntry(facture);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Seance_ID = new SelectList(unitOfWork.SeanceRepository.Get(), "Seance_ID", "Adresse", facture.Seance_ID);
            return View(facture);
        }

        // GET: Factures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facture facture = unitOfWork.FactureRepository.GetByID(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facture facture = unitOfWork.FactureRepository.GetByID(id);
            unitOfWork.FactureRepository.Delete(facture);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
