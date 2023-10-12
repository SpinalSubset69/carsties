using MongoDB.Entities;

namespace SearchService;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuctionServiceHttpClient(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
        .Sort(x => x.Descending(a => a.UpdatedAt))
        .Project(x => x.UpdatedAt.ToString())
        .ExecuteFirstAsync();

        return await _httpClient.GetFromJsonAsync<List<Item>>(_configuration["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
    }
}
