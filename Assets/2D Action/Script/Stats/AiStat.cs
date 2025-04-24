using Ai;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Stats
{
    public class AiStat : StatInfo
    {
        private NavMeshAgent agent;
        private AiStatModiflier aiStatModiflier;

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            aiStatModiflier = GetComponent<AiStatModiflier>();

            agent.speed = speed;
            if (aiStatModiflier) aiStatModiflier.SetDiffcalityStat();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Died()
        {
            base.Died();
        }

        
    }
}