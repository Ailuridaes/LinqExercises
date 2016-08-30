using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class CustomersController : ApiController
    {
        private NORTHWNDEntities _db;

        public CustomersController()
        {
            _db = new NORTHWNDEntities();
        }

        // GET: api/customers/city/London
        [HttpGet, Route("api/customers/city/{city}"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetAll(string city)
        {
            var resultSet = _db.Customers.Where(c => c.City.Equals(city, StringComparison.OrdinalIgnoreCase));
            return Ok(resultSet);
        }

        // GET: api/customers/mexicoSwedenGermany
        [HttpGet, Route("api/customers/mexicoSwedenGermany"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetAllFromMexicoSwedenGermany()
        {
            String countries = "MexicoSwedenGermany";
            var resultSet = _db.Customers.Where(c => countries.Contains(c.Country));
            return Ok(resultSet);
        }

        // GET: api/customers/shippedUsing/Speedy Express
        [HttpGet, Route("api/customers/shippedUsing/{shipperName}"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetCustomersThatShipWith(string shipperName)
        {
            var shipper = _db.Shippers.Where(s => s.CompanyName.Equals(shipperName)).First();

            // query syntax
            //var resultSet = (from c in _db.Customers
            //                join o in _db.Orders
            //                on c.CustomerID equals o.CustomerID
            //                where o.ShipVia == shipper.ShipperID
            //                select c).Distinct();


            // method syntax
            var resultSet = _db.Customers
                .Join(_db.Orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { c, o })
                .Where(n => n.o.ShipVia == shipper.ShipperID)
                .Select(n => n.c)
                .Distinct();

            return Ok(resultSet);
        }

        // GET: api/customers/withoutOrders
        [HttpGet, Route("api/customers/withoutOrders"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetCustomersWithoutOrders()
        {
            var resultSet = _db.Customers
                .GroupJoin(_db.Orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { c, o })
                .Where(x => x.o.Count() == 0)
                .Select(x => x.c);

            return Ok(resultSet);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
