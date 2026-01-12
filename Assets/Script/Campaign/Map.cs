using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    [SerializeField]
    private List<Nodes> allNodes = new();
    private Nodes startNode;
    private Nodes endNode;
    public Nodes[,] grid { get; private set; }
    
    private int height = 0;  // HEIGHT (rows/depth)
    private int width = 0;   // WIDTH (columns/paths)
    
    public Map(int inWidth, int inHeight) 
    {
        width = inWidth;
        height = inHeight;
        
        GenerateMap();
    }
    
    void GenerateMap()
    {
        grid = new Nodes[height, width];
        int idCounter = 0;
        int middleColumn = width / 2;
        int interval = (height - 1) / 2;

        int startingBudget = 50; //this will definitely be in a scriptable object at some point
        
        float[] fightChance = new float[width];
        float[] baseFightChance = new float[width];

        for (int w = 0; w < width; w++)
        {
            baseFightChance[w] = Random.Range(60f, 80f) / 100; //hardcoded for now but will make it a scriptable object if I make more then a prototype
            fightChance[w] = baseFightChance[w];
        }

        startNode = CreateNode(idCounter++, 0, 1, NodeType.Fight, startingBudget);
        grid[0, 1] = startNode;
        allNodes.Add(startNode);
        
        for (int h = 1; h < height - 1; h++)
        {
            bool isMiddleOnlyRow = (h % interval == 0);

            for (int w = 0; w < width; w++)
            {
                if (isMiddleOnlyRow && w != middleColumn)
                    continue;
                
                NodeType type;
                float roll = Random.Range(0f, 1f);

                if (roll < fightChance[w])
                {
                    type = NodeType.Fight;
                    fightChance[w] = baseFightChance[w]; 
                }
                else
                {
                    type = NodeType.Merchant; 
                    fightChance[w] *= 0.85f; //future scriptable object value
                }

                Nodes newNode = CreateNode(idCounter++, h, w, type, startingBudget + (5 * h));
                grid[h, w] = newNode;
                allNodes.Add(newNode);
            }
        }
        
        endNode = CreateNode(idCounter++, height - 1,1, NodeType.Boss, startingBudget + (5 * height));
        grid[height - 1, 1] = endNode;
        allNodes.Add(endNode);
        
        for (int h = 1; h < height - 1; h++)
        {
            bool isMiddleOnlyRow = (h % interval == 0);

            for (int w = 0; w < width; w++)
            {
                //previous nodes
                Nodes currentNode = grid[h, w];
                Nodes previousNode = grid[h -1, w];
                if(currentNode == null || currentNode == startNode || currentNode == endNode) continue;
                
                if (isMiddleOnlyRow && w == middleColumn)
                {
                    currentNode.AddPreviousNode(grid[h - 1, w]);
                    currentNode.AddPreviousNode(grid[h - 1, w - 1]);
                    currentNode.AddPreviousNode(grid[h - 1, w + 1]);
                    grid[h - 1, w].AddNextNode(currentNode);
                    grid[h - 1, w - 1].AddNextNode(currentNode);
                    grid[h - 1, w + 1].AddNextNode(currentNode);
                    
                    grid[h + 1, w].AddPreviousNode(currentNode);
                    grid[h + 1, w - 1].AddPreviousNode(currentNode);
                    grid[h + 1, w + 1].AddPreviousNode(currentNode);
                }
                else if(previousNode != null)
                {
                    currentNode.AddPreviousNode(previousNode);
                }
                
                //next nodes
                Nodes nextNode = grid[h +1, w];
                if(currentNode == null || currentNode == startNode || currentNode == endNode) continue;
                
                if (isMiddleOnlyRow && w == middleColumn)
                {
                    currentNode.AddNextNode(grid[h + 1, w]);
                    currentNode.AddNextNode(grid[h + 1, w - 1]);
                    currentNode.AddNextNode(grid[h + 1, w + 1]);
                }
                else if(nextNode != null)
                {
                    currentNode.AddNextNode(nextNode);
                }
            }
        }

        
        startNode.AddNextNode(grid[1, 0]);
        startNode.AddNextNode(grid[1, 1]);
        startNode.AddNextNode(grid[1, 2]);

        grid[1, 0].AddPreviousNode(startNode);
        grid[1, 1].AddPreviousNode(startNode);
        grid[1, 2].AddPreviousNode(startNode);
        
        grid[height - 2, 0].AddNextNode(endNode);
        grid[height - 2, 1].AddNextNode(endNode);
        grid[height - 2, 2].AddNextNode(endNode);

        endNode.AddPreviousNode(grid[height - 2, 0]);
        endNode.AddPreviousNode(grid[height - 2, 1]);
        endNode.AddPreviousNode(grid[height - 2, 2]);
        
        startNode.ChangeState(NodeState.Available);
    }

    Nodes CreateNode(int id, int depth, int path, NodeType nodeType, int MoneyPerNode)
    {
        switch (nodeType)
        {
            case NodeType.Fight:
                return new BattleNode(id,depth, path, MoneyPerNode);
                break;
            case NodeType.Boss:
                return new BossNode(id,depth,path,MoneyPerNode);
                break;
            case NodeType.Merchant:
                return new MerchantNode(id,depth,path,MoneyPerNode);
                break;
        }
        return null;
    }
}