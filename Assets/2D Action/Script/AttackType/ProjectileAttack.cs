using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class ProjectileAttack : AttackStat
    {
        [SerializeField] float speed = 5;
        [SerializeField] float lifeTime = 3;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

        }

        // Update is called once per frame
        void Update()
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
            }
            else { Destroy(this.gameObject); }
        }
    }
}