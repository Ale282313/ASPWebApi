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
	public class DiaryEntriesController : BaseApiController
	{
		private IIdentityService _identityService;
		public DiaryEntriesController(IRepository repo, IIdentityService identityService) : base(repo)
		{
			_identityService = identityService;
		}
		public IEnumerable<Models.DiaryEntry> Get(DateTime diaryid)
		{

			var diaryEntries = TheRepository.GetDiaryEntries(diaryid).
					ToList().
					Select(de => TheModelFactory.Create(de));
			return diaryEntries;

		}
		public HttpResponseMessage Get(DateTime diaryid, int id)
		{

			var result = TheRepository.getDiaryEntryItem(diaryid, id);
			if (result == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}
			return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
		}
		public HttpResponseMessage Post(DateTime diaryid, [FromBody]Models.DiaryEntry model)
		{
			try
			{
				var entity = TheModelFactory.Parse(model);
				if(entity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not create diary entry in body");
				}
				var diary = TheRepository.getDiary(_identityService.CurrentUser, diaryid);
				if( diary == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Diary not found");
				}
				// Make sure that entry is not duplicate
				if(diary.DiaryEntries.Any(e=>e.Measure.id == entity.Measure.id))
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Mesure duplicate not allowed");
				}
				//Save the new Entry
				diary.DiaryEntries.Add(entity);
				if (TheRepository.SaveAll()==1)
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

				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.StackTrace);
			}
		}
		public HttpResponseMessage Delete(DateTime diaryid, int id)
		{
			try
			{
				if (TheRepository.GetDiaryEntries(diaryid).Any(e => e.id == id) == false)
				{
					return Request.CreateResponse(HttpStatusCode.NotFound);
				}
				if ( TheRepository.DeleteDiaryEntry(diaryid,id))
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
		[HttpPatch]
		[HttpPut]
		public HttpResponseMessage Patch( DateTime diaryid, int id, [FromBody]Models.DiaryEntry model)
		{
			try
			{
				var entry = TheRepository.getDiaryEntryItem(diaryid, id);
				if( entry == null)
				{
					return Request.CreateResponse(HttpStatusCode.NotFound);
				}
				var parsedValue = TheModelFactory.Parse(model);
				if (parsedValue.quantity == null)
					return Request.CreateResponse(HttpStatusCode.BadRequest);
				if(entry.quantity != parsedValue.quantity)
				{
					entry.quantity = parsedValue.quantity;
					if (TheRepository.SaveAll() == 1)
					{
						return Request.CreateResponse(HttpStatusCode.OK);
					}
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.NotModified);
				}
				return Request.CreateResponse(HttpStatusCode.BadRequest);

			}
			catch (Exception ex)
			{

				return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.StackTrace);
			}
		}
	}
}
