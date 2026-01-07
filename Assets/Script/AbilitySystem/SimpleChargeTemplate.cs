using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SimpleCharge")]
public class SimpleChargeTemplate : AbilityTemplate
{
    public override AbilityInstance CreateInstance()
    {
        return new SimpleChargeInstance(this);
    }
}