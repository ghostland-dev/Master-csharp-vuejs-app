
// AutoMapping.cs
using AutoMapper;
using ASPRad.Models;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
    		CreateMap<MonnaiesAddDTO, Monnaies>();
		CreateMap<MonnaiesEditDTO, Monnaies>();
		CreateMap<UtilisateursRegisterDTO, Utilisateurs>();
		CreateMap<UtilisateursAccounteditDTO, Utilisateurs>();
		CreateMap<UtilisateursAddDTO, Utilisateurs>();
		CreateMap<UtilisateursEditDTO, Utilisateurs>();
    }
}
