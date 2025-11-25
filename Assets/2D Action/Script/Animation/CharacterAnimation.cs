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

        [SerializeField] protected Sprite curSprite;
        [SerializeField] protected bool isMoving;
        [SerializeField] protected float animationSpeed;
        // Start is called before the first frame update
        virtual protected void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        virtual protected void Update()
        {
            animationSpeed = animator.speed;
            curSprite = spriteRenderer.sprite;
        }

        public void SetParameter(string parameter, bool value)
        {
            animator.SetBool(parameter, value);
            animator.Update(0f);
        }

        public void SetTrigger(string parameter)
        {
            animator.SetTrigger(parameter);
            animator.Update(0f);
        }

        public void OverrideSprite(Sprite sprite)
        {
            animator.enabled = false;
            if (sprite != null) spriteRenderer.sprite = sprite;
        }

        public void ResumeAnimator()
        {
            animator.enabled = true;
            if (animator.speed <= 0) animator.speed = 1;
            spriteRenderer.sprite = null;
            animator.Update(0f);
        }

        public void ChangeAnimationSpeed(float value)
        {
            animator.speed = value;
        }
    }
}