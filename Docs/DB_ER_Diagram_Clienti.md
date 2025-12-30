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
	
	AssociaClienteEnteCommessa
	{
        int cliente_id  FK PK "Riferimento al cliente"
        int ente_id     FK PK "Riferimento all'ente"
        int commessa_id FK PK "Riferimento alla commessa"
	}
	
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