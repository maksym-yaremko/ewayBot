﻿using EwayBot.DAL.Models;
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
        public SensitiveTokens _sensitiveTokens { get; set; }
        public JavaScriptSerializer _serializer { get; set; }
        public HttpClient _httpClient { get; set; }
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

        public async Task<GetStopsNearPointModel> GetStopsNearPoint(string lat,string lng)
        {
            var response = await _httpClient.GetAsync($"{_sensitiveTokens.EasyWayApiToken}&function=stops.GetStopsNearPoint&city=lviv&lat={lat}&lng={lng}");
            var content = await response.Content.ReadAsStringAsync();

            var result = _serializer.Deserialize<GetStopsNearPointModel>(content);
            return result;
        }
    }
}
