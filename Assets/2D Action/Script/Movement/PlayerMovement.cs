using Stats;
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
        [SerializeField] GameObject weaponSpawnPoint;
        PlayerStat playerStat;


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
            playerStat = GetComponent<PlayerStat>();

            weaponSpawnPoint = transform.Find("WeaponSpawnPoint").gameObject;
        }

        private void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //RotateWithVelocity();
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

            // Apply acceleration only when player is moving
            if (movementInput != Vector2.zero 
                && !isDashing 
                && !playerStat.isAttacking)
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
            weaponSpawnPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        void RotateWithVelocity()
        {
            if (movementInput.sqrMagnitude > 0.01f) // Check if moving
            {
                // Normalize movement input
                Vector2 direction = movementInput.normalized;

                // Round to the nearest 8-way direction
                float x = Mathf.Round(direction.x);
                float y = Mathf.Round(direction.y);

                // Ensure diagonal movement is correctly handled
                if (x != 0 && y != 0)
                {
                    direction = new Vector2(x, y).normalized; // Keep diagonal at correct length
                }
                else
                {
                    direction = new Vector2(x, y); // Keep exact horizontal/vertical movement
                }

                // Convert to angle
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                weaponSpawnPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public IEnumerator DashCoroutine()
        {
            //Debug.Log("Dashing");
            canDash = false;
            isDashing = true;

            rb.velocity = movementInput.normalized * (maxSpeed + 30);

            yield return new WaitForSeconds(dashTime);

            //Debug.Log("Finish Dashing");
            canDash = true;
            isDashing = false;

        }
    }

    
}