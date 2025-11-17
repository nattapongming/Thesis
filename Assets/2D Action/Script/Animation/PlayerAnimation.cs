using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimation : CharacterAnimation
    {
        float moveThreshold = 0.1f;

        protected override void Update()
        {
            base.Update();

            // Check horizontal velocity instead of input—super precise!
            float horizontalSpeed = rb.velocity.magnitude;
            //Debug.Log($"Speed = {rb.velocity}");
            bool isWalking = horizontalSpeed > moveThreshold;

            // Toggle the animation: Walk if speedy, Idle if chill!
            animator.SetBool("IsWalking", isWalking);

            // Optional: Flip based on velocity direction (if moving)
            if (horizontalSpeed > moveThreshold)
            {
                spriteRenderer.flipX = rb.velocity.x < 0f;  // Left if negative, right if positive—nyaa, clever!
            }
        }
    }
}