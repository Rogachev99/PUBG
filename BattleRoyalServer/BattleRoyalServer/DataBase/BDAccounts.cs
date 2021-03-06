﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace BattleRoyalServer
{
    static class BDAccounts
    {
		private static readonly string NameDirectory = "Accounts";

		private static XmlSerializer formatter = new XmlSerializer(typeof(DataOfAccount));

		private static string CreatePathByLogin(string Login)
		{
			return NameDirectory + "\\" + Login; ;
		}

		public static bool ExistAccount(string Login, string Password)
        {
			try
			{
				//проверяем существует ли папка
				if (!Directory.Exists(NameDirectory))
					if (!CreateDirectoryForBD()) return false;

				//пока просто создаем аккаунт, если такой не существует
				if (!File.Exists(CreatePathByLogin(Login))) CreateAccount(Login, Password);

				//проверяем пароль
				if (CheckPassword(Login, Password)) return true;
				return false;

			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return false;
			}
		}

		private static bool CheckPassword(string Login, string Password)
		{
			DataOfAccount data = ReadData(CreatePathByLogin(Login));
			if (data.Password == Password) return true;

			return false;

		}

		private static bool CreateDirectoryForBD()
		{
			try
			{
				Directory.CreateDirectory(NameDirectory);
				return true;
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return false;
			}
		}

		private static bool CreateFileForAccount(string nameFile)
		{
			try
			{
				using (File.Create(nameFile))
				{

				}

			return true;
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return false;
			}
		}

		private static bool RecordInFile(string nameFile, DataOfAccount data)
		{
			using (FileStream fs = new FileStream(nameFile, FileMode.OpenOrCreate))
			{
				formatter.Serialize(fs, data);
				return true;
			}
		}

		private static DataOfAccount ReadData(string nameFile)
		{
			using (FileStream fs = new FileStream(nameFile, FileMode.OpenOrCreate))
			{
				return (DataOfAccount)formatter.Deserialize(fs);
			}
		}

        public static bool CreateAccount(string Login, string Password)
        {
			try
			{
				string Path = CreatePathByLogin(Login);
				CreateFileForAccount(Path);
				//инициализируем ее стандартными данными
				DataOfAccount emptyAccount = new DataOfAccount(Login, Password, 0, 0, 0, new TimeSpan());
				//записываем
				if (!RecordInFile(Path, emptyAccount)) return false;

				return true;
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return false;
			}
        }

        public static DataOfAccount GetDataOfAccount(string login, string password)
        {
			try
			{
				if (CheckPassword(login, password))
					return ReadData(CreatePathByLogin(login));

				return null;
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return null;
			}
		}

        public static bool AddToStatistic(DataOfAccount achievements)
        {
			try
			{
				string Path = CreatePathByLogin(achievements.NickName);
				DataOfAccount mainData = ReadData(Path);
				mainData.AddData(achievements);
				if (RecordInFile(Path, mainData)) return true;
				return false;
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
				return false;
			}
        }
    }
}