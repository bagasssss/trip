using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TravelPlanner.BusinessLogic.Models;

namespace TravelPlanner.Identity.IdentityValidators
{
    /// <summary>
    ///     Used to validate some basic password policy like length and number of non alphanumerics
    /// </summary>
    public class ApplicationPasswordValidator : PasswordValidator
    {
        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
            {
                errors.Add(ValidationResultCodes.PasswordShort);
            }
            if (RequireNonLetterOrDigit && item.All(IsLetterOrDigit))
            {
                errors.Add(ValidationResultCodes.PasswordInvalidFormat);
            }
            if (RequireDigit && item.All(c => !IsDigit(c)))
            {
                errors.Add(ValidationResultCodes.PasswordInvalidFormat);
            }
            if (RequireLowercase && item.All(c => !IsLower(c)))
            {
                errors.Add(ValidationResultCodes.PasswordInvalidFormat);
            }
            if (RequireUppercase && item.All(c => !IsUpper(c)))
            {
                errors.Add(ValidationResultCodes.PasswordInvalidFormat);
            }
            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(String.Join(" ", errors)));
        }
    }
}