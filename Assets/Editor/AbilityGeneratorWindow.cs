using UnityEditor;
using UnityEngine;
using System.IO;

public class AbilityGeneratorWindow : EditorWindow
{
    private string abilityName = "Example";
    private AbilityType abilityType = AbilityType.OnAttack;
    private string outputPath = "Assets/Script/AbilitySystem";
    
    private bool hasDistanceConditions = true;
    private bool hasCustomScoring = false;

    [MenuItem("Tools/Ability System/Generate Ability")]
    public static void OpenWindow()
    {
        GetWindow<AbilityGeneratorWindow>("Ability Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Ability Generator", EditorStyles.boldLabel);
        GUILayout.Space(5);

        abilityName = EditorGUILayout.TextField("Ability Name", abilityName);
        abilityType = (AbilityType)EditorGUILayout.EnumPopup("Ability Type", abilityType);
        
        GUILayout.Space(10);
        GUILayout.Label("Options", EditorStyles.boldLabel);
        
        hasDistanceConditions = EditorGUILayout.Toggle("Distance Conditions", hasDistanceConditions);
        hasCustomScoring = EditorGUILayout.Toggle("Custom Scoring Logic", hasCustomScoring);
        
        GUILayout.Space(10);
        
        outputPath = EditorGUILayout.TextField("Output Folder", outputPath);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Ability"))
        {
            GenerateAbility();
        }
    }

    private void GenerateAbility()
    {
        if (string.IsNullOrWhiteSpace(abilityName))
        {
            Debug.LogError("Ability name is empty.");
            return;
        }

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string templateClassName = $"{abilityName}Template";
        string instanceClassName = $"{abilityName}Instance";

        string templatePath = Path.Combine(outputPath, $"{templateClassName}.cs");
        string instancePath = Path.Combine(outputPath, $"{instanceClassName}.cs");

        if (File.Exists(templatePath) || File.Exists(instancePath))
        {
            Debug.LogError("Ability already exists.");
            return;
        }

        File.WriteAllText(templatePath, GenerateTemplateScript(abilityName));
        File.WriteAllText(instancePath, GenerateInstanceScript(abilityName));

        AssetDatabase.Refresh();
        Debug.Log($"Ability '{abilityName}' created successfully at {outputPath}");
    }

    private string GenerateTemplateScript(string name)
    {
        return
$@"using UnityEngine;

[CreateAssetMenu(menuName = ""Abilities/{name}"")]
public class {name}Template : AbilityTemplate
{{
    public override AbilityInstance CreateInstance()
    {{
        return new {name}Instance(this);
    }}
}}";
    }

    private string GenerateInstanceScript(string name)
    {
        string calculateScoreMethod = hasCustomScoring ? GenerateCustomScoring() : GenerateBasicScoring();
        
        return
$@"using UnityEngine;

[System.Serializable]
public class {name}Instance : AbilityInstance
{{
    public {name}Instance({name}Template template) : base(template)
    {{
    }}
    
{calculateScoreMethod}

    public override bool Execute(Unit unit, GameObject target)
    {{
        if (!base.Execute(unit, target))
            return false;
            
        // {name} logic here
        Debug.Log($""Executing {name} on {{target.name}}"");
        
        return true;
    }}

    public override void OnAbilityEnd(Unit unit)
    {{
        base.OnAbilityEnd(unit);
        Debug.Log(""{name} ended"");
    }}
}}";
    }

    private string GenerateBasicScoring()
    {
        if (!hasDistanceConditions)
        {
            return
@"    public override float CalculateScore(Unit unit, GameObject target)
    {
        if (IsOnCooldown()) 
            return 0;
        
        float score = priority;
        score += (lastUsedTime * 0.25f);
        
        return score;
    }";
        }
        
        return
@"    public override float CalculateScore(Unit unit, GameObject target)
    {
        if (IsOnCooldown()) 
            return 0;

        float distance = Vector3.Distance(unit.transform.position, target.transform.position);
    
        if (distance < minDistanceToTarget) 
            return 0;
        if (distance > maxDistanceToTarget) 
            return 0;
    
        float score = priority;
        score += (lastUsedTime * 0.25f);
        score += distance;
        
        return score;
    }";
    }

    private string GenerateCustomScoring()
    {
        string baseScoring = hasDistanceConditions ? 
@"        float distance = Vector3.Distance(unit.transform.position, target.transform.position);
    
        if (distance < minDistanceToTarget) 
            return 0;
        if (distance > maxDistanceToTarget) 
            return 0;
    
        float score = priority;
        score += (lastUsedTime * 0.25f);
        score += distance;
        
        // TODO: Add custom scoring logic here
        // Example: Bonus for specific distance ranges
        // if (distance > 5f)
        //     score += (distance - 5f) * 2f;
        " :
@"        float score = priority;
        score += (lastUsedTime * 0.25f);
        
        // TODO: Add custom scoring logic here
        ";

        return
$@"    public override float CalculateScore(Unit unit, GameObject target)
    {{
        if (IsOnCooldown()) 
            return 0;

{baseScoring}
        return score;
    }}";
    }
}