using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbContratti.Models
{
  public class AssociazioneContrattoContraenti
  {
    [Key, Column("CONTR_CONEN_IdClienteFatturazione", Order = 0)]
    [ForeignKey(nameof(ClienteFatturazione))]
    public int CONTR_CONEN_IdClienteFatturazione { get; set; } // Riferimento al cliente di fatturazione

    [Key, Column("CONTR_CONEN_IdClienteContratto", Order = 0)]
    [ForeignKey(nameof(ClienteContratto))]
    public int CONTR_CONEN_IdClienteContratto { get; set; } // Riferimento al cliente di contratto

    [Key, Column("CONTR_CONEN_IdEnteBeneficiario", Order = 1)]
    [ForeignKey(nameof(EnteBeneficiario))]
    public int CONTR_CONEN_IdEnteBeneficiario { get; set; } // Riferimento all'ente beneficiario del contratto

    [Key, Column("CONTR_COMME_Id", Order = 2)]
    [ForeignKey(nameof(Commessa))]
    public int CONTR_COMME_Id { get; set; } // Riferimento alla commessa

    public Contraente ClienteFatturazione { get; set; } = null!;
    public Contraente ClienteContratto { get; set; } = null!;
    public Contraente EnteBeneficiario { get; set; } = null!;
    public Commessa Commessa { get; set; } = null!;
  }
}