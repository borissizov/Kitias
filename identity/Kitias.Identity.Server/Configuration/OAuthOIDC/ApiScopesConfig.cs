﻿using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Kitias.Identity.Server.OAuthOIDC.Configuration
{
	public static partial class Config
	{
		public static IEnumerable<ApiScope> GetApiScopes => TakeElements
		(
			new ApiScope
			{
				Name = DATA_ACCESS_SCOPE,
				Description = "Get access to data from Kitias",
				DisplayName = "Kitias Web Api Data",
				UserClaims = TakeElements(
					JwtClaimTypes.Email,
					JwtClaimTypes.Role,
					JwtClaimTypes.Name
				)
			});
	}
}
