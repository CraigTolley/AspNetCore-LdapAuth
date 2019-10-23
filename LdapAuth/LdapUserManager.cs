using TestAuth2Mvc.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TestAuth2Mvc.Services;

namespace TestAuth2Mvc.Identity
{
    public class LdapUserManager : UserManager<LdapUser>
    {
        private readonly ILdapService _ldapService;

        public LdapUserManager(
            ILdapService ldapService,
            IUserStore<LdapUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<LdapUser> passwordHasher,
            IEnumerable<IUserValidator<LdapUser>> userValidators,
            IEnumerable<IPasswordValidator<LdapUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<LdapUserManager> logger)
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
            this._ldapService = ldapService;
        }

        public LdapUser GetAdministrator()
        {
            return this._ldapService.GetAdministrator();
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(LdapUser user, string password) {
            return  this._ldapService.Authenticate(user.DistinguishedName, password);
        }

        public override Task<IList<string>> GetRolesAsync(LdapUser user) {
            return Task.FromResult<IList<string>>(user.MemberOfNameOnly.ToList());
        }

        public override Task<IList<Claim>> GetClaimsAsync(LdapUser user) {
            var userClaims = new List<Claim>();

            // In testing, going much above 30 claims seems to cause the login to hang. 
            user.MemberOfNameOnly.ToList().Take(25).ToList().ForEach(g => userClaims.Add(new Claim("AdGroup",g)));
            return Task.FromResult<IList<Claim>>(userClaims.ToList());
        }

        public override Task<bool> HasPasswordAsync(LdapUser user) {
            return Task.FromResult(true);
        }

        public override Task<LdapUser> FindByIdAsync(string userId)
        {
            return this.FindByNameAsync(userId);
        }

        public override Task<LdapUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(this._ldapService.GetUserByUserName(userName));
        }
                
        public override Task<string> GetEmailAsync(LdapUser user)
        {
            return base.GetEmailAsync(user);
        }

        public override Task<string> GetUserIdAsync(LdapUser user)
        {
            return base.GetUserIdAsync(user);
        }

        public override Task<string> GetUserNameAsync(LdapUser user)
        {
            return base.GetUserNameAsync(user);
        }

        public override Task<string> GetPhoneNumberAsync(LdapUser user)
        {
            return base.GetPhoneNumberAsync(user);
        }

        public override IQueryable<LdapUser> Users => this._ldapService.GetAllUsers().AsQueryable();
    }
}
