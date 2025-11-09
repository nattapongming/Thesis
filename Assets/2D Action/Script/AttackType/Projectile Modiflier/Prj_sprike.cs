using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace Projectile
{
    public class Prj_sprike : MonoBehaviour
    {
        private ProjectileAttack projectileAttack;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;

        [SerializeField] private float lifetime = 0f;
        [SerializeField] private Sprite[] spikeSprites;

        void Start()
        {
            projectileAttack = GetComponent<ProjectileAttack>();
            if (spriteRenderer == null)spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();

            boxCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            lifetime += Time.deltaTime;
            if (lifetime >= 67f) // Stage 3: Final sprite + enable collider
            {
                
                if (spikeSprites.Length > 1 && spikeSprites[2] != null)
                {
                    spriteRenderer.sprite = spikeSprites[2];
                }
                boxCollider.enabled = true;
            }
            else if (lifetime >= 0.34f && lifetime < 67f) // Stage 2
            {
                
                if (spikeSprites.Length > 1 && spikeSprites[1] != null)
                {
                    spriteRenderer.sprite = spikeSprites[1];
                }
            }
            else if (lifetime >= 0f && lifetime < 0.34f) // Stage 1
            {
                if (spikeSprites.Length > 0 && spikeSprites[0] != null)
                {
                    spriteRenderer.sprite = spikeSprites[0];
                }
            }
        }
    }
}