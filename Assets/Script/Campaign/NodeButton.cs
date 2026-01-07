using UnityEngine;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    [SerializeField]
    public Nodes node;
    private Image image;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        switch (node.type)
        {
            case NodeType.Merchant:
                image.color = Color.green;
                break;
            case NodeType.Fight:
                image.color = Color.blue;
                break;
            case NodeType.Boss:
                image.color = Color.red;
                break;
        } 
        
        switch (node.state)
        {
            case NodeState.Available:
                button.interactable = true;
                break;
            case NodeState.Completed:
                button.interactable = false;
                break;
            case NodeState.Locked:
                button.interactable = false;
                break;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        node.Execute();
    }
}
