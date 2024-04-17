using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolution.Models.Sql
{
    public class AnswerRecord
    {
        public int AnswerRecordID { get; set; }
        public DateTime AnswerDate { get; set; }
        public string FileName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
