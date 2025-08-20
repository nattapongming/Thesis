using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get all available screen resolutions
        Resolution[] resolutions = Screen.resolutions;

        // Find the highest resolution with a width less than or equal to 1920
        Resolution bestResolution = resolutions[0];

        foreach (var res in resolutions)
        {
            if (res.width <= 1920 && res.height >= bestResolution.height)
            {
                bestResolution = res;
            }
        }

        // Set the screen resolution to the selected one in fullscreen mode
        Screen.SetResolution(bestResolution.width, bestResolution.height, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
