using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TravelPlanner.BusinessLogic.Models;
using TravelPlanner.DomainModel;

namespace TravelPlanner.Identity.IdentityValidators
{
    public class ApplicationUserValidator<TUser> : UserValidator<TUser, Guid>
        where TUser : User
    {
        public bool RequireUniqueAlias { get; set; }

        public ApplicationUserValidator(UserManager<TUser, Guid> manager) : base(manager)
        {
            this.Manager = manager;
        }

        private UserManager<TUser, Guid> Manager { get; set; }

        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            await ValidateUserName(item, errors);
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (RequireUniqueAlias)
            {
                await ValidateAlias(item, errors);
            }
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private Task ValidateUserName(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(ValidationResultCodes.UserNameRequiredOrInvalid);
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
            {
                // If any characters are not letters or digits, its an illegal user name
                errors.Add(ValidationResultCodes.UserNameRequiredOrInvalid);
            }

            return Task.FromResult(0);
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add(ValidationResultCodes.EmailRequiredOrInvalid);
                return;
            }
            try
            {
                var m = new MailAddress(user.Email);
            }
            catch (FormatException)
            {
                errors.Add(ValidationResultCodes.EmailRequiredOrInvalid);
                return;
            }
            var owner = await Manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<Guid>.Default.Equals(owner.Id, user.Id))
            {
                errors.Add(ValidationResultCodes.DuplicateEmail);
            }
        }

        private Task ValidateAlias(TUser user, List<string> errors)
        {
            return Task.FromResult(0);
        }
    }
}