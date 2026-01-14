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
    Soggetti ||--o{ CarichiDettaglio : "ha"
    Carichi ||--o{ CarichiDettaglio : "ha"
    CarichiDettaglio ||--o{ CarichiDettaglioVoci : "ha"
    
    Soggetti {

        int Id PK "Identificativo univoco"
        string CodiceFiscale varchar(16)
        string PartitaIva varchar(11)
        string IdTipoSoggetto int
        string Nome varchar(100)
        string Cognome varchar(100)
        string RagioneSociale varchar(150)
        date DataNascita Date
        date DataFine Date "Data decesso/cessazione"
        Guid UtenteModifica uniqueidentifier "Utente modifica"
        datetime DataModifica "Data modifica"
    }

    Indirizzi { %%TODO: vedi quella su DbContratti + valutare se mettere solo indirizzo di residenza/sede legale o gestire anche gli altri indirizzi
        int Id PK "Identificativo univoco"
        int IdSoggetto FK int
    }

    TipiSoggetto {
        int Id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Maschio, 2 = Femmina, 3 = Persona giuridica, 4 = Ditta individuale, 5 = Ente, 6 = Altro TODO: individuare altre tipologie
    }

    Carichi {
		int Id PK "Identificativo univoco"
		string Descrizione varchar(150) %% in Risko arriva massimo a 135 caratteri
		date DataCreazione Date
		date DataArrivo Date
		date DataValidazione Date
        Guid UtenteModifica uniqueidentifier "Utente modifica"
        datetime DataModifica "Data modifica"
    }

    CarichiDettaglio {
        int Id PK int
        int IdCarico FK int
        int IdCommessa FK int
        int IdTipoProvenienza FK int %% può capitare che un carico da tracciato possa essere integrato manualmente, quindi a parità di carico possono esserci più dettagli con provenienze diverse
        int IdTipoEntrata FK int
        int IdTipoDocumento FK int
        string NumeroDocumento varchar(50) %% Numero del documento che effettivamente deve essere riscosso
        date DataDocumento Date
        int IdTipoDocumentoRiferito FK int  %% Opzionale: tipo del documento antecedente il documento principale che stiamo passando al coattivo
        string NumeroDocumentoRiferito varchar(50) %% Opzionale: numero del documento antecedente il documento principale che stiamo passando al coattivo
        date DataDocumentoRiferito Date %% Opzionale: data del documento antecedente il documento principale che stiamo passando al coattivo
        date DataNotifica Date
        date DataScadenza Date %% se non presente, è uguale alla data notifica + x giorni (da configurare)
        date DataInizioInteressi Date %% Data da cui calcolare gli interessi: viene calcolata a partire dalla data scadenza + 1, oppure viene passata direttamente
        date DataPrescrizione Date %% se non presente, viene calcolata secondo le indicazioni dell'ufficio, es.°: Data documento + 5 anni ???
        int IdAttoSuccessivo FK int %%TODO: Atto successivo (tabella Atti): serve qui? Come lo vogliamo gestire? Vedere eventualmente quando tratteremo la definizione di atto e pratica
        int IdStato FK int
        bool HasRecuperoSpeseCarico Bit %%In Risko non viene quasi mai usato (dal 2015 solo 1 volta): serve per riuscire a recuperare le spese sostenute da Andreani per la notifica
        string TargaVeicolo varchar(10)
        string InfoVeicolo varchar(60) %% marca, modello
        string InfoSanzione varchar(50) %% articolo della violazione cds o altra sanzione
        string SollecitoOriginale varchar(50) %% indica il numero del sollecito originale da cui è stato generato questo carico dettaglio (se presente)
        string Sentenza varchar(200) %% indica i riferimenti della sentenza (se presente)
        int IdNormativa FK int "normativa di riferimento"
        string KeyRuolo varchar(100)
        Guid UtenteModifica uniqueidentifier "Utente modifica"
        datetime DataModifica "Data modifica"
    }

    Normative {
        int Id PK "Identificativo univoco"
        string Tipo varchar(100)
        date DataInizio Date
        date DataFine Date %% se null significa che è quella attualmente in vigore
    }

    CarichiDettaglioVoci {
        int Id PK int
        int IdCaricoDettaglio FK int
        int IdCodiceEntrata FK int
        int IdTipoImportoVoce FK int
        int IdMacroVoceEntrata FK int
        smallint AnnoRiferimento smallint %% anno di riferimento della voce (es. anno di imposta)        
        decimal Importo Decimal(13,2)
    }

    CodiciEntrata {
        int Id PK "Identificativo univoco"
        string CodiceEntrata UK varchar(10) %% codice dell'entrata interno: se il codice Agenzia delle Entrate non è disponibile, ne usiamo uno nostro
        string CodiceAE UK varchar(10) %% codice dell'entrata secondo l'Agenzia delle Entrate
        string DenominazioneEntrata varchar(120)
        string DenominazioneEstesa varchar(160)
    }

    TipiImportoVoce {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Netto (N), 2 = Accessorio (A)
    }

    MacroVociEntrata {
        int Id PK "Identificativo univoco"
        string Descrizione varchar(30) %% select Corrispondenza, count(*) from rs_CodiciTributo group by Corrispondenza order by Corrispondenza
    }

    SoggettiCarichiDettaglio {
        int IdSoggetto FK int
        int IdCaricoDettaglio FK int
        int IdCaricoDettaglioPadre FK int %% per gestire gli eredi
        int IdRelazione FK int %% es. 1 = Debitore principale, 2 = Coobbligato, 3 = Esecutato, 4 = Terzo, ecc. TODO: prendi le relazioni dal tracciato 600
        decimal QuotaPossesso decimal(5,2) %% percentuale di possesso del carico da parte del soggetto erede (per default 100)
    }
    
    Note {
        int Id PK "Identificativo univoco"
        int NomeTabella FK varchar(30) %% indica a quale tabella si riferisce la nota (CarichiDettaglio, Carichi, Soggetti, ecc.)
        int IdTabella FK int
        string Testo nvarchar(400)
        Guid UtenteModifica uniqueidentifier "Utente modifica"
        datetime DataModifica "Data modifica"
    }

    TipiProvenienza {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Manuale, 2 = Excel, 3 = Tracciato 290, 4 = Tracciato 600, 99 = TODO
    }

    TipiDocumento {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Avviso di accertamento, 2 = Cartella di pagamento, 3 = Avviso bonario, 4 = Ordinanza ingiunzione, 5 = Altri documenti, 99 = TODO VEDI Gruppo = 'TIPO.DOC' su TABELLE
    }

    TipiStato {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Attivo (ATT), 2 = Chiuso (CHS), 3 = Deceduto (DEC), 4 = Cancellato (eliminazione logica) (DEL), 5 = Sospeso (SOS), 99 = TODO
    }
	
```

## Relazioni

TODO

# Approccio alla migrazione

TotInteressiAtti, TotSanzioniAtti, TotInteressiRateizzo sono campi calcolati che attualmente stanno in CarichiDettaglio, ma andrebbero spostati in CarichiDettaglioVoci: andranno quindi opportunamente codificati prendendoli da rs_AttiDettaglio
Annotazioni StatoAnnotazioni e campi DescCaricoX andranno mappati nelle note collegate al carico dettaglio

