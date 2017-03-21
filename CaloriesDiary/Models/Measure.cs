﻿using System.Collections.Generic;

namespace CaloriesDiary.Models
{
	public class Measure
	{
		public ICollection<Link> Links { get; set; }
		public string Description { get; set; }
		public int Calories { get; set; }
		public int Fat { get; set; }
	}
}