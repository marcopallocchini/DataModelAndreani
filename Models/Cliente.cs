using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbClienti.Models
{
  public class Cliente
  {
    [Key]
    [Column("CLIEN_Id")]
    public int CLIEN_Id { get; set; }
    
    // Identificatore univoco per il cliente
    [Column("CLIEN_Codice")]
    public string CLIEN_Codice { get; set; } = null!;

    [CodiceFiscale]
    [Column("CLIEN_CodiceFiscale", TypeName = "varchar(16)")]
    [MaxLength(16)]
    public string CLIEN_CodiceFiscale { get; set; } = null!;

    [PartitaIva]
    [Column("CLIEN_PartitaIva", TypeName = "varchar(11)")]
    [MaxLength(11)]
    public string CLIEN_PartitaIva { get; set; } = null!;

    //[Column(TypeName = "varchar(100)")]
    //[MaxLength(100)]
    //public string CLIEN_Nome { get; set; } = null!;

    //[Column(TypeName = "varchar(100)")]
    //[MaxLength(100)]
    //public string CLIEN_Cognome { get; set; } = null!;

    [Column("CLIEN_RagioneSociale", TypeName = "nvarchar(150)")]
    [MaxLength(150)]
    public string CLIEN_RagioneSociale { get; set; } = null!;

    [Column("CLIEN_UtenteModifica", TypeName = "varchar(15)")]
    [MaxLength(15)]
    public string CLIEN_UtenteModifica { get; set; } = null!; //TODO: Capire se lunghezza ok: utente di dominio? Fare tabella a parte utenti e mettere qui solo Id utente?

    private DateTime _dataModifica;

    [Column("CLIEN_DataModifica", TypeName = "datetime")]
    public DateTime CLIEN_DataModifica
    {
        get => DateTime.SpecifyKind(_dataModifica, DateTimeKind.Utc);
        set => _dataModifica = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
    }

    public ICollection<Indirizzo> Indirizzi { get; set; } = new List<Indirizzo>();

    public ICollection<Contatto> Contatti { get; set; } = new List<Contatto>();

    public ICollection<Ente> Enti { get; set; } = new List<Ente>();
  }
}
