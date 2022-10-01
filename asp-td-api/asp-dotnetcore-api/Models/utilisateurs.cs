using ASPRad.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPRad.Models{
	[Table("utilisateurs")]
	public class Utilisateurs : BaseRecord
	{
		public string? nom { get; set; } = default!;
		public string? prenom { get; set; } = default!;
		public string? email { get; set; } = default!;
		public string? mot_de_passe { get; set; } = default!;
		public string? tel { get; set; } = default!;
		[Key]
		public int id { get; set; } = default!;
	}
	public class UtilisateursRegisterDTO
	{
		[RecordUnique("nom", "utilisateurs")]
		[Required]
		public string? nom { get; set; } = default!;
		[RecordUnique("email", "utilisateurs")]
		[Required]
		[EmailAddress]
		public string? email { get; set; } = default!;
		[Required]
		public string? mot_de_passe { get; set; } = default!;
		[Compare("mot_de_passe")]
		[NotMapped]
		public string Confirm_Password { get; set; } = default!;
		public string? tel { get; set; } = default!;
	}
	public class UtilisateursAccounteditDTO
	{
		public string? nom { get; set; } = default!;
		public string? prenom { get; set; } = default!;
		public string? tel { get; set; } = default!;
	}
	public class UtilisateursAddDTO
	{
		[RecordUnique("nom", "utilisateurs")]
		[Required]
		public string? nom { get; set; } = default!;
		[RecordUnique("email", "utilisateurs")]
		[Required]
		[EmailAddress]
		public string? email { get; set; } = default!;
		[Required]
		public string? mot_de_passe { get; set; } = default!;
		[Compare("mot_de_passe")]
		[NotMapped]
		public string Confirm_Password { get; set; } = default!;
		public string? tel { get; set; } = default!;
	}
	public class UtilisateursEditDTO
	{
		[RequiredIfEmpty]
		public string? nom { get; set; } = default!;
		public string? prenom { get; set; } = default!;
		public string? tel { get; set; } = default!;
	}
}
