using CaloriesDiary.Models;
using CaloriesDiary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CaloriesDiary.Services;
using CaloriesDiary.ActionResults;

namespace CaloriesDiary.Controllers
{
	public abstract class BaseApiController : ApiController
	{
		private IRepository _repo;
		private ModelFactory _modelFactory;
		private IIdentityService _identityService= new IdentityService();

		public BaseApiController(IRepository repo)
		{
			_repo = repo;
		}
		protected IRepository TheRepository
		{
			get
			{
				return _repo;
			}
		}
		protected ModelFactory TheModelFactory
		{
			get
			{
				if (_modelFactory == null)
				{
					_modelFactory = new ModelFactory(this.Request,TheRepository,_identityService);
				}
				return _modelFactory;
			}
		}
		protected IHttpActionResult Versioned<T>(T body, string version="v1") where T:class
		{
			return new VersionedActionResult<T>(Request,version,body);
		}
	}
}
