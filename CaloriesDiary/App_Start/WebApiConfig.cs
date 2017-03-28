using CacheCow.Server;
using CacheCow.Server.EntityTagStore.SqlServer;
using Microsoft.Data.Edm;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Batch;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;


namespace CaloriesDiary.App_Start
{

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// TODO: Add any additional configuration code.

			// Web API routes
			config.MapHttpAttributeRoutes();
			//OData
			config.AddODataQueryFilter();
			config.Routes.MapODataServiceRoute("odata", null, GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
			config.EnsureInitialized();


			//config.Routes.MapHttpRoute(
			//	name: "Diaries",
			//	routeTemplate: "api/diaries/{id}",
			//	defaults: new { controller = "diaries", id = RouteParameter.Optional }
			//);
			config.Routes.MapHttpRoute(
				name: "DiarySummary",
				routeTemplate: "api/diaries/{id}/summary",
				defaults: new { controller = "diarysummary" }
			);
			config.Routes.MapHttpRoute(
				name: "Entries",
				routeTemplate: "api/diaries/{diaryid}/entries/{id}",
				defaults: new { controller = "diaryentries", id = RouteParameter.Optional }
			);
			//config.Routes.MapHttpRoute(
			//	name: "Foods",
			//	routeTemplate: "api/foods/{id}",
			//	defaults: new { controller = "foods", id = RouteParameter.Optional }
			//);
			//config.Routes.MapHttpRoute(
			//	name: "Measures",
			//	routeTemplate: "api/foods/{foodid}/measures/{id}",
			//	defaults: new { controller = "measures", id = RouteParameter.Optional }
			//)
			config.Routes.MapHttpRoute(
				name: "Measures",
				routeTemplate: "api/foods/{foodid}/measures/{id}/{id1}",
				defaults: new { controller = "measures", id = RouteParameter.Optional, id1 = RouteParameter.Optional }
			);
			config.Formatters.JsonFormatter.SupportedMediaTypes
	.Add(new MediaTypeHeaderValue("application/json"));

			//var JsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
			//JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			// WebAPI when dealing with JSON & JavaScript!
			// Setup json serialization to serialize classes to camel (std. Json format)
			//var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			//formatter.SerializerSettings.ContractResolver =
			//	new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

			//Configure Caching
			// Store ETag and other informations in database so that at build they would not change
			string connection = System.Configuration.ConfigurationManager.
							ConnectionStrings["CaloriesDiaryEntities"].
							ConnectionString;
			var eTagStore = new SqlServerEntityTagStore(connection);

			var cacheHandler = new CachingHandler(config,eTagStore);
			cacheHandler.AddLastModifiedHeader = false;
			config.MessageHandlers.Add(cacheHandler);

			// Add support for CORS -cross origin resource sharing
			var attr = new EnableCorsAttribute("*","*","GET"); // for hole project
			config.EnableCors(attr);
		}
		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.Namespace = "CaloriesDiary";
			builder.ContainerName = "DefaultContainer";
			builder.EntitySet<Diary>("Diaries");
			var edmModel = builder.GetEdmModel();
			return edmModel;
		}
	}
	

}