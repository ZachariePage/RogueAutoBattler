using UnityEditor;
using UnityEngine;
using System.IO;

public class StateGeneratorWindow : EditorWindow
{
    private string stateName = "Example";
    private string outputPath = "Assets/Script/Units";

    [MenuItem("Tools/State Machine/Generate State")]
    public static void OpenWindow()
    {
        GetWindow<StateGeneratorWindow>("State Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("State Generator", EditorStyles.boldLabel);

        stateName = EditorGUILayout.TextField("State Name", stateName);
        outputPath = EditorGUILayout.TextField("Output Folder", outputPath);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate State"))
        {
            GenerateState();
        }
    }

    private void GenerateState()
    {
        if (string.IsNullOrWhiteSpace(stateName))
        {
            Debug.LogError("State name is empty.");
            return;
        }

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string stateClassName = $"{stateName}State";
        string soClassName = $"{stateName}StateSO";

        string statePath = Path.Combine(outputPath, $"{stateClassName}.cs");
        string soPath = Path.Combine(outputPath, $"{soClassName}.cs");

        if (File.Exists(statePath) || File.Exists(soPath))
        {
            Debug.LogError("State already exists.");
            return;
        }

        File.WriteAllText(statePath, GenerateStateScript(stateName));
        File.WriteAllText(soPath, GenerateStateSOScript(stateName));

        AssetDatabase.Refresh();
        Debug.Log($"State '{stateName}' created successfully.");
    }

    private string GenerateStateScript(string name)
    {
        return
$@"using UnityEngine;

public class {name}State : State
{{
    private {name}StateSO config;
    
    public {name}State(Unit unit, StateMachine stateMachine, {name}StateSO config) 
        : base(unit, stateMachine)
    {{
        this.config = config;
    }}
    
    public override void EnterState()
    {{
        base.EnterState();
    }}

    public override void ExitState()
    {{
        base.ExitState();
    }}

    public override void FrameUpdate()
    {{
        base.FrameUpdate();
    }}
    
    public override void PhysicUpdate()
    {{
        base.PhysicUpdate();
    }}

    public override StateScriptableObject GetConfig()
    {{
        return config;
    }}
}}";
    }

    private string GenerateStateSOScript(string name)
    {
        return
$@"using UnityEngine;

[CreateAssetMenu(menuName = ""State Machine/{name} State"")]
public class {name}StateSO : StateScriptableObject
{{
}}";
    }
}
