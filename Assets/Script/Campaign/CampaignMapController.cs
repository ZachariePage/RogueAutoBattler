using System;
using UnityEngine;
using System.Collections.Generic;

public class CampaignMapController : MonoBehaviour
{
    [SerializeField]
    public Map map;
    int width = 3;
    int height = 7;
    
    public GameObject NodePrefab;
    private void Start()
    {
        map = new Map(width, height);
        CreateUI();
    }

    private void CreateUI()
    {
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        float screenWidth = canvasRect.rect.width;
        float screenHeight = canvasRect.rect.height;

        float xSpacing = screenWidth / (width + 1);
        float ySpacing = screenHeight / (height + 1);

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Nodes nodeData = map.grid[h, w];
                if (nodeData == null) continue;

                GameObject uiNode = Instantiate(NodePrefab, transform);
                RectTransform rt = uiNode.GetComponent<RectTransform>();

                float x = (-screenWidth / 2f) + xSpacing * (w + 1);
                
                int invertedH = height - 1 - h;
                float y = (screenHeight / 2f) - ySpacing * (invertedH + 1);

                rt.anchoredPosition = new Vector2(x, y);

                NodeButton btn = uiNode.GetComponent<NodeButton>();
                btn.node = nodeData;
            }
        }
    }


}