#nullable enable
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace ASPRad.Controller{
	using System;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;
    using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Hosting;
	using System.Linq.Dynamic.Core;
	using ASPRad.Models;
	using ASPRad.Helpers;
    using Microsoft.Extensions.Configuration;
	using Microsoft.EntityFrameworkCore;
	

	/// <summary>
	/// Components Data Controller
	/// </summary>
	
	public class Components_Data:BaseController
	{
		private readonly IConfiguration Config;
		private readonly AppDBContext DB;
		private readonly IWebHostEnvironment hostEnvironment;
		public Components_Data(AppDBContext dbContext, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment, IConfiguration Configuration) :base(dbContext, httpContextAccessor, environment, Configuration)
		{
			DB = dbContext;
			Config = Configuration;
			hostEnvironment = environment;
		}
	

	/// <summary>
	/// check if field value already exist in a Utilisateurs table
	/// </summary>
	/// <param name="id">value to check if exists in a table records.</param>
	/// <returns>True or False</returns>
	[HttpGet]
	public ActionResult utilisateurs_nom_exist(string id = ""){
		bool exist = DB.Utilisateurs.Any(p => p.nom == id);
		if(exist == true){
			return Ok("true");
		}
		return Ok("false") ;
	}
	

	/// <summary>
	/// check if field value already exist in a Utilisateurs table
	/// </summary>
	/// <param name="id">value to check if exists in a table records.</param>
	/// <returns>True or False</returns>
	[HttpGet]
	public ActionResult utilisateurs_email_exist(string id = ""){
		bool exist = DB.Utilisateurs.Any(p => p.email == id);
		if(exist == true){
			return Ok("true");
		}
		return Ok("false") ;
	}
	}
}
