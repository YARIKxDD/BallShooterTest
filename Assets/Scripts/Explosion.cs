using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.CheckToDestroy(other.gameObject);
    }
}