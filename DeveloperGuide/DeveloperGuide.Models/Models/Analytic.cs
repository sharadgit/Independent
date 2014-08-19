using System;

namespace DGuide.Infrastructure.Models
{
    public class Analytic
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}