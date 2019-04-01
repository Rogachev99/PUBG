﻿using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface IComponent
	{
		IGameObject Parent { get; }
		IMessage State { get; }

		void Dispose();
		void Setup();
		void UpdateComponent(IMessage msg);
	}
}