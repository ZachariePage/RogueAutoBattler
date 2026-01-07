using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementAuthority
{
    StateMachine,
    Ability
}

public class UnitEvents
{
    public event Action OnTargetAcquired;
    public event Action OnCharge;
    public event Action OnAttack;
    public event Action OnAbilityWindow;
    public event Action OnDamaged;
    public event Action OnDeath;
}

public class Unit : MonoBehaviour
{
    public StateMachine StateMachine;

    public ChaseState ChaseState;
    public AttackState AttackState;
    public ChargeState ChargeState;

    [Header("State Configs")]
    [SerializeField] private ChaseStateSO chaseStateSO;
    [SerializeField] private AttackStateSO attackStateSO;
    [SerializeField] private ChargeStateSO chargeStateSO;
    
    public AbilityCoordinator abilityCoordinator;
    
    public UnitEvents Events { get; private set; }
    
    [Header("agent")]
    public NavMeshAgent Agent;
    [SerializeField] public float speed = 2;
    private Rigidbody rb;
    public MovementAuthority movementAuthority = MovementAuthority.StateMachine;

    [Header("anim")]
    public Animator anim;
    
    [Header("Target")]
    public GameObject targetGO;
    public Vector3 target;
    
    [Header("Team")]
    public int team;

    [Header("Colliders")] 
    public Collider ChargeRange;
    
    [Header("stats")]
    public float health;
    public float damage;

    public string teamString = "Team1";
    
    private void Awake()
    {
        StateMachine = new StateMachine();
        
        ChaseState = new ChaseState(this, StateMachine, chaseStateSO);
        AttackState = new AttackState(this, StateMachine, attackStateSO);
        ChargeState = new ChargeState(this, StateMachine, chargeStateSO);

        abilityCoordinator = GetComponent<AbilityCoordinator>();
        
        Events = new UnitEvents();
        
        StateMachine.Init(ChaseState);
    }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        Agent.speed = speed;
    }

    public virtual void Update()
    {
        float dt = Time.deltaTime;
        StateMachine.CurrentState.FrameUpdate();

        if (abilityCoordinator.CurrentAbility != null)
        {
            abilityCoordinator.CurrentAbility.Update(dt);
        }
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicUpdate();
    }
    
    public GameObject FindClosestPlayerInRadius(Vector3 center, float radius)
    {
        GameObject closestPlayer = null;
        float closestDistanceSqr = float.MaxValue;
        
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Team1"));
        foreach (var hitCollider in hitColliders)
        {
            float dSqrToPlayer = (hitCollider.transform.position - this.transform.position).sqrMagnitude;

            if (dSqrToPlayer < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToPlayer;
                closestPlayer = hitCollider.gameObject;
            }
        }

        targetGO = closestPlayer;
        return closestPlayer;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        StateMachine.CurrentState.OnTriggerEnter(other);
    }


    public void helperDebugState()
    {
        Debug.Log($"my state : {StateMachine.CurrentState}");
    }
    
    private void OnDestroy()
    {
        StateMachine.CurrentState?.ExitState();
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
