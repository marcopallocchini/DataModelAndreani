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