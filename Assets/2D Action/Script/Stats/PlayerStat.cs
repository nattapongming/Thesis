using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Ui;

namespace Stats
{
    public class PlayerStat : StatInfo
    {
        [Header("Invincibility Settings")]
        [SerializeField] private float invTimer = 1.5f;           // How long invincible after hit
        [SerializeField] private int blinkCount = 8;              // How many flashes
        [SerializeField] private float blinkSpeed = 0.1f;         // Speed of each flash
        [SerializeField] private Color damageFlashColor = new Color(1f, 0.3f, 0.3f, 1f); // Red tint

        private float invTimeRemaining = 0f;
        private Coroutine invincibilityCoroutine = null;

        private SpriteRenderer spriteRenderer;
        private CinemachineImpulseSource impulseSource;
        [SerializeField] private Color originalColor;

        public float playerCurrentAttackCoolDown = 0;

        [SerializeField] private PlayerHPUi hpUi;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
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

        public override void TakeDamage(float damage)
        {
            if (isInvincible)
            {
                //Debug.Log("Denined");
                return;
            }

            base.TakeDamage(damage);
            hpUi.UpdateHPUi();

            if (invincibilityCoroutine != null)
                StopCoroutine(invincibilityCoroutine);
            invincibilityCoroutine = StartCoroutine(InvincibilityFlash());
        }

        protected override void Died()
        {

            //GameManager.Instance.levelResetManager.StartRespawn(this.gameObject);

            //StartCoroutine(GameManager.Instance.levelResetManager.DeathScreenFadeIn());
            GameManager.Instance.UpdateGamePause(GamePauseType.EndLevel);
            gameObject.SetActive(false);

        }

        private IEnumerator InvincibilityFlash()
        {
            isInvincible = true;
            invTimeRemaining = invTimer;

            int flashes = 0;
            bool isRed = true;

            while (invTimeRemaining > 0f)
            {
                invTimeRemaining -= Time.deltaTime;

                // Blink logic
                if (Time.frameCount % Mathf.RoundToInt(blinkSpeed / Time.deltaTime) == 0)
                {
                    flashes++;
                    isRed = !isRed;

                    if (flashes <= blinkCount * 2) // *2 because on/off
                    {
                        spriteRenderer.color = isRed ? damageFlashColor : originalColor;
                    }
                    else
                    {
                        spriteRenderer.color = originalColor; // Ensure ends white
                    }
                }

                yield return null;
            }

            // Final cleanup
            spriteRenderer.color = originalColor;
            isInvincible = false;
            invincibilityCoroutine = null;
        }

        public void RespawnPlayer()
        {
            this.gameObject.SetActive(true);
            curHp = maxHp;
        }
    }
}