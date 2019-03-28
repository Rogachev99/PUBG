﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
    public class RoyalRoomLogic : IRoomLogic
    {
        private IGameModel roomContext;
        private Timer timerNewIteration;
		private QuantTimer quantTimer;
		private const int minValueGamerInBattle = 0;
		public event RoomLogicEndWork EventRoomLogicEndWork;


		public RoyalRoomLogic(int GamersInRoom)
        {
			roomContext = new RoyalGameModel(GamersInRoom);
			timerNewIteration = new Timer(70)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerNewIteration.Elapsed += TickQuantTimer;
			quantTimer = new QuantTimer();
        }

		/// <summary>
		/// Создает список состяний игровых объектов (только активных)
		/// </summary>
		public IMessage RoomState
		{
			get
			{
				List<IMessage> states = new List<IMessage>();
				foreach (var gameObject in roomContext.GameObjects)
				{
					if (gameObject.Value.TypesBehave == TypesBehaveObjects.Active)
					{
						IMessage msg = gameObject.Value.State;
						if (msg != null) states.Add(msg);
					}
				}
				return new RoomState(states);
			}
		}

		//возврашает состояние всех объектов (активных и неактивных)
		public IMessage GetInitializeData()
		{
			List<IMessage> states = new List<IMessage>();
			foreach (var gameObject in roomContext.GameObjects)
			{		
					IMessage msg = gameObject.Value.State;
					if (msg != null) states.Add(msg);	
			}
			return new RoomState(states);
		}

		/// <summary>
		/// Посредник между коллецией игроков игровой модели и внешней средой
		/// </summary>
		public IList<IPlayer> Players {
			get
			{
				return roomContext.Players;
			}
		}
		/// <summary>
		/// Посредник между коллецией событий игровой модели и внешней средой
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents {
			get {
				return roomContext.HappenedEvents;
			}
		}

		public void AddPlayer()
        {
            throw new NotImplementedException();
        }

        //вызывается при срабатывании таймера
        private void TickQuantTimer(object sender, ElapsedEventArgs e)
        {
			quantTimer.Tick();
			roomContext.Field.Step((float)quantTimer.QuantValue/1000, 8, 3);

			TimeQuantPassed msg = new TimeQuantPassed(quantTimer.QuantValue);
			for (Body list = roomContext.Field.GetBodyList(); list != null; list = list.GetNext())
			{
				if (list.GetUserData() != null)
				{
					ISolidBody solidBody = (ISolidBody)list.GetUserData();
					if (!solidBody.Parent.Destroyed)
					{
						if (solidBody.Parent.TypesBehave == TypesBehaveObjects.Active)
						{
							//запускаем  обработку всех событий на этом объекте
							solidBody.Parent.Update(msg);
						}
					}
					else
					{				
						roomContext.GameObjects.Remove(solidBody.Parent.ID);
						if (solidBody.Parent is Gamer)
						{
							RemovePlayer((IPlayer)solidBody.Parent);
						}
					} 
				}
			}			
		}
		/// <summary>
		/// Проверяет на условия завершения игры и в случае,
		/// если игра завершена уведолмяет об этом подписчиков
		/// </summary>
		private void CheckEndGamer()
		{
			if (roomContext.Players.Count <= minValueGamerInBattle)
			{
				timerNewIteration.Stop();
				EventRoomLogicEndWork?.Invoke(this);
			}
		}

        public void RemovePlayer(IPlayer player)
        {
			if (roomContext.Players.Remove(player))
			{
				CheckEndGamer();
			}
		}

        public void Start()
        {
			timerNewIteration.Start();
        }

        public void Dispose()
        {
			timerNewIteration.Dispose();
			//осовобождение ресурсво модели
			roomContext.Dispose();
		}

	}
}
