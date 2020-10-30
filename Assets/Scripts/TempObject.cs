using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.1f;

    private void Start()
    {
        StartCoroutine(LifeRoutine());
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}