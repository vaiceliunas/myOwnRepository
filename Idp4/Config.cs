using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Idp4
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Arnas",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim ("given_name", "Arnelis"),
                        new Claim("nickname", "Arnello"),
                        new Claim ("family_name", "Vaic"),
                        new Claim ("address", "Vilnius Lietuva"),
                        new Claim ("role", "Admin"),
                        new Claim ("subscriptionLevel", "Admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-84da-791ba61b07c7",
                    Username = "User2",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim ("given_name", "User2"),
                        new Claim ("family_name", "User2FamilyName"),
                        new Claim ("address", "Kaunas Lietuva"),
                        new Claim ("role", "Free"),
                        new Claim ("subscriptionLevel", "Free")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" }),
                new IdentityResource(
                    "subscriptionLevel",
                    "your subscription level",
                    new List<string>(){ "subscriptionLevel"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "BasicPwa",
                    ClientId = "BasicPwa",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenLifetime = 5000,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret
                        {
                            Value = "plain_text".Sha256()
                        }
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44300/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "myApi",
                        "subscriptionLevel",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44300/signout-callback-oidc"
                    },
                }
            };
        }
    }
}
