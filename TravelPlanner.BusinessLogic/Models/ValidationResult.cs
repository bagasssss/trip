using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace TravelPlanner.BusinessLogic.Models
{
    public class ValidationResult: IdentityResult
    {
        public ValidationResult()
        {
        }

        public ValidationResult(bool success) : base(success)
        {
        }

        public ValidationResult(string error) : base(new [] { error })
        {
        }

        public ValidationResult(IEnumerable<string> errors) : base(errors)
        {
        }

        public new static ValidationResult Success => new ValidationResult(true);

        public static ValidationResult Failed(string error)
        {
            return new ValidationResult(error);
        }
    }
}
