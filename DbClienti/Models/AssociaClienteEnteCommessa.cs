using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClienti.Models
{
  public class AssociaClienteEnteCommessa
  {
    [Key, Column("ASCEC_CLIEN_Id", Order = 0)]
    [ForeignKey(nameof(Cliente))]
    public int ASCEC_CLIEN_Id { get; set; } // Riferimento al cliente

    [Key, Column("ASCEC_ENTE_Id", Order = 1)]
    [ForeignKey(nameof(Ente))]
    public int ASCEC_ENTE_Id { get; set; } // Riferimento all'ente

    [Key, Column("ASCEC_COMME_Id", Order = 2)]
    [ForeignKey(nameof(Commessa))]
    public int ASCEC_COMME_Id { get; set; } // Riferimento alla commessa

    public Cliente Cliente { get; set; } = null!;
    public Ente Ente { get; set; } = null!;
    public Commessa Commessa { get; set; } = null!;
  }
}