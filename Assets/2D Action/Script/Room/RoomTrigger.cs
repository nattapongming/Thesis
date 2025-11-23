using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] RoomComponent partOfRoomComponent;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Other is {collision.gameObject}");
        if (collision.gameObject.CompareTag("Player"))
        {
            partOfRoomComponent.StartRoom();
            enabled = false;
        }
    }
}
