using Ai;
using Animation;
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
        private AiAnimation anim;
        //private SpriteRenderer spriteRenderer;

        [Header("Ai Setting")]
        [SerializeField] private AiStatModiflier aiStatModiflier;
        [SerializeField] private bool isPlayAnimWhenDied;
        [SerializeField] private bool isEndGameWhenDied;
        [SerializeField] private GameObject spawnInEffect;
        [SerializeField] private float spawnTimer = 1f;

        [Header("Ai stat")]
        public bool hasCollisionDamage;
        [SerializeField] float collisionDamage = 1;

        [Header("Random Stat")]
        [SerializeField] bool hasRandomSpeed;
        [SerializeField] int randomSpeedOffSet = 2;

        // Other called component
        [Header("Other Componet")]
        public RoomComponent partOfRoomComponent;

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<AiAnimation>();
            aiStatModiflier = GetComponent<AiStatModiflier>();
            GameManager.Instance.levelResetManager.enemyGameObject.Add(this.gameObject);

            if (spawnInEffect != null)
            StartCoroutine(StartSpawn());

            Randomness();

            if (aiStatModiflier) aiStatModiflier.SetDiffcalityStat(this);
            agent.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Died()
        {
            GetComponent<AiAttack>().enabled = false;

            if (partOfRoomComponent != null)
            {
                partOfRoomComponent.activeEnemy.Remove(gameObject);
                //Debug.Log($"Dead, current active enemy in {partOfRoomComponent.gameObject} is {partOfRoomComponent.activeEnemy.Count}");
                if (partOfRoomComponent.activeEnemy.Count <= 0) partOfRoomComponent.UpdateRoom();
            }

            if (isEndGameWhenDied)
            {
                GameManager.Instance.UpdateGamePause(GamePauseType.EndLevel);
            }

            if (isPlayAnimWhenDied)
            {
                // Disable AI
                AiAgent aiAgent = GetComponent<AiAgent>();
                AiAttack aiAttack = GetComponent<AiAttack>();
                if (aiAgent) aiAgent.enabled = false;
                if (agent) agent.enabled = false;
                if (aiAttack) aiAttack.enabled = false;

                Animator animator = GetComponent<Animator>();
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

                animator.SetTrigger("Death");

                //StartCoroutine(ForceDeathClipNoLoop(animator));

                spriteRenderer.color = new Color32(100, 100, 100, 255);
            }
            else base.Died();


        }

        private IEnumerator StartSpawn()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(0, 0, 0, 0);

            AiAgent aiAgent = GetComponent<AiAgent>();
            AiAttack aiAttack = GetComponent<AiAttack>();
            if (aiAgent) aiAgent.enabled = false;
            if (aiAttack) aiAttack.enabled = false;
            if (agent) agent.enabled = false;

            if (spawnInEffect != null)
                Instantiate(spawnInEffect, transform.position, Quaternion.identity);

            float timer = 0f;
            while (timer < spawnTimer)
            {
                timer += Time.deltaTime;
                float t = timer / spawnTimer;

                // Smooth fade (ease-out for extra polish!)
                float ease = 1f - Mathf.Pow(1f - t, 3f); // Cubic ease-out

                spriteRenderer.color = Color.Lerp(
                    new Color(0, 0, 0, 0),      // from black + transparent
                    originalColor,              // to original color
                    ease
                );

                yield return null;
            }

            spriteRenderer.color = originalColor;
            enabled = true;
            if (aiAgent) aiAgent.enabled = true;
            if (aiAttack) aiAttack.enabled = true;
            if (agent) agent.enabled = true;


        }

        private IEnumerator ForceDeathClipNoLoop(Animator animator)
        {
            // Wait one frame so the death state actually starts playing
            yield return null;

            // Now we are 100% sure the death clip is active
            if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                AnimationClip deathClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                deathClip.wrapMode = WrapMode.Once;   // ← NOW IT WORKS!!
            }

            /*// Optional: auto-destroy after animation
            float clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            Destroy(gameObject, clipLength + 0.1f);*/
        }

        private void OnDestroy()
        {
            GameManager.Instance.levelResetManager.enemyGameObject.Remove(this.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StatInfo otherstat = collision.gameObject.GetComponent<StatInfo>();
            if (otherstat && otherstat.faction != faction && isAttacking && hasCollisionDamage)
            {
                otherstat.TakeDamage(collisionDamage);
            }
        }

        private void Randomness()
        {
            if (hasRandomSpeed)
            {
                float newSpeed = Random.Range(speed - randomSpeedOffSet, speed + randomSpeedOffSet + 1);
                speed = newSpeed;
                UpdateSpeed(speed);
            }
        }

        public void UpdateSpeed(float speed)
        {
            agent.speed = speed;
        }
    }
}