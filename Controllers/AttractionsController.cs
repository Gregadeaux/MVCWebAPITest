using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GuildWars2API.Models;

namespace GuildWars2API.Controllers
{
    public class AttractionsController : ApiController
    {
        private TourismContext db = new TourismContext();

        // GET api/Default1
        public IEnumerable<TouristAttraction> GetTouristAttractions()
        {
            return db.TouristAttractions.AsEnumerable();
        }

        // GET api/Default1/5
        public TouristAttraction GetTouristAttraction(int id)
        {
            TouristAttraction touristattraction = db.TouristAttractions.Find(id);
            if (touristattraction == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return touristattraction;
        }

        public TouristAttraction GetTouristAttraction(double longitude, double latitude)
        {
            var location = DbGeography.FromText(
                string.Format("POINT ({0} {1})", longitude, latitude));

            var query = from a in db.TouristAttractions
                        orderby a.Location.Distance(location)
                        select a;

            return query.FirstOrDefault();
        }

        // PUT api/Default1/5
        public HttpResponseMessage PutTouristAttraction(int id, TouristAttraction touristattraction)
        {
            if (ModelState.IsValid && id == touristattraction.TouristAttractionId)
            {
                db.Entry(touristattraction).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Default1
        public HttpResponseMessage PostTouristAttraction(TouristAttraction touristattraction)
        {
            if (ModelState.IsValid)
            {
                db.TouristAttractions.Add(touristattraction);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, touristattraction);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = touristattraction.TouristAttractionId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Default1/5
        public HttpResponseMessage DeleteTouristAttraction(int id)
        {
            TouristAttraction touristattraction = db.TouristAttractions.Find(id);
            if (touristattraction == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.TouristAttractions.Remove(touristattraction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, touristattraction);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}