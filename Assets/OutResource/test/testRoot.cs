using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRoot : MonoBehaviour
{
    [SerializeField] Character player;
    void Start()
    {
        StartCoroutine(hurtCoroutine());
    }
    IEnumerator hurtCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            player.TakeDamage(50f);
        }
    }
}
