using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContratti.Models
{
  public class TipoCategoriaContratto
  {
    [Key]
    [Column("TCATC_Id")]
    [Range(1, 6, ErrorMessage = "L'Id della categoria contratto deve essere compreso tra 1 e 6.")]
    public int TCATC_Id { get; set; }

    [Column("TCATC_Tipo", TypeName = "varchar(100)")]
    [MaxLength(100)]
    public string TCATC_Tipo
    {
      get => TCATC_Id switch
      {
        1 => "Diretto con l'ente beneficiario",
        2 => "Tramite concessionario",
        3 => "Tramite raggruppamento temporaneo di imprese (RTI) - Mandante",
        4 => "Tramite raggruppamento temporaneo di imprese (RTI) - Mandatario",
        5 => "In subappalto",
        6 => "Accordo quadro",
        _ => string.Empty
      };
      set { /* setter richiesto da EF, la descrizione è determinata da Id */ }
    }
  }
}