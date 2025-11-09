using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ai
{
    public class AiAgent : MonoBehaviour
    {
        public Transform target;

        NavMeshAgent agent;
        Rigidbody2D rb;
        AiAttack aiTurret;


        private Vector2 lastTargetPosition;
        private Vector2 lastSelfPosition;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            agent = GetComponent<NavMeshAgent>();

            if (TryGetComponent<AiAttack>(out aiTurret))
            {
                aiTurret = GetComponent<AiAttack>();
                aiTurret.target = target;
            }

            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.updatePosition = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (aiTurret) aiTurret.target = target;
            SetNewTarget(target);
            //RotateTowardsMovement();
            
            if (agent.remainingDistance <= agent.stoppingDistance + 0.05f)
            {
                rb.velocity = Vector2.zero;
            } else
            {
                Vector2 dir = (agent.steeringTarget - transform.position).normalized;
                rb.velocity = dir * agent.speed;
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
                agent.SetDestination(target.position);
            }

        }

        private void RotateTowardsMovement()
        {
            Vector2 moveDirection = agent.desiredVelocity.normalized;
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