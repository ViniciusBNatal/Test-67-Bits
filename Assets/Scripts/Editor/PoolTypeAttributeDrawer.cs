using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PoolTypeAttribute))]
internal sealed class PoolTypeAttributeDrawer : PropertyDrawer
{
    private static PoolTypeList _poolList;

    private void Initialize()
    {
        _poolList = Resources.LoadAll<PoolTypeList>("ObjectPooling")[0];
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.propertyType == SerializedPropertyType.String
                   ? EditorGUIUtility.singleLineHeight
                   : EditorGUIUtility.singleLineHeight * 2f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_poolList)
            Initialize();

        if (_poolList && property.propertyType == SerializedPropertyType.Integer)
            DrawDropdown(position, property, label);
        else
            DrawPropertyWithWarning(position, property, label);
    }

    private void DrawDropdown(Rect position, SerializedProperty property, GUIContent label)
    {
        int value = property.intValue;
        int selectedIndex = property.intValue < _poolList.PoolTypes.Length ? value: 0;

        EditorGUI.BeginChangeCheck();

        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, _poolList.PoolTypes);

        if (EditorGUI.EndChangeCheck())
            property.intValue = selectedIndex;
    }

    private void DrawPropertyWithWarning(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = base.GetPropertyHeight(property, label);
        if(!_poolList) EditorGUI.HelpBox(position, "Please create the PoolTypeList asset before changing this value", MessageType.Warning);
        else EditorGUI.HelpBox(position, "Pool Type Attribute is only valid for int", MessageType.Warning);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, property, label);
    }
}
