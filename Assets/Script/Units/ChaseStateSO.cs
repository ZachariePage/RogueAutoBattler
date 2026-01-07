using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Chase State")]
public class ChaseStateSO : StateScriptableObject
{
    public float refreshLookForTargetTimer;
    public float LookForRadius;

    public float chargeDistance;
    public float attackDistance;
}