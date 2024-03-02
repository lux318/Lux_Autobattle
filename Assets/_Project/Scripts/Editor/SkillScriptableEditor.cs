using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillScriptable))]
public class SkillScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkillScriptable skill = target as SkillScriptable;

        skill.skillName = EditorGUILayout.TextField("Skill name: ", skill.skillName);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("When");
        skill.skillTrigger = (When)EditorGUILayout.EnumPopup("Skill trigger: ", skill.skillTrigger);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target");
        skill.skillTarget = (Target)EditorGUILayout.EnumPopup("Skill target: ", skill.skillTarget);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("What");
        skill.skillEffect = (What)EditorGUILayout.EnumPopup("Skill effect: ", skill.skillEffect);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("How Much");
        skill.type = (HowMuchType)EditorGUILayout.EnumPopup("How much type: ", skill.type);

        if (skill.type == HowMuchType.dice)
        {
            skill.dice = (Dices)EditorGUILayout.EnumPopup("Dice: ", skill.dice);
            skill.dicesNumber = EditorGUILayout.IntField("Diced number: ", skill.dicesNumber);
        }
        if (skill.type == HowMuchType.percentage)
        {
            skill.percentageValue = EditorGUILayout.IntField("Percentage: ", skill.percentageValue);
        }
        if (skill.type == HowMuchType.value)
        {
            skill.value = EditorGUILayout.FloatField("value: ", skill.value);
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("How Long");
        skill.skillRoundDuration = EditorGUILayout.IntField("Round duration: ", skill.skillRoundDuration);


        EditorUtility.SetDirty(skill);
        serializedObject.ApplyModifiedProperties();
        PrefabUtility.RecordPrefabInstancePropertyModifications(skill);
    }
}
