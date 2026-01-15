# Database ER Diagram - MS Risko Carichi

Questo documento rappresenta la struttura del database del microservizio MS Risko Carichi attraverso un diagramma Entity-Relationship (ER).

## Struttura Database

Il database è composto da tre entità principali:
- **Soggetti**: Tabella contenente i dati dei soggetti
- **Carichi**: Tabella principale contenente i dati dei carichi
- **CarichiDettaglio**: Tabella contenente i documenti associati ai carichi
- **CarichiDettaglioVoci**: Tabella contenente le voci dettagliate dei documenti

## Diagramma ER

```mermaid
erDiagram
    Carichi ||--o{ SoggettiCarichiDettaglio : "ha"
    Carichi ||--o{ CarichiDettaglio : "ha"
    CarichiDettaglio ||--o{ CarichiDettaglioVoci : "ha"
    Soggetti ||--o{ SoggettiCarichiDettaglio : "ha"
    Soggetti ||--o{ Indirizzi : "ha"
    Soggetti ||--o{ Contatti : "ha"
    Soggetti }o--|| TipiPersona : "tipo"
    Soggetti }o--|| TipiNaturaSoggetto : "natura"
    Contatti }o--|| TipiContatto : "tipo"
    Carichi ||--o{ CarichiDettaglio : "ha"
    CarichiDettaglio }o--|| TipiProvenienza : "tipo"
    CarichiDettaglio }o--|| TipiDocumento : "tipo"
    CarichiDettaglio }o--|| TipiStato : "stato"
    CarichiDettaglio }o--|| TipiNormative : "normativa"
    CarichiDettaglio ||--o{ CarichiDettaglioVoci : "ha"
    CarichiDettaglioVoci }o--|| CodiciEntrata : "codifica"
    CarichiDettaglioVoci }o--|| TipiImportoVoce : "tipo"
    CarichiDettaglioVoci }o--|| MacroVociEntrata : "macro codifica"
    
    Soggetti {
        int Id PK "Identificativo univoco"
        string CodiceFiscale "varchar(16) | Codice fiscale"
        string PartitaIva "varchar(11) | Partita IVA"
        string IdTipoPersona "tinyint | Riferimento tipo persona"
        string IdTipoNatura "tinyint | Riferimento natura soggetto"
        string Nome "varchar(100) | Nome"
        string Cognome "varchar(100) | Cognome"
        string RagioneSociale "varchar(150) | Ragione sociale"
        date DataNascita "Date | Data nascita/inizio attività"
        date DataFine "Date | Data decesso/cessazione"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    Indirizzi {
        int Id PK "Identificativo univoco"
        int IdContraente FK "Riferimento contraente"
        string Toponimo "varchar(15) | Toponimo"
        string DenominazioneStradale "varchar(100) | Denominazione stradale"
        string Civico "varchar(5) | Civico"
        string Km "varchar(10) | Km"
        string Esponente "varchar(10) | Esponente"
        string Edificio "varchar(10) | Edificio"
        string Scala "varchar(5) | Scala"
        string Piano "varchar(5) | Piano"
        string Interno "varchar(5) | Interno"
        string Cap "varchar(12) | CAP"
        string Comune "varchar(50) | Comune"
        string Localita "varchar(50) | Localita"
        string Provincia "varchar(2) | Provincia"
        string Nazione "varchar(50) | Nazione"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    Contatti {
        int Id PK "int | Identificativo univoco"
        int IdTipo FK "tinynt | Tipo contatto"
        int IdContraente FK "int | Riferimento contraente"
        string Contatto "varchar(254) | Contatto"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    TipiContatto {
        int Id PK "tinynt | Identificativo univoco"
        string Tipo "varchar(15) | Tipo contatto"
    }
    
    TipiPersona {
        int Id PK "tinynt | Identificativo univoco"
        string Tipo "varchar(20) | Tipo persona" %% 1 = Persona fisica, 2 = Persona giuridica
    }

    TipiNaturaSoggetto {
        int Id PK "tinynt | Identificativo univoco"
        string Natura "varchar(25) | Sesso/Natura giuridica soggetto" %% 1 = Maschio, 2 = Femmina, 3 = Ditta individuale, 4 = Pubblica amministrazione, 5 = Società di persone, 6 = Società di capitali, 99 = TODO
    }

    Carichi {
		int Id PK "int | Identificativo univoco"
		string Descrizione "varchar(150) | Descrizione carico"
		date DataCreazione "date | Data creazione carico"
		date DataArrivo "date | Data arrivo carico"
        date DataValidazione "date | Data in cui il carico diventa effettivo"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    CarichiDettaglio {
        int Id PK "int | Identificativo univoco"
        int IdCarico FK "int | Riferimento carico"
        int IdCommessa FK "int | Riferimento commessa"
        int IdTipoProvenienza FK "tinyint | Tipo provenienza carico" %% può capitare che un carico da tracciato possa essere integrato manualmente, quindi a parità di carico possono esserci più dettagli con provenienze diverse
        int IdTipoDocumento FK "tinyint | Tipo documento da riscuotere"
        string NumeroDocumento "varchar(50) | Numero documento da riscuotere"
        date DataDocumento "date | Data documento da riscuotere"
        int IdTipoDocumentoRiferito FK "tinyint | Opzionale: tipo documento antecedente il documento principale che stiamo passando al coattivo"
        string NumeroDocumentoRiferito "varchar(50) | Opzionale: numero documento antecedente il documento principale che stiamo passando al coattivo"
        date DataDocumentoRiferito "date | Opzionale: data documento antecedente il documento principale che stiamo passando al coattivo"
        date DataNotifica "date | Data notifica documento da riscuotere"
        date DataScadenza "date | Data scadenza del documento da riscuotere: se non presente, è uguale alla data notifica + x giorni (da configurare)"
        date DataInizioInteressi "date | Data da cui inizia il calcolo degli interessi: viene calcolata a partire dalla data scadenza + 1, oppure viene passata direttamente"
        date DataPrescrizione "date | Data prescrizione: se non presente, viene calcolata secondo le indicazioni dell'ufficio (es.°: Data documento + x anni)"
        int IdStato FK "tinyint | Stato carico dettaglio"
        string TargaVeicolo "varchar(10) | Targa del veicolo (se presente)"
        string InfoVeicolo "varchar(60) | Info aggiuntive sul veicolo (marca, modello, ecc.)"
        string InfoSanzione "varchar(50) | Articolo violazione C.D.S. o altra sanzione"
        string SollecitoOriginale "varchar(50) | Indica il numero del sollecito originale da cui è stato generato questo carico dettaglio (se presente)"
        string Sentenza "varchar(200) | Indica i riferimenti della sentenza (se presente)"
        int IdNormativa FK "tinyint | Normativa di riferimento"
        string KeyRuolo "varchar(100) | Nota aggiuntiva sul carico" %% TODO: Indagare meglio a cosa serve questo campo in Risko
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    SoggettiCarichiDettaglio {
        int IdSoggetto FK "int | Riferimento soggetto"
        int IdCaricoDettaglio FK "int | Riferimento carico dettaglio"
        int IdCaricoDettaglioPadre FK "int | Riferimento carico dettaglio padre (per gestire gli eredi)"
        int IdRelazione FK "tinyint | Tipo relazione tra i soggetti del carico"
        decimal QuotaCarico "decimal(5,2) | Percentuale di 'possesso' del carico da parte del soggetto erede (per default 100)"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    TipiRelazioniSoggettiCarichiDettaglio {
        int Id PK "tinyint | Identificativo univoco"
        string Tipo "varchar(50) | Tipo relazione" %% es°.: 1 = Debitore principale, 2 = Coobbligato, 3 = Esecutato, 4 = Terzo, ecc. TODO: prendi le relazioni dal tracciato 600
    }
    
    TipiNormative {
        int Id PK "tinyint | Identificativo univoco"
        string Tipo "varchar(100) | Normativa di riferimento"
        date DataInizio "date | Data inizio validità"
        date DataFine "date | Data fine validità" %% se null significa che è quella attualmente in vigore
    }

    CarichiDettaglioVoci {
        int Id PK "int | Identificativo univoco"
        int IdCaricoDettaglio FK "int | Riferimento carico dettaglio"
        int IdCodiceEntrata FK "smallint | Codice entrata"
        smallint AnnoRiferimento "smallint | Anno di riferimento della voce (es.°: anno di imposta)"
        decimal Importo "decimal(13,2) | Importo voce"
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    Note {
        int Id PK "Identificativo univoco"
        int NomeTabella FK varchar(48) %% indica a quale tabella si riferisce la nota (CarichiDettaglio, Carichi, Soggetti, ecc.)
        int IdTabella FK int
        string Testo nvarchar(400)
        string UtenteCreazione "varchar(100) | Utente creazione"
        datetime DataCreazione "datetime2(3) | Data creazione"
        string UtenteModifica "varchar(100) | Utente modifica"
        datetime DataModifica "datetime2(3) | Data modifica"
    }

    TipiProvenienza {
        int id PK "tinyint | Identificativo univoco"
        string Tipo "varchar(25) | Tipo provenienza" %% 1 = Manuale, 2 = Excel, 3 = Tracciato 290, 4 = Tracciato 600, 99 = TODO
    }

    TipiDocumento {
        int id PK "tinyint | Identificativo univoco"
        string Tipo "varchar(25) | Tipologia documento" %% 1 = Avviso di accertamento, 2 = Cartella di pagamento, 3 = Avviso bonario, 4 = Ordinanza ingiunzione, 5 = Altri documenti, 99 = TODO VEDI Gruppo = 'TIPO.DOC' su TABELLE
    }

    TipiStato {
        int id PK "tinyint | Identificativo univoco"
        string Tipo "varchar(25) | Stato carico" %% 1 = Attivo (ATT), 2 = Chiuso (CHS), 3 = Soggetto deceduto (DEC), 4 = Cancellato (DEL), 5 = Sospeso (SOS), 6 = Annullato dall'Ente, 99 = TODO
    }
	
```

## Relazioni

TODO

# Approccio alla migrazione

TotInteressiAtti, TotSanzioniAtti, TotInteressiRateizzo sono campi calcolati che attualmente stanno in CarichiDettaglio, ma andrebbero spostati in CarichiDettaglioVoci: andranno quindi opportunamente codificati prendendoli da rs_AttiDettaglio
Annotazioni StatoAnnotazioni e campi DescCaricoX andranno mappati nelle note collegate al carico dettaglio
bool HasRecuperoSpeseCarico Bit %%In Risko non viene quasi mai usato (dal 2015 solo 1 volta): serve per riuscire a recuperare le spese sostenute da Andreani per la notifica

