using Godot;
using System;
using System.Collections.Generic;

public partial class Persistence : Node
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
	}
	
	public void SaveSettings()
	{
		string saveString = "";
		foreach(KeyValuePair<string, string> setting in SM.Settings)
		{
			saveString += setting.Key + " ";
			saveString += setting.Value + " ";
		}
		
		using var file = FileAccess.Open("user://settings.dat", FileAccess.ModeFlags.ReadWrite);
    	file.StoreString(saveString);
	}

	public void LoadSettings()
	{
		if(FileAccess.FileExists("user://settings.dat"))
		{
			List<string> keys = new List<string>();
			List<string> values = new List<string>();
			bool isValue = false;

			using var file = FileAccess.Open("user://settings.dat", FileAccess.ModeFlags.Read);
    		string content = file.GetAsText();
			string[] contentSplit = content.Split(" ");

			foreach(String str in contentSplit)
			{
				if(isValue)
				{
					values.Add(str);
				}
				else
				{
					keys.Add(str);
				}

				isValue = !isValue;
			}

			for(int i = 0; i < keys.Count; i++)
			{
				SM.Settings[keys[i]] = values[i];
			}
		}
		else
		{
			GD.Print("No settings file found.");
		}
	}
}
