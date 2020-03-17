using EwayBot.DAL.Models;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EwayBot.DAL.Services
{
    public class EwayApiService
    {
        private SensitiveTokens _sensitiveTokens { get; set; }
        private JavaScriptSerializer _serializer { get; set; }
        private HttpClient _httpClient { get; set; }
        public EwayApiService(IOptions<SensitiveTokens> sensitiveTokens)
        {
            _sensitiveTokens = sensitiveTokens.Value;
            _serializer = new JavaScriptSerializer();
            _httpClient = new HttpClient();
        }

        public async Task<GetStopInfoModel> GetStopInfo(string stopId)
        {
            var response = await _httpClient.GetAsync($"{_sensitiveTokens.EasyWayApiToken}&function=stops.GetStopInfo&city=lviv&id={stopId}");
            var content = await response.Content.ReadAsStringAsync();
            content = content.Replace("@attributes", "attributes");

            var result = _serializer.Deserialize<GetStopInfoModel>(content);
            return result;
        }
    }
}
