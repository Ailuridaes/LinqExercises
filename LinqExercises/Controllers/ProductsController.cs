using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class ProductsController : ApiController
    {
        private NORTHWNDEntities _db;

        public ProductsController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/products/discontinued/count
        [HttpGet, Route("api/products/discontinued/count"), ResponseType(typeof(int))]
        public IHttpActionResult GetDiscontinuedCount()
        {
            var count = _db.Products.Where(p => p.Discontinued).Count();

            return Ok(count);
        }

        //GET: api/products/discontinued
        [HttpGet, Route("api/products/discontinued"), ResponseType(typeof(int))]
        public IHttpActionResult GetDiscontinued()
        {
            var resultSet = _db.Products.Where(p => p.Discontinued);

            return Ok(resultSet);
        }

        // GET: api/categories/Condiments/products
        [HttpGet, Route("api/categories/{categoryName}/products"), ResponseType(typeof(IQueryable<Product>))]
        public IHttpActionResult GetProductsInCategory(string categoryName)
        {
            var resultSet = _db.Products.Where(p => p.Category.CategoryName.Equals(categoryName));

            return Ok(resultSet);
        }

        // GET: api/products/reports/stock
        [HttpGet, Route("api/products/reports/stock"), ResponseType(typeof(IQueryable<object>))]
        public IHttpActionResult GetStockReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/

            var resultSet = _db.Products
                .Select(
                    p => new
                    {
                        Product = p,
                        TotalStockUnits = p.UnitsInStock + p.UnitsOnOrder
                    }
                )
                .Where(p => p.TotalStockUnits >= 100);

            return Ok(resultSet);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
