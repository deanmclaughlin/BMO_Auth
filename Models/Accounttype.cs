using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Data;

namespace BMO_Auth.Models
{
    [Table("accounttype")]
    public partial class Accounttype
    {
        public Accounttype()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Precision(3, 2)]
        public decimal InterestRate { get; set; }

        [NotMapped]
        public string StrId
        {
            get
            {
                return this.Id.ToString("0000");
            }
        }

        [NotMapped]
        public string AcctNameAndId
        {
            get
            {
                return (StrId + "  -  " + Name);
            }
        }

        [InverseProperty("AccountType")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
