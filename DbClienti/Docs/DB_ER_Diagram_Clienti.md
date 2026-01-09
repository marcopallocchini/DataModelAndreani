# Database ER Diagram - MS Clienti

Questo documento rappresenta la struttura del database del microservizio MS Clienti attraverso un diagramma Entity-Relationship (ER).

## Struttura Database

Il database è composto da tre entità principali:
- **CLIENTI**: Tabella principale contenente i dati dei clienti
- **ENTI**: Tabella contenente gli enti associati ai clienti
- **COMMESSE**: Tabella contenente le commesse associate agli enti

## Diagramma ER

```mermaid
erDiagram
    CLIENTI ||--o{ ENTI : "ha"
    ENTI ||--o{ COMMESSE : "ha"
    
    Clienti {
        int id PK "Identificativo univoco"
        string codice_cliente UK "Codice cliente"
        string ragione_sociale "Ragione sociale"
        string partita_iva "Partita IVA"
        string codice_fiscale "Codice fiscale"
        string email "Email"
        string telefono "Telefono"
        string indirizzo "Indirizzo"
        string cap "CAP"
        string citta "Città" varchar(50)
        string provincia "Provincia" varchar(2)
        string Utente_modifica "Utente modifica" varchar(15)
        datetime data_modifica "Data modifica"
    }

    Enti {
        int id PK "Identificativo univoco"
        string codice_ente UK "Codice ente"
        string denominazione "Denominazione"
        string tipologia "Tipologia ente"
        string indirizzo "Indirizzo"
        string citta "Città"
        string provincia "Provincia"
        string cap "CAP"
        datetime data_creazione "Data creazione"
        datetime data_modifica "Data ultima modifica"
    }
    
    Commesse {
        int id PK "Identificativo univoco"
        string codice_commessa UK "Codice commessa (CIG)"
        string descrizione "Descrizione"
        string stato "Stato commessa"
        date data_inizio "Data inizio"
        date data_fine "Data fine prevista"
        datetime data_creazione "Data creazione"
        datetime data_modifica "Data ultima modifica"
    }
	
	AssociaClienteEnteCommessa {
        int IdCliente  FK PK "Riferimento al cliente"
        int IdEnte     FK PK "Riferimento all'ente"
        int IdCommessa FK PK "Riferimento alla commessa"
	}
	
	AssociaCommessaEntrata {
        int IdCommessa     FK PK "Riferimento alla commessa"
        int IdTipoEntrata  FK PK "Riferimento all'entrata"
        int idTipoAttivita FK PK "Riferimento al tipo attività"
	}

    %% TODO: parametri a livello servizio/prodotto divisi per anno di imposta - in orizzontale oppure categorizziamo i parametri e li mettiamo in verticale?
    %%       chiedere a Raffaele quali sono i parametri che servono!!!
	
	TipiStatoCommessa {
		int Id PK int
		string Descrizione varchar(15) //1 = aperta; 2 = chiusa; 3 = sospesa fino a rinnovo
	}
	
	Indirizzi {
		int Id PK int
		string Presso varchar(150)
		string Toponimo varchar(15)
		string DenominazioneStradale varchar(100)
		string Civico varchar(5)
		string Km varchar(10)
		string Esponente varchar(10)
		string Edificio varchar(10)
		string Scala varchar(5)
		string Piano varchar(5)
		string Interno varchar(5)
	}
	
	Contatti {
		int Id PK int
		int IdTipo FK TipiContatto int
		int IdCliente FK Clienti int
		int IdEnte FK Enti int
		string Contatto varchar(254)
		string Nota nvarchar(50)
	}
	
	TipiContatto {
		int Id PK int
		string Descrizione varchar(15) //1 = Telefono fisso; 2 = Telefono mobile; 3 = PEC; 4 = E-Mail; 5 = Fax; 6 = Sito web
	}

    TipiEntrata {
        int Id PK "Identificativo univoco"
        string CodiceAT varchar(10) "Codice tipologia entrata Andreani Tributi" %% Vedi valori tabella sotto (ex VEDI Gruppo = 'TIPO.TRIBUTO' su TABELLE)
        string CodiceAE varchar(3) "Codice tipologia entrata Agenzia Entrate"
        string Tipo varchar(25) %% Vedi valori tabella sotto (ex VEDI Gruppo = 'TIPO.TRIBUTO' su TABELLE)
        string DescrizioneAT varchar(150) "Descrizione estesa tipologia entrata Andreani Tributi" %% Vedi valori tabella sotto (ex VEDI Gruppo = 'TIPO.TRIBUTO' su TABELLE)
        string DescrizioneAE varchar(150) "Descrizione estesa tipologia entrata Agenzia Entrate"
        int IdTipoNaturaEntrata FK int
        int IdTipoMacroEntrata FK int
    }

    TipiNaturaEntrata {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Tributaria, 2 = Patrimoniale, 3 = Extra tributaria %% TODO: sentire Novella
    }

    TipiMacroEntrata {
        int id PK "Identificativo univoco"
        string Tipo varchar(25) %% 1 = Immobili, 2 = Rifiuti solidi urbani, 3 = Tributi minori, 4 = Infrazioni al codice della strada, 5 = Idrico, 6 = Servizi scolastici, 7 = Servizi cimiteriali, 8 = Imposta di soggiorno, 99 = Altre entrate
    }
    

```

