using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbClienti.Models
{
  public class Commessa
  {
    [Key]
    [Column("COMME_Id")]
    public int COMME_Id { get; set; }

    // Identificatore univoco per la commessa
    [Required]
    [Column("COMME_Codice", TypeName = "varchar(15)")]
    [MaxLength(15)]
    public string COMME_Codice { get; set; } = null!;

    [Column("COMME_TSTCO_Id")]
    public string COMME_TSTCO_Id { get; set; } = null!;

    [Column("COMME_DataInizio", TypeName = "date")]
    public DateTime COMME_DataInizio { get; set; }

    [Column("COMME_DataFine", TypeName = "date")]
    public DateTime COMME_DataFine { get; set; }
  }
}
