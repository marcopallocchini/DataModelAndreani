using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbClienti.Models
{
  public class Ente
  {
    [Key]
    [Column("ENTE_Id")]
    public int ENTE_Id { get; set; }
    
    [Column("ENTE_Codice")]
    public string ENTE_Codice { get; set; } = null!;

    public ICollection<Indirizzo> Indirizzi { get; set; } = new List<Indirizzo>();

    public ICollection<Contatto> Contatti { get; set; } = new List<Contatto>();

    public ICollection<Commessa> Commesse { get; set; } = new List<Commessa>();
  }
}