## Relazioni

- Un **CLIENTE** può avere uno o più **ENTI** e un **ENTE** può appartenere a più **CLIENTI** (relazione N:N)
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

### Valori TipiEntrata

| CodiceAT      | Tipo                                      | DescrizioneAT                                              | IdTipoNaturaEntrata | IdTipoMacroEntrata |
|---------------|-------------------------------------------|------------------------------------------------------------|---------------------|--------------------|
| ACQUEDOTTO    | IDRICO / REFLUE / ACQUEDOTTO              | Canoni idrici/acque reflue/aquedotto                       | 2                   | 5                  |
| BDS_ALLOG     | BORSA DI STUDIO - Alloggi                 | Alloggi università                                         | 2                   | 6                  |
| BDS_MAGG      | BORSA DI STUDIO - Maggiorazione 689/81    | Maggiorazione 689/71 università                            | 2                   | 6                  |
| BDS_MENSA     | BORSA DI STUDIO - Mensa                   | Mensa università                                           | 2                   | 6                  |
| BDS_Q_CONT    | BORSA DI STUDIO - Quota contante          | Quota contante università                                  | 2                   | 6                  |
| BDS_SPESA     | BORSA DI STUDIO - Spesa atto              | Spesa atto università                                      | 2                   | 6                  |
| CIMP          | CIMP                                      | Canone iniziative pubblicitarie                            | 1                   | 3                  |
| CIP           | CIP                                       | Canone per le iniziative pubblicitarie                     | 2                   | 3                  |
| COATTIVO_P    | RECUPERO SPESE E DIRITTI PRECEDENTI PATR. | Recupero spese e diritti precedenti (P)                    | 2                   | 99                 |
| COATTIVO_T    | RECUPERO SPESE E DIRITTI PRECEDENTI TRIB. | Recupero spese e diritti precedenti (T)                    | 1                   | 99                 |
| CONTRIBUTO    | CONTRIBUTO                                | Contributo                                                 | 2                   | 99                 |
| COSAP         | COSAP                                     | Canone occupazione spazi ed aree pubbliche (P)             | 2                   | 3                  |
| COSAP_AE      | COSAP_AE                                  | Canone occupazione spazi ed aree pubbliche (T)             | 1                   | 3                  |
| CRE           | CENTRO RICREATIVO ESTIVO                  | Centro ricreativo estivo                                   | 2                   | 99                 |
| CUP           | CUP                                       | Canone unico patrimoniale (P)                              | 2                   | 3                  |
| CUP-T         | CUP-T                                     | Canone unico patrimoniale (T)                              | 1                   | 3                  |
| DPA           | DPA                                       | Diritti sulle pubbliche affissioni                         | 1                   | 3                  |
| ENT.PAT       | ENTRATE PATRIMONIALI                      | Entrate patrimoniali                                       | 2                   | 99                 |
| ENT.TRIB      | ENTRATE TRiBUTARIE                        | Entrate tributarie                                         | 1                   | 99                 |
| GAS           | GAS                                       | Gas metano                                                 | 2                   | 99                 |
| GPL           | GPL                                       | Gas propano liquido                                        | 2                   | 99                 |
| ICDS          | INFRAZIONI CDS                            | Infrazione al codice della strada                          | 2                   | 4                  |
| ICI           | ICI                                       | Imposta comunale sugli immobili                            | 1                   | 1                  |
| ICP           | ICP                                       | Imposta sulla pubblicità                                   | 1                   | 3                  |
| IMU           | IMU                                       | Imposta municipale unica                                   | 1                   | 1                  |
| LOCAZIONE     | CANONE DI LOCAZIONE                       | Canone di locazione                                        | 2                   | 99                 |
| MENSA         | MENSA                                     | Mensa                                                      | 2                   | 6                  |
| MUNIC_P       | RISCOSSIONE COATTIVA DI MUNICIPIA         | Riscossione coattiva di Municipia                          | 2                   | 99                 |
| MUNIC_T       | RISCOSSIONE COATTIVA DI MUNICIPIA         | Riscossione coattiva di Municipia                          | 1                   | 99                 |
| NIDO          | ASILO NIDO                                | Asilo nido                                                 | 2                   | 6                  |
| NIDO_ESTIV    | ASILO NIDO ESTIVO                         | Asilo nido estivo                                          | 2                   | 6                  |
| NIDO_PROL     | ASILO NIDO PROLUNGATO                     | Asilo nido prolungato                                      | 2                   | 6                  |
| ONER          | ONERI                                     | Oneri comunali vari                                        | 2                   | 99                 |
| ONER_URBA     | ONERI DI URBANIZZAZIONE                   | Oneri e contributi di costruzione                          | 2                   | 99                 |
| ORDC          | ORDINANZE COMUNALI                        | Ordinanze comunali                                         | 2                   | 99                 |
| RIF.IND       | RIFIUTI INDUSTRIALI                       | Rifiuti industriali                                        | 2                   | 2                  |
| SANZ.AMM      | SANZIONI AMMINISTRATIVE                   | Sanzioni amministrative                                    | 2                   | 99                 |
| SCIM          | SERVIZI CIMITERIALI                       | Servizi cimiteriali                                        | 2                   | 7                  |
| SCUOLA        | SERVIZI SCOLASTICI                        | Servizi scolastici                                         | 2                   | 6                  |
| SCUOLA.LAB    | LABORATORIO SCOLASTICO                    | Laboratorio scolastico                                     | 2                   | 6                  |
| SCUOLA_PP     | PRE-POST SCUOLA                           | Pre-post scuola                                            | 2                   | 6                  |
| SENTENZACC    | SENTENZA CORTE DEI CONTI                  | Sentenza corte dei conti                                   | 2                   | 99                 |
| SOGGIORNO     | IMPOSTA DI SOGGIORNO                      | Imposta di soggiorno                                       | 1                   | 8                  |
| TARES         | TARES                                     | Tributo comunale sui rifiuti e sui servizi                 | 1                   | 2                  |
| TARESG        | TARES GIORNALIERA                         | Tributo comunale sui rifiuti e sui servizi giornaliero     | 1                   | 2                  |
| TARI          | TARI                                      | Tassa rifiuti                                              | 1                   | 2                  |
| TARIP         | TARIP                                     | Tariffa rifiuti avente natura corrispettiva                | 2                   | 2                  |
| TARIP_AE      | TARIP_AE                                  | Tariffa rifiuti avente natura corrispettiva                | 1                   | 2                  |
| TARSU         | TARSU                                     | Tassa per lo smaltimento dei rifiuti solidi urbani         | 1                   | 2                  |
| TARSUG        | TARSUG                                    | Tarsu giornaliera                                          | 1                   | 2                  |
| TASI          | TASI                                      | Servizi indivisibili                                       | 1                   | 1                  |
| TEFA          | TEFA                                      | Tributo esercizio funzioni ambientali                      | 1                   | 2                  |
| TIA           | TIA                                       | Tariffa igiene ambientale                                  | 1                   | 2                  |
| TIA1          | TIA 1                                     | Tariffa igiene ambientale (TIA 1)                          | 1                   | 2                  |
| TIA2          | TIA 2                                     | Tariffa igiene ambientale (TIA 2)                          | 2                   | 2                  |
| TOSAP         | TOSAP                                     | Tassa occupazione spazi ed aree pubbliche                  | 1                   | 3                  |
| TOSAPG        | TOSAP GIORNALIERA                         | Tassa occupazione spazi ed aree pubbliche giornaliera      | 1                   | 3                  |
| TRASPORTO     | TRASPORTO                                 | Trasporto                                                  | 2                   | 6                  |
| UNIVERSITA    | SERVIZI UNIVERSITARI                      | Servizi universitari                                       | 2                   | 6                  |
| VOTIVE        | LUCI VOTIVE                               | Lampade votive                                             | 2                   | 7                  |

