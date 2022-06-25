using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
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

		public async Task<string> SendMessage(String message)
		{
			// var registrationToken = "AIzaSyDFNS-Nr2NSI2JrPM-pSgNBkr-BOj5q7WA";
			var msg = new Message()
			{
				Data = new Dictionary<string, string>()
				{
					{ "message", message }
				},
				Topic = "all",
				Notification = new Notification()
				{
					Title = "Hailo",
					Body = message
				}
				// Token = registrationToken,
			};

			string response = await FirebaseMessaging.DefaultInstance.SendAsync(msg);
			return response;
		}

	}
}