using UnityEngine;

[CreateAssetMenu(menuName = "Strategies/ClosestTarget")]
public class ClosestTargetStrategySO : TargetingStrategySO
{
    [SerializeField] private float radius = 5f;

    public override ITargetingStrategy Create()
    {
        return new ClosestTargetStrategy(radius);
    }
}
