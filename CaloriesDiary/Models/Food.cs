using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Models
{
	public class Food
	{
		public ICollection<Link> Links { get; set; }
		public string Description { get; set; }

		public virtual IEnumerable<Measure> Measures { get; set; }
	}
}

