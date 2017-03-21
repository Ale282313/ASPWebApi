using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Models
{
	public class DiaryEntry
	{
		public ICollection<Link> Links { get; set; }
		public int Quantity { get; set; }

		public Link FoodLink { get; set; }
		public Link MeasureLink { get; set; }
	}
}