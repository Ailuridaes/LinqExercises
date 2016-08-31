using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class ShippersController : ApiController
    {
        private NORTHWNDEntities _db;

        public ShippersController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/shippers/reports/freight
        [HttpGet, Route("/api/shippers/reports/freight"), ResponseType(typeof(IQueryable<object>))]
        public IHttpActionResult GetFreightReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/
            var resultSet = _db.Shippers.Select(
                    s => new
                    {
                        Shipper = s,
                        FreightTotals = Math.Round((double)s.Orders.Sum(o => o.Freight))
                    }
                )
                .OrderByDescending(s => s.FreightTotals);

            return Ok(resultSet);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
