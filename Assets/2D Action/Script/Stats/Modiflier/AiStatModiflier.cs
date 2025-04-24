using Manager;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    public class AiStatModiflier : MonoBehaviour
    {
        AiAgent aiAgent;
        AiStat aiStat;


        [SerializeField] private float speedModiflier = 15;
        [SerializeField] private float attackSpeedModiflier = 15;

        // Start is called before the first frame update
        void Start()
        {
            aiAgent = GetComponent<AiAgent>();
            SetDiffcalityStat();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDiffcalityStat()
        {
            switch (GameManager.Instance.curDifficulty)
            {
                case Difficulty.Normal:

                    break;
            }

        }
    }
}