using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    public class EnemyAttackPattern : BulletPattern
    {
        
        [SerializeField] int pellet = 4;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            if (pellet <= 0) return;

            float startAngle = -angle / 2;
            // If there's only 1 pellet then shot normally
            float angleStep = (pellet > 1) ? angle / (pellet - 1f) : 0;

            for (int i = 0; i < pellet; i++)
            {
                float currentAngle = startAngle + (i * angleStep);

                bulletGameObject = Instantiate(bulletPrefab[0]);
                projectileMovement = SetUpBullet(bulletGameObject);
                
                if (projectileMovement != null)
                {
                    Vector3 rotatedDirection = Quaternion.Euler(0, 0, currentAngle) * direction;
                    projectileMovement.SetDirection(ApplyAccuracy(rotatedDirection));
                }
                
            }
        }
    }
}