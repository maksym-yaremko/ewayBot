using System.Net.Http;
using System.Threading.Tasks;
using EwayBot.BLL.EwayAPI.Models;
using Microsoft.Extensions.Options;
using Nancy.Json;

namespace EwayBot.BLL.EwayAPI
{
    public class EwayAPIClient
    {
        private readonly EwayAPISettings _ewayApiSettings;
        private readonly JavaScriptSerializer _serializer;

        public EwayAPIClient(IOptions<EwayAPISettings> ewaySettings)
        {
            _ewayApiSettings = ewaySettings.Value;
            _serializer = new JavaScriptSerializer();
        }

        public async Task<GetStopInfoModel> GetStopInfo(int stopId)
        {
            var client = HttpClientFactory.Create();

            var response = await client.GetAsync($"{_ewayApiSettings.BaseUrl}function=stops.GetStopInfo&city=lviv&id={stopId}");
            var content =  await response.Content.ReadAsStringAsync();
            content = content.Replace("@attributes", "attributes");

            var result = _serializer.Deserialize<GetStopInfoModel>(content);
            return result;
        }
    }
}
