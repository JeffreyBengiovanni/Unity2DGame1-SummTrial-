using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    protected virtual void Start()
    {
        StartCoroutine(Countdown());
        
    }

    protected IEnumerator Countdown()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
