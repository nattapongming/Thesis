using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class BaseMovement : MonoBehaviour
    {
        [SerializeField] protected float curSpeed;
        public float maxSpeed = 15;
        public float acceleration = 0.015f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}