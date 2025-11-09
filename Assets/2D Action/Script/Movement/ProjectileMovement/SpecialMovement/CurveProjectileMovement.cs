using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CurveProjectileMovement : MonoBehaviour
    {

        public Transform target;
        [SerializeField] private float curveHeightMultiplier = 0.3f; // Higher = taller curve
        [SerializeField] private bool faceAlongCurve = false; // rotate to follow the curve

        private ProjectileMovement projectileMovement;
        private CircleCollider2D circleCollider;

        private Vector2 startPos;
        private Vector2 endPos;
        private Vector2 flightDir;
        private Vector2 curvePerp;

        private float totalDist;
        private float maxCurveHeight;
        private float progress;

        private Vector2 lastPos; // Smooth rotation

        private void Awake()
        {
            projectileMovement = GetComponent<ProjectileMovement>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            if (target != null)
            {
                Initialize(target);
            }
        }

        public void Initialize(Transform targ)
        {
            target = targ;
            startPos = transform.position;
            endPos = target.position;
            flightDir = (endPos - startPos).normalized;
            totalDist = Vector2.Distance(startPos, endPos);
            maxCurveHeight = totalDist * curveHeightMultiplier;
            curvePerp = Vector2.up;
            progress = 0f;
            lastPos = startPos;

            projectileMovement.enabled = false;
            

            // If true then rotate toward the curve when initialize
            if (faceAlongCurve)
            {
                float angle = Mathf.Atan2(flightDir.y, flightDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (totalDist <= 0) return;

            
            progress += (projectileMovement.speed * Time.deltaTime) / totalDist;
            progress = Mathf.Clamp01(progress);

            // Straight line for startPos to endPos
            Vector2 straightPos = Vector2.Lerp(startPos, endPos, progress);

            // When the progress reach 0.5, it will be at the highest of the curve.
            float curveOffset = Mathf.Sin(progress * Mathf.PI) * maxCurveHeight;
            
            // Update position to match the current curve progress
            Vector2 curvyPos = straightPos + (curvePerp * curveOffset);

            transform.position = curvyPos;

            if (faceAlongCurve)
            {
                Vector2 vel = (curvyPos - lastPos) / Time.deltaTime;
                if (vel != Vector2.zero)
                {
                    float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }

            lastPos = curvyPos;

            if (progress >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}