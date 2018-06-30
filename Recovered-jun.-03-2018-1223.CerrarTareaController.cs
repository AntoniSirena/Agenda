using Agenda.Models;
using Agenda.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Agenda.Controllers
{
    public class CerrarTareaController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();
        private SqlDbType id;




        // GET: CerrarTarea
        public ActionResult Index()
        {

            var JoinCerrarTarea = (from E in db.Estatus 
                                   join CT in db.Tareas on E.Id equals CT.EstatuId
                                   where E.Descripcion == "Activa"
                             select new
                             {
                                CT.Id,
                                CT.Titulo,
                                E.Descripcion,
                                CT.FechaOrigen,
                                CT.FechaEjecución

                             }).ToList();


            var FechaActual = DateTime.Now.ToLocalTime();
           
         
            var CerrarTarea = JoinCerrarTarea.Select(x => new ViewModelCerrarTarea()
            {

                Id = x.Id,
                Titulo = x.Titulo,
                DescripcionEstatu = x.Descripcion,
                FechaOrigen = x.FechaOrigen,
                FechaEjecución = x.FechaEjecución,
                TiempoEstimado = ((x.FechaEjecución - x.FechaOrigen).Days),
                TiempoVigencia = ((FechaActual - x.FechaOrigen).Days),

            });


            return View(CerrarTarea);
        }



        // GET: CerrarTarea/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: CerrarTarea/Create
        public ActionResult Create(int id)
        {
            CerrarTarea cerrarTarea = new  CerrarTarea();

            cerrarTarea.TareaId = id;


            return View(cerrarTarea);
        }

        // POST: CerrarTarea/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TareaId,FechaCierre,DescripcionTareaCierre")] CerrarTarea cerrarTarea)
        {

            if (ModelState.IsValid)
            {
                db.CerrarTareas.Add(cerrarTarea);
                db.SaveChanges();
                db.Database.SqlQuery<Tarea>("SP_UpdateEstatuTarea @id",
                            new SqlParameter("@id",id));

                return RedirectToAction("Index");
            }
            return View(cerrarTarea);

        }


        // GET: CerrarTarea/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CerrarTarea/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CerrarTarea/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CerrarTarea/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
