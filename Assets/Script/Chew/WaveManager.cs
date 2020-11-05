using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyBox;

public class WaveManager : MonoBehaviour
{
    [MustBeAssigned] public Collider spawnArea;

    public GameObject spawnEffect;
    [HideInInspector] public static int enemyCount = 0;

    [System.Serializable]
    public class SpawnInfo
    {
        public Enemy prototype;
        public int spawnLimit;
        public bool spawnInSpecificLocation;
        [ConditionalField(nameof(spawnInSpecificLocation),false,true)] public Vector3 spawnLocation;
    }
    
    [System.Serializable]
    public class WaveDetail
    {
        public SpawnInfo[] spawnInfo;
        public int maxEnemyPerSpawn;
        public bool randomSpawn;
        [HideInInspector] public bool isSpawned;
    }

    public int spawnDelay;
    [PositiveValueOnly] public int waveNumber;
    public WaveDetail[] wavePattern;

    private bool waveOngoing;
    private int spawnSequence = 0;
    private float timer = 0;



    // Start is called before the first frame update
    void Start()
    {
        waveOngoing = false;
        foreach( WaveDetail wave in wavePattern)
        {
            wave.isSpawned = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < spawnDelay )
        {
            return;
        }
        if (enemyCount <= 0)
        {
            waveOngoing = false;
        }
        foreach (WaveDetail wave in wavePattern)
        {
            if(waveOngoing )
            {
                break;
            }

            if (wave.isSpawned)
            {
                continue;
            }
            // Start spawn enemy until max enemy per spawn
            for (int i = 0; i < wave.maxEnemyPerSpawn; i++)
            {
                // If random spawn, spawn and continue
                if (wave.randomSpawn)
                {
                    int rand = Random.Range(0, wave.spawnInfo.Length);
                    StartCoroutine(SpawnEnemy(wave.spawnInfo[rand]));
                    continue;
                }

                // Spawn in sequence
                for (int j = 0; j < wave.spawnInfo.Length;)
                {
                    if (wave.spawnInfo[j].spawnLimit <= 0)
                    {
                        j++; continue;
                    }

                    StartCoroutine(SpawnEnemy(wave.spawnInfo[j]));
                    break;
                }
            }

            waveOngoing = true;
            bool allSpawned = true;
            for (int j = 0; j< wave.spawnInfo.Length;j++)
            {
                //allSpawned = false if one of the enemy is not spawned yet
                allSpawned &= (wave.spawnInfo[j].spawnLimit <= 0);
            }
            if (allSpawned)
            {
                wave.isSpawned = true;
            }
        }
        
    }

    IEnumerator SpawnEnemy(SpawnInfo prototype)
    {
        if (prototype == null)
        {
            prototype.spawnLimit = 0;
            yield break;
        }
        if (prototype.spawnLimit > 0)
        {
            var randPos = RandomPointInBounds(spawnArea.bounds);
            var spawnFX = Instantiate(spawnEffect, randPos, transform.rotation) as GameObject;
            var ps = spawnFX.GetComponent<ParticleSystem>();
            Destroy(spawnFX, ps.main.duration );
            prototype.spawnLimit--;
            enemyCount++;
            yield return new WaitForSeconds(ps.main.duration - 1f);
            if (prototype.spawnInSpecificLocation)
            {
                Instantiate(prototype.prototype, prototype.spawnLocation, Quaternion.identity);
            }
            else
            {
                Instantiate(prototype.prototype, randPos, Quaternion.identity);
            }
        }
        yield break;
    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(0.1f, 0.1f),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

 
    //IEnumerator SpawnEffect(Vector3 pos)
    //{
        
    //}
}


