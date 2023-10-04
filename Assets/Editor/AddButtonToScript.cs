using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Machia.Input;

public abstract class ButtonAdder : Editor
{
    protected List<Tuple<string, Action>> buttonActions = new List<Tuple<string, Action>>();

    public override void OnInspectorGUI()
    {
        foreach (var buttonAction in buttonActions)
        {
            if (GUILayout.Button(buttonAction.Item1))
            {
                buttonAction.Item2?.Invoke();
            }
        }
        DrawDefaultInspector();
    }

    private void OnEnable() { InitializeButtons(); }

    protected abstract void InitializeButtons();

    protected void AddButton(string buttonText, Action buttonAction)
    {
        buttonActions.Add(new Tuple<string, Action>(buttonText, buttonAction));
    }

}

[CustomEditor(typeof(PlayerConnector))]
public class AddButtonToConnector : ButtonAdder
{
    protected override void InitializeButtons()
    {
        AddButton("Debug", Debug);
    }

    private void Debug()
    {
        UnityEngine.Debug.Log("Hello World!");
    }
}


