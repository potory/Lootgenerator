using AutoMapper;
using LootGenerator.Contracts.Requests.Items;
using LootGenerator.Contracts.Responses.Items;
using LootGenerator.Models;

namespace LootGenerator.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // https://habr.com/ru/post/649645/
        CreateMap<PostItemRequest, Item>().ReverseMap();
        CreateMap<PutItemRequest, Item>().ReverseMap();
        CreateMap<Item, GetItemResponse>();
    }
}