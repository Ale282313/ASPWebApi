using CaloriesDiary.Repository;
using CaloriesDiary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace CaloriesDiary.Controllers
{
	public class DiariesController : BaseApiController
	{
		private IIdentityService _identityService;

		public DiariesController(IRepository repository, IIdentityService identityService) : base(repository)
		{
			_identityService = identityService;
		}
		[EnableQuery] // install package Install-Package Microsoft.AspNet.WebApi.OData -Version 5.3.1
		public IQueryable<Models.Diary> Get()
		{
			var _username = _identityService.CurrentUser;
			var diaries =TheRepository.getDiaries(_username);
			return diaries;
		}
		public HttpResponseMessage Get(DateTime id)
		{
			var username = _identityService.CurrentUser;
			var result = TheRepository.getDiaryItem(username, id);
			if (result == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}
			return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
		}
		public HttpResponseMessage Post([FromBody]Models.Diary model)
		{
			try
			{
				var entity = TheModelFactory.Parse(model);
				if (entity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not create diary in body");
				}
				// Make sure that entry is not duplicate
				if (TheRepository.getDiaries(_identityService.CurrentUser).Any(d => d.Date == entity.date))
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, " duplicate diary not allowed");
				}
				//Save the new Entry
				bool answer = TheRepository.InsertDiary(entity);
				if (answer)
				{
					return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
				}
				else
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
				}
			}
			catch (Exception ex)
			{

				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Exception from catch!!!!!!!!!" + ex.StackTrace);
			}
		}
		public HttpResponseMessage Delete(DateTime id)
		{
			try
			{
				if (!TheRepository.getDiaries(_identityService.CurrentUser).Any(e => e.Date == id))
					return Request.CreateResponse(HttpStatusCode.NotFound);
				if (TheRepository.DeleteDiary(id))
				{
					return Request.CreateResponse(HttpStatusCode.OK);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.BadRequest);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.StackTrace);
			}
		}
	}
}
