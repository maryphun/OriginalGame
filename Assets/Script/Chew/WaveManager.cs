using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyBox;

public class WaveManager : MonoBehaviour
{
    [MustBeAssigned] public Collider spawnArea;

    [System.Serializable]
    public class SpawnInfo
    {
        public Enemy prototype;
        public int spawnNum;
    }
    
    [System.Serializable]
    public class WaveDetail
    {
        public SpawnInfo[] spawnInfo;
        public int maxNumPerSpawn;
        public bool randomSpawn;
        [HideInInspector] public bool isSpawned;
    }

    public int spawnDelay;
    [PositiveValueOnly] public int waveNumber;
    public WaveDetail[] wavePattern;


    private float timer = 0;



    // Start is called before the first frame update
    void Start()
    {
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
        foreach (WaveDetail wave in wavePattern)
        {
            if(wave.isSpawned)
            {
                continue;
            }
            for (int i = 0; i < wave.maxNumPerSpawn; i++)
            {
                if (wave.randomSpawn)
                {
                    int rand = Random.Range(0, wave.spawnInfo.Length);
                    SpawnEnemy(wave.spawnInfo[rand]);
                }
                else
                {
                    for (int j = 0; j < wave.spawnInfo.Length; j++)
                    {
                        if (SpawnEnemy(wave.spawnInfo[j]))
                        {
                            //spawn same enemy until run out
                            break;
                        }
                    }
                }
            }
            wave.isSpawned = true;
        }
    }

    bool SpawnEnemy(SpawnInfo prototype)
    {
        if (prototype.spawnNum > 0)
        {
            Instantiate(prototype.prototype, RandomPointInBounds(spawnArea.bounds), Quaternion.identity);
            prototype.spawnNum--;
            return true;
        }
        return false;
    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(1, 1),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}


