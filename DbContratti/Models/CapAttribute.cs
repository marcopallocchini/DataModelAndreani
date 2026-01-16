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