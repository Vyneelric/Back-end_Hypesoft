using AutoMapper;
using Hypersoft.Application.Queries;
using Hypersoft.Domain.Entities;

namespace Hypersoft.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product -> ProductDto (das Queries)
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.nome))
            .ForMember(dest => dest.descricao, opt => opt.MapFrom(src => src.descricao))
            .ForMember(dest => dest.preco, opt => opt.MapFrom(src => src.preco))
            .ForMember(dest => dest.quantidade_estoque, opt => opt.MapFrom(src => src.quantidade_estoque))
            .ForMember(dest => dest.category, opt => opt.Ignore()); // Mapeado manualmente nos handlers

        // Category -> CategoryDto (das Queries)
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.nome))
            .ForMember(dest => dest.descricao, opt => opt.MapFrom(src => src.descricao));
    }
}
