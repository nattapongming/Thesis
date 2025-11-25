using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ai
{
    public class AiAgent : MonoBehaviour
    {
        public Transform target;

        [HideInInspector] public NavMeshAgent nevMeshAgent;
        Rigidbody2D rb;
        AiAttack aiAttack;
        SpriteRenderer spriteRenderer;

        private Vector2 lastTargetPosition;
        private Vector2 lastSelfPosition;
        private int lastFacing = 1; // 1 = right, -1 = left

        private void Awake()
        {
            nevMeshAgent = GetComponent<NavMeshAgent>();

            nevMeshAgent.angularSpeed = 9999f;
            nevMeshAgent.acceleration = 9999f;
            nevMeshAgent.stoppingDistance = 0.5f;

            nevMeshAgent.autoBraking = false;
            nevMeshAgent.autoRepath = true;

            nevMeshAgent.updateRotation = false;
            nevMeshAgent.updateUpAxis = false;
            nevMeshAgent.updatePosition = false;
            
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (TryGetComponent<AiAttack>(out aiAttack))
            {
                aiAttack = GetComponent<AiAttack>();
                aiAttack.target = target;
            }

            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (aiAttack) aiAttack.target = target;

            if (target != null)
                SetNewTarget(target);
            //RotateTowardsMovement();

            if (nevMeshAgent.hasPath || nevMeshAgent.pathPending)
            {
                // Snap transform to agent
                Vector3 navTargetPos = nevMeshAgent.nextPosition;
                transform.position = navTargetPos;

                Vector2 moveDir = nevMeshAgent.desiredVelocity.normalized;
                if (moveDir.sqrMagnitude > 0.01f)
                {
                    lastFacing = moveDir.x > 0f ? 1 : -1;
                    spriteRenderer.flipX = moveDir.x < 0f;
                }
                else
                {
                    spriteRenderer.flipX = lastFacing < 0;
                }

            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        public void SetNewTarget(Transform newTarget = null)
        {
            if (newTarget == null) return;

            target = newTarget;

            if (IsTargetOrSelfMoveFarEnough())
            {
                lastSelfPosition = transform.position;
                lastTargetPosition = target.position;
                nevMeshAgent.SetDestination(target.position);
            }

        }

        private void RotateTowardsMovement()
        {
            Vector2 moveDirection = nevMeshAgent.desiredVelocity.normalized;
            if (moveDirection.sqrMagnitude > 0.01f) // Ensure AI is moving
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                rb.rotation = angle; 
            }
        }

        private bool IsTargetOrSelfMoveFarEnough()
        {
            return Vector2.Distance(target.position, lastTargetPosition) > 0.1f ||
                   Vector2.Distance(transform.position, lastSelfPosition) > 0.1f;
        }
    }
}