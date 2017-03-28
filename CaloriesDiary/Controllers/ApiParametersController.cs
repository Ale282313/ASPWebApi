using CaloriesDiary.Models;
using System.Web.Http;

namespace CaloriesDiary.Controllers
{
	public class ApiParametersController : ApiController
	{
		[Route("~/api/params")]
		public IHttpActionResult Get([FromUri]ApiParameters parameters)
		{
			if (parameters.Age != default(int) || parameters.Name != null)
				return Ok(new { Name = parameters.Name, Age = parameters.Age });
			return NotFound();
		}


		[Route("~/api/parameters")]
		public IHttpActionResult Post([FromBody]ApiParameters parameters)
		{
			if (parameters.Age != default(int) || parameters.Name != null)
				return Ok(new { Name = parameters.Name, Age = parameters.Age });
			return NotFound();
		}
	}
}
