﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class JoinedToQueue : IMessage
	{		
		public TypesProgramMessage TypeMessage { get; private set; } = TypesProgramMessage.JoinedToQueue;	
	}
}