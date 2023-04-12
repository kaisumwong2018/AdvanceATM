using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceATM.Model
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public decimal? Total { get; set; }

        public string? Description { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }
        public string? Account_number { get; set; } = null!;


    }
}
