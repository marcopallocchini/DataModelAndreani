using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClienti.Models
{
  public class Contatto : IValidatableObject
  {
    [Key]
    [Column("CONTT_Id")]
    public int CONTT_Id { get; set; }

    [ForeignKey(nameof(TipoContatto))]
    [Column("CONTT_TCONT_Id")]
    public int CONTT_TCONT_Id { get; set; }

    [ForeignKey(nameof(Cliente))]
    [Column("CONTT_CLIEN_Id")]
    public int CONTT_CLIEN_Id { get; set; }

    [ForeignKey(nameof(Ente))]
    [Column("CONTT_ENTE_Id")]
    public int CONTT_ENTE_Id { get; set; }

    [Column("CONTT_Contatto", TypeName = "varchar(254)")]
    [MaxLength(254)]
    public string CONTT_Contatto { get; set; } = null!;

    [Column("CONTT_Nota", TypeName = "nvarchar(50)")]
    [MaxLength(50)]
    public string CONTT_Nota { get; set; } = null!;

    public TipoContatto TipoContatto { get; set; } = null!;
    
    public Cliente Cliente { get; set; } = null!;
    
    public Ente Ente { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      // 3 = PEC, 4 = E-Mail
      if (CONTT_TCONT_Id == 3 || CONTT_TCONT_Id == 4)
      {
        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(CONTT_Contatto))
        {
          yield return new ValidationResult(
            "Il formato dell'indirizzo email/PEC non è valido.",
            new[] { nameof(CONTT_Contatto) }
          );
        }
      }
    }
  }
}