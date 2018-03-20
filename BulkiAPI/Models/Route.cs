using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BulkiAPI.Models
{
    [Bind(Exclude = "Id,Distance")]
    public class Route
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name ="Source address")]
        public string SourceAddress { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }

        public double Distance { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+(\.{0,1}[0-9]+)*")]
        public decimal Price { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}