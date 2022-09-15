using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    protected Collider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[15];

    protected virtual void Start()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            // Collision Work
            boxCollider.OverlapCollider(filter, hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                if (hits[i])
                {
                    OnCollide(hits[i]);
                }

                // The array is not cleaned up, so we clean it
                hits[i] = null;
            }

            // Collision Work for interactions
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                OnCollide(hits[i]);

                // The array is not cleaned up, so we clean it
                hits[i] = null;
            }
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in " + this.name);
    }
}
