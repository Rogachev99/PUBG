﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;

namespace BattleRoayleServer
{
	public class Bush:GameObject
	{
		private readonly SizeF bushSize = new SizeF(15, 15);

		public Bush(IModelForComponents model, PointF location) : base(model)
		{
			TransparentBody bushBody = new TransparentBody(this, new RectangleF(location, bushSize));
			Components.Add(bushBody);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.Bush;
	}
}