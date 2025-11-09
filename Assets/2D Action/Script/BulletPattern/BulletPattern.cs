using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    public class BulletPattern : MonoBehaviour
    {
        public Vector2 direction;
        [SerializeField] protected GameObject[] bulletPrefab;
        protected GameObject bulletGameObject;
        [SerializeField] protected ProjectileMovement projectileMovement;


        [Header("Pattern Stat")]
        public float angle = 0;
        public float accuracy = 100;
        public float inAccuracyAngle = 15;
        public float lifeTime = 0.5f;

        virtual protected void Awake()
        {
        }

        virtual protected void Update()
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
            } else
            {
                Destroy(gameObject);
            }
        }

        protected ProjectileMovement SetUpBullet(GameObject bullet)
        {
            bullet.transform.position = transform.position;
            bullet.transform.parent = null;
            return bullet.GetComponent<ProjectileMovement>();
        }

        protected Vector2 ApplyAccuracy(Vector2 baseDirection, float customAccuracy = -1f)
        {
            float acc = (customAccuracy >= 0f) ? customAccuracy : accuracy;
            if (acc >= 100f) return baseDirection;

            float inaccuracy = (100f - acc) / 100f;
            float maxDeviation = inaccuracy * inAccuracyAngle;
            float randomAngle = UnityEngine.Random.Range(-maxDeviation, maxDeviation);

            return Quaternion.Euler(0f, 0f, randomAngle) * baseDirection;
        }
    }
}