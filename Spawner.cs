using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int maxSpawned;
    public float cooldownMin;
    public float cooldownMax;
    public Enemy enemyUnit;
    public HashSet<Enemy> collection;
    public HashSet<Enemy> toRemove;
    private float spawnTimer;
    private bool outOfView;


    private void Start()
    {
        spawnTimer = Random.Range(cooldownMin, cooldownMax);
        collection = new HashSet<Enemy>();
        toRemove = new HashSet<Enemy>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
            ManageSpawner();
    }

    public void ManageSpawner()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            outOfView = false;
        }
        else
        {
            outOfView = true;
        }

        if (collection.Count < maxSpawned)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f && !outOfView)
            {
                collection.Add(Instantiate(enemyUnit, transform.position, transform.rotation));
                spawnTimer = Random.Range(cooldownMin, cooldownMax);
            }
        }
        foreach (Enemy i in collection)
        {
            if (i == null)
            {
                toRemove.Add(i);
            }
        }
        foreach (Enemy i in toRemove)
        {
            collection.Remove(i);
        }
    }

}
