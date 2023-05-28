using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGen))]
public class MazeGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeGen mazeGen = (MazeGen)target;

        if (GUILayout.Button("Generate"))
        {
            mazeGen.Generate();
        }
    }
}