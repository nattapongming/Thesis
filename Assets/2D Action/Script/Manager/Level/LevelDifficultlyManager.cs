using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class LevelDifficultlyManager : MonoBehaviour
    {
        GameManager gameManager;

        [Header("Difficulty")]
        public int enemySpawnDifficulty = 0;
        
        void Start()
        {
            gameManager = GameManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}