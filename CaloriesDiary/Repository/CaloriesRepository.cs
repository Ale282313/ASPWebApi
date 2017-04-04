using CaloriesDiary.DAL;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;
using CaloriesDiary.Models;
using System.Web.Http.Routing;
using CaloriesDiary.Controllers;
using System.Net.Http;

namespace CaloriesDiary.Repository
{
	public class CaloriesRepository : IRepository
	{
		string connection = System.Configuration.ConfigurationManager.
							ConnectionStrings["CaloriesDiaryEntities"].
							ConnectionString;
		DiaryContext db;

		public CaloriesRepository(DiaryContext dbContext)
		{
			db = dbContext;
		}

		public bool DeleteDiary(DateTime id)
		{
			using (var conn = new SqlConnection(connection))
			{
				conn.Open();
				var result =
					from d in db.Diaries
					where d.date == id
					select d;
				foreach (var d in result)
				{
					db.Diaries.Remove(d);
				}

				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					throw e;
					// Provide for exceptions.
				}
				conn.Close();
				return true;
			}
		}

		public bool DeleteDiaryEntry(DateTime diaryid, int id)
		{
			using (var conn = new SqlConnection(connection))
			{
				conn.Open();
				var result = 
					from d in db.DiaryEntris
					where d.id == id
					select d;
				foreach (var d in result)
				{
					db.DiaryEntris.Remove(d);
				}

				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					throw e;
					// Provide for exceptions.
				}
				conn.Close();
				return true;
			}
		}

		public IQueryable<Models.Diary> getDiaries(string username,HttpRequestMessage request)
		{
			var _urlHelper = new UrlHelper(request);
			using (var conn = new SqlConnection(connection))
			{ 
				conn.Open();
				var diaries = db.Diaries.AsEnumerable().Where(d => d.username == username).
							  Select(d => new Models.Diary()
							  {
								  Date = d.date,
								  Username = username,
								  Links = new Link {
									 Href= _urlHelper.Link("Diaries", new { id = d.date.ToString("yyy-MM-dd") }),
									 Method="goooo",
									 Rel ="self"
								  }
									  
							  });

				conn.Close();
				return diaries.AsQueryable();
			}
			
		}

		public Diary getDiary(string username, DateTime id)
		{
			using (var conn = new SqlConnection(connection))
			{
				conn.Open();
				var diary = from d in db.Diaries
							where d.date == id && d.username == username
							select d;
				conn.Close();
				return diary.Single();
			}
		}

		public IEnumerable<DiaryEntry> GetDiaryEntries(DateTime id)
		{
			var result = from de in db.DiaryEntris
						 where de.Diary.date == id
						 select de;
			return result;
		}

		public DiaryEntry getDiaryEntryItem(DateTime diaryid, int id)
		{
			var result = from de in db.DiaryEntris
						 where de.Diary.date == diaryid && de.id == id
						 select de;
			return result.Single();
		}

		public Diary getDiaryItem(string username, DateTime id)
		{
			using (var conn = new SqlConnection(connection))
			{
				conn.Open();
				var diary = from d in db.Diaries
							where d.date == id && d.username == username
							select d;
				conn.Close();
				return diary.Single();
			}
		}

		public Food getFoodItem(int id)
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var food = from f in db.Foods
						   where f.id == id
						   select f;
				conn.Close();
				return food.Single();
			}
		}

		public IQueryable<Food> getFoods()
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var foods = from f in db.Foods
							select f;
				conn.Close();
				return foods;
			}
		}

		public IQueryable<Food> getFoodsWithoutMeasurements()
		{
			using (var conn = new SqlConnection(connection))
			{
				db.Configuration.LazyLoadingEnabled = false;
				conn.Open();
				var foods = from f in db.Foods
							select f;
				conn.Close();
				return foods;
			}
		}

		public Measure getMeasure(int id)
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var measure = from m in db.Measures
							  where m.id == id
							  select m;
				conn.Close();
				return measure.Single();
			}
		}

		public IEnumerable<Measure> getMeasuresForFood(int foodid)
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var measures = from f in db.Foods
							   where f.id == foodid
							   join m in db.Measures on f.id equals m.foodid
							   select m;
				conn.Close();
				return measures;
			}
		}

		public Measure getSpecificMeasureforFood(int foodid, int id)
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var measure = from f in db.Foods
							  where f.id == foodid
							  join m in db.Measures on f.id equals m.foodid
							  where m.id == id
							  select m;
				conn.Close();
				return measure.Single();
			}
		}

		public Measure getSpecificMeasureforFood1(int foodid, int id, int id1)
		{
			using (var conn = new SqlConnection(connection))
			{

				conn.Open();
				var measure = from f in db.Foods
							  where f.id == foodid
							  join m in db.Measures on f.id equals m.foodid
							  where m.id == id && id1 == m.fat
							  select m;
				conn.Close();
				return measure.Single();
			}
		}

		public bool InsertDiary(Diary entity)
		{
			db.Diaries.Add(entity);
			try
			{
				db.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}
			return true;
		}

		public int SaveAll()
		{
			return db.SaveChanges();
		}
	}
}