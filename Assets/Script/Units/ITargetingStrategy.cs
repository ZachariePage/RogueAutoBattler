using UnityEngine;

public interface ITargetingStrategy
{
    GameObject SelectTarget(Unit unit, string mask);
}
