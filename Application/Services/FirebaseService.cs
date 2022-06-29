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
		FirebaseApp _firebaseApp;
		FirebaseAuth _firebaseAuth;
		FirebaseMessaging _firebaseMessaging;

		public FirebaseService(IConfiguration config)
		{
			_config = config;
			_firebaseApp = FirebaseApp.GetInstance("[DEFAULT]");
			_firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
			_firebaseMessaging = FirebaseMessaging.GetMessaging(_firebaseApp);
		}

		public async Task<FirebaseToken> VerifyIdToken(string token)
		{
			var result = await _firebaseAuth.VerifyIdTokenAsync(token);
			return result;
		}

		public async Task<Boolean> VerifyFCMToken(string token)
		{
			Message msg = new Message()
			{
				Token = token,
				Data = new Dictionary<string, string>() { { "message", "Welcome To The System" } }
			};

			try
			{
				await _firebaseMessaging.SendAsync(msg);
				return true;
			}
			catch
			{
				return false;
			}
		}

		// public async Task<Boolean> SendMessageToDevice(string token)
		// {
		// 	Message msg = new Message()
		// 	{
		// 		Token = token,
		// 		Data = new Dictionary<string, string>() { { "message", "Welcome To The System" } }
		// 	};

		// 	try
		// 	{
		// 		await fm.SendAsync(msg);
		// 		return true;
		// 	}
		// 	catch
		// 	{
		// 		return false;
		// 	}
		// }

		public async Task<string> SendMessageToAll(String message)
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

			string response = await _firebaseMessaging.SendAsync(msg);
			return response;
		}

	}
}