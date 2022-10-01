#nullable enable
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace ASPRad.Controller{
	using System;
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using Microsoft.Extensions.Logging;
	using System.Linq;
	using CsvHelper;
    using System.IO;
	using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Hosting;
	using ASPRad.Models;
	using ASPRad.Helpers;
	using Microsoft.EntityFrameworkCore;
	using System.Linq.Dynamic.Core;
    using Microsoft.Extensions.Configuration;
	using AutoMapper;
	using WkWrap.Core;
    using System.Text;
	
	

	/// <summary>
	/// Controller for Utilisateurs api
	/// </summary>
	public class UtilisateursController:BaseController
	{
		private readonly IConfiguration Config;
		private readonly EmailHelper Mailer;
		private readonly IHttpContextAccessor HttpAccessor;
		private readonly AppDBContext DB;
		private readonly IMapper Mapper;
		private readonly IWebHostEnvironment hostEnvironment;
		
		public UtilisateursController(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment, IConfiguration Configuration, EmailHelper mailer) :base(dbContext, httpContextAccessor, environment, Configuration)
		{
			Config = Configuration;
			Mailer = mailer;
			HttpAccessor = httpContextAccessor;
			DB = dbContext;
			Mapper = mapper;
			hostEnvironment = environment;
			
		}
		

		/// <summary>
		/// List  Utilisateurs records
		/// Support searching, filtering and ordering of table records
		/// </summary>
		/// <param name="fieldname">Filter records based on table field.</param>
		/// <param name="fieldvalue">Filter value.</param>
   		/// <returns>JSON Array of Utilisateurs records</returns>
		[HttpGet]
		public async Task<IActionResult> Index(string? fieldname, string? fieldvalue, string? search, string? orderby, string ordertype = "desc", int page = 1, int limit = 10){
			try{
				var query = (from Utilisateurs in DB.Utilisateurs
					select new Utilisateurs{
						id = Utilisateurs.id,
						nom = Utilisateurs.nom,
						prenom = Utilisateurs.prenom,
						email = Utilisateurs.email,
						tel = Utilisateurs.tel
					}
				);
				if(search != null){
					query = query.Where(
						p => EF.Functions.Like(p.nom, $"%{search}%") || 
						EF.Functions.Like(p.prenom, $"%{search}%") || 
						EF.Functions.Like(p.email, $"%{search}%") || 
						EF.Functions.Like(p.tel, $"%{search}%") 
					);
				}
				if(orderby != null){
					bool asc = (ordertype.ToLower() == "asc" ? true : false);
					query = query.OrderByField(orderby, asc);
				}
				else{
					query = query.OrderByField("id", false);
				}
				if(fieldvalue != null)
				{
					// filter record by table field
					query = query.Where(fieldname + " == @0", fieldvalue);
				}
				// export page records
				if (Request.Query.ContainsKey("export")){
					var exportRecords = query.ToList();
					return await ExportListRecords(exportRecords);
				}
				int total_records = query.Count();
				int offset = ((page - 1) * limit);
				var records = query.Skip(offset).Take(limit).ToList();
				int record_count = records.Count;
				double t = total_records / limit;
				int total_page = (int)Math.Ceiling(t);
				var result = new { records, total_records, record_count, total_page};
				return Ok(result);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
	

	/// <summary>
	/// Import csv file data into Utilisateurs
	/// </summary>
	/// <returns>Number of records imported</returns>
	public ActionResult ImportData([FromForm(Name = "file")] List<IFormFile> CsvFiles)
	{
		try
		{
			int totalRows = 0;
			foreach (IFormFile file in CsvFiles)
			{
				if (file.Length > 0) // ensures file has readable content
				{
					string fileName = Path.GetFileName(file.FileName);
					string ext = Path.GetExtension(fileName).Trim('.').ToLower();
					if (ext != "csv")
					{
						return BadRequest(ext + " files not allowed");
					}
					using (var memoryStream = new MemoryStream())
					{
						file.CopyTo(memoryStream);
						memoryStream.Position = 0;
						TextReader textReader = new StreamReader(memoryStream);
						var csvReader = new CsvReader(textReader);
						csvReader.Configuration.HasHeaderRecord = true;
						var records = csvReader.GetRecords<Utilisateurs>().ToList();
						DB.Utilisateurs.AddRange(records);
						DB.SaveChanges();
						totalRows +=  records.Count;
					}
				}
			}
			return Ok(totalRows + " Records Imported");
		}
		catch (Exception ex)
		{
			return ServerError(ex);
		}
	}
		

		/// <summary>
		/// Select single Utilisateurs record by ID
		/// </summary>
		/// <param name="id">Table primary key.</param>
   		/// <returns>JSON Object of Utilisateurs record</returns>
		[HttpGet]
		[Route("api/utilisateurs/view/{id}")]
		public async Task<IActionResult> Detail(int id){
			try{
				var query = (from Utilisateurs in DB.Utilisateurs
					select new Utilisateurs{
						id = Utilisateurs.id,
						nom = Utilisateurs.nom,
						prenom = Utilisateurs.prenom,
						email = Utilisateurs.email,
						tel = Utilisateurs.tel
					}
				);
				query = query.Where(p => p.id.Equals(id));
				// export page records
				if (Request.Query.ContainsKey("export")){
					var exportRecords = query.ToList();
					return await ExportViewRecords(exportRecords);
				}
				var record = query.FirstOrDefault();
				if (record == null )
				{
					return NotFound("Enregistrement non trouvé");
				}
				
					record.nextRecordId = getNextRecordId(id);
					record.previousRecordId = getPreviousRecordId(id);
				return Ok(record);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		

		/// <summary>
		/// Save form record to the  table
		/// </summary>
   		/// <returns>JSON Object of newly created Utilisateurs record</returns>
		[HttpPost]
		public ActionResult Add([FromBody] UtilisateursAddDTO postdata){
			try{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var modeldata = Mapper.Map<Utilisateurs>(postdata);
				string passwordText = postdata.mot_de_passe;
				modeldata.mot_de_passe = Hash.ComputeHash(passwordText);
				// save Utilisateurs record
				DB.Utilisateurs.Add(modeldata);
				var record = modeldata; //newly created record
				DB.SaveChanges();
				var recId = record.id; //newly created record id
				return Ok(record);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		

		/// <summary>
		/// Get  Utilisateurs record for edit
		/// </summary>
		/// <param name="id">Select record by table primary key.</param>
   		/// <returns>JSON Object of Utilisateurs record</returns>
		[HttpGet]
		public ActionResult Edit(int id){
			try{
				var query = (
					from Utilisateurs in DB.Utilisateurs
					select new Utilisateurs{
						id = Utilisateurs.id,
						nom = Utilisateurs.nom,
						prenom = Utilisateurs.prenom,
						tel = Utilisateurs.tel
					}
				);
				query = query.Where(p => p.id.Equals(id));
				var record = query.FirstOrDefault();
				if (record == null)
				{
					return NotFound("Enregistrement non trouvé");
				}
				
				return Ok(record);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		

		/// <summary>
		/// Update  Utilisateurs record with form data
		/// </summary>
		/// <param name="id">Select record by table primary key.</param>
   		/// <returns>JSON Object of Utilisateurs record</returns>
		[HttpPost]
		public ActionResult Edit(int id, [FromBody] UtilisateursEditDTO postdata){
			try{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var query = (
					from Utilisateurs in DB.Utilisateurs
					select Utilisateurs
				);
				if (DB.Utilisateurs.Any(p => p.nom.Equals(postdata.nom) &&  !p.id.Equals(id))){
					return BadRequest(postdata.nom + " " + " Existe déjà!");
				}
				query = query.Where(p => p.id.Equals(id));
				var record = query.FirstOrDefault();
				if (record == null)
				{
					return NotFound("Enregistrement non trouvé");
				}
				
				var modeldata = Mapper.Map(postdata, record);
				DB.Update(modeldata);
				DB.SaveChanges();
				return Ok(record);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		

		/// <summary>
		/// Delete Utilisateurs record
		/// Support multi delete by separating record id by comma.
		/// </summary>
		/// <param name="id">Record ID.</param>
   		/// <returns>Deleted Records ID</returns>
		[HttpGet]
		public ActionResult Delete(string id){
			try{
				var query = DB.Utilisateurs.AsQueryable();
				List<string> arrId = id.Split(",").ToList();
				query = query.Where(p => arrId.Contains(p.id.ToString()));
				DB.Utilisateurs.RemoveRange(query);
				DB.SaveChanges();
				return Ok(id);
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
	private string getNextRecordId(int recid){
		try{
			var query = DB.Utilisateurs.AsQueryable();
			query = query.Where(p => p.id > recid);
			query = query.OrderByField("id", true);
			var record = query.FirstOrDefault();
			if(record != null){
				return record.id.ToString();
			}
		}
		catch(Exception ex){
			Console.WriteLine(ex);
		}
		return string.Empty;
	}
	private string getPreviousRecordId(int recid){
		try{
			var query = DB.Utilisateurs.AsQueryable();
			query = query.Where(p => p.id < recid);
			query = query.OrderByField("id", false);
			var record = query.FirstOrDefault();
			if(record != null){
				return record.id.ToString();
			}
		}
		catch(Exception ex){
			Console.WriteLine(ex);
		}
		return string.Empty;
	}
		

		/// <summary>
		/// Export  Utilisateurs records to different format such as: pdf, cvs, excel, html
		/// </summary>
   		/// <returns>PDF | CSV | Excel | HTML File</returns>
		[HttpGet]
		private async Task<IActionResult> ExportListRecords(List<Utilisateurs> records)
		{
			string exportFormat = Request.Query["export"].ToString().ToLower();
			string viewName = "ExportList"; // html template for pdf
			try{
				var fileName = "UtilisateursReport";
				if (exportFormat == "excel" || exportFormat == "csv")
				{
					List<DataHeader> Columns = new List<DataHeader>()
					{
					    new DataHeader { Header = "Id", Key = "id" },
					    new DataHeader { Header = "Nom", Key = "nom" },
					    new DataHeader { Header = "Prenom", Key = "prenom" },
					    new DataHeader { Header = "Email", Key = "email" },
					    new DataHeader { Header = "Tel", Key = "tel" }
					};
					var dataTable = records.ToDataTable(Columns);
					dataTable.TableName = "Utilisateurs"; // Excel worksheet title
					return ExportToExcel(dataTable, fileName, exportFormat);
				}
				else if (exportFormat == "pdf")
				{
					var htmlContent = await this.RenderViewAsync(viewName, records, true);
					var wkhtmltoExePath = Config["WKHTML_EXE_PATH"];
					var wkhtmltopdf = new FileInfo(wkhtmltoExePath);
					var converter = new HtmlToPdfConverter(wkhtmltopdf);
					var settings = new ConversionSettings(pageSize: PageSize.A3, orientation: PageOrientation.Portrait, enableExternalLinks: true, enableImages: true);
					var pdfBytes = converter.ConvertToPdf(htmlContent, Encoding.UTF8, settings);
					return File(pdfBytes, "application/pdf", $"{fileName}.pdf");
				}
				else if (exportFormat == "print")
				{
					return View(viewName, records);
				}
				return BadRequest("Export Format not Supported");
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		

		/// <summary>
		/// Export  Utilisateurs records to different format such as: pdf, cvs, excel, html
		/// </summary>
   		/// <returns>PDF | CSV | Excel | HTML File</returns>
		[HttpGet]
		private async Task<IActionResult> ExportViewRecords(List<Utilisateurs> records)
		{
			string exportFormat = Request.Query["export"].ToString().ToLower();
			string viewName = "ExportView"; // html template for pdf
			try{
				var fileName = "UtilisateursReport";
				if (exportFormat == "excel" || exportFormat == "csv")
				{
					List<DataHeader> Columns = new List<DataHeader>()
					{
					    new DataHeader { Header = "Id", Key = "id" },
					    new DataHeader { Header = "Nom", Key = "nom" },
					    new DataHeader { Header = "Prenom", Key = "prenom" },
					    new DataHeader { Header = "Email", Key = "email" },
					    new DataHeader { Header = "Tel", Key = "tel" }
					};
					var dataTable = records.ToDataTable(Columns);
					dataTable.TableName = "Utilisateurs"; // Excel worksheet title
					return ExportToExcel(dataTable, fileName, exportFormat);
				}
				else if (exportFormat == "pdf")
				{
					var htmlContent = await this.RenderViewAsync(viewName, records, true);
					var wkhtmltoExePath = Config["WKHTML_EXE_PATH"];
					var wkhtmltopdf = new FileInfo(wkhtmltoExePath);
					var converter = new HtmlToPdfConverter(wkhtmltopdf);
					var settings = new ConversionSettings(pageSize: PageSize.A3, orientation: PageOrientation.Portrait, enableExternalLinks: true, enableImages: true);
					var pdfBytes = converter.ConvertToPdf(htmlContent, Encoding.UTF8, settings);
					return File(pdfBytes, "application/pdf", $"{fileName}.pdf");
				}
				else if (exportFormat == "print")
				{
					return View(viewName, records);
				}
				return BadRequest("Export Format not Supported");
			}
			catch (Exception ex){
				return ServerError(ex);
			}
		}
		
	}
}
