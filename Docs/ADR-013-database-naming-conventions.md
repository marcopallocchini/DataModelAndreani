# ADR-013: Database Naming Conventions

**Status**: Proposed  
**Date**: 2026-01-15  
**Authors**: Architecture Team, DBA Lead  
**Related ADRs**: ADR-007 (Database per Service)  
**Tags**: backend, data, standards

---

## Context

Con l'adozione del pattern Database-per-Service (ADR-007), ogni microservizio gestisce il proprio database SQL Server in modo autonomo. In un greenfield project con 14 database indipendenti è fondamentale stabilire convenzioni di nomenclatura uniformi per:

**Obiettivi**:
- **Consistency**: nomenclatura uniforme tra tutti i microservizi
- **Readability**: codice self-documenting senza abbreviazioni ambigue
- **Maintainability**: identificazione rapida di relazioni e ownership
- **Bounded Context Isolation**: prefissi univoci per evitare collision

**Requisiti**:
- Supporto per nomi in **italiano** (dominio business locale)
- Supporto a **SQL Server** come DBMS target
- **Greenfield project**: nessun legacy da migrare
- **14 database separati**: conflitti di naming improbabili
- **Pattern audit standard**: tracking modifiche (CreatedAt, ModifiedAt, etc.)

**Vincoli**:
- SQL Server identifier limit: 128 caratteri
- Case-insensitive collation (Latin1_General_CI_AS)
- Convention over configuration (minimizzare configurazioni)

---

## Decision

Adottiamo un sistema di naming conventions strutturato e coerente per tutti gli oggetti database.

### 1. Tabelle

**Formato**: `{PREFIX}_NomeTabella`

- **PREFIX**: 3 caratteri maiuscoli che identificano univocamente la tabella nel bounded context
- **NomeTabella**: PascalCase, max 45 caratteri, nomi completi senza abbreviazioni
- **Tabelle di lookup/enumerazione**: prefisso inizia con `T` (es. `TDO_TipoDocumento`)

**Regole prefisso**:
- Derivato dalle iniziali delle parole di `NomeTabella`
- Univoco all'interno del database/bounded context
- Facilita identificazione rapida (mnemonic)

**Esempi**:
```sql
DOC_Documenti
DDE_DocumentiDettaglio
DCO_DocumentiCorpo
TDO_TipoDocumento
TPR_TipoPriorita
CON_Contribuenti
CAR_Carichi
PAG_Pagamenti
```

---

### 2. Colonne

**Formato**: `{PREFIX}_NomeColonna`

- **PREFIX**: uguale a quello della tabella
- **NomeColonna**: PascalCase, max 45 caratteri, nomi completi
- **Eccezione**: `Id` è l'unica abbreviazione consentita

**Esempi**:
```sql
-- Tabella DOC_Documenti
DOC_Id                  -- PK
DOC_Numero
DOC_DataEmissione
DOC_Importo
DOC_TDO_Id             -- FK a TDO_TipoDocumento
DOC_Descrizione

-- Tabella CON_Contribuenti
CON_Id                  -- PK
CON_CodiceFiscale
CON_PartitaIva
CON_RagioneSociale
CON_DataNascita
```

**Pattern booleani** (prefisso `Is` o `Has`):
```sql
DOC_IsAttivo
DOC_IsApprovato
DOC_IsFirmato
CON_HasPec
CON_HasSedeEstera
```

---

### 3. Chiavi Primarie

**Formato**: `{PREFIX}_PK`

- PREFIX uguale a quello della tabella
- Ogni tabella **deve** avere una PK

**Implementazione**:
```sql
ALTER TABLE DOC_Documenti
ADD CONSTRAINT DOC_PK PRIMARY KEY (DOC_Id);
```

**Tipo chiave**:
- **IDENTITY(1,1)** per SQL Server (standard, performance ottimali)
- INT per volumi standard (<2 miliardi)
- BIGINT per logging/audit/high-volume

---

### 4. Chiavi Esterne

**Naming**: `{PREFIX}_FK_{DescrizioneRelazione}`

**Regola colonne FK**:
- Mantenere **prefisso della tabella di riferimento**
- Nome identico alla colonna referenziata

