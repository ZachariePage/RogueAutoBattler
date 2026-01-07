using UnityEngine;

public class MerchantNode : Nodes
{
    public int MaxItemCominedMoney;
    
    public MerchantNode(int id, int depth, int path, int maxItemCominedMoney) : base(id, depth, path)
    {
        MaxItemCominedMoney = maxItemCominedMoney;
        type = NodeType.Merchant;
    }

    public override void Execute()
    {
        base.Execute();
    }
    
    private void CreateItemStack()
    {
        
    }
}
