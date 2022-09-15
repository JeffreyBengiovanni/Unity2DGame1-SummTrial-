using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Using 4 for default lines
    protected int numLines = 22;
    protected string[] lines = new string[22];
    protected bool canInteract = true, close = false, k = false, canTalk = true;

    public ContactFilter2D filter;
    protected CircleCollider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[15];
    protected Animator animSpeechBubble;
    protected SpriteRenderer spriteSpeechBubble;
    protected Transform playerTransform;
    protected SpriteRenderer spriteRenderer0, spriteRenderer1, spriteRenderer2, spriteRenderer3;
    public List<Sprite> npcSprites;


    protected void Awake()
    {
        lines[0] = "Let us see how you will fare from now on.";
        lines[1] = "I'll be surprised if you can handle this one.";
        lines[2] = "You might want to dodge.";
        lines[3] = "Here is a tip: You should probably deal with enemies quickly.";
        lines[4] = "I wonder if you know what you are doing?";
        lines[5] = "You should not take this lightly.";
        lines[6] = "People always overestimate their abilities.";
        lines[7] = "I recommend you don't stand still.";
        lines[8] = "Can you handle this wave?";
        lines[9] = "You think yourself stronger than you are.";
        lines[10] = "What do you think you can possibly do here?";
        lines[11] = "Under different circumstances, you could've been my apprentice. Of course, the offer no longer stands.";
        lines[12] = "Who do you think you are standing before me?";
        lines[13] = "Power is everything... and I'm afraid you lack enough of it.";
        lines[14] = "You are going to have to get stronger if you want to pass this trial.";
        lines[15] = "Try to make this entertaining for me.";
        lines[16] = "You can quit anytime if this is too challenging for you.";
        lines[17] = "Don't waste my time with this futile attempt.";
        lines[18] = "You are using this skill? Not my first choice.";
        lines[19] = "You should quit before you hurt yourself.";
        lines[20] = "Maybe this wave will be your last.";
        lines[21] = "Perhaps one day you will reach my power. But that day is not today.";

        //        lines[5] = "If you think you get reach the pinnacle, you have to get through me.";
    }
    protected virtual void Start()
    {
        boxCollider = GetComponent<CircleCollider2D>();
        animSpeechBubble = transform.Find("SpeechBubble").GetComponent<Animator>();
        spriteSpeechBubble = transform.Find("SpeechBubble").GetComponent<SpriteRenderer>();
        canInteract = true;
        canTalk = true;
        spriteRenderer0 = this.transform.Find("NPCSprite").Find("Sprite0").GetComponent<SpriteRenderer>();
        spriteRenderer1 = this.transform.Find("NPCSprite").Find("Sprite1").GetComponent<SpriteRenderer>();
        spriteRenderer2 = this.transform.Find("NPCSprite").Find("Sprite2").GetComponent<SpriteRenderer>();
        spriteRenderer3 = this.transform.Find("NPCSprite").Find("Sprite3").GetComponent<SpriteRenderer>();
    }


    protected virtual void Update()
    {
        if (GameManager.instance.spawnerManager.waveOver && GameManager.instance.player.alive)
        {
            spriteRenderer0.enabled = true;
            spriteRenderer1.enabled = true;
            spriteRenderer2.enabled = true;
            spriteRenderer3.enabled = true;

            transform.Find("MinimapIcon").GetComponent<SpriteRenderer>().enabled = true;
            spriteSpeechBubble.enabled = true;
            canTalk = true;
        }
        else
        {
            spriteRenderer0.enabled = false;
            spriteRenderer1.enabled = false;
            spriteRenderer2.enabled = false;
            spriteRenderer3.enabled = false;

            transform.Find("MinimapIcon").GetComponent<SpriteRenderer>().enabled = false;
            spriteSpeechBubble.enabled = false;
            canTalk = false;
        }


        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            // Look at player
            LookAtPlayer();

            // Collision Work
            boxCollider.OverlapCollider(filter, hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                if (hits[i].name == "Player")
                {
                    close = true;
                    if (canTalk)
                        InRadius();
                }

                // The array is not cleaned up, so we clean it
                hits[i] = null;
            }

            if (canTalk && Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < .5)
            {
                animSpeechBubble.ResetTrigger("hide");
                animSpeechBubble.SetTrigger("show");
            }
            else
            {
                animSpeechBubble.ResetTrigger("show");
                animSpeechBubble.SetTrigger("hide");
            }

        }
    }

    protected virtual void LookAtPlayer()
    {
        playerTransform = GameManager.instance.player.transform;
        Vector3 aimDirection = (playerTransform.position - transform.position);

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //         Debug.Log("NUM " + (GameManager.instance.player.GetVelocity() * enemyThrowable.speedTotal));
        //         Debug.Log("Aim: " + (playerTransform.position - transform.position).normalized);
        //         Debug.Log("Aim Angle: " + Mathf.Atan2((playerTransform.position - transform.position).y, (playerTransform.position - transform.position).x) * Mathf.Rad2Deg);
        //         Debug.Log("Corrected Aim: " + aimDirection);
        //         Debug.Log("Corrected Aim Angle: " + angle);
        //         Debug.Log

        Vector3 a = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            a.y = -1f;
        }
        else
        {
            a.y = +1f;
        }

        // Right
        if (angle < 45 && angle > -45)
        {
            spriteRenderer0.sprite = npcSprites[12];
            spriteRenderer1.sprite = npcSprites[13];
            spriteRenderer2.sprite = npcSprites[14];
            spriteRenderer3.sprite = npcSprites[15];
        }
        // Left
        if (angle < -135 || angle > 135)
        {
            spriteRenderer0.sprite = npcSprites[8];
            spriteRenderer1.sprite = npcSprites[9];
            spriteRenderer2.sprite = npcSprites[10];
            spriteRenderer3.sprite = npcSprites[11];
        }
        // Down
        if (angle > 45 && angle < 135)
        {
            spriteRenderer0.sprite = npcSprites[4];
            spriteRenderer1.sprite = npcSprites[5];
            spriteRenderer2.sprite = npcSprites[6];
            spriteRenderer3.sprite = npcSprites[7];
        }
        // Up
        if (angle > -135 && angle < -45)
        {
            spriteRenderer0.sprite = npcSprites[0];
            spriteRenderer1.sprite = npcSprites[1];
            spriteRenderer2.sprite = npcSprites[2];
            spriteRenderer3.sprite = npcSprites[3];
        }
    }

    protected virtual void InRadius()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canInteract && canTalk && GameManager.instance.spawnerManager.waveOver)
            {
                GameManager.instance.spawnerManager.waveOver = false;
                canInteract = false;
                canTalk = false;
                int ind = Random.Range(0, numLines);
                GameManager.instance.dialogueMenu.OpenMenu("The Dark Mage", lines, Random.Range(0, numLines), true);
                StartCoroutine(InteractCooldown());
                GameManager.instance.currentWave++;
                GameManager.instance.spawnerManager.StartSpawning();
                GameManager.instance.UpdatePitch();
            }
        }
    }

    protected IEnumerator InteractCooldown()
    {
        yield return new WaitForSeconds(.5f);
        canInteract = true;
    }

    protected virtual void OnCollide(Collider2D coll)
    {

    }
}
