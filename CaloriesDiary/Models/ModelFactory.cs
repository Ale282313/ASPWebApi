using CaloriesDiary.Repository;
using CaloriesDiary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace CaloriesDiary.Models
{
	public class ModelFactory
	{
		private UrlHelper _urlHelper;
		private IRepository _repo;
		private IIdentityService _identifyService;

		public ModelFactory(HttpRequestMessage request, IRepository repo, IIdentityService service)
		{
			_urlHelper = new UrlHelper(request);
			_repo = repo;
			_identifyService = service;
		}
		internal Diary Create(CaloriesDiary.Diary d)
		{
			return new Diary()
			{
				Links = new List<Link>()
				{
					CreateLink(_urlHelper.Link("Diaries", new { id = d.date.ToString("yyyy-MM-dd") }),"self"),
					CreateLink(_urlHelper.Link("Entries", new { diaryid = d.date.ToString("yyyy-MM-dd") }),"newDiaryEntry","POST")
				},
				Date = d.date,
				Username = d.username,
				Entries = d.DiaryEntries.Select(de => Create(de))

			};
		}

		public Link CreateLink(string href, string rel, string method = "GET", bool isTemplated = false)
		{
			return new Link
			{
				Href = href,
				Rel = rel,
				Method = method,
				IsTemplated = isTemplated
			};
		}

		internal DiaryEntry Create(CaloriesDiary.DiaryEntry d)
		{
			return new DiaryEntry()
			{
				Links = new List<Link>()
				{
					CreateLink(_urlHelper.Link("Entries", new { diaryid = d.Diary.date.ToString("yyyy-MM-dd"), id = d.id }),"self")
				},
				Quantity = d.quantity ?? 0,
				FoodLink = CreateLink(_urlHelper.Link("Foods", new { id = d.Food.id }), "self"),
				MeasureLink = CreateLink(_urlHelper.Link("Measures", new { foodid = d.Food.id, id = d.measureid }), "self")
			};
		}

		public DiarySummary CreateSummary(CaloriesDiary.Diary diary)
		{
			return new DiarySummary
			{
				DiaryDate = diary.date,
				TotalCalories = diary.DiaryEntries.Sum(e => e.Measure.calories * e.quantity) ?? 0
			};
		}
		internal Food Create(CaloriesDiary.Food f)
		{
			return new Food()
			{
				Links = new List<Link>()
				{
					CreateLink(_urlHelper.Link("Foods", new { id = f.id }),"self")
				},
				Description = f.description,
				Measures = f.Measures1.Select(m => Create(m))
			};
		}

		internal Measure Create(CaloriesDiary.Measure m)
		{
			return new Measure()
			{
				Links = new List<Link>()
				{
					CreateLink(_urlHelper.Link("Measures", new { foodid = m.Food.id, id = m.id }),"self")
				},
				Calories = m.calories ?? 0,
				Fat = m.fat,
				Description = m.description
			};
		}

		public CaloriesDiary.DiaryEntry Parse(DiaryEntry model)
		{
			try
			{
				var entry = new CaloriesDiary.DiaryEntry();
				if (model.Quantity != default(int))
				{
					entry.quantity = model.Quantity;
				}


				if (!string.IsNullOrWhiteSpace(model.MeasureLink.Href))
				{
					var uri = new Uri(model.MeasureLink.Href);
					var measureid = int.Parse(uri.Segments.Last());
					var measure = _repo.getMeasure(measureid);
					entry.Measure = measure;
					entry.Food = measure.Food;
				}
				return entry;
			}
			catch (Exception)
			{

				return null;
			}
		}
		public CaloriesDiary.Diary Parse(Diary model)
		{
			try
			{
				var entry = new CaloriesDiary.Diary();
				var selflink = model.Links.Where(l => l.Rel == "self").FirstOrDefault();
				if (selflink != null && !string.IsNullOrWhiteSpace(selflink.Href))
				{
					var uri = new Uri(selflink.Href);
					entry.id = int.Parse(uri.Segments.Last());
					if (model.Date != default(DateTime))
					{
						entry.date = model.Date;
					}
				}
				entry.username = _identifyService.CurrentUser;
				return entry;
			}
			catch (Exception)
			{

				return null;
			}
		}
	}
}