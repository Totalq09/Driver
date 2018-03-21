﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulkiAPI.Models
{
    public class MonthlyReportItem
    {
        public string date { get; set; }
        public double total_distance { get; set; }
        public decimal total_price { get; set; }
        public double avg_distance { get; set; }
        public decimal avg_price { get; set; }
    }
}