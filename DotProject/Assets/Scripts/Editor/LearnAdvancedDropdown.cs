using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System;

class WeekdaysDropdown : AdvancedDropdown
{
    public Action<string> OnItemSelectedAction = null;
    public WeekdaysDropdown(AdvancedDropdownState state) : base(state)
    {
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("Weekdays");

        var firstHalf = new AdvancedDropdownItem("First half");
        var secondHalf = new AdvancedDropdownItem("Second half");
        var weekend = new AdvancedDropdownItem("Weekend");

        firstHalf.AddChild(new AdvancedDropdownItem("Monday"));
        firstHalf.AddChild(new AdvancedDropdownItem("Tuesday"));
        secondHalf.AddChild(new AdvancedDropdownItem("Wednesday"));
        secondHalf.AddChild(new AdvancedDropdownItem("Thursday"));
        weekend.AddChild(new AdvancedDropdownItem("Friday"));
        weekend.AddChild(new AdvancedDropdownItem("Saturday"));
        weekend.AddChild(new AdvancedDropdownItem("Sunday"));

        root.AddChild(firstHalf);
        root.AddSeparator();
        root.AddChild(secondHalf);
        root.AddChild(weekend);

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        base.ItemSelected(item);
        OnItemSelectedAction?.Invoke(item.name);
    }
}

public class AdvancedDropdownTestWindow : EditorWindow
{
    [MenuItem("Test/Open AdvancedDropdown")]
    static void ShowWin()
    {
        EditorWindow.GetWindow<AdvancedDropdownTestWindow>().Show();
    }

    private string selectedItemName = "Show";

    void OnGUI()
    {
        var rect = GUILayoutUtility.GetRect(new GUIContent("Show"), EditorStyles.toolbarButton);
        if (GUI.Button(rect, new GUIContent("Show"), EditorStyles.toolbarButton))
        {
            var dropdown = new WeekdaysDropdown(new AdvancedDropdownState());
            dropdown.OnItemSelectedAction = (name) =>
            {
                selectedItemName = name;
            };
            dropdown.Show(rect);
        }
        EditorGUILayout.LabelField(selectedItemName);
    }
}