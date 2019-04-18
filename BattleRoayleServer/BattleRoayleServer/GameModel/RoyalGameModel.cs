﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Specialized;
using System.Drawing;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
    public class RoyalGameModel : IGameModel, IModelForComponents
	{

		/// <summary>
		///ширина одной стороны игровой карты 
		/// </summary>
		private const float lengthOfSide = 500;
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
		/// <summary>
		/// Коллекция всех игровых объектов в игре
		/// </summary>
        public Dictionary<ulong,IGameObject> GameObjects { get; private set; }
		public DeathZone Zone { get; private set; }
        public World Field { get; private set; }
		/// <summary>
		/// Колекция событий произошедших в игре
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents { get; private set; }

		public GameObjectState State
		{
			get
			{
				var listState = new List<IMessage>();
				//отправляем характеристики карты
				listState.Add(new FieldState(new SizeF(lengthOfSide, lengthOfSide)));

				return new GameObjectState(0, TypesGameObject.Field, listState);
			}
		}

		/// <summary>
		/// Содержит алгоритм наполнения карты игровыми объектами
		/// </summary>
		/// <remarks>
		/// По умолчанию создает список объектов.
		/// Должен создавать объекты, добавлять их в список и на карту.
		/// </remarks>
		private void CreateStaticGameObject()
		{
			//скрипт создания игровых объектов
			Stone stone = new Stone(this, new PointF(10, 10), new Size(14,14));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(30, 28), new Size(12, 12));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(78, 30), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);

			Box box = new Box(this, new PointF(40, 100), new Size(12, 12));
			GameObjects.Add(box.ID, box);
			
			box = new Box(this, new PointF(150, 10), new Size(12, 12));
			GameObjects.Add(box.ID, box);

			Gun gun = new Gun(this, new PointF(50, 70));
			GameObjects.Add(gun.ID, gun);

			AssaultRifle assaultRifle = new AssaultRifle(this, new PointF(100,80));
			GameObjects.Add(assaultRifle.ID, assaultRifle);

			ShotGun shotGun = new ShotGun(this, new PointF(200, 50));
			GameObjects.Add(shotGun.ID, shotGun);

			GrenadeCollection grenade = new GrenadeCollection(this, new PointF(40, 200));
			GameObjects.Add(grenade.ID, grenade);

			Bush bush = new Bush(this, new PointF(45, 90));
			GameObjects.Add(bush.ID, bush);

			Tree tree = new Tree(this, new PointF(100, 250));
			GameObjects.Add(tree.ID, tree);

			DeathZone deathZone = new DeathZone(this, lengthOfSide);
			GameObjects.Add(deathZone.ID, deathZone);
			Zone = deathZone;
		}

		private void Model_EventGameObjectDeleted(IGameObject gameObject)
		{
			GameObjects.Remove(gameObject.ID);
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				Gamer gamer = new Gamer(this, CreatePlayerLocation(i));
				Players.Add(gamer);
				GameObjects.Add(gamer.ID,gamer);
			}
		}

		public void RemovePlayer(IPlayer player)
		{
			Players.Remove(player);
		}
		
		private PointF CreatePlayerLocation(int index)
		{
			switch (index)
			{
				case 0:
					return new PointF(50, 70);
				case 1:
					return new PointF(50, 85);
				default:
					return new PointF(0, 0);
			}
		}

		public RoyalGameModel(int gamersInRoom)
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new Dictionary<ulong, IGameObject>();
			HappenedEvents = new ObservableQueue<IMessage>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0,0);
			frameField.UpperBound.Set(lengthOfSide, lengthOfSide);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();


			//создание и добавление в GameObjects и Field статических объектов карты
			CreateStaticGameObject();
			CreatePlayers(gamersInRoom);

			//настраиваем игровые объекты
			foreach (var key in GameObjects.Keys)
			{
				GameObjects[key].Setup();
			}

		}

		//только для тестов
		public RoyalGameModel()
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new Dictionary<ulong, IGameObject>();
			HappenedEvents = new ObservableQueue<IMessage>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0, 0);
			frameField.UpperBound.Set(lengthOfSide, lengthOfSide);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();
		}

		private void CreateFrame()
		{
			//bottom
			BodyDef bDefBottom = new BodyDef();
			bDefBottom.Position.Set(0, 0);
			bDefBottom.Angle = 0;

			PolygonDef pDefBottom = new PolygonDef();
			pDefBottom.Restitution = 0f;
			pDefBottom.Friction = 0;
			pDefBottom.Density = 0;
			pDefBottom.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefBottom.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefBottom.SetAsBox(lengthOfSide, 1);

			var frame  = Field.CreateBody(bDefBottom);
			frame.CreateShape(pDefBottom);

			//left
			BodyDef bDefLeft = new BodyDef();
			bDefLeft.Position.Set(0, 0);
			bDefLeft.Angle = 0;

			PolygonDef pDefLeft = new PolygonDef();
			pDefLeft.Restitution = 0;
			pDefLeft.Friction = 0;
			pDefLeft.Density = 0;
			pDefLeft.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefLeft.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefLeft.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefLeft);
			frame.CreateShape(pDefLeft);

			//top
			BodyDef bDefTop = new BodyDef();
			bDefTop.Position.Set(0, lengthOfSide - 1);
			bDefTop.Angle = 0;

			PolygonDef pDefTop = new PolygonDef();
			pDefTop.Restitution = 0;
			pDefTop.Friction = 0;
			pDefTop.Density = 0;
			pDefTop.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefTop.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefTop.SetAsBox(lengthOfSide, 1);

			frame = Field.CreateBody(bDefTop);
			frame.CreateShape(pDefTop);

			//right
			BodyDef bDefRight = new BodyDef();
			bDefRight.Position.Set(lengthOfSide - 1, 0);
			bDefRight.Angle = 0;

			PolygonDef pDefRight = new PolygonDef();
			pDefRight.Restitution = 0;
			pDefRight.Friction = 0;
			pDefRight.Density = 0;
			pDefRight.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefRight.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefRight.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefRight);
			frame.CreateShape(pDefRight);
		}

		public void AddOrUpdateGameObject(IGameObject gameObject)
		{
			if (!GameObjects.ContainsKey(gameObject.ID))
			{
				GameObjects.Add(gameObject.ID, gameObject);
			}
			
			gameObject.Setup();
			//посылваем сообщение об добавлении нового объекта
			//отправляем просто состояние объекта(не вижу смысла создавать специальное сообщение для этого)
			HappenedEvents.Enqueue(gameObject.State);
		}

		public void RemoveGameObject(IGameObject gameObject)
		{
			gameObject.Dispose();
			GameObjects.Remove(gameObject.ID);
		}

		public void Dispose()
		{
			//выполняем все необходимые действия при уничтожении для всех оставшихся игроков
			for(; Players.Count!=0;)
			{
				(Players[0] as GameObject).Dispose();
				Players.RemoveAt(0);
			}

			Players.Clear();
			GameObjects.Clear();
			Field.Dispose();
			HappenedEvents.Clear();
		}

		public void AddEvent(IMessage message)
		{
			HappenedEvents.Enqueue(message);
		}

	}
}