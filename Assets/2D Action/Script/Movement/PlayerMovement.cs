using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace Movement
{
    public class PlayerMovement : BaseMovement
    {

        // Other Component
        BoxCollider2D boxCollider;
        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;

        [SerializeField] GameObject weaponSpawnPoint;
        PlayerStat playerStat;


        [HideInInspector] public Vector2 movementInput;
        private Vector2 smoothedMovement;
        private Vector2 mousePos;


        // Dash

        public float dashTime = 0.1f;
        public float dashSpeed = 15f;
        public float dashCoolDown = 0.25f;
        [HideInInspector] public bool canDash = true;
        [HideInInspector] public bool isDashing;

        // Start is called before the first frame update
        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            playerStat = GetComponent<PlayerStat>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (weaponSpawnPoint == null)
            {
                Debug.LogError("WeaponSpawnPoint not assigned in Inspector!");
            }
        }

        private void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //RotateWithVelocity();
            RotateFollowMouse();

            if (weaponSpawnPoint != null)
            {
                weaponSpawnPoint.transform.position = transform.position;
            }
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
                )
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
            playerStat.isInvincible = true;

            rb.velocity = movementInput.normalized * (dashSpeed);

            yield return new WaitForSeconds(dashTime);

            //Debug.Log("Finish Dashing");
            isDashing = false;
            playerStat.isInvincible = false;

            yield return new WaitForSeconds(dashCoolDown  - 0.1f);
            spriteRenderer.color = Color.cyan;

            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            canDash = true;
        }
    }

    
}