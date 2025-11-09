using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency 
{
    private const string GemsKey = "PlayerGems"; // Key for PlayerPrefs
    private static int _gem = -1; // Lazy init flag (-1 = not loaded)

    public static int Gem
    {
        get
        {
            if (_gem == -1) Load(); // Lazy load on first access
            return _gem;
        }
        private set
        {
            _gem = value;
            Save();
        }
    }

    public static void AddGems(int amount)
    {
        if (amount <= 0) return; // Optional: Ignore invalid
        Gem += amount;
    }

    public static void SubtractGems(int amount)
    {
        if (amount <= 0) return;
        Gem = Mathf.Max(0, Gem - amount); // Clamp to 0
    }

    private static void Load()
    {
        _gem = PlayerPrefs.GetInt(GemsKey, 0); // Default 0 if no save
    }

    private static void Save()
    {
        PlayerPrefs.SetInt(GemsKey, _gem);
        PlayerPrefs.Save(); // Force write (safe for infrequent calls)
    }
}
