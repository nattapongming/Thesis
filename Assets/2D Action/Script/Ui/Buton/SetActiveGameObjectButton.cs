using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveGameObjectButton : MonoBehaviour
{
    [SerializeField] GameObject targetGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnable()
    {
        targetGameObject.SetActive(true);
    }

    public void SetDisable()
    {
        targetGameObject.SetActive(false);
    }
}
