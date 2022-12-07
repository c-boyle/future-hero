using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiotKid : MonoBehaviour
{
    public bool isFollow = false;
    [SerializeField] private GameObject ball;
    [SerializeField] private Rigidbody _rigidbody;

    void FixedUpdate() {
        if(isFollow) MoveTowards(ball);
    }

    void MoveTowards(GameObject obj) {
        Vector3 force = Vector3.Scale(obj.transform.position - transform.position, new Vector3(1,0,1));
        _rigidbody.AddForce(Vector3.Normalize(force) * 15);
        transform.LookAt(obj.transform.position);
    }

    public void SetIsFollow(bool follow) {
        isFollow = follow;
    }
}
