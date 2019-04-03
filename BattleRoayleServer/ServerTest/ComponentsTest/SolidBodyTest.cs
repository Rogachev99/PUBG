﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;
using Box2DX.Common;
using CSInteraction.ProgramMessage;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class SolidBodyTest
	{
		[TestMethod]
		public void Test_CreateSolidBody1()
		{
			ISolidBody solidBody = new SolidBody(new StubPlayer(), new RectangleF(60, 70, 10, 10), 0, 0, 0.5f,
				TypesBody.Circle, 0, 0);
			Assert.IsNotNull(solidBody.Body);
		}

		[TestMethod]
		public void Test_CreateSolidBody2()
		{
			ISolidBody solidBody = new SolidBody(new StubPlayer(), new RectangleF(60, 70, 10, 10), 0, 0, 0.5f,
				TypesBody.Rectangle, 0, 0);
			Assert.IsNotNull(solidBody.Body);
		}

		[TestMethod]
		public void Test_SolidBodyState()
		{
			ISolidBody solidBody = new SolidBody(new StubPlayer(), new RectangleF(60, 70, 10, 10), 0, 0, 0.5f,
				TypesBody.Rectangle, 0, 0);
			Assert.IsNotNull(solidBody.State);
		}

		
		[TestMethod]
		public void Test_BodyDelete()
		{
			var player = new StubPlayer();
			int count = player.Model.Field.GetBodyCount();
			ISolidBody solidBody = new SolidBody(player, new RectangleF(60, 70, 10, 10), 0, 0, 0.5f,
				TypesBody.Circle, 0, 0);
			player.Components.Add(solidBody);
			player.Setup();
			Assert.AreEqual(solidBody.Body.GetWorld().GetBodyCount(), count+1);
			solidBody.BodyDelete();
			Assert.AreEqual(solidBody.Body.GetWorld().GetBodyCount(), count);

		}

		[TestMethod]
		public void Test_UpdateComponent_TimeQuantPassed()
		{
			var Room = new RoyalGameModel();
			var player1 = new Gamer(Room, new PointF(50, 70));
			player1.Setup();
			Room.GameObjects.Add(player1.ID, player1);
			Room.Players.Add(player1);

			ISolidBody solid = player1.Components.GetComponent<SolidBody>();
			RectangleF compareShape = solid.Shape;
			Assert.AreEqual(solid.Shape, compareShape);
			Vec2 compareVec = solid.Body.GetPosition();
			solid.Body.SetLinearVelocity(new Vec2(40f,0));

			float quantTime = 1f / 60f;
			Room.Field.Step(quantTime, 8, 3);
			var A = solid.Body.GetPosition();
			Assert.AreNotEqual(A, compareVec);

			solid.UpdateComponent(new TimeQuantPassed(quantTime));
			Assert.AreNotEqual(solid.Shape, compareVec);
		}
	}
}
