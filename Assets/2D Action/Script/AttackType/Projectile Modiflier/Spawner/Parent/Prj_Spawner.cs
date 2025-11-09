using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public abstract class Prj_Spawner : MonoBehaviour
    {
        [SerializeField] protected GameObject spawnBullet;
        [SerializeField] protected float spawnCooldown = 1f;
        protected float currentSpawnCooldown = 0;

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (spawnBullet == null) return;

            if (currentSpawnCooldown >= spawnCooldown)
            {
                GameObject bulletGO = Instantiate(spawnBullet);
                bulletGO.transform.position = transform.position;
                currentSpawnCooldown = 0;
            }
            else { currentSpawnCooldown += Time.deltaTime; }
        }
    }
}