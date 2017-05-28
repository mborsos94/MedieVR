using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.gameObject.tag == "Arrow")
        {
            c.transform.SetParent(this.transform.parent, true);
            c.rigidbody.isKinematic = true;
            Destroy(c.gameObject, 5);
        }
    }
}
