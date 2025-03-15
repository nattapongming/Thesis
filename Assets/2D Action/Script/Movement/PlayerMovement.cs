using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : BaseMovement
    {

        // Other Component
        BoxCollider2D boxCollider;
        Rigidbody2D rb;
        [HideInInspector] public Vector2 movementInput;
        private Vector2 smoothedMovement;
        private Vector2 mousePos;


        // Dash

        public float dashTime = 0.1f;
        [HideInInspector] public bool canDash = true;
        [HideInInspector] public bool isDashing;

        // Start is called before the first frame update
        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RotateFollowMouse();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Smooth movement
            if (Vector2.Distance(smoothedMovement, movementInput) < 0.1f)
            {
                smoothedMovement = movementInput;
            }
            else
            {
                smoothedMovement = Vector2.Lerp(smoothedMovement, movementInput, 0.1f);
            }

            // Apply acceleration
            if (movementInput != Vector2.zero && !isDashing)
            {
                curSpeed = Mathf.Min(curSpeed + acceleration * maxSpeed, maxSpeed); // Accelerate
            }
            else if (!isDashing)
            {
                curSpeed = 0f; // Stop instantly when no input
            }

            if (!isDashing)
            {
                // Apply movement
                rb.velocity = movementInput * curSpeed;
            }
            

        }



        void RotateFollowMouse()
        {
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }

        public IEnumerator DashCoroutine()
        {
            Debug.Log("Dashing");
            canDash = false;
            isDashing = true;

            rb.velocity = movementInput.normalized * (maxSpeed + 30);

            yield return new WaitForSeconds(dashTime);

            Debug.Log("Finish Dashing");
            canDash = true;
            isDashing = false;

        }
    }

    
}