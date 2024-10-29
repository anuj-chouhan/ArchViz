using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    //Using enums so that i don't make mistakes while loading and saving player prefs!
    //I can use these enumes and convert it into strings this way i can minimize the chances of mistakes
    public enum Settings
    {
        GyroscopeToggle,       // int (0 = off, 1 = on)
        MusicToggle,           // int (0 = off, 1 = on)
        GyroscopeSensitivity,  // float
        TouchSensitivity,      // float
        FOVValue,              // float
        Graphics,              // Int
    }

    // Default values for each setting
    private static Dictionary<Settings, object> defaultValues = new Dictionary<Settings, object>
    {
        { Settings.GyroscopeToggle, 0 },            // Default to off
        { Settings.MusicToggle, 1 },                // Default to on
        { Settings.GyroscopeSensitivity, 250f },    // Default sensitivity
        { Settings.TouchSensitivity, 250f },        // Default sensitivity
        { Settings.FOVValue, 60.0f },               // Default FOV
        { Settings.Graphics, 1 },                   // Default to Medium
    };

    //Method to save int in player prefs
    public static void SaveInt(Settings settings, int value)
    {
        PlayerPrefs.SetInt(settings.ToString(), value);
    }

    //Method to save float in player prefs
    public static void SaveFloat(Settings settings, float value)
    {
        PlayerPrefs.SetFloat(settings.ToString(), value);
    }

    //Method to load int in player prefs
    public static int LoadInt(Settings settings)
    {
        return PlayerPrefs.GetInt(settings.ToString(), (int)defaultValues[settings]);
    }

    //Method to load float in player prefs
    public static float LoadFloat(Settings settings)
    {
        return PlayerPrefs.GetFloat(settings.ToString(), (float)defaultValues[settings]);
    }

    // Check if a setting exists
    public static bool HasKey(Settings settings)
    {
        return PlayerPrefs.HasKey(settings.ToString());
    }

    // Reset a specific setting to its default value
    public static void ResetSetting(Settings settings)
    {
        if (defaultValues[settings] is int)
        {
            SaveInt(settings, (int)defaultValues[settings]);
        }
        else if (defaultValues[settings] is float)
        {
            SaveFloat(settings, (float)defaultValues[settings]);
        }
    }

    // Reset all settings to default values
    public static void ResetAllSettings()
    {
        foreach (var setting in defaultValues.Keys)
        {
            ResetSetting(setting);
        }
    }
}
