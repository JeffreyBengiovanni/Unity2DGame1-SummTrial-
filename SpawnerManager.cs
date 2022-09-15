using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveAction
{
    public string name;
    public float delay;
    public Transform prefab;
    public int spawnCount;
    public string message;
}

[System.Serializable]
public class Wave
{
    public string name;
    public List<WaveAction> actions;
}

public class SpawnerManager : MonoBehaviour
{
    public int maxWaves;
    public List<Transform> spawnerLocations;
    public float difficultyFactor = 0.95f;
    public List<Wave> waves;
    private Wave m_CurrentWave;
    public Wave CurrentWave { get { return m_CurrentWave; } }
    private float m_DelayFactor = 1.0f;
    private Wave activeWave;
    private bool spawnedWaveEnemy;
    public int numEnemies;
    public bool waveOver = true;
    public bool doneWaveAction = true;
    public bool doneSpawning = true;

    public List<Transform> enemies;
    public HashSet<Transform> collection;
    public HashSet<Transform> toRemove;
    public bool canceled = false;



    private static SpawnerManager objectInstance;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (objectInstance == null)
        {
            objectInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        canceled = false;
        collection = new HashSet<Transform>();
        toRemove = new HashSet<Transform>();
        waveOver = true;
    }

    IEnumerator SpawnLoop(WaveAction A)
    {
        doneSpawning = false;
        if (A.message != "")
        {
            Debug.Log(A.message);
        }
        if (A.prefab != null && A.spawnCount > 0)
        {
            for (int i = 0; i < A.spawnCount; i++)
            {
                if (canceled)
                    break;
                if (A.delay > 0)
                {
                    numEnemies = collection.Count;
                    yield return new WaitForSeconds(A.delay * m_DelayFactor);
                }
                SpawnEnemy(A);
                if (canceled)
                    break;
            }
        }
        doneSpawning = true;
    }

    public void SpawnEnemy(WaveAction A)
    {
        if (canceled)
            return;
        int spawnerNum = Random.Range(0, 8);
        bool outOfView = true;
        Vector3 viewPos = Camera.main.WorldToViewportPoint(spawnerLocations[spawnerNum].position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            outOfView = false;
        }
        if (outOfView)
        {
            Transform t = Instantiate(A.prefab, spawnerLocations[spawnerNum].position, spawnerLocations[spawnerNum].rotation);
            collection.Add(t);
            enemies.Add(t);
            numEnemies = collection.Count;
            spawnedWaveEnemy = true;
        }
        else
        {
            SpawnEnemy(A);
        }
    }
    public void Update()
    {

        Check();
    }

    public void Check()
    {
        if (MainMenu.inMainMenu || canceled)
        {
            foreach (Transform i in collection)
            {
                toRemove.Add(i);
            }
            foreach (Transform i in toRemove)
            {
                collection.Remove(i);

            }

            StopAllCoroutines();
            canceled = false;
            spawnedWaveEnemy = false;
            waveOver = true;
            GameManager.instance.player.DamageTaken();
        }
        else
        {
            foreach (Transform i in collection)
            {
                if (i == null || i.GetComponent<Enemy>().alive == false)
                {
                    toRemove.Add(i);
                }
            }
            foreach (Transform i in toRemove)
            {
                collection.Remove(i);
            }
            if (spawnedWaveEnemy && doneSpawning && doneWaveAction)
            {
                numEnemies = collection.Count;
                if (numEnemies <= 0)
                {
                    spawnedWaveEnemy = false;
                    waveOver = true;
                    canceled = false;
                    GameManager.instance.completedWave++;
                    GameManager.instance.SaveState();
                    GameManager.instance.player.DamageTaken();
                    GameManager.instance.player.manaAmount = GameManager.instance.player.maxMana;
                    GameManager.instance.hud.UpdateManaBar();
                }
            }
        }
    }

    public void StartSpawning()
    {
        if (!MainMenu.inMainMenu)
        {
            doneWaveAction = false;
            activeWave = waves[GameManager.instance.currentWave-1];
            m_DelayFactor *= difficultyFactor;
            foreach (WaveAction A in activeWave.actions)
            {
                StartCoroutine(SpawnLoop(A));
            }
            doneWaveAction = true;
        }
    }

}