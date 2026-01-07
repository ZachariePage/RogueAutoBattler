using UnityEngine;

public class BossNode : Nodes
{
    private int UnitsPower;

    public BossNode(int id, int depth, int path, int unitsPower) : base(id, depth, path)
    {
        UnitsPower = unitsPower;
        type = NodeType.Boss;
    }

    public override void Execute()
    {
        base.Execute();
    }
    
    private void CreateUnitStack()
    {
        
    }
}
