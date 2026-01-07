using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Berserk Charge")]
public class BerserkChargeTemplate : AbilityTemplate
{
    public float JumpForce = 5f;
    public override AbilityInstance CreateInstance()
    {
        return new BerserkChargeInstance(this);
    }
}