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

        private Vector2 lastTargetPosition;
        private Vector2 lastSelfPosition;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = false;
            agent.updateUpAxis = false;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            SetNewTarget(target);
            //RotateTowardsMovement();
        }

        public void SetNewTarget(Transform newTarget = null)
        {
            if (newTarget == null) return;
            if (IsTargetOrSelfMoveFarEnough())
            {
                lastSelfPosition = transform.position;
                lastTargetPosition = newTarget.position;

                agent.SetDestination(newTarget.position);

                if (newTarget != target) target = newTarget;
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