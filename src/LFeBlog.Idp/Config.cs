// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;

namespace LFeBlog.Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(), 
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("restapi", "My RESTful API",new List<string>
                {
                    "name",
                    "gender",
                    JwtClaimTypes.PreferredUserName,
                    JwtClaimTypes.Picture
                })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "mvcClient",
                    ClientName = "MVX 客户端",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    RedirectUris = {"https://localhost:7001/singin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:7001/singout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:7001/singout-callback-oidc"},
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "restapi"
                    }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "blog-Client",
                    ClientName = "Blog Client",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 180,
                    
                    RedirectUris =
                    {
                        "http://localhost:4200/signin-oidc",
                        "http://localhost:4200/redirect-silentrenew"
                    },
                   
                    
                    PostLogoutRedirectUris = {  "http://localhost:4200/" },

                    AllowedCorsOrigins = { "http://localhost:4200" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "restapi" 
                    }
                },

                // SPA client using implicit flow
               
            };
        }
    }
}