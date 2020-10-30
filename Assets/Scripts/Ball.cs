using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;

    public void OnShot(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Layers.Finish)
        {
            GameManager.Instance.StopWaitResult();
        }
        else
        {
            GameManager.Instance.CheckToDestroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}