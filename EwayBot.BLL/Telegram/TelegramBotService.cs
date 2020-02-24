using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EwayBot.BLL.EwayAPI;
using EwayBot.BLL.EwayAPI.Models;
using EwayBot.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Stop = EwayBot.DAL.Entities.Stop;

namespace EwayBot.BLL.Telegram
{
    public class TelegramBotService
    {
        public EwayAPIClient _client { get; set; }
        public ApplicationContext _context { get; set; }

        public TelegramBotService(EwayAPIClient сlient, ApplicationContext context)
        {
            _client = сlient;
            _context = context;
        }

        public async Task<List<Stop>> GetStopsByTitle(string title)
        {
          return await _context.Stops.AsNoTracking().Where(x => x.Title.ToLower().Contains(title.ToLower())).ToListAsync();
        }

        public async Task<GetStopInfoModel> GetStopInfo(int id)
        {
            return await _client.GetStopInfo(id);
        }
    }
}
