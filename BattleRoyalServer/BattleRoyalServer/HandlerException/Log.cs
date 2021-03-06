﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Server;
using CommonLibrary;

namespace BattleRoyalServer
{
	static class Log
	{
		private static ILogger logger = new ConsoleLogger();

		/// <summary>
		/// Обрабатывает ошибки потери соединения с клиентом
		/// </summary>
		/// <param name="client"></param>
		public static void Handler_LostConnectServerClient(ConnectedClient<IMessage> client)
		{
			client.Controler = null;
			logger.AddInLog("Потеря соединения с клиентом", "Клиент разорвал подключение с сервером");
		}

		/// <summary>
		/// Обрабатывает ошибки невозможности обработки контроллером сообщения от клиента
		/// </summary>
		public static void Handler_ErrorHandlingClientMsg(string typeControler, string TypeMsg)
		{
			logger.AddInLog($"{typeControler} не может обработать {TypeMsg}");
		}

		public static void AddNewRecord(string record)
		{
			logger.AddInLog("Событие в: " + DateTime.Now, record);
		}

		public static void AddNewRecord(string header, string description)
		{
			logger.AddInLog("Событие в: " + DateTime.Now + " " + header, description);
		}
	}
}
