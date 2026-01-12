using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContratti.Models
{
  public class Contratto
  {
    [Key, Column("CONTR_Id", Order = 0)]
    public int CONTR_CONEN_IdClienteFatturazione { get; set; } // Riferimento al cliente di fatturazione

    [Key, Column("CONTR_Natura", Order = 0)]
    [ForeignKey(nameof(ClienteContratto))]
    public int CONTR_CONEN_IdClienteContratto { get; set; } // Riferimento al cliente di contratto

    [Key, Column("CONTR_Categoria", Order = 1)]
    [ForeignKey(nameof(EnteBeneficiario))]
    public int CONTR_CONEN_IdEnteBeneficiario { get; set; } // Riferimento all'ente beneficiario del contratto

    [Key, Column("CONTR_Descrizione", Order = 2)]
    public int CONTR_Descrizione { get; set; } // Riferimento alla commessa

    public Contraente ClienteFatturazione { get; set; } = null!;
    public Contraente ClienteContratto { get; set; } = null!;
    public Contraente EnteBeneficiario { get; set; } = null!;
    public Commessa Commessa { get; set; } = null!;
  }
}