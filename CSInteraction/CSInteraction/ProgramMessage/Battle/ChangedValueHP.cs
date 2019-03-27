﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangedValueHP:IMessage
	{
		public float NewValueHP { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangedValueHP;

		public ChangedValueHP(float newValueHp)
		{
			NewValueHP = newValueHp;
		}
	}
}