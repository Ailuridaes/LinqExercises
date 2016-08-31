using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class OrdersController : ApiController
    {
        private NORTHWNDEntities _db;

        public OrdersController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/orders/between/01.01.1997/12.31.1997
        [HttpGet, Route("api/orders/between/{startDate}/{endDate}"), ResponseType(typeof(IQueryable<Order>))]
        public IHttpActionResult GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            var resultsSet = _db.Orders.Where(o => o.RequiredDate <= endDate && o.RequiredDate >= startDate);
            return Ok(resultsSet);
        }

        //GET: api/orders/between/01.01.1997/12.31.1997
        [HttpGet, Route("api/orders/smallbetween/{startDate}/{endDate}"), ResponseType(typeof(IQueryable<Order>))]
        public IHttpActionResult GetSmallOrdersBetween(DateTime startDate, DateTime endDate)
        {
            var resultsSet = _db.Orders.Where(o => o.RequiredDate <= endDate && o.RequiredDate >= startDate && o.Freight < 100);
            return Ok(resultsSet);
        }

        //GET: api/orders/reports/purchase
        [HttpGet, Route("api/orders/reports/purchase"), ResponseType(typeof(IQueryable<object>))]
        public IHttpActionResult PurchaseReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/
            // query to return an array of anonymous objects that have two properties. A Product property and the quantity ordered for that product labelled as 'QuantityPurchased' ordered by QuantityPurchased in descending order.

            // To sum historical quantity purchased over all orders
            // Saving old Join and GroupJoin methods for posterity :)
            //var resultSet = _db.Products
            //    .Join(_db.Order_Details,
            //        p => p.ProductID,
            //        o => o.ProductID,
            //        (p, o) => new { p, o })
            //    .GroupBy(x => x.p)
            //    .Select(
            //        x => new
            //        {
            //            Product = x.Key,
            //            QuantityPurchased = x.Sum(y => y.o.Quantity)
            //        }
            //    ).OrderByDescending(x => x.QuantityPurchased);

            //var resultSet = _db.Products
            //    .GroupJoin(_db.Order_Details,
            //        p => p.ProductID,
            //        o => o.ProductID,
            //        (p, o) => new { p, o })
            //    .Select(
            //        x => new
            //        {
            //            Product = x.p,
            //            QuantityPurchased = x.o.Sum(y => y.Quantity)
            //        }
            //    ).OrderByDescending(x => x.QuantityPurchased);


            var resultSet = _db.Products.Select(
                    x => new
                    {
                        Product = x,
                        QuantityPurchased = x.Order_Details.Sum(y => y.Quantity)
                    }
                ).OrderByDescending(x => x.QuantityPurchased);

            return Ok(resultSet);

        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
