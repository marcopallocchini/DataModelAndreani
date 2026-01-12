using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContratti.Models
{
  public class TipoContatto
  {
    [Key]
    [Column("TCONT_Id")]
    [Range(1, 6, ErrorMessage = "L'Id del tipo contatto deve essere compreso tra 1 e 6.")]
    public int TCONT_Id { get; set; }

    [Column("TSTCO_Tipo", TypeName = "varchar(15)")]
    [MaxLength(15)]
    public string TSTCO_Tipo
    {
      get => TCONT_Id switch
      {
        1 => "Telefono fisso",
        2 => "Telefono mobile",
        3 => "PEC",
        4 => "E-mail",
        5 => "Fax",
        6 => "Sito web",
        _ => string.Empty
      };
      set { /* setter richiesto da EF, ma la descrizione è determinata da Id */ }
    }
  }
}