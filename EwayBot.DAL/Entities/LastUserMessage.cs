using System.ComponentModel.DataAnnotations;

namespace EwayBot.DAL.Entities
{
    public class LastUserMessage
    {
        [Key]
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Message { get; set; }
    }
}
