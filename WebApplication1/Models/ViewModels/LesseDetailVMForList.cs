using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LesseDetailVMForList
    {
        public List<tblLesseDetail> LesseDetailList { get; set; } = new List<tblLesseDetail>();
        [NotMapped]
        public DateTime? FromDate { get; set; }
        [NotMapped]
        public DateTime? ToDate { get; set; }
    }
}