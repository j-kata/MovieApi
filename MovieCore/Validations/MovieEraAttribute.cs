using System.ComponentModel.DataAnnotations;

namespace MovieCore.Validations
{
    public class MovieEraAttribute(int startYear = 1888, int maxYearsAhead = 0) : ValidationAttribute
    {
        private readonly int _eraStartYear = startYear;
        private readonly int _maxYearsAhead = maxYearsAhead;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not int year)
                return new ValidationResult("Year must be a number");

            int eraEndYear = DateTime.Now.Year + _maxYearsAhead;

            if (year < _eraStartYear || year > eraEndYear)
                return new ValidationResult($"Year must be between {_eraStartYear} and {eraEndYear}");

            return ValidationResult.Success;
        }
    }
}