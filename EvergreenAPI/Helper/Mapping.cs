using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DiseaseCategory, DiseaseCategoryDto>().ReverseMap();
            CreateMap<Disease, DiseaseDto>().ReverseMap();
            CreateMap<MedicineCategory, MedicineCategoryDto>().ReverseMap();
            CreateMap<Medicine, MedicineDto>().ReverseMap();
            CreateMap<Treatment, TreatmentDto>().ReverseMap();
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<Account, UserDto>().ReverseMap();
            CreateMap<Thumbnail, ThumbnailDto>().ReverseMap();
            

        }
    }
}
