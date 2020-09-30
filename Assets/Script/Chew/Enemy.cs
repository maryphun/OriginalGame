using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyStat enemyStat;
    [ReadOnly]
    [SerializeField]
    private string currentState;
    private StateMachine<Enemy> stateMachine;
    private GameObject targetPlayer;
    public NavMeshAgent agent;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyMovement());
        currentState = stateMachine.GetCurrentState.ToString();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        currentState = stateMachine.GetCurrentState.ToString();
     
    }

    public EnemyStat EnemyStat
    {
        get { return enemyStat; }
    }

    public GameObject TargetPlayer
    {
        get { return targetPlayer; }
    }
    public string CurrentState
    {
        get { return currentState; }
    }

}
