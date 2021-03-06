﻿using BattleRoyalServer;
using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest.Common
{
	delegate void DelegateForComponent();

	class MockComponent : IComponent
	{
		public IGameObject Parent { get; set; }
		public IMessage State { get; set; }

		public event DelegateForComponent HandlerSetup;
		public event DelegateForComponent HandlerDispose;

		public void Dispose()
		{
			HandlerDispose?.Invoke();
		}

		public void Setup()
		{
			HandlerSetup?.Invoke();
		}
	}
}
