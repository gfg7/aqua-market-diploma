using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AquaServer.Extensions.ModelValidator
{
    public static class ModelValidator
    {
        public static void Validate(this object obj)
        {
            var context = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, context, result, true))
            {
                var error = result.First().ErrorMessage;
                throw new ValidationException(error);
            }
        }
    }
}
