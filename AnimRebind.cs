using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRebind : MonoBehaviour
{

    public Animator anim;

    public void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    public void Update()
    {
        anim.applyRootMotion = false;
        anim.Rebind();
    }
}
