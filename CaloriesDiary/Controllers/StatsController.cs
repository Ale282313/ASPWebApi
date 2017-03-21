using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CaloriesDiary.Repository;
using CaloriesDiary.Services;
using System.Web.Http.Cors;

namespace CaloriesDiary.Controllers
{
	[RoutePrefix("api/stats")]
	//[EnableCors("*","*","GET")] // first=origin suported-* all second = headers third = methods(GET POST..); for the hole controller
	public class StatsController : BaseApiController
	{
		IIdentityService identityService;
		public StatsController(IRepository repo,IIdentityService service) : base(repo)
		{
			identityService = service;
		}
		//[Route("api/stats")]
		[Route("")]
		//[EnableCors("*", "*", "GET")] -for specific method
		//[DisableCors()]// turn off for a specific method
		public IHttpActionResult Get()
		{
			var result = new
			{
				NumFoods = TheRepository.getFoods().Count(),
				NumDiaries = TheRepository.getDiaries(identityService.CurrentUser).Count()
			};

			return Ok(result);
		}
		//[Route("api/stats/{id}")]
		[Route("~/api/status/{id:int}")]// - override so it won't use the RoutePrefix
		//[Route("{id}")]
		public IHttpActionResult Get(int id)
		{
			if (id == 1)
			{
				return Ok(new { NumFoods = TheRepository.getFoods().Count() });
			}
			if(id == 2)
			{
				return Ok(new {
					NumDiaries = TheRepository.getDiaries(identityService.CurrentUser).Count()
				});
			}
			return NotFound();
		}
		[Route("~/api/status/{name:alpha}")] //name:alpha is used for constraints
		public IHttpActionResult Get(string name)
		{
			if (name == "foods")
			{
				return Ok(new { NumFoods = TheRepository.getFoods().Count() });
			}
			if (name == "diaries")
			{
				return Ok(new
				{
					NumDiaries = TheRepository.getDiaries(identityService.CurrentUser).Count()
				});
			}
			return NotFound();
		}
	}
}
