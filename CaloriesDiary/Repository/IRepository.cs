using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Repository
{
	public interface IRepository
	{
		IQueryable<Food> getFoods();
		Food getFoodItem(int id);
		IQueryable<Models.Diary> getDiaries(string username);
		Diary getDiaryItem(string username,DateTime id);
		IEnumerable<Measure> getMeasuresForFood(int id);
		Measure getSpecificMeasureforFood(int foodid,int id);
		IQueryable<Food> getFoodsWithoutMeasurements();
		IEnumerable<DiaryEntry> GetDiaryEntries(DateTime id);
		DiaryEntry getDiaryEntryItem(DateTime diaryid, int id);
		Measure getSpecificMeasureforFood1(int foodid, int id, int id1);
		Measure getMeasure(int id);
		Diary getDiary(string username, DateTime id);
		int SaveAll();
		bool DeleteDiaryEntry(DateTime diaryid, int id);
		bool InsertDiary(Diary entity);
		bool DeleteDiary(DateTime id);
	}
}