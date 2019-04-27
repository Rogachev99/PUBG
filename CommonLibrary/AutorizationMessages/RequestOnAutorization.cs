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
	public class RequestOnAutorization: IMessage
	{
		public string Login { get; private set; }
		public string Password { get; private set; }

		public long Kills => throw new NotImplementedException();

		public long Deaths => throw new NotImplementedException();

		public long Battles => throw new NotImplementedException();

		public TimeSpan Time => throw new NotImplementedException();

		public bool Result => throw new NotImplementedException();

		public Direction Direction => throw new NotImplementedException();

		public PointF Location => throw new NotImplementedException();

		public float Angle => throw new NotImplementedException();

		public int Count => throw new NotImplementedException();

		public TypesWeapon TypeWeapon => throw new NotImplementedException();

		public float HP => throw new NotImplementedException();

		public float Distance => throw new NotImplementedException();

		public bool StartOrEnd => throw new NotImplementedException();

		public int TimePassed => throw new NotImplementedException();

		public float Damage => throw new NotImplementedException();

		public RectangleF Shape => throw new NotImplementedException();

		public float Radius => throw new NotImplementedException();

		public SizeF Size => throw new NotImplementedException();

		public TypesGameObject TypeGameObject => throw new NotImplementedException();

		public List<IMessage> InsertCollections => throw new NotImplementedException();

		public TypesMessage TypeMessage { get; } = TypesMessage.RequestOnAutorization;

		public ulong ID => throw new NotImplementedException();

		public RequestOnAutorization(string login, string password)
		{
			Login = login;
			Password = password;
		}
	}
}