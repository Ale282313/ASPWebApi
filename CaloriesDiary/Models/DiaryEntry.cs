using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Models
{
	public class DiaryEntry
	{
		public string Url { get; set; }
		public int Quantity { get; set; }

		public string FoodUrl { get; set; }
		public string MeasureUrl { get; set; }
	}
}