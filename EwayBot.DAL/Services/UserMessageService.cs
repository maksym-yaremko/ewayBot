using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EwayBot.DAL.Services
{
    public class UserMessageService
    {
        public ApplicationContext db { get; set; }
        public UserMessageService()
        {
            db = new ApplicationContext();
        }

        public void Create(long chatId,string message)
        {
            LastUserMessage record = new LastUserMessage()
            {
                ChatId = chatId,
                Message = message
            };
            db.LastUserMessages.Add(record);
            db.SaveChanges();
        }

        public LastUserMessage Get(long chatId)
        {
            LastUserMessage record = db.LastUserMessages.SingleOrDefault(x => x.ChatId == chatId);
            return record;
        }

        public void Update(long chatId,string message)
        {
            var record = db.LastUserMessages.Where(c => c.ChatId == chatId).FirstOrDefault();

            record.Message = message;

            db.SaveChanges();
        }

    }
}
