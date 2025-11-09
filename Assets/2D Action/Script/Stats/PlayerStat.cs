using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Stats
{
    public class PlayerStat : StatInfo
    {
        private CinemachineImpulseSource impulseSource;
        public float playerCurrentAttackCoolDown = 0;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            impulseSource = GetComponent<CinemachineImpulseSource>();

            GameManager.Instance.player = this.gameObject;
        } 
        
        // Update is called once per frame
        void Update()
        {
            
        }

        public void TriggerShake(float intensity = 1.0f)
        {
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse(intensity);
            }
        }

        protected override void Died()
        {
            
            GameManager.Instance.levelResetManager.StartRespawn(this.gameObject);

            //StartCoroutine(GameManager.Instance.levelResetManager.DeathScreenFadeIn());
            gameObject.SetActive(false);
        }

        public void RespawnPlayer()
        {
            this.gameObject.SetActive(true);
            curHp = maxHp;
        }
    }
}