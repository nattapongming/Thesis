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
        [SerializeField] private AiStatModiflier aiStatModiflier;
        [SerializeField] private bool isPlayAnimWhenDied;

        [Header("Ai stat")]
        public bool hasCollisionDamage;
        [SerializeField] float collisionDamage = 1;

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

            if (aiStatModiflier) aiStatModiflier.SetDiffcalityStat(this);
            agent.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Died()
        {
            if (partOfRoomComponent != null)
            {
                partOfRoomComponent.activeEnemy.Remove(gameObject);
                //Debug.Log($"Dead, current active enemy in {partOfRoomComponent.gameObject} is {partOfRoomComponent.activeEnemy.Count}");
                if (partOfRoomComponent.activeEnemy.Count <= 0) partOfRoomComponent.UpdateRoom();
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
            if (isAttacking && hasCollisionDamage) Debug.Log("Enemy take damage!");
            if (otherstat && otherstat.faction != faction && isAttacking && hasCollisionDamage)
            {
                Debug.Log("This is enemy!");
                otherstat.TakeDamage(collisionDamage);
            }
        }

        public void UpdateSpeed(float speed)
        {
            agent.speed = speed;
        }
    }
}