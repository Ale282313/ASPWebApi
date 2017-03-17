using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Models
{
	public class Diary
	{
		public string Url { get; set; }
		public DateTime Date { get; set; }
		public string Username { get; set; }
		public IEnumerable<DiaryEntry> Entries { get; set; }
	}
}