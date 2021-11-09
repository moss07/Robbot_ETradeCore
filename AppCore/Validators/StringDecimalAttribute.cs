using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AppCore.Validators
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple =false,Inherited =true)]
    public class StringDecimalAttribute : ValidationAttribute
    {
        public StringDecimalAttribute() : base()
        {

        }
        public StringDecimalAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            bool validationResult;
            if (value == null)
                validationResult = true;
            else
            {
                double result;
                string valueText = value.ToString().Trim().Replace(",", ".");
                validationResult= double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
            return validationResult;
        }
    }
}
