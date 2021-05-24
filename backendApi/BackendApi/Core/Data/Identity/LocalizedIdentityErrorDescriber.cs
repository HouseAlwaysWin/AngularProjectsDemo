using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BackendApi.Core.Data.Identity
{
    public class LocalizedIdentityErrorDescriber:IdentityErrorDescriber
    {
        private readonly IStringLocalizer _localizer;
        public LocalizedIdentityErrorDescriber(IStringLocalizer localizer)
        {
            this._localizer = localizer;
        }

        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a concurrency
        //     failure.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a concurrency failure.
        public override IdentityError ConcurrencyFailure(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns the default Microsoft.AspNetCore.Identity.IdentityError.
        //
        // 傳回:
        //     The default Microsoft.AspNetCore.Identity.IdentityError.
        public override IdentityError DefaultError(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     email is already associated with an account.
        //
        // 參數:
        //   email:
        //     The email that is already associated with an account.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specified email
        //     is already associated with an account.
        public override IdentityError DuplicateEmail(string email){ 
            return new IdentityError{
                Code = nameof(DuplicateEmail),
                Description = _localizer["DupllicateEmail",email]
         };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     role name already exists.
        //
        // 參數:
        //   role:
        //     The duplicate role.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specific role role
        //     name already exists.
        public override IdentityError DuplicateRoleName(string role){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     userName already exists.
        //
        // 參數:
        //   userName:
        //     The user name that already exists.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specified userName
        //     already exists.
        public override IdentityError DuplicateUserName(string userName){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     email is invalid.
        //
        // 參數:
        //   email:
        //     The email that is invalid.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specified email
        //     is invalid.
        public override IdentityError InvalidEmail(string email){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     role name is invalid.
        //
        // 參數:
        //   role:
        //     The invalid role.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specific role role
        //     name is invalid.
        public override IdentityError InvalidRoleName(string role){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating an invalid
        //     token.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating an invalid token.
        public override IdentityError InvalidToken(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating the specified
        //     user userName is invalid.
        //
        // 參數:
        //   userName:
        //     The user name that is invalid.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating the specified user
        //     userName is invalid.
        public override IdentityError InvalidUserName(string userName){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating an external
        //     login is already associated with an account.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating an external login is
        //     already associated with an account.
        public override IdentityError LoginAlreadyAssociated(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     mismatch.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password mismatch.
        public override IdentityError PasswordMismatch(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     entered does not contain a numeric character, which is required by the password
        //     policy.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password entered
        //     does not contain a numeric character.
        public override IdentityError PasswordRequiresDigit(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     entered does not contain a lower case letter, which is required by the password
        //     policy.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password entered
        //     does not contain a lower case letter.
        public override IdentityError PasswordRequiresLower(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     entered does not contain a non-alphanumeric character, which is required by the
        //     password policy.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password entered
        //     does not contain a non-alphanumeric character.
        public override IdentityError PasswordRequiresNonAlphanumeric(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     does not meet the minimum number uniqueChars of unique chars.
        //
        // 參數:
        //   uniqueChars:
        //     The number of different chars that must be used.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password does not
        //     meet the minimum number uniqueChars of unique chars.
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     entered does not contain an upper case letter, which is required by the password
        //     policy.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password entered
        //     does not contain an upper case letter.
        public override IdentityError PasswordRequiresUpper(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a password
        //     of the specified length does not meet the minimum length requirements.
        //
        // 參數:
        //   length:
        //     The length that is not long enough.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a password of the specified
        //     length does not meet the minimum length requirements.
        public override IdentityError PasswordTooShort(int length){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a recovery
        //     code was not redeemed.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a recovery code was
        //     not redeemed.
        public override IdentityError RecoveryCodeRedemptionFailed(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a user already
        //     has a password.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a user already has
        //     a password.
        public override IdentityError UserAlreadyHasPassword(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a user is already
        //     in the specified role.
        //
        // 參數:
        //   role:
        //     The duplicate role.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a user is already in
        //     the specified role.
        public override IdentityError UserAlreadyInRole(string role){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating user lockout
        //     is not enabled.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating user lockout is not
        //     enabled.
        public override IdentityError UserLockoutNotEnabled(){ return new IdentityError{ };}
        //
        // 摘要:
        //     Returns an Microsoft.AspNetCore.Identity.IdentityError indicating a user is not
        //     in the specified role.
        //
        // 參數:
        //   role:
        //     The duplicate role.
        //
        // 傳回:
        //     An Microsoft.AspNetCore.Identity.IdentityError indicating a user is not in the
        //     specified role.
        public override IdentityError UserNotInRole(string role){ return new IdentityError{ };} 
    }
}