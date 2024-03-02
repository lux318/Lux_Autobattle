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
        skill.hmType = (HowMuchType)EditorGUILayout.EnumPopup("How much type: ", skill.hmType);

        if (skill.hmType == HowMuchType.Dice)
        {
            skill.diceMaxValue = EditorGUILayout.IntField("Dice max value: ", skill.diceMaxValue);
            skill.dicesNumber = EditorGUILayout.IntField("Dice number: ", skill.dicesNumber);
        }
        if (skill.hmType == HowMuchType.Percentage)
        {
            skill.percentageValue = EditorGUILayout.IntField("Percentage: ", skill.percentageValue);
        }
        if (skill.hmType == HowMuchType.Value)
        {
            skill.value = EditorGUILayout.FloatField("value: ", skill.value);
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("How Long");
        skill.dType = (DurationType)EditorGUILayout.EnumPopup("How much type: ", skill.dType);

        if (skill.dType == DurationType.Round)
            skill.skillRoundDuration = EditorGUILayout.IntField("Round duration: ", skill.skillRoundDuration);


        EditorUtility.SetDirty(skill);
        serializedObject.ApplyModifiedProperties();
        PrefabUtility.RecordPrefabInstancePropertyModifications(skill);
    }
}
