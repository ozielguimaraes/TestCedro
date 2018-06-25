using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TestCedro.Service.WebAPI.Helpers
{
    public class EntityState
    {
        public static string GetErrors(ModelStateDictionary modelState)
        {
            var errors = new StringBuilder();
            foreach (var error in modelState.Values.SelectMany(x => x.Errors))
            {
                errors.AppendLine(error.ErrorMessage);
            }
            return errors.ToString();
        }
    }
}