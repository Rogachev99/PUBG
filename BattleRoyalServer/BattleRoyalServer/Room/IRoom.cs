﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
    public interface IRoom
    {
        INetwork NetworkLogic { get; }
        IRoomLogic GameLogic { get; }

        /// <summary>
        /// Запускает Start у полей комнаты
        /// </summary>
        void StartRoom();
        /// <summary>
        /// Освобождает все ресурыс комнаты
        /// </summary>
        void Dispose();

		event RoomEndWork EventRoomEndWork;

	}
}
