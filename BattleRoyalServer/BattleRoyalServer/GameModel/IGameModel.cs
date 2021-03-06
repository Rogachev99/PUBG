﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using System.Drawing;
using CommonLibrary.GameMessages;
using CommonLibrary;
using ObservalableExtended;


namespace BattleRoyalServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
		IGameObject DeathZone { get; }
		IMessage RoomState { get; }
		IMessage FullRoomState { get; }

		event HappenedEndGame Event_HappenedEndGame;

		void AddOrUpdateGameObject(IGameObject gameObject);
		void RemoveGameObject(IGameObject gameObject);
		void Dispose();
		void MakeStep(int passedTime);
		IMessage GetOutgoingMessage();
		void AddIncomingMessage(IMessage message);
	}
}