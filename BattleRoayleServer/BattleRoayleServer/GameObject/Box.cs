﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	 class Box:GameObject
	{
		private const float restetution = 0.2f;
		private const float friction = 0.1f;
		private const float density = 0;

		public Box(IGameModel context, PointF location, Size size) : base(context)
		{
			this.Components = new List<Component>(1);
			Components.Add(new SolidBody(this, new RectangleF(location, size), restetution, 
				friction, density, TypesBody.Rectangle,TypesSolid.Solid, (ushort)CollideCategory.Box, 
				(ushort)CollideCategory.Player));
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get;  } = TypesGameObject.Box;
	}


}
