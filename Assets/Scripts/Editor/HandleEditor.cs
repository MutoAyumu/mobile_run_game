using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandlePoint))]
public class HandleEditor : Editor
{
    HandlePoint _handle;

    private void OnEnable()
    {
        _handle = target as HandlePoint;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Add"))
        {
            Undo.RecordObject(target, "Add Node");

            Vector3 pos;

            if (_handle.LocalNodes.Length > 0)
            {
                pos = _handle.LocalNodes[_handle.LocalNodes.Length - 1] + Vector3.right;
            }
            else
            {
                pos = _handle.transform.position + Vector3.right;
            }

            ArrayUtility.Add(ref _handle.LocalNodes, pos);
        }

        EditorGUIUtility.labelWidth = 50;
        int delete = -1;

        for (int i = 0; i < _handle.LocalNodes.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();

            int size = 50;
            EditorGUILayout.BeginVertical(GUILayout.Width(size));
            EditorGUILayout.LabelField("Node " + i, GUILayout.Width(size));

            if (GUILayout.Button("Delete", GUILayout.Width(size)))
            {
                delete = i;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            Vector3 pos;

            pos = EditorGUILayout.Vector3Field("Position", _handle.LocalNodes[i]);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Position");

                _handle.LocalNodes[i] = pos;
            }
        }

        EditorGUIUtility.labelWidth = 0;

        if (delete != -1)
        {
            Undo.RecordObject(target, "Delete Node");

            ArrayUtility.RemoveAt(ref _handle.LocalNodes, delete);
        }
    }
    private void OnSceneGUI()
    {
        for (int i = 0; i < _handle.LocalNodes.Length; i++)
        {
            Vector3 pos;

            pos = _handle.transform.TransformPoint(_handle.LocalNodes[i]);

            Vector3 newPos = pos;

            newPos = Handles.PositionHandle(pos, Quaternion.identity);

            Handles.color = Color.yellow;

            if (newPos != pos)
            {
                Undo.RecordObject(target, "Moved Point");
                _handle.LocalNodes[i] = _handle.transform.InverseTransformPoint(newPos);
            }

            Handles.SphereHandleCap(0, pos, Quaternion.identity, 0.2f, EventType.Repaint);
            Handles.Label(pos, $"Node{i}:{pos - _handle.transform.position}");
        }
    }
}