using UnityEngine;

public class ClosestTargetStrategy : ITargetingStrategy
{
    private float radius;

    public ClosestTargetStrategy(float radius)
    {
        this.radius = radius;
    }

    public GameObject SelectTarget(Unit unit, string mask)
    {
        GameObject closestPlayer = null;
        float closestDistanceSqr = float.MaxValue;
        
        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, radius, LayerMask.GetMask(mask));
        foreach (var hitCollider in hitColliders)
        {
            float dSqrToPlayer = (hitCollider.transform.position - unit.transform.position).sqrMagnitude;

            if (dSqrToPlayer < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToPlayer;
                closestPlayer = hitCollider.gameObject;
            }
        }
        return closestPlayer;
    }
}


