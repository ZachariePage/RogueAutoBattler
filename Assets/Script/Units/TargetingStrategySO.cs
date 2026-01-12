using UnityEngine;

public abstract  class TargetingStrategySO : ScriptableObject
{
    public abstract ITargetingStrategy Create();
}
