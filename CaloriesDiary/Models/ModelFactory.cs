using CaloriesDiary.Repository;
using CaloriesDiary.Services;
using System;
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

		public ModelFactory(HttpRequestMessage request, IRepository repo,IIdentityService service)
		{
			_urlHelper = new UrlHelper(request);
			_repo = repo;
			_identifyService = service;
		}
		internal Diary Create(CaloriesDiary.Diary d)
		{
			return new Diary()
			{
				Url = _urlHelper.Link("Diaries", new { id = d.date.ToString("yyyy-MM-dd") }),
				Date = d.date,
				Username = d.username,
				Entries = d.DiaryEntries.Select(de => Create(de))

			};
		}
		internal DiaryEntry Create(CaloriesDiary.DiaryEntry d)
		{
			return new DiaryEntry()
			{
				Url = _urlHelper.Link("Entries", new { diaryid = d.Diary.date.ToString("yyyy-MM-dd"), id = d.id }),
				Quantity = d.quantity ?? 0,
				MeasureUrl = Create(d.Measure).Url,
				FoodUrl = Create(d.Food).Url

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
				Url = _urlHelper.Link("Foods", new { id = f.id }),
				Description = f.description,
				Measures = f.Measures1.Select(m => Create(m))
			};
		}

		internal Measure Create(CaloriesDiary.Measure m)
		{
			return new Measure()
			{
				Url = _urlHelper.Link("Measures", new { foodid = m.Food.id, id = m.id }),
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
				if (!string.IsNullOrWhiteSpace(model.MeasureUrl))
				{
					var uri = new Uri(model.MeasureUrl);
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
				if (model.Date != default(DateTime))
				{
					entry.date = model.Date;
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