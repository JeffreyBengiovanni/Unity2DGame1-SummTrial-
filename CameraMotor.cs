using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    Vector3 ler, dir;
    public Transform closestEnemyRef;
    public Enemy closestEnemyRefScript;
    public Transform playerRef;
    public bool targetLock = false;
    public bool changeTarget = true;
    public SpriteRenderer targetVisual;

    private static CameraMotor objInstance;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (objInstance == null)
        {
            objInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerRef = GameObject.Find("Player").transform;
        lookAt = playerRef.Find("CameraLock").transform;
        targetVisual = playerRef.Find("TargetVisual").GetComponent<SpriteRenderer>();
        UpdateTargetVisual();
    }

    private void Update()
    {
        if (!GameManager.instance.loadingScreen.activeInHierarchy && !MainMenu.inMainMenu)
        {
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && GameManager.instance.player.alive)
            {
                TargetChecks();

                // Target Lock Toggle
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    targetLock = !targetLock;
                    DoTargeting();
                    UpdateTargetVisual();
                    GameManager.instance.hud.UpdateTargetToggle();
                }
            }
        }
                
    }

    private void DoTargeting()
    {
        if (targetLock)
        {
            closestEnemyRef = FindClosetEnemy();

            if (closestEnemyRef != null)
            {
                closestEnemyRefScript = closestEnemyRef.GetComponent<Enemy>();
                LockTarget();
            }
        }
        else
        {
            lookAt.position = playerRef.position;
        }
    }

    private void TargetChecks()
    {
        // In case player dies
        if (!GameManager.instance.player.alive || !GameManager.instance.player.onScreen)
        {
            changeTarget = true;
            lookAt.position = playerRef.position;
        }

        // In case enemy dies
        if (closestEnemyRef == null && lookAt.position != playerRef.position)
        {
            changeTarget = true;
            lookAt.position = playerRef.position;
        }

        if (closestEnemyRef != null)
        {
            if (!closestEnemyRefScript.onScreen || !closestEnemyRefScript.alive)
            {
                changeTarget = true;
                lookAt.position = playerRef.position;
            }
        }

        if(changeTarget && targetLock)
        {
            DoTargeting();
        }
        
        if (closestEnemyRef != null)
        {
            changeTarget = false;
        }

        if (targetLock && closestEnemyRef != null)
        {
            LockTarget();
        }
        UpdateTargetVisual();
    }

    private void UpdateTargetVisual()
    {
        if (closestEnemyRef != null && targetLock)
        {
            targetVisual.enabled = true;
            targetVisual.transform.position = closestEnemyRef.position;
        }
        else
        {
            targetVisual.enabled = false;
            targetVisual.transform.position = playerRef.position;
        }
    }

    private void LockTarget()
    {
        Vector3 additive = closestEnemyRef.position - playerRef.position;
        lookAt.position = playerRef.position + new Vector3(additive.x/3, additive.y/3, 0);
        UpdateTargetVisual();
    }

    private Transform FindClosetEnemy()
    {
        Transform tempShortest = null;
        float shortestDistance = -1;
        foreach (Transform i in GameManager.instance.spawnerManager.collection)
        {
            Enemy reference = i.GetComponent<Enemy>();
            if(reference.alive && reference.onScreen)
            {
                if (tempShortest == null)
                {
                    tempShortest = i;
                    shortestDistance = Vector3.Distance(i.position, playerRef.position);
                }
                else
                {
                    if (Vector3.Distance(i.position, playerRef.position) < shortestDistance)
                    {
                        tempShortest = i;
                        shortestDistance = Vector3.Distance(i.position, playerRef.position);
                    }
                }
            }
        }
        return tempShortest;
    }

    private void FixedUpdate()
    {
        Vector3 delta = Vector3.zero;

        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
         }
        


        //transform.Translate((float)(delta.x * Time.deltaTime * 5), 0, 0);
        //transform.Translate(0, (float)(delta.y * Time.deltaTime * 5), 0);

        transform.Translate(new Vector3((float)(delta.x * Time.deltaTime * 5), (float)(delta.y * Time.deltaTime * 5), 0));

    }

    public void UpdatePosition(Vector3 pos)
    {
        transform.Translate(transform.position + pos);

    }
}
