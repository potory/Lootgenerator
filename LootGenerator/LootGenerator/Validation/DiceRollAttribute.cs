using System.ComponentModel.DataAnnotations;
using LootGenerator.Utilities;

namespace LootGenerator.Validation;

public class DiceRollAttribute : ValidationAttribute
{
    private readonly DiceUtility _util;

    public DiceRollAttribute()
    {
        _util = new DiceUtility();
    }
    public string GetErrorMessage(string str)
    {
        return string.IsNullOrEmpty(ErrorMessage) ? $"Incorrect roll formula: {str}" : ErrorMessage;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var str = (string) value;

        return !_util.IsCorrectDiceString(str) ? new ValidationResult(GetErrorMessage(str)) : ValidationResult.Success;
    }
}