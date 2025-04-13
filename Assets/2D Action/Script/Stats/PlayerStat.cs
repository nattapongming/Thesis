using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class PlayerStat : StatInfo
    {
        private CinemachineImpulseSource impulseSource;



        // Start is called before the first frame update
        void Start()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void TriggerShake(float intensity = 1.0f)
        {
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse(intensity);
            }
        }
    }
}