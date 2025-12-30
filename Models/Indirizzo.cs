using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClienti.Models
{
  public class Indirizzo
  {
    [Key]
    [Column("INDIR_Id")]
    public int INDIR_Id { get; set; }

    [Column("INDIR_Presso", TypeName = "varchar(150)")]
    [MaxLength(150)]
    public string INDIR_Presso { get; set; } = null!;

    [Column("INDIR_Toponimo", TypeName = "varchar(15)")]
    [MaxLength(15)]
    public string INDIR_Toponimo { get; set; } = null!;

    [Column("INDIR_DenominazioneStradale", TypeName = "varchar(100)")]
    [MaxLength(100)]
    public string INDIR_DenominazioneStradale { get; set; } = null!;

    [Column("INDIR_Civico", TypeName = "varchar(5)")]
    [MaxLength(5)]
    public string INDIR_Civico { get; set; } = null!;

    [Column("INDIR_Km", TypeName = "varchar(10)")]
    [MaxLength(10)]
    public string INDIR_Km { get; set; } = null!;

    [Column("INDIR_Esponente", TypeName = "varchar(10)")]
    [MaxLength(10)]
    public string INDIR_Esponente { get; set; } = null!;

    [Column("INDIR_Edificio", TypeName = "varchar(10)")]
    [MaxLength(10)]
    public string INDIR_Edificio { get; set; } = null!;

    [Column("INDIR_Scala", TypeName = "varchar(5)")]
    [MaxLength(5)]
    public string INDIR_Scala { get; set; } = null!;

    [Column("INDIR_Piano", TypeName = "varchar(5)")]
    [MaxLength(5)]
    public string INDIR_Piano { get; set; } = null!;

    [Column("INDIR_Interno", TypeName = "varchar(5)")]
    [MaxLength(5)]
    public string INDIR_Interno { get; set; } = null!;

    [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "CAP non valido")]
    [Column("INDIR_Cap", TypeName = "varchar(5)")]
    [MaxLength(5)]
    public string INDIR_Cap { get; set; } = null!;

    [Column("INDIR_Citta", TypeName = "varchar(50)")]
    [MaxLength(50)]
    public string INDIR_Citta { get; set; } = null!;

    [Column("INDIR_Localita", TypeName = "varchar(50)")]
    [MaxLength(50)]
    public string INDIR_Localita { get; set; } = null!;

    [Column("INDIR_Provincia", TypeName = "varchar(2)")]
    [MaxLength(2)]
    public string INDIR_Provincia { get; set; } = null!;

    [ForeignKey(nameof(Cliente))]
    [Column("INDIR_CLIEN_Id")]
    public int INDIR_CLIEN_Id { get; set; }

    [ForeignKey(nameof(Ente))]
    [Column("INDIR_ENTE_Id")]
    public int INDIR_ENTE_Id { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public Ente Ente { get; set; } = null!;
  }
}