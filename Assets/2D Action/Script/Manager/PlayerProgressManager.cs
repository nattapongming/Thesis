using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    public static PlayerProgressManager Instance { get; private set; }

    public float currentUnlockStage { get; private set; } = 0f;

    public List<EnchantmentSO> unlockedEnchantments = new List<EnchantmentSO>();

    public List<EnchantmentSO> preUpadateTestUnlockEnchantment;
    public List<EnchantmentSO> NewUnlockEnchantmentList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadProgress();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Testing: Add test unlocks (fires events)
        if (Application.isEditor && preUpadateTestUnlockEnchantment.Count > 0)
        {
            foreach (var enchant in preUpadateTestUnlockEnchantment)
            {
                AddEnchantment(enchant);
            }
        }
    }

    public void AddEnchantment(EnchantmentSO enchantment)
    {
        if (!unlockedEnchantments.Contains(enchantment))
        {
            unlockedEnchantments.Add(enchantment);
            unlockedEnchantments = unlockedEnchantments.OrderBy(e => e.enchantmentIndex).ToList();
        }
    }

    public void RemoveEnchantment(EnchantmentSO enchantment)
    {
        unlockedEnchantments.Remove(enchantment);
    }

    public List<EnchantmentSO> GetUnlockEnchantment()
    {
        return unlockedEnchantments.OrderBy(e => e.enchantmentIndex).ToList();
    }

    /*private void SaveProgress()
    {
        var saveData = new SaveData
        {
            stage = currentUnlockStage,
            enchantmentUnlock = unlockedEnchantments.Select(e => e.enchantmentName).ToList()
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("PlayerUnlocks", json); PlayerPrefs.Save();
    }*/

    /*private void LoadProgress()
    {
        if (!PlayerPrefs.HasKey("PlayerUnlocks")) return;

        string json = PlayerPrefs.GetString("PlayerUnlocks");
        var saveData = JsonUtility.FromJson<SaveData>(json);

        currentUnlockStage = saveData.stage;

        // Rebuild HashSet 
        foreach (var name in saveData.enchantmentUnlock)
        {
            // You'll need a way to lookup SO by name—e.g., from a Resources folder or dict
            var enchant = Resources.Load<EnchantmentSO>("Enchantments/" + name); // Adjust path
            if (enchant != null)
            {
                unlockedEnchantments.Add(enchant); // No event fire on load (handle full refresh elsewhere)
            }
            else
            {
                Debug.LogWarning($"Missing EnchantmentSO for name: {name}");
            }
        }
    }*/

    /*[System.Serializable]
    private class SaveData
    {
        public float stage;
        public List<string> enchantmentUnlock;

        public Dictionary<string, string> storyChoice;
    }*/

}
