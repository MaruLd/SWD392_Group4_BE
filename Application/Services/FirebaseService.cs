using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
	public class FirebaseService
	{
		private IConfiguration _config;

		public FirebaseService(IConfiguration config)
		{
			_config = config;
		}

		public FirebaseApp GetInstance()
		{
			return FirebaseApp.GetInstance("[DEFAULT]");
		}

		public async Task<FirebaseToken> VerifyIdToken(string token)
		{
			FirebaseAuth auth = FirebaseAuth.GetAuth(GetInstance());
			var result = await auth.VerifyIdTokenAsync(token);
			return result;
		}
	}
}