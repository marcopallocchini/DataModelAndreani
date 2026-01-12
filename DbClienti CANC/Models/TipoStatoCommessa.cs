using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContratti.Models
{
  public class TipoStatoCommessa
  {
    [Key]
    [Column("TSTCO_Id")]
    [Range(1, 3, ErrorMessage = "L'Id del tipo stato commessa deve essere compreso tra 1 e 3.")]
    public int TSTCO_Id { get; set; }

    [Column("TSTCO_Tipo", TypeName = "varchar(25)")]
    [MaxLength(25)]
    public string TSTCO_Tipo
    {
      get => TSTCO_Id switch
      {
        1 => "Aperta",
        2 => "Chiusa",
        3 => "Sospesa fino a rinnovo",
        _ => string.Empty
      };
      set { /* setter richiesto da EF, ma la descrizione è determinata da Id */ }
    }
  }
}