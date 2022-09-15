using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public ContactFilter2D filter;
    protected Collider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[15];
    protected Skill sSkill;
    protected Animator anim;
    
    // Pierce
    public HashSet<Collider2D> collidedWith;
    public SpriteRenderer spriteRender;

    protected void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    protected virtual void Start()
    {        
        // WILL NEED TO CHANGE FOR OTHER SKILLS THAT CREATE. MIGHT NEED MULTIPLE CHILD CLASSES
        sSkill = GameManager.instance.fireball;
        boxCollider = GetComponent<CircleCollider2D>();
        collidedWith = new HashSet<Collider2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        StartCoroutine(Duration());
    }

    protected virtual void Update()
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
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            if (collidedWith.Contains(coll))
            {
                return;
            }
            else
            {
                // Create a new Damage object, send to fighter we hit
                Damage dmg = new Damage
                {
                    damageAmount = GameManager.instance.currentSkill.GetDamage()/2,
                    origin = transform.position,
                    pushForce = GameManager.instance.currentSkill.GetSpecial()
                };

                coll.SendMessage("RecieveDamage", dmg);

                collidedWith.Add(coll);
            }
        }
    }
  

    protected IEnumerator Duration()
    {
        yield return new WaitForSeconds(.7f);
        Destroy(gameObject);
    }
}
