using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            print("Hit " + collision.gameObject.name + "!");
            Destroy(gameObject);
           
        }
    }
}
