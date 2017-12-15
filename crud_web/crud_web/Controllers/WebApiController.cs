using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using crud_web.Models;
using crud_web.data;

namespace crud_web.Controllers
{
    /*
    A classe WebApiConfig pode requerer alterações adicionais para adicionar uma rota para esse controlador. Misture essas declarações no método Register da classe WebApiConfig conforme aplicável. Note que URLs OData diferenciam maiúsculas e minúsculas.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using crud_web.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Cliente>("WebApi");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class WebApiController : ODataController
    {
        private context db = new context();

        // GET: odata/WebApi
        [EnableQuery]
        public IQueryable<Cliente> GetWebApi()
        {
            return db.Cliente;
        }

        // GET: odata/WebApi(5)
        [EnableQuery]
        public SingleResult<Cliente> GetCliente([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cliente.Where(cliente => cliente.id == key));
        }

        // PUT: odata/WebApi(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Cliente> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cliente cliente = db.Cliente.Find(key);
            if (cliente == null)
            {
                return NotFound();
            }

            patch.Put(cliente);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cliente);
        }

        // POST: odata/WebApi
        public IHttpActionResult Post(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cliente.Add(cliente);
            db.SaveChanges();

            return Created(cliente);
        }

        // PATCH: odata/WebApi(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Cliente> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cliente cliente = db.Cliente.Find(key);
            if (cliente == null)
            {
                return NotFound();
            }

            patch.Patch(cliente);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cliente);
        }

        // DELETE: odata/WebApi(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Cliente cliente = db.Cliente.Find(key);
            if (cliente == null)
            {
                return NotFound();
            }

            db.Cliente.Remove(cliente);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteExists(int key)
        {
            return db.Cliente.Count(e => e.id == key) > 0;
        }
    }
}
