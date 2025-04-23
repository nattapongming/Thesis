using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifeTime = 3;
    public bool onHitDestory = true;

    [HideInInspector] public Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else { Destroy(this.gameObject); }
    }

    public void SetDirection(float angle)
    {
        Debug.Log($"Set direction = {angle}");
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
