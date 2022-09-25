using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    private enum PathChoice
    {
        pathA,
        pathB
    }


    [SerializeField] private List<Transform> pathA;
    [SerializeField] private List<Transform> pathB;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float velocity;
    [SerializeField] private int pointOfNoReturn = 2;
    private int currentPathPoint = 0;
    private PathChoice pathChoice = PathChoice.pathA;

    private float trainStartDelay = 6f;

    private List<Transform> Path { 
        get { 
          switch (pathChoice)
            {
                case PathChoice.pathA:
                    return pathA;
                case PathChoice.pathB:
                    return pathB;
                default:
                    return pathA;
            }
        } 
    }



    // Update is called once per frame
    void Update()
    {
        trainStartDelay -= Time.deltaTime;
        if (trainStartDelay > 0)
        {
            return;
        }
        if (currentPathPoint >= Path.Count)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 direction = Path[currentPathPoint].position - rb.position;
        direction = new(direction.x, 0f, direction.z);
        if (direction.magnitude <= 4f)
        {
            currentPathPoint++;
            return;
        }

        rb.velocity = direction.normalized * velocity;
        transform.LookAt(transform.position + direction, Vector3.up);
    }

    public void SwitchPath()
    {
        if (currentPathPoint >= pointOfNoReturn)
        {
            return;
        }
        if (pathChoice == PathChoice.pathA)
        {
            pathChoice = PathChoice.pathB;
        } else
        {
            pathChoice = PathChoice.pathB;
        }
    }
}
