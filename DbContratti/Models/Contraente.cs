using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbContratti.Models
{
  public class Contraente
  {
    [Key]
    [Column("CONEN_Id")]
    public int CONEN_Id { get; set; }
    
    // Identificatore univoco per il cliente
    [Column("CONEN_CodiceInterno", TypeName = "varchar(20)")]
    [MaxLength(20)]
    public string CONEN_CodiceInterno { get; set; } = null!;

    [CodiceFiscale]
    [Column("CONEN_CodiceFiscale", TypeName = "varchar(16)")]
    [MaxLength(16)]
    public string CONEN_CodiceFiscale { get; set; } = null!;

    [PartitaIva]
    [Column("CONEN_PartitaIva", TypeName = "varchar(11)")]
    [MaxLength(11)]
    public string CONEN_PartitaIva { get; set; } = null!;

    //[Column(TypeName = "varchar(100)")]
    //[MaxLength(100)]
    //public string CONEN_Nome { get; set; } = null!;

    //[Column(TypeName = "varchar(100)")]
    //[MaxLength(100)]
    //public string CONEN_Cognome { get; set; } = null!;

    [Column("CONEN_RagioneSociale", TypeName = "nvarchar(150)")]
    [MaxLength(150)]
    public string CONEN_RagioneSociale { get; set; } = null!;

    [Column("CONEN_UtenteModifica", TypeName = "uniqueidentifier")]
    public Guid CONEN_UtenteModifica { get; set; }

    private DateTime _dataModifica;

    [Column("CONEN_DataModifica", TypeName = "datetime")]
    public DateTime CONEN_DataModifica
    {
        get => DateTime.SpecifyKind(_dataModifica, DateTimeKind.Utc);
        set => _dataModifica = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
    }

    public ICollection<Indirizzo> Indirizzi { get; set; } = new List<Indirizzo>();

    public ICollection<Contatto> Contatti { get; set; } = new List<Contatto>();

    public ICollection<ContrattoContraenti> ContrattiContraenti { get; set; } = new List<ContrattoContraenti>();
  }
}
