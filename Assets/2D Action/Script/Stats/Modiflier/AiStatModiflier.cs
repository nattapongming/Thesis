using Manager;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    public class AiStatModiflier : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;

        [SerializeField] private float speedModiflier = 10;
        [SerializeField] private float attackSpeedModiflier = 15;

        // Start is called before the first frame update
        void Awake()
        {
            speedModiflier = 100 + speedModiflier;
            attackSpeedModiflier = 100 + attackSpeedModiflier;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDiffcalityStat(AiStat aiStat)
        {
            switch (GameManager.Instance.curDifficulty)
            {
                case Difficulty.Hard:
                    aiStat.speed = (aiStat.speed * speedModiflier) / 100;
                    break;

                case Difficulty.Nightmare:
                    aiStat.speed = (aiStat.speed * (speedModiflier * 2)) / 100;
                    break;

                default:
                    break;
            }

        }
    }
}