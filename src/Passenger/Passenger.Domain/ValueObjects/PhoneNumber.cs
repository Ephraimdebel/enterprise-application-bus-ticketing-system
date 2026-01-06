using System.Text.RegularExpressions;
using Passenger.Domain.Exceptions;

namespace Passenger.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex CountryCodeRegex = new(@"^\+[1-9]\d{0,2}$", RegexOptions.Compiled);
    private static readonly Regex NumberRegex = new(@"^\d{6,14}$", RegexOptions.Compiled);

    public string CountryCode { get; }
    public string Number { get; }

    private PhoneNumber(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    public static PhoneNumber Create(string countryCode, string number)
    {
        countryCode = (countryCode ?? string.Empty).Trim();
        number = (number ?? string.Empty).Trim();

        if (!CountryCodeRegex.IsMatch(countryCode))
            throw new ValidationException("Country code is invalid. Example: +251");

        // store digits only for number
        number = Regex.Replace(number, @"\D", "");
        if (!NumberRegex.IsMatch(number))
            throw new ValidationException("Phone number is invalid.");

        return new PhoneNumber(countryCode, number);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return Number;
    }

    public override string ToString() => $"{CountryCode}{Number}";
}
