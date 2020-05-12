// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Ijp.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "subscriptionlevel",
                    "your subscription level",
                    new List<string>(){ "subscriptionlevel" }
                    )
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource(
                    "imagegalleryapi",
                    "Image Gallery API",
                    new List<string>() { "role" })
                {
                    ApiSecrets = { new Secret("apisecret".Sha256()) }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName="Image Gallery",
                    ClientId="imagegalleryclient",
                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce = true,
                    RedirectUris= new List<string>()
                    {
                        "https://localhost:44330/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44330/signout-callback-oidc"
                    },
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "imagegalleryapi",
                        "subscriptionlevel"
                    },
                    ClientSecrets=
                    {
                        new Secret("miosecret".Sha256())
                    }
                }
            };

    }
}