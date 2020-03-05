using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Bitly_Clone.Models
{
    public class ShortUrls
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortUrl { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
