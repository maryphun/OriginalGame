using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyBox;

public class EnemyManager : MonoBehaviour
{
    [MustBeAssigned] public Collider spawnArea;
    [System.Serializable]
    public class WaveDetail
    {
        public Enemy prototype;
        public int spawnNum;
        public int maxNumPerSpawn;
        [HideInInspector] public bool isSpawned;
    }
    
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
        foreach (WaveDetail wave in wavePattern)
        {
        }
            timer += Time.deltaTime;
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


