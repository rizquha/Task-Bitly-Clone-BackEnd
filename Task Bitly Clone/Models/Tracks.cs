using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Bitly_Clone.Models
{
    public class Tracks
    {
        public Guid Id { get; set; }
        public int ShortUrlId { get; set; }
        public string IpAddress { get; set; }
        public string ReferrerUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
