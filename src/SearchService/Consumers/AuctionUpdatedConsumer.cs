using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("--> Consumig updated auction: " + context.Message.Id);

        var newItemValues = _mapper.Map<Item>(context.Message);
        
        await DB.Update<Item>()
        .Match(x => x.ID == context.Message.Id)        
        .ModifyOnly(b => new { b.Make, b.Model, b.Year, b.Color, b.Mileage, b.ImageUrl }, newItemValues)
        .ExecuteAsync();
    }
}
