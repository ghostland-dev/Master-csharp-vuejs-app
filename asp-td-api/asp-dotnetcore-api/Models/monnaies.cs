using ASPRad.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPRad.Models{
	[Table("monnaies")]
	public class Monnaies : BaseRecord
	{
		public string? symbole { get; set; } = default!;
		public string? description { get; set; } = default!;
		public double? valeur { get; set; } = default!;
		public DateTime? date { get; set; } = default!;
		[Key]
		public int id { get; set; } = default!;
	}
	public class MonnaiesAddDTO
	{
		public string? symbole { get; set; } = default!;
		public string? description { get; set; } = default!;
		public double? valeur { get; set; } = default!;
	}
	public class MonnaiesEditDTO
	{
		public string? symbole { get; set; } = default!;
		public string? description { get; set; } = default!;
		public double? valeur { get; set; } = default!;
	}
}
