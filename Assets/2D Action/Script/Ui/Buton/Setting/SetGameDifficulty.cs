using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameDifficulty : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty()
    {
        GameSetting.CurrentDifficulty = difficulty;
    }
}
