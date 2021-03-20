using Microsoft.Win32;
using System;
using System.Linq;

namespace GameEngine.Core.impl
{
	class ReestryConfigurator : IConfigurator
	{
		public int Music { get; set; }
		public int Value { get; set; }
		public bool FullScreen { get; set; }
		public string NickName1 { get; set; }
		public string NickName2 { get; set; }

		public static readonly string rootModule = @"Software\MyTerraria";
		public static readonly string settingsModule = @"Software\MyTerraria\Settings";

		public static readonly string musicSettingKey = "Music";
		public static readonly string valueSettingKey = "Value";
		public static readonly string fullSreenSettingKey = "FullScreen";
		public static readonly string nameSettingKey1 = "PlayerName01";
		public static readonly string nameSettingKey2 = "PlayerName02";

		public static readonly int defaultMusic = 100;
		public static readonly int defaultValue = 100;
		public static readonly bool defaultFullScreen = true;
		public static readonly string defaultNickName1 = "Player 0";
		public static readonly string defaultNickName2 = "Player 1";

		public void LoadConfiguration()
		{
			try
			{
				RegistryKey currentConfig = Registry.CurrentUser;
				RegistryKey settings = OpenOrCreateSubKey(currentConfig, settingsModule);

				Music = (int)settings.GetValue(musicSettingKey, defaultMusic);
				Value = (int)settings.GetValue(valueSettingKey, defaultMusic);
				FullScreen = (int)settings.GetValue(fullSreenSettingKey, defaultFullScreen) == 1 ? true : false;
				NickName1 = settings.GetValue(nameSettingKey1, defaultNickName1).ToString();
				NickName2 = settings.GetValue(nameSettingKey2, defaultNickName2).ToString();

				settings.Close();
			}
			catch (Exception)
			{

			}
		}

		public void SaveConfiguration()
		{
			try
			{
				RegistryKey currentConfig = Registry.CurrentUser;
				RegistryKey settings = OpenOrCreateSubKey(currentConfig, settingsModule);

				settings.SetValue(musicSettingKey, Music);
				settings.SetValue(valueSettingKey, Value);
				settings.SetValue(fullSreenSettingKey, FullScreen, RegistryValueKind.DWord);
				settings.SetValue(nameSettingKey1, NickName1);
				settings.SetValue(nameSettingKey2, NickName2);

				settings.Close();
			}
			catch (Exception)
			{

			}
		}

		public void ResetConfiguration()
		{
			RegistryKey currentConfig = Registry.CurrentUser;
			RegistryKey settings = currentConfig.OpenSubKey(settingsModule);

			if (settings != null)
			{
				currentConfig.DeleteSubKeyTree(rootModule);
			}

			Music = defaultMusic;
			Value = defaultValue;
			FullScreen = defaultFullScreen;
			NickName1 = defaultNickName1;
			NickName2 = defaultNickName2;
		}

		private RegistryKey OpenOrCreateSubKey(RegistryKey key, string name)
		{
			return isContainsSubKey(key, name) ?
				key.OpenSubKey(name, true) :
				key.CreateSubKey(name);
		}

		private bool isContainsSubKey(RegistryKey key, string name)
		{
			return key.GetSubKeyNames().Contains(name);
		}
	}
}
