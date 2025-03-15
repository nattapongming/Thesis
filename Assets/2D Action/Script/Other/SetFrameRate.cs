using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameRate : MonoBehaviour
{
    void Start()
    {
        // Disable V-Sync to prevent the GPU from being locked to the monitor refresh rate
        QualitySettings.vSyncCount = 0;

        // Set the target frame rate to 60 FPS
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