**Esempi**:
```sql
-- Tabella DDE_DocumentiDettaglio
ALTER TABLE DDE_DocumentiDettaglio
ADD CONSTRAINT DDE_FK_Documento 
FOREIGN KEY (DDE_DOC_Id) REFERENCES DOC_Documenti(DOC_Id);

-- Tabella CAR_Carichi
ALTER TABLE CAR_Carichi
ADD CONSTRAINT CAR_FK_Contribuente 
FOREIGN KEY (CAR_CON_Id) REFERENCES CON_Contribuenti(CON_Id);

ALTER TABLE CAR_Carichi
ADD CONSTRAINT CAR_FK_TipoCarico 
FOREIGN KEY (CAR_TCA_Id) REFERENCES TCA_TipoCarico(TCA_Id);
```

**Vantaggi**: il nome della colonna `DDE_DOC_Id` rende immediatamente chiaro che referenzia `DOC_Documenti.DOC_Id`

---

### 5. Unique Constraints

**Formato**: `{PREFIX}_UK_{NomeVincolo}`

**Esempi**:
```sql
ALTER TABLE CON_Contribuenti
ADD CONSTRAINT CON_UK_CodiceFiscale UNIQUE (CON_CodiceFiscale);

ALTER TABLE DOC_Documenti
ADD CONSTRAINT DOC_UK_NumeroAnno UNIQUE (DOC_Numero, DOC_Anno);
```

---

### 6. Check Constraints

**Formato**: `{PREFIX}_CK_{NomeVincolo}`

**Usare per validazione domini rigidi**:
- Codice Fiscale (16 caratteri alfanumerici)
- Partita IVA (11 cifre)
- Email/PEC (formato)
- CAP (5 cifre)
- Codifiche custom business

**Esempi**:
```sql
ALTER TABLE CON_Contribuenti
ADD CONSTRAINT CON_CK_CodiceFiscale 
CHECK (CON_CodiceFiscale LIKE '[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z][0-9][0-9][A-Z][0-9][0-9][A-Z][0-9][0-9][0-9][A-Z]');

ALTER TABLE CON_Contribuenti
ADD CONSTRAINT CON_CK_PartitaIva 
CHECK (CON_PartitaIva LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');

ALTER TABLE CON_Contribuenti
ADD CONSTRAINT CON_CK_Email 
CHECK (CON_Email LIKE '%@%.%');

ALTER TABLE IND_Indirizzi
ADD CONSTRAINT IND_CK_Cap 
CHECK (IND_Cap LIKE '[0-9][0-9][0-9][0-9][0-9]');
```

---

### 7. Indici

**Formato**: `{PREFIX}_IX_{NomeIndice}`

**Esempi**:
```sql
CREATE INDEX DOC_IX_DataEmissione 
ON DOC_Documenti(DOC_DataEmissione);

CREATE INDEX CON_IX_RagioneSociale 
ON CON_Contribuenti(CON_RagioneSociale);

CREATE INDEX CAR_IX_ContribuenteData 
ON CAR_Carichi(CAR_CON_Id, CAR_DataCarico);
```

**Nota**: evitare FULLTEXT INDEX salvo casi eccezionali concordati con DBA

---

### 8. Viste

**Formato**: `v_{NomeVista}`

**Esempi**:
```sql
CREATE VIEW v_DocumentiConDettaglio AS
SELECT 
    d.DOC_Id,
    d.DOC_Numero,
    d.DOC_DataEmissione,
    dd.DDE_Descrizione,
    dd.DDE_Quantita
FROM DOC_Documenti d
INNER JOIN DDE_DocumentiDettaglio dd ON d.DOC_Id = dd.DDE_DOC_Id;

CREATE VIEW v_CarichiByCOntribuente AS
SELECT 
    c.CON_RagioneSociale,
    COUNT(*) AS NumeroCarichi,
    SUM(car.CAR_Importo) AS ImportoTotale
FROM CON_Contribuenti c
INNER JOIN CAR_Carichi car ON c.CON_Id = car.CAR_CON_Id
GROUP BY c.CON_Id, c.CON_RagioneSociale;
```

---

### 9. Stored Procedures

**Formato**: `usp_{NomeProcedura}`

**Nota**: uso `usp_` invece di `sp_` per evitare conflitti con stored procedure di sistema SQL Server

**Esempi**:
```sql
CREATE PROCEDURE usp_InserisciDocumento
    @Numero NVARCHAR(50),
    @DataEmissione DATE,
    @Importo DECIMAL(12,2)
AS
BEGIN
    INSERT INTO DOC_Documenti (DOC_Numero, DOC_DataEmissione, DOC_Importo)
    VALUES (@Numero, @DataEmissione, @Importo);
END;

CREATE PROCEDURE usp_CalcolaTotaleCarichi
    @ContribuenteId INT,
    @TotaleOut DECIMAL(12,2) OUTPUT
AS
BEGIN
    SELECT @TotaleOut = SUM(CAR_Importo)
    FROM CAR_Carichi
    WHERE CAR_CON_Id = @ContribuenteId;
END;
```

---

### 10. Funzioni

**Tabular Functions**: `ft_{NomeFunzione}`  
**Scalar Functions**: `fs_{NomeFunzione}`

**Esempi**:
```sql
-- Funzione tabulare
CREATE FUNCTION ft_GetDocumentiByAnno(@Anno INT)
RETURNS TABLE
AS
RETURN
(
    SELECT DOC_Id, DOC_Numero, DOC_DataEmissione
    FROM DOC_Documenti
    WHERE YEAR(DOC_DataEmissione) = @Anno
);

-- Funzione scalare
CREATE FUNCTION fs_CalcolaImpostaSuImporto(@Importo DECIMAL(12,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @Importo * 0.22;
END;
```

---

### 11. Tipologie Dati Standard

**Interi**:
```sql
TINYINT     -- Enum/lookup (0-255): priorità, livelli, step
SMALLINT    -- Codici limitati, anni (-32K a 32K)
INT         -- Chiavi primarie, contatori standard
BIGINT      -- Log, audit, high-volume
```

**Stringhe**:
```sql
CHAR(n)         -- Codici fissi: CHAR(16) per CodiceFiscale, CHAR(11) per P.IVA
VARCHAR(n)      -- Testi ASCII: codici variabili, username
NVARCHAR(n)     -- Testi UNICODE: nomi persone, aziende, città, indirizzi, note
```

**Sempre specificare lunghezza**. Evitare `VARCHAR(MAX)` e `VARBINARY(MAX)` salvo casi eccezionali.

**Decimali**:
```sql
DECIMAL(12,2)   -- Importi monetari
DECIMAL(5,2)    -- Percentuali (0.00 - 100.00)
DECIMAL(13,8)   -- Tariffe, coefficienti, calcoli alta precisione
```

**Date/Orari**:
```sql
DATE            -- Date business (DataEmissione, DataNascita)
DATETIME2(3)    -- Timestamp log/audit (UTC con millisecondi)
```

**Booleani**:
```sql
BIT             -- True/False (IsAttivo, HasPec)
```

---

### 12. Pattern Audit Standard

Ogni tabella business **deve** includere colonne di audit per tracciamento modifiche:

```sql
CREATE TABLE DOC_Documenti (
    -- Business columns
    DOC_Id INT IDENTITY(1,1) NOT NULL,
    DOC_Numero NVARCHAR(50) NOT NULL,
    DOC_DataEmissione DATE NOT NULL,
    DOC_Importo DECIMAL(12,2) NOT NULL,
    
    -- Audit columns (standard pattern)
    DOC_DataCreazione DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    DOC_UtenteCreazione NVARCHAR(100) NOT NULL,
    DOC_DataModifica DATETIME2(3) NULL,
    DOC_UtenteModifica NVARCHAR(100) NULL,
    
    CONSTRAINT DOC_PK PRIMARY KEY (DOC_Id)
);
```

**Colonne audit obbligatorie**:
- `{PREFIX}_DataCreazione`: timestamp creazione record (UTC)
- `{PREFIX}_UtenteCreazione`: username utente che ha creato il record
- `{PREFIX}_DataModifica`: timestamp ultima modifica (UTC, NULL se mai modificato)
- `{PREFIX}_UtenteModifica`: username utente ultima modifica (NULL se mai modificato)

**Timestamp in UTC**:
- Usare `SYSUTCDATETIME()` per consistenza multi-timezone
- Application layer converte in local time per display

**Eliminazioni**:
- Utilizzare `DELETE` fisico per rimozione dati
- Se necessario tracking eliminazioni, implementare tabella di log separata

---

### 13. Schema Database

**Default schema**: `dbo` (standard SQL Server)

**Schema opzionali** (per organizzazione logica in DB complessi):
```sql
app.Documenti           -- Schema applicativo
core.Contribuenti       -- Schema core domain
lookup.TipoDocumento    -- Schema tabelle lookup
audit.LogOperazioni     -- Schema audit/logging
```

**Decisione**: iniziare con `dbo`, introdurre schema solo se necessario (>50 tabelle)

---

### 14. Codifiche Business

Dove concordato con business, affiancare **codici alfanumerici** agli `Id` per riconoscibilità utente:

```sql
CREATE TABLE CON_Contribuenti (
    CON_Id INT IDENTITY(1,1) NOT NULL,
    CON_Codice CHAR(10) NOT NULL,          -- Codice business: "CONT000123"
    CON_CodiceFiscale CHAR(16) NOT NULL,
    CON_RagioneSociale NVARCHAR(200) NOT NULL,
    
    CONSTRAINT CON_PK PRIMARY KEY (CON_Id),
    CONSTRAINT CON_UK_Codice UNIQUE (CON_Codice),
    CONSTRAINT CON_UK_CodiceFiscale UNIQUE (CON_CodiceFiscale)
);

CREATE TABLE COM_Commesse (
    COM_Id INT IDENTITY(1,1) NOT NULL,
    COM_Codice CHAR(15) NOT NULL,          -- "COMM-2026-0001"
    COM_Descrizione NVARCHAR(500) NOT NULL,
    
    CONSTRAINT COM_PK PRIMARY KEY (COM_Id),
    CONSTRAINT COM_UK_Codice UNIQUE (COM_Codice)
);
```

**Vantaggi**: utenti riferiscono "Commessa COMM-2026-0001" invece di "Commessa ID 4782"

---

### 15. Tabelle Temporanee e Staging

**Tabelle temporanee locali**: `#TempNomeTabella`  
**Tabelle temporanee globali**: `##GlobalTempTabella`  
**Tabelle staging**: `STG_NomeTabella`

```sql
-- Temp locale (session-scoped)
CREATE TABLE #TempDocumenti (
    Id INT,
    Numero NVARCHAR(50)
);

-- Staging per ETL/import
CREATE TABLE STG_ImportContribuenti (
    STG_CodiceFiscale CHAR(16),
    STG_RagioneSociale NVARCHAR(200),
    STG_DataImport DATETIME2(3) DEFAULT SYSUTCDATETIME()
);
```

---

### 16. Omogeneità tra Bounded Context

**Principio**: mantenere naming consistente per entità condivise semanticamente

**Esempio - Indirizzi**:
```sql
-- Database ms-contribuenti
CREATE TABLE IND_Indirizzi (
    IND_Id INT IDENTITY(1,1),
    IND_Via NVARCHAR(200),
    IND_Civico NVARCHAR(10),
    IND_Cap CHAR(5),
    IND_Citta NVARCHAR(100)
);

-- Database ms-sedi (stesso schema)
CREATE TABLE IND_Indirizzi (
    IND_Id INT IDENTITY(1,1),
    IND_Via NVARCHAR(200),
    IND_Civico NVARCHAR(10),    -- NO "IND_NumeroCivico"
    IND_Cap CHAR(5),
    IND_Citta NVARCHAR(100)
);
```

**Eccezione**: se il significato semantico diverge, è accettabile variare (es. `IND_Civico` vs `IND_CodiceCivico` se hanno significati diversi nei rispettivi bounded context)

---

## Alternatives Considered

### Alternative 1: Prefisso a 5 caratteri
- **Descrizione**: Prefisso di 5 caratteri come proposto nel documento originale (es. `DOCUM_Documenti`)
- **Pro**: 
  - Maggiore univocità (più combinazioni possibili)
  - Prefisso più "parlante" (`DOCUM` vs `DOC`)
- **Contro**: 
  - Verbosità eccessiva: `DOCDE_DocumentiDettaglio` = 26 caratteri solo per prefisso+base
  - Ridondanza: il prefisso cerca di "ricordare" il nome già esplicito
  - Spreco di spazio in 14 database separati (collision improbabili)
  - Oltre limite 30 caratteri facilmente raggiunto
- **Decisione**: Troppo verboso per beneficio marginale in context isolati

### Alternative 2: Nessun prefisso (solo PascalCase)
- **Descrizione**: Tabelle/colonne senza prefisso (es. `Documenti.Numero`, `Contribuenti.CodiceFiscale`)
- **Pro**: 
  - Massima leggibilità e concisione
  - Approccio ORM-friendly (Entity Framework convention)
  - Standard in molti progetti moderni
- **Contro**: 
  - FK ambigue: `DocumentiDettaglio.DocumentoId` non indica chiaramente la tabella origine
  - Collision su nomi comuni (`Id`, `Codice`, `Descrizione`) in query JOIN complesse
  - Difficile identificare ownership colonna in query multi-tabella
  - Migrazione cross-DB complessa (no namespace)
- **Decisione**: Scartato per mantenere tracciabilità esplicita e ridurre ambiguità

### Alternative 3: Schema SQL come namespace
- **Descrizione**: Usare schema SQL invece di prefissi (es. `doc.Documenti`, `con.Contribuenti`)
- **Pro**: 
  - Namespace nativo SQL
  - Organizzazione logica chiara
  - Permessi granulari per schema
- **Contro**: 
  - Colonne non hanno namespace (ancora necessità prefisso)
  - Verbosità in query (`SELECT doc.Numero FROM doc.Documenti doc`)
  - Overhead configurazione permessi
  - Overkill per database con <50 tabelle
- **Decisione**: Troppo complesso per beneficio attuale, lasciato come opzione futura

### Alternative 4: Prefisso per bounded context invece di tabella
- **Descrizione**: Prefisso identifica il microservizio, non la tabella (es. `CONT_Contribuenti`, `CONT_Indirizzi` in ms-contribuenti)
- **Pro**: 
  - Identifica ownership a livello servizio
  - Facilita identificazione cross-DB in reporting
- **Contro**: 
  - Perde significato mnemonic (CONT non ricorda "Indirizzi")
  - FK non identificano tabella origine (`CONT_CONT_Id` ambiguo)
  - Ridondante in database-per-service (già isolato)
- **Decisione**: Utile solo in shared database, non applicabile a ADR-007

### Alternative 5: Nomi in inglese
- **Descrizione**: Nomenclatura in inglese invece di italiano
- **Pro**: 
  - Standard internazionale
  - Evita problemi collation/encoding
  - Code più "professionale"
- **Contro**: 
  - Disallineamento con business domain (documenti, contribuenti, carichi)
  - Team prevalentemente italiano
  - Traduzione introduce ambiguità semantica
  - Stakeholder non tecnici non riconoscono entità
- **Decisione**: Requisito esplicito per naming in italiano, non negoziabile

---

## Consequences

### Positive Consequences
- ✅ **Clarity**: naming self-documenting riduce necessità di documentazione esterna
- ✅ **Consistency**: uniform conventions tra 14 database e team multipli
- ✅ **Traceability**: FK con prefisso origine rendono relazioni esplicite senza schema diagrams
- ✅ **Audit built-in**: pattern standard per tracking modifiche in ogni tabella
- ✅ **Business alignment**: nomi italiani allineati con dominio business
- ✅ **Maintainability**: prefisso a 3 char bilancia concisione e identificabilità
- ✅ **Validation**: check constraints catturano errori dati in DB layer
- ✅ **Simplicity**: audit pattern minimalista (4 colonne) riduce overhead

### Negative Consequences
- ⚠️ **Verbosità query**: `SELECT DOC_Numero FROM DOC_Documenti WHERE DOC_Id = 1` più lungo di `SELECT Numero FROM Documenti WHERE Id = 1`
- ⚠️ **ORM friction**: Entity Framework conventions richiedono mapping esplicito
- ⚠️ **Learning curve**: sviluppatori abituati a naming OOP devono adattarsi
- ⚠️ **Refactoring risk**: rinominare tabella richiede update prefisso in tutte le colonne e FK
- ⚠️ **No soft-delete**: eliminazioni fisiche, necessaria tabella log separata per audit eliminazioni

### Neutral Consequences
- 📌 Necessità di **code generation tools** per scaffold tabelle con pattern audit
- 📌 Documentazione e **training team** su conventions
- 📌 **Code review checklist** per verificare aderenza a standard
- 📌 Possibile **script SQL di validation** per check conformità schema

---

## Implementation Notes

### 1. Entity Framework Mapping

Le conventions richiedono mapping esplicito in EF Core:

```csharp
public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("DOC_Documenti");
        
        builder.HasKey(e => e.Id)
            .HasName("DOC_PK");
        
        builder.Property(e => e.Id)
            .HasColumnName("DOC_Id")
            .UseIdentityColumn();
        
        builder.Property(e => e.Numero)
            .HasColumnName("DOC_Numero")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(e => e.DataEmissione)
            .HasColumnName("DOC_DataEmissione")
            .HasColumnType("DATE")
            .IsRequired();
        
        // Audit properties
        builder.Property(e => e.DataCreazione)
            .HasColumnName("DOC_DataCreazione")
            .HasColumnType("DATETIME2(3)")
            .HasDefaultValueSql("SYSUTCDATETIME()");
        
        // Indexes
        builder.HasIndex(e => e.DataEmissione)
            .HasDatabaseName("DOC_IX_DataEmissione");
    }
}
```

### 2. Migration Template

Template per generazione migration con audit pattern:

```csharp
migrationBuilder.CreateTable(
    name: "DOC_Documenti",
    columns: table => new
    {
        DOC_Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        DOC_Numero = table.Column<string>(maxLength: 50, nullable: false),
        DOC_DataEmissione = table.Column<DateTime>(type: "DATE", nullable: false),
        DOC_Importo = table.Column<decimal>(type: "DECIMAL(12,2)", nullable: false),
        
        // Audit columns
        DOC_DataCreazione = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, 
            defaultValueSql: "SYSUTCDATETIME()"),
        DOC_UtenteCreazione = table.Column<string>(maxLength: 100, nullable: false),
        DOC_DataModifica = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
        DOC_UtenteModifica = table.Column<string>(maxLength: 100, nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("DOC_PK", x => x.DOC_Id);
    });
```

### 3. Audit Interceptor

SaveChanges interceptor per popolare colonne audit automaticamente:

```csharp
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void UpdateAuditFields(DbContext context)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditableEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        var currentUser = _currentUser.Username;
        var currentTime = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCreazione").CurrentValue = currentTime;
                entry.Property("UtenteCreazione").CurrentValue = currentUser;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("DataModifica").CurrentValue = currentTime;
                entry.Property("UtenteModifica").CurrentValue = currentUser;
            }
        }
    }
}
```

### 4. Validation Script

Script T-SQL per verificare conformità schema a conventions:

```sql
-- Check tabelle senza PK
SELECT t.name AS TabellaSenzaPK
FROM sys.tables t
LEFT JOIN sys.key_constraints kc ON t.object_id = kc.parent_object_id AND kc.type = 'PK'
WHERE kc.object_id IS NULL
  AND t.name NOT LIKE '#%';  -- Escludi temp tables

-- Check colonne senza prefisso
SELECT 
    t.name AS Tabella,
    c.name AS ColonnaSenzaPrefisso
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE c.name NOT LIKE SUBSTRING(t.name, 1, 3) + '_%'
  AND c.name NOT LIKE '#%';

-- Check PK naming convention
SELECT 
    t.name AS Tabella,
    kc.name AS PrimaryKeyNonConforme
FROM sys.tables t
INNER JOIN sys.key_constraints kc ON t.object_id = kc.parent_object_id
WHERE kc.type = 'PK'
  AND kc.name <> SUBSTRING(t.name, 1, CHARINDEX('_', t.name)) + 'PK';

-- Check colonne audit mancanti (DataCreazione obbligatoria)
SELECT t.name AS TabellaSenzaAudit
FROM sys.tables t
WHERE NOT EXISTS (
    SELECT 1 FROM sys.columns c 
    WHERE c.object_id = t.object_id 
    AND c.name LIKE '%_DataCreazione'
)
AND t.name NOT LIKE 'STG_%'  -- Escludi staging
AND t.name NOT LIKE 'T%_%'   -- Escludi tabelle lookup
AND t.name NOT LIKE '#%';    -- Escludi temp

-- Check pattern audit completo (4 colonne)
SELECT 
    t.name AS Tabella,
    CASE WHEN c1.name IS NULL THEN 'Manca DataCreazione' END AS Errore1,
    CASE WHEN c2.name IS NULL THEN 'Manca UtenteCreazione' END AS Errore2,
    CASE WHEN c3.name IS NULL THEN 'Manca DataModifica' END AS Errore3,
    CASE WHEN c4.name IS NULL THEN 'Manca UtenteModifica' END AS Errore4
FROM sys.tables t
LEFT JOIN sys.columns c1 ON t.object_id = c1.object_id AND c1.name LIKE '%_DataCreazione'
LEFT JOIN sys.columns c2 ON t.object_id = c2.object_id AND c2.name LIKE '%_UtenteCreazione'
LEFT JOIN sys.columns c3 ON t.object_id = c3.object_id AND c3.name LIKE '%_DataModifica'
LEFT JOIN sys.columns c4 ON t.object_id = c4.object_id AND c4.name LIKE '%_UtenteModifica'
WHERE (c1.name IS NULL OR c2.name IS NULL OR c3.name IS NULL OR c4.name IS NULL)
  AND t.name NOT LIKE 'STG_%'
  AND t.name NOT LIKE 'T%_%'
  AND t.name NOT LIKE '#%';
```

### 5. Code Generation Template

Template T4/Razor per generazione entity con convenzioni:

```csharp
public abstract class AuditableEntity
{
    public DateTime DataCreazione { get; set; }
    public string UtenteCreazione { get; set; }
    public DateTime? DataModifica { get; set; }
    public string UtenteModifica { get; set; }
}

public class Documento : AuditableEntity
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public DateTime DataEmissione { get; set; }
    public decimal Importo { get; set; }
    
    // Navigation properties
    public TipoDocumento TipoDocumento { get; set; }
    public ICollection<DocumentoDettaglio> Dettagli { get; set; }
}
```

### 6. Checklist Code Review

- [ ] Tabella ha prefisso 3 caratteri maiuscoli
- [ ] Tutte le colonne hanno prefisso tabella
- [ ] Primary key naming: `{PREFIX}_PK`
- [ ] Foreign key naming: `{PREFIX}_FK_{Descrizione}`
- [ ] Unique constraint naming: `{PREFIX}_UK_{Descrizione}`
- [ ] Check constraint naming: `{PREFIX}_CK_{Descrizione}`
- [ ] Indici naming: `{PREFIX}_IX_{Descrizione}`
- [ ] Stored proc naming: `usp_{Nome}` (non `sp_`)
- [ ] Viste naming: `v_{Nome}`
- [ ] Funzioni naming: `ft_{Nome}` o `fs_{Nome}`
- [ ] Colonne audit presenti (4 colonne standard: DataCreazione, UtenteCreazione, DataModifica, UtenteModifica)
- [ ] Timestamp in UTC (`DATETIME2(3)` + `SYSUTCDATETIME()`)
- [ ] Check constraints per CF/PIVA/Email/CAP dove applicabile
- [ ] Tipi dati corretti (DECIMAL per importi, NVARCHAR per UNICODE)
- [ ] No abbreviazioni (eccetto `Id`)
- [ ] PascalCase rispettato
- [ ] Lunghezza nome completo < 48 caratteri

---

## References

- [SQL Server Naming Conventions Best Practices](https://www.sqlshack.com/learn-sql-naming-conventions/)
- [Entity Framework Core Fluent API](https://learn.microsoft.com/en-us/ef/core/modeling/)
- [SQL Server Data Types](https://learn.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql)
- [SQL Server Constraints](https://learn.microsoft.com/en-us/sql/relational-databases/tables/unique-constraints-and-check-constraints)
- ADR-007: Database per Service Pattern
- ADR-002: Clean Architecture Pattern (Entity/Repository layer)

---

## Review Schedule

**Next review**: 2026-07-15 (6 months)

**Review triggers**:
- Feedback team dopo primi 3 microservizi implementati
- Problemi ricorrenti in code review
- Richieste modifica da DBA o architecture team
