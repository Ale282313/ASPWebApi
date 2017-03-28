using CaloriesDiary.Repository;
using System.Collections.Generic;
using System.Linq;

namespace CaloriesDiary.Controllers
{
	public class MeasuresController : BaseApiController
	{
		public MeasuresController(IRepository repo) : base(repo)
		{
		}
		public IEnumerable<Models.Measure> Get(int foodid)
		{
			var results = TheRepository.getMeasuresForFood(foodid).ToList().Select(m => TheModelFactory.Create(m));
			return results;
		}
		public Models.Measure Get(int foodid, int id, int id1)
		{
			return TheModelFactory.Create(TheRepository.getSpecificMeasureforFood1(foodid, id, id1));
		}
	}
}
