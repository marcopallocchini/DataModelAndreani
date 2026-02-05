# Definizione degli attributi di validazione dei campi

## Oggetto Contatto
- Le eMail e le PEC devono rispettare il formato standard di un indirizzo eMail.

### Campo Valore
```csharp
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      // 3 = PEC, 4 = E-Mail
      if (IdTipoContatto == 3 || IdTipoContatto == 4)
      {
        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(CONTT_Contatto))
        {
          yield return new ValidationResult(
            "Il formato dell'indirizzo email/PEC non è valido.",
            new[] { nameof(CONTT_Contatto) }
          );
        }
      }
    }
```

## Oggetti Contraente/Soggetto
- Il Codice Fiscale deve rispettare il formato standard di un codice fiscale italiano.
- La Partita IVA deve rispettare il formato standard di una partita IVA italiana.

### Campo CodiceFiscale (CodiceFiscaleAttribute.cs)
```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DbContratti.Models
{
    public class CodiceFiscaleAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var cf = value as string;
            if (cf is null)
                return true; // Campo opzionale: valido se null

            if (cf.Length == 0)
                return false;

            // Codice fiscale numerico (11 cifre, inizia con 8 o 9)
            if (cf.Length == 11 && long.TryParse(cf, out _) && (cf[0] == '8' || cf[0] == '9'))
                return true;

            // Codice fiscale alfanumerico standard (16 caratteri)
            if (cf.Length != 16 || !Regex.IsMatch(cf, @"^[A-Z0-9]{16}$"))
                return false;

            return CheckControlChar(cf);
        }

        private static bool CheckControlChar(string cf)
        {
            // Tabelle per il calcolo del carattere di controllo
            int[] pari = {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                0, 1, 2, 3, 4, 5
            };
            int[] dispari = {
                1, 0, 5, 7, 9, 13, 15, 17, 19, 21,
                1, 0, 5, 7, 9, 13, 15, 17, 19, 21,
                1, 0, 5, 7, 9, 13
            };
            string omocodia = "LMNPQRSTUV";
            int sum = 0;

            for (int i = 0; i < 15; i++)
            {
                char c = cf[i];
                int n;
                if (char.IsDigit(c))
                {
                    n = c - '0';
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // Gestione omocodia: lettere che sostituiscono numeri
                    int idx = omocodia.IndexOf(c);
                    n = idx >= 0 ? idx : c - 'A';
                }
                else
                {
                    return false;
                }

                if ((i + 1) % 2 == 0) // posizione pari (1-based)
                    sum += pari[n];
                else
                    sum += dispari[n];
            }

            int resto = sum % 26;
            char expected = (char)('A' + resto);
            return cf[15] == expected;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Codice fiscale non valido secondo le regole italiane.";
        }
    }
}
```

### Campo PartitaIva (PartitaIvaAttribute.cs)
```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace DbContratti.Models
{
    public class PartitaIvaAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var partitaIva = value as string;
            if (partitaIva is null)
                return true; // Campo opzionale: valido se null

            if (partitaIva.Length == 0 || partitaIva.Length != 11 || !long.TryParse(partitaIva, out _))
                return false;

            // Se inizia con 8 o 9: codice fiscale numerico, valido senza ulteriori controlli
            if (partitaIva[0] == '8' || partitaIva[0] == '9')
                return true;

            // Cifre 8-10 (index 7-9) devono essere tra 001 e 100
            int codiceUfficio = int.Parse(partitaIva.Substring(7, 3));
            if (codiceUfficio < 1 || codiceUfficio > 100)
                return false;

            // Cifra di controllo (algoritmo ufficiale)
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                int n = partitaIva[i] - '0';
                if ((i % 2) == 0)
                {
                    sum += n;
                }
                else
                {
                    int doubled = n * 2;
                    sum += doubled > 9 ? doubled - 9 : doubled;
                }
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (partitaIva[10] - '0');
        }

        public override string FormatErrorMessage(string name)
        {
            return "Partita IVA o codice fiscale numerico non valido secondo le regole italiane.";
        }
    }
}
```
### Oggetto Indirizzo
```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PostalCodeAttribute : ValidationAttribute
{
    private readonly string _countryPropertyName;

    // Regex
    private static readonly Regex ItRegex =
        new Regex(@"^\d{5}$", RegexOptions.Compiled);

    private static readonly Regex GlobalRegex =
        new Regex(@"^(?=.{3,12}$)[\p{L}0-9]+(?:[ -]?[\p{L}0-9]+)*$",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public PostalCodeAttribute(string countryPropertyName)
    {
        _countryPropertyName = countryPropertyName;
        ErrorMessage = "Codice postale non valido.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // CAP opzionale → valida solo se valorizzato
        if (value == null)
            return ValidationResult.Success;

        var postalCode = value.ToString()?.Trim();
        if (string.IsNullOrEmpty(postalCode))
            return ValidationResult.Success;

        // Recupero proprietà Nazione
        PropertyInfo countryProp =
            validationContext?.ObjectType.GetProperty(_countryPropertyName);

        if (countryProp == null)
            return new ValidationResult($"Proprietà '{_countryPropertyName}' non trovata.");

        var countryValue = countryProp.GetValue(validationContext.ObjectInstance)?.ToString();

        if (!string.IsNullOrEmpty(countryValue) &&
            (string.Equals(countryValue, "IT", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(countryValue, "ITALIA", StringComparison.OrdinalIgnoreCase)))
        {
            if (!ItRegex.IsMatch(postalCode))
                return new ValidationResult("Il CAP italiano deve essere composto da 5 cifre.");
        }
        else
        {
            if (!GlobalRegex.IsMatch(postalCode))
                return new ValidationResult("Codice postale non valido per la nazione selezionata.");
        }

        return ValidationResult.Success;
    }
}
```

#### Esempio di utilizzo:
```csharp
public class Indirizzo
{
    public string Nazione { get; set; }

    [PostalCode(Nazione)]
    public string CAP { get; set; }
}
```