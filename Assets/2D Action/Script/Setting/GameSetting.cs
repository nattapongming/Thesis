using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Normal, Hard, Nightmare }
public enum Language { Thai, English };


public static class GameSetting
{

    public static Difficulty CurrentDifficulty = Difficulty.Normal;
    public static Language CurLanguage = Language.English;

    public static int MasterSound = 100;
    public static int SFXSound = 100;
    public static int MusicSound = 100;

    
}
