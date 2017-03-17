using CaloriesDiary.Models;
using CaloriesDiary.Repository;
using CaloriesDiary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CaloriesDiary.Controllers
{
	public class DiarySummaryController : BaseApiController
	{
		private IIdentityService _identityService;

		public DiarySummaryController(IRepository repository, IIdentityService identityService) : base(repository)
		{
			_identityService = identityService;
		}
		public object Get(DateTime id)
		{
			try
			{
				var diary = TheRepository.getDiary(_identityService.CurrentUser, id);
				if( diary == null)
				{
					return Request.CreateResponse(HttpStatusCode.NotFound);
				}
				return TheModelFactory.CreateSummary(diary);
			}
			catch (Exception ex)
			{

				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.StackTrace);
			}
			
		}
	}
}
