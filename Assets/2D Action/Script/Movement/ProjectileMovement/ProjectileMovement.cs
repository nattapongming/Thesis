using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5;
    public bool isMoveBaseOnDirection;
    public bool isRotateBaseOnDirection;

    [Header("Lifetime & Hit")]
    public float lifeTime = 3;
    public bool onHitDestory = true;

    [Header ("Direction (if isMoveBaseOnDirectionIsTrue)")]
    public Vector2 direction = Vector2.right;

        
    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = isMoveBaseOnDirection ? direction.normalized : (Vector2)transform.right;
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) Destroy(gameObject);
        
    }

    public void SetAngle(float angle)
    {
        //Debug.Log($"Set direction = {angle}");
        transform.rotation = Quaternion.Euler(0, 0, angle);
        isMoveBaseOnDirection = false;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        isMoveBaseOnDirection = true;
        if (isRotateBaseOnDirection && dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


}
