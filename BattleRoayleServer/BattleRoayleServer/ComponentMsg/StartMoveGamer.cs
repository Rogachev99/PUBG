﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class StartMoveGamer:IComponentMsg
	{
		public Direction Direction { get; private set; }

		public TypesComponentMsg Type { get; } = TypesComponentMsg.StartMoveGamer;

		public StartMoveGamer(Direction direction)
		{
			Direction = direction;
		}
	}
}