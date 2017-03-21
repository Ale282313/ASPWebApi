using CaloriesDiary.Models;
using CaloriesDiary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace CaloriesDiary.Controllers
{
	[RoutePrefix("api/foods")]
	public class FoodsController : BaseApiController
	{
		public FoodsController(IRepository repo) : base(repo)
		{

		}
		const int _pageSize = 1;
		[Route("",Name ="Foodss")] // for the nextPage link
		public object Get(bool includeMeasures = true, int page = 0)
		{
			IQueryable<Food> query;
			if (includeMeasures == true)
				query = TheRepository.getFoods();
			else
				query = TheRepository.getFoodsWithoutMeasurements();

			var baseQuery = query.
				OrderBy(f => f.description);

			var totalCount = baseQuery.Count();
			var totalPages = Math.Ceiling((double)totalCount / _pageSize);
			var _helper = new UrlHelper(this.Request);
			List<Link> links = new List<Link>();
			if (page > 0)
			{
				links.Add(TheModelFactory.CreateLink(_helper.Link("Foodss", new { page = page - 1 }), "prevPage"));
			}
			if (page < totalPages - 1)
			{
				links.Add(TheModelFactory.CreateLink(_helper.Link("Foodss", new { page = page + 1 }), "nextPage"));
			}

			var result = baseQuery.
				Skip(_pageSize * page).
				Take(_pageSize).
				ToList().
				Select(f => TheModelFactory.Create(f));

			return new
			{
				TotalCount = totalCount,
				Pages = totalPages,
				Links = links,
				Result = result
			};
		}
		[Route("{id}", Name ="Foods")] // to give a name to the r
		public IHttpActionResult Get(int id)
		{
			var food = TheRepository.getFoodItem(id);
			return Versioned(TheModelFactory.Create(food));
		}
	}
}
