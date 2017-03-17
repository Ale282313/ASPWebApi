using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Models
{
	public class Food
	{
		public string Url { get; set; }
		public string Description { get; set; }

		public virtual IEnumerable<Measure> Measures { get; set; }
	}
}

