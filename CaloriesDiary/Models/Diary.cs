using System;
using System.Collections.Generic;

namespace CaloriesDiary.Models
{
	public class Diary
	{
		public Link Links { get; set; }
		public DateTime Date { get; set; }
		public string Username { get; set; }
		public IEnumerable<DiaryEntry> Entries { get; set; }
	}
}