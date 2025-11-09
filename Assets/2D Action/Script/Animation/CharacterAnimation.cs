using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace Animation
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Rigidbody2D rb;

        [SerializeField] protected bool isMoving;
        // Start is called before the first frame update
        protected void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        virtual protected void Update()
        {
            
        }
    }
}