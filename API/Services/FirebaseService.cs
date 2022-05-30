using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace API.Services
{
	public class FirebaseService
	{
		private IConfiguration _config;

		public FirebaseService(IConfiguration config)
		{
			_config = config;
		}

		public FirebaseApp GetDefaultApp() {
			return FirebaseApp.GetInstance("DEFAULT");
		}

		public async Task<FirebaseToken> VerifyIdToken(string token) {
			FirebaseAuth auth = FirebaseAuth.GetAuth(GetDefaultApp());
;			var result = await auth.VerifyIdTokenAsync(token);
			return result;
		}
	}
}