using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public enum Faction { Ally, MainEnemy, SideEnemy, None }

    public class StatInfo : MonoBehaviour
    {
        public float maxHp = 1;
        public float atk = 1;
        public float speed = 5;
        public Faction faction = Faction.None;

        public float curHp;
        public bool isAttacking;
        public bool isInvincible;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            curHp = maxHp;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void TakeDamage(float damage)
        {
            //Debug.Log($"Take damge {curHp} -> {curHp - damage}");

            if (damage >= curHp)
            {
                curHp = 0;
                Died();
            } else
            {
                curHp -= damage;
            }

        }

        protected virtual void Died()
        {
            Destroy(this.gameObject);
        }
    }
}