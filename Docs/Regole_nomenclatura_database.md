# Regole e formalismi per la nomenclatura dei database

## Tabelle

- Il nome completo di ogni tabella deve essere composto da:
  **Prefisso di 5 caratteri maiuscoli** + `_` + **NomeTabella**  
  > Valutare se ridurre il prefisso a 3 caratteri.

- Se `NomeTabella` inizia con **Tipo** e prosegue con altre parole:
  - la prima lettera del prefisso deve essere `T`
  - le altre lettere devono appartenere alle altre parole

- Il prefisso deve:
  - identificare univocamente la tabella all’interno del *bounded context*
  - ricordare il nome della tabella usando le prime lettere delle parole

- `NomeTabella` deve:
  - essere in **PascalCase**
  - non superare i **30 caratteri**
  - non contenere abbreviazioni o sigle, salvo superamento dei 30 caratteri

**Esempi**

- `DOCUM_Documenti`
- `DOCDE_DocumentiDettaglio`
- `DOCCO_DocumentiCorpo`

---

## Campi

- Il nome completo di ogni campo deve essere composto da:
  **Prefisso di 5 caratteri maiuscoli** + `_` + **NomeCampo**  
  > Valutare se ridurre il prefisso a 3 caratteri.

- Il prefisso deve essere lo stesso usato per `NomeTabella`

- `NomeCampo` deve:
  - essere in **PascalCase**
  - non superare i **30 caratteri**
  - non contenere abbreviazioni o sigle, ad eccezione di `Id`

- Se `NomeTabella` inizia con `Tipo`, la tabella deve contenere almeno:
  - `Id`
  - `Tipo`

- I campi `Id`:
  - possono essere usati solo come **chiavi primarie o esterne**
  - devono essere legati a chiavi primarie
  - valutare l’uso di **SEQUENCE** invece di `IDENTITY` (migliori performance)

---

## Tipologie dei campi

### Campi interi

- `TINYINT`: valori tra 2 e 20 (enumerazioni, livelli, step, priorità)
- `SMALLINT`: codici limitati, anni, versioni
- `INT`: chiavi (`Id`) e contatori
- `BIGINT`: log, integrazioni, audit, sistemi ad alto volume

### Campi stringa

- Devono avere **sempre lunghezza definita**
- Tipi consentiti:
  - `CHAR`: codici a lunghezza fissa
  - `VARCHAR`: stringhe ASCII / Latin
  - `NVARCHAR`: note, nomi persone, aziende, città, indirizzi (UNICODE)

> Evitare `VARBINARY` e `VARCHAR(MAX)`, salvo casi eccezionali da concordare

### Campi decimali

- Importi: `DECIMAL(12,2)`
- Percentuali: `DECIMAL(5,2)`
- Tariffe / coefficienti: `DECIMAL(13,8)`

### Date

- Usare `DATE`
- Eccezione: **log** in `DATETIME` **UTC**

---

## Codifiche alfanumeriche

Dove concordato, prevedere codici alfanumerici univoci in parallelo agli `Id`, per permettere all’utente di riconoscere l’entità.

**Esempi**:
- Codice ente
- Codice commessa

---

## Campi booleani (bit)

- Devono iniziare con:
  - `Is` oppure
  - `Has`

---

## Chiavi esterne

- I campi di chiave esterna devono:
  - mantenere il **prefisso della tabella di riferimento**
  - essere identici al campo a cui fanno riferimento

**Esempio**

- Tabella `DOCUM_Documenti` → `DOCUM_Id`
- Tabella `DOCDE_DocumentiDettaglio` → `DOCDE_Id`, `DOCDE_DOCUM_Id`

---

## Omogeneità tra bounded context

Mantenere la stessa nomenclatura per campi che rappresentano la stessa informazione, anche in contesti diversi.

**Esempio**

- `INDIRIZZARIO.IND_Indirizzi (IND_Id, IND_Via, IND_Civico)`
- `SOGGETTI.IND_Indirizzi (IND_Id, IND_Via, IND_Civico)`

> Evitare: `IND_NumeroCivico`

---

## Chiavi primarie

- Nome: **Prefisso (5 char)** + `_PK`
- Prefisso uguale a quello della tabella
- Ogni tabella deve avere una chiave primaria

---

## Chiavi univoche

- Nome: **Prefisso** + `_UK_` + `NomeChiave`
- `NomeChiave` in **PascalCase**

---

## Chiavi esterne

- Nome: **Prefisso** + `_FK_` + `NomeChiave`
- `NomeChiave` in **PascalCase**

---

## Indici

- Nome: **Prefisso** + `_IX_` + `NomeIndice`
- `NomeIndice` in **PascalCase`

> Evitare indici full-text salvo casi eccezionali da concordare

---

## Vincoli

- Nome: **Prefisso** + `_CK_` + `NomeVincolo`
- `NomeVincolo` in **PascalCase**

- Usare vincoli per campi con formati rigidi:
  - Codice fiscale
  - Partita IVA
  - Email / PEC
  - CAP
  - Codifiche personalizzate

---

## Viste

- Nome: `vw_` + `NomeVista`
- `NomeVista` in **PascalCase**

---

## Stored procedure

- Nome: `sp_` + `NomeProcedura`
- `NomeProcedura` in **PascalCase**

---

## Funzioni tabellari

- Nome: `ft_` + `NomeFunzione`
- `NomeFunzione` in **PascalCase**

---

## Funzioni scalari

- Nome: `fs_` + `NomeFunzione`
- `NomeFunzione` in **PascalCase**

