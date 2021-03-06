﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	[Serializable]
    public class DataOfAccount
    {
		public string NickName { get; set; }

		public string Password { get; set; }

		public long QuantityKills { get; set; }
        
        public long QuantityDeaths { get; set; }
      
        public long QuantityBattles { get; set; }

        public int TimeInGame_Seconds { get; set; }

		public int TimeInGame_Minutes { get; set; }

		public int TimeInGame_Hours { get; set; }

		public int TimeInGame_Days { get; set; }

		public void AddData(DataOfAccount addData)
		{
			QuantityKills += addData.QuantityKills;
			QuantityBattles += addData.QuantityBattles;
			QuantityDeaths += addData.QuantityDeaths;
			TimeInGame_Seconds += addData.TimeInGame_Seconds;
			TimeInGame_Minutes += addData.TimeInGame_Minutes;
			TimeInGame_Hours += addData.TimeInGame_Hours;
			TimeInGame_Days += addData.TimeInGame_Days;
		}

		public TimeSpan GetTimeInGame()
		{
			return new TimeSpan(TimeInGame_Days, TimeInGame_Hours, 
				TimeInGame_Minutes, TimeInGame_Seconds);
		}


		public DataOfAccount() { }

		public DataOfAccount(string nickName, string password, long quantityKills, 
			long quentityDeaths, long quentityBattles, TimeSpan quentityGameTime)
		{
			NickName = nickName;
			Password = password;
			QuantityKills = quantityKills;
			QuantityDeaths = quentityDeaths;
			QuantityBattles = quentityBattles;
			TimeInGame_Seconds = quentityGameTime.Seconds;
			TimeInGame_Minutes = quentityGameTime.Minutes;
			TimeInGame_Hours = quentityGameTime.Hours;
			TimeInGame_Days = quentityGameTime.Days;
		}
	}
}
