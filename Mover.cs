using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hitx;
    protected RaycastHit2D hity;
    protected float ySpeed;
    protected float xSpeed;

    
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        alive = true;
    }

    protected override void RecieveDamage(Damage dmg)
    {
        base.RecieveDamage(dmg);
    }

    protected virtual void UpdateMotor(Vector3 input, float xSpeed = 1.5f, float ySpeed = 1.5f, float speed = 1f)
    {
        if (alive && !MainMenu.inMainMenu)
        {
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
            {
                // Reset MoveDelta
                moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0).normalized * speed;

                // Child transformation???
                //transform.GetChild(0).GetComponent<Transform>().localScale = ...

                
                /*
                // Swap spite direction
                if (moveDelta.x > 0)
                    transform.localScale = Vector3.one;
                else if (moveDelta.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                */

                // Add push vector, if any
                moveDelta += pushDirection;

                //Reduce push force every frame, based off recovery speed
                pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

                // See if we can move in this direction by casting a box there. If the box returns null, means we can move there
                hity = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * .90), (float)(boxCollider.size.y * .90)), 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking", "AllBlocking"));
                if (hity.collider == null)
                {
                    // Movement
                    transform.Translate(0, (float)(moveDelta.y * Time.deltaTime * 0.6), 0);

                }



                hitx = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * .90), (float)(boxCollider.size.y * .90)), 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking", "AllBlocking"));
                if (hitx.collider == null)
                {
                    // Movement
                    transform.Translate((float)(moveDelta.x * Time.deltaTime * 0.6), 0, 0);
                }
                
                if(hitx.collider != null || hity.collider != null)
                {
                    pushDirection = Vector3.zero;
                }
            }
        }
        else
        {
            // Remember to re-enable when reviving
            boxCollider.enabled = false;
        }
    }
}
