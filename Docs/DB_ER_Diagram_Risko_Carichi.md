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
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Maschio, 2 = Femmina, 3 = Persona giuridica, 4 = Ditta individuale, 5 = Ente, 6 = Altro TODO: individuare altre tipologie
    }

    Carichi {
		int Id PK "Identificativo univoco"
		string Descrizione varchar(150) %% in Risko arriva massimo a 135 caratteri
		date DataCreazione Date
		date DataArrivo Date
		date DataApprovazione Date
		bool HasRecuperoSpeseCarico Bit %%In Risko non viene quasi mai usato (dal 2015 solo 1 volta) TODO: Chiedere a Raffaele!!
		datetime data_modifica DateTime
    }

    CarichiDettaglio {
       int Id PK int
       int IdCarico FK int
       int IdTipoProvenienza FK int
       int IdSoggetto FK int
       int IdTipoEntrata FK int
       int IdTributo FK int %%TODO: chiedere a Raffaele: mettere a livello di carico o di dettaglio carico?
       int IdTipoDocumento FK int
       string NumeroDocumento varchar(50)
       date DataDocumento Date
       date DataScadenza Date
       date DataNotifica Date
       data DataPrescrizione Date %%TODO: E' calcolata/calcolabile? Serve?
       int IdAttoSuccessivo FK int %%TODO: Atto successivo (tabella Atti): serve qui? Come lo vogliamo gestire?
       int IdStato FK int
    }
    
    TipiProvenienza {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Manuale, 2 = Excel, 3 = Tracciato 290, 4 = Tracciato 600, 99 = TODO
    }

    TipiEntrata {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %1 = Tributaria, 2 = Patrimoniale
    }

    TipiTributo {
        int Id PK "Identificativo univoco"
        int IdTipoMacroTributo FK int
        string Tipo varchar(25) %1 = IMU, 2 = TARI, 3 = ICDS, 4 = TOSAP, 5 = COSAP, 6 = Lampade votive, 99 = TODO
    }

    TipiMacroTributo {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %1 = IUC, 2 = RSU, 3 = ICDS, 4 = Tributi minori, 6 = Idrico, 7 = Servizi scolastici, 8 = Servizi cimiteriali, 99 = TODO
    }

    TipiDocumento {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Avviso di accertamento, 2 = Cartella di pagamento, 3 = Avviso bonario, 4 = Ordinanza ingiunzione, 5 = Altri documenti, 99 = TODO
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
- Un **CLIENTE** può avere uno o più **INDIRIZZI** (relazione 1:N)
- Un **INDIRIZZO** appartiene sempre ad un solo **CLIENTE**
- Un **ENTE** può avere uno o più **INDIRIZZI** (relazione 1:N)
- Un **INDIRIZZO** appartiene sempre ad un solo **ENTE**
- Un **CLIENTE** può avere uno o più **CONTATTI** (relazione 1:N)
- Un **CONTATTO** appartiene sempre ad un solo **CLIENTE**
- Un **ENTE** può avere uno o più **CONTATTI** (relazione 1:N)
- Un **CONTATTO** appartiene sempre ad un solo **ENTE**

Queste informazioni non sono presenti su Zucchetti, andrebbero gestite sul ms-clienti
e rese disponibili per tutta la digital-platform

- gestione di una tabella di referenti dell'ente/cliente (riferimento di un ufficio)
- gestione delle sedi dell'ente/cliente

- anagrafica dei responsabili di contratto/commerciali (a livello di ente o cliente?)
serve per fare i SAL interni
oppure per gli operatori per capire a chi rivolgersi in caso di controversie

simone -> ci passa i dati

le abbiamo su Zucchetti
tabelle prodotti servizi
prodotto (tipo tributo IMU TARI ICDS)
servizi (RISCOSSIONE ACCERTAMENTO ORDINARIA)
string tipo_contratto "colonna Tipo Gestione (appalto/concessione)"
codice commessa zucchetti

attività (tabella dei centri di costo)

Informazioni bancarie (non nel dominio dei clienti)

commessa
prodotto
data validazione
stato (chiuso o sostituito)

è un'anagrafica a parte bisogna gestire anche la tipologia di pagamento
serve per rendicontazione, versamenti e bollettini

# approccio alla migrazione

1. chiamo zucchetti e restituisce tutta la lista/liste
2. data la insert sul data model nuovo devo ricavarmi la chiave per recuperare
    CodEnte risko
    CodCommessa risko

naming convention database -> simone