using System.ComponentModel.DataAnnotations;

namespace Todoweb.Model.Helpers
{
    public class ValidationHelper
    {
        public static List<ValidationResult> Validate<T>(T obj)
        {
            var context = new ValidationContext(obj!);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj!, context, results, validateAllProperties: true);
            return results;
        }
    }
}
