using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class TestSeyTarget : MonoBehaviour
{
    public Transform target;

    NavMeshAgent agent;

    private Vector2 lastTargetPosition;
    private Vector2 lastSelfPosition;

    Queue<Vector2> pathCorners = new Queue<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetNewTarget(target);
    }

    public void SetNewTarget(Transform newTarget = null)
    {
        lastSelfPosition = transform.position;
        lastTargetPosition = newTarget.position;

        agent.SetDestination(newTarget.position);
    }

    private void RotateToThePoint()
    {
        
    }
}
