using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color targetColor;
    [SerializeField] private float timeChange;

    public void SetReadyToDestroy()
    {
        StartCoroutine(ChangeColorRoutine());
    }

    private IEnumerator ChangeColorRoutine()
    {
        Material material = Instantiate(meshRenderer.material);
        meshRenderer.material = material;

        Color startColor = material.color;

        for (float t = 0; t < timeChange; t += Time.deltaTime)
        {
            material.color = Color.Lerp(startColor, targetColor, t / timeChange);
            yield return null;            
        }
    }
}