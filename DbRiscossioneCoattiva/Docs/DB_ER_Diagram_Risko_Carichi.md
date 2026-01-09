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
        datetime DataModifica DateTime
    }

    Indirizzi { %%TODO: vedi quella su DbClienti + valutare se mettere solo indirizzo di residenza/sede legale o gestire anche gli altri indirizzi
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
		datetime data_modifica DateTime
    }

    CarichiDettaglio {
        int Id PK int
        int IdCarico FK int
        int IdTipoProvenienza FK int %% può capitare che un carico da tracciato possa essere integrato manualmente, quindi a parità di carico possono esserci più dettagli con provenienze diverse
        int IdSoggetto FK int
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
        int IdAttoSuccessivo FK int %%TODO: Atto successivo (tabella Atti): serve qui? Come lo vogliamo gestire?
        int IdStato FK int
        bool HasRecuperoSpeseCarico Bit %%In Risko non viene quasi mai usato (dal 2015 solo 1 volta): serve per riuscire a recuperare le spese sostenute da Andreani per la notifica
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
        string Tipo varchar(25) %% 1 = Attivo (ATT), 2 = Chiuso (CHS), 3 = Deceduto (DEC), 4 = ??? (DEL), 5 = Sospeso (SOS), 99 = TODO
    }

    CarichiDettaglioVoci {
        
    }
	
```

## Relazioni
- Un **CLIENTE** può avere uno o più **ENTI** e un **ENTE** può appartenere a più **CLIENTI**(relazione N:N)
- Un **ENTE** può avere una o più **COMMESSE** (relazione 1:N)
- Una **COMMESSA** appartiene sempre ad un solo **ENTE**

# approccio alla migrazione

1. TODO
