using System;
using System.ComponentModel.DataAnnotations;

namespace DbContratti.Models
{
    public class PartitaIvaAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var partitaIva = value as string;
            if (string.IsNullOrEmpty(partitaIva) || partitaIva.Length != 11 || !long.TryParse(partitaIva, out _))
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
