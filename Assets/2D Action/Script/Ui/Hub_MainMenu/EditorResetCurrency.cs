using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorResetCurrency : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor)
        {
            PlayerCurrency.SubtractGems(999);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
