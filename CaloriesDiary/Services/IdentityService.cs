using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaloriesDiary.Services
{
	public class IdentityService : IIdentityService
	{
		public string CurrentUser {
			get
			{
				return "alexandra";
			}
		}
	}
}