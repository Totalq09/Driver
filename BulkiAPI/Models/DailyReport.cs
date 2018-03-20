using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulkiAPI.Models
{
    public class DailyReport
    {
        public double total_distance { get; set; }
        public decimal total_price { get; set; }
        [JsonIgnore]
        public int RoutesNumber { get; set; }
    }
}