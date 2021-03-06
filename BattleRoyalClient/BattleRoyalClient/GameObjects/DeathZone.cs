﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class DeathZone : GameObject, IDeathZoneForView
	{
		private TimeSpan timeToChange;
		public TimeSpan TimeToChange { get => timeToChange; set => timeToChange = value; }

		public DeathZone(ulong ID) : base(ID)
		{
			timeToChange = new TimeSpan();
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.DeathZone;

		public void ChangeTimeToReduction(TimeSpan timeSpan)
		{

		}
		
	}
}
