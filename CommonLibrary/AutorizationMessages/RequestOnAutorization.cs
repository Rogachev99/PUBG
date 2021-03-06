﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.AutorizationMessages
{
	[Serializable]
	public class RequestOnAutorization: Message
	{
		public override string Login { get; }
		public override string Password { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.RequestOnAutorization;

		public RequestOnAutorization(string login, string password)
		{
			Login = login;
			Password = password;
		}
	}
}
