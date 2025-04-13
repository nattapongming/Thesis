using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGradeUi : MonoBehaviour
{
    public Vector2 offset = new Vector2(50, 50); // Adjust offset as needed

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition; // Get mouse position
        transform.position = mousePosition + (Vector3)offset; // Apply offset
    }
}
