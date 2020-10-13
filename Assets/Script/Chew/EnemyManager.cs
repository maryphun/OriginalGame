using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyManager : MonoBehaviour
{

    [System.Serializable]
    public class SpawnData
    {
        public Vector3 spawnPosition;
        public float SpawnTime;
        public Enemy prototype;
        public float spawnNum;
        [ReadOnly]
        public bool isSpawned;
    }

    public SpawnData[] spawnDatas;
    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        foreach (SpawnData spawner in spawnDatas)
        {
            spawner.isSpawned = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SpawnData spawner in spawnDatas)
        {
            if (timer > spawner.SpawnTime)
            {
                Spawn(spawner);
            }
        }
        timer+= Time.deltaTime;
    }

    Enemy Spawn(SpawnData spawner)
    {
        if(spawner.isSpawned)
        {
            return null;
        }
        spawner.isSpawned = true;
        return Instantiate(spawner.prototype,spawner.spawnPosition, Quaternion.identity);
    }
}

