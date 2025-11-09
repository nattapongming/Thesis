using Ai;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Stats
{
    public class AiStat : StatInfo
    {
        private NavMeshAgent agent;
        [SerializeField] private AiStatModiflier aiStatModiflier;

        [Header("Ai stat")]
        public bool isCollisionDamage;
        [SerializeField] float collisionDamage = 1;

        // Other called component
        [HideInInspector] public EnemySpawn partOfSpawnArena;

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            aiStatModiflier = GetComponent<AiStatModiflier>();
            GameManager.Instance.levelResetManager.enemyGameObject.Add(this.gameObject);

            if (aiStatModiflier) aiStatModiflier.SetDiffcalityStat(this);
            agent.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Died()
        {
            base.Died();
        }

        private void OnDestroy()
        {
            if (partOfSpawnArena) partOfSpawnArena.enemySpawnNumber--;
            GameManager.Instance.levelResetManager.enemyGameObject.Remove(this.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StatInfo otherstat = collision.gameObject.GetComponent<StatInfo>();
            if (otherstat && otherstat.faction != faction && isAttacking && isCollisionDamage)
            {
                //Debug.Log("This is enemy!");
                otherstat.TakeDamage(collisionDamage);
            }
        }

        public void UpdateSpeed(float speed)
        {
            agent.speed = speed;
        }
    }
}