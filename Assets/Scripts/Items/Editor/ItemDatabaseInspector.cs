using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseInspector : Editor
{
    private ItemDatabase database;
    private ItemMap registeredItems;
    private StringMap missingItems;

    private void OnEnable()
    {
        database = (ItemDatabase)target;
        registeredItems = database.GetItemMap();
        missingItems = database.GetMissingMap();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Update Database"))
        {
            database.UpdateDatabase();
            registeredItems = database.GetItemMap();
            missingItems = database.GetMissingMap();
        }

        base.OnInspectorGUI();

        DrawItems();
        DrawMissingItems();
    }

    private void DrawItems()
    {
        EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
        GUILayout.BeginVertical("box");
        {
            foreach (var pair in registeredItems)
            {
                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label(pair.Key.ToString());
                    GUILayout.Label(pair.Value.GetName());
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawMissingItems()
    {
        if (missingItems.Count > 0)
        {
            EditorGUILayout.LabelField("Missing Items", EditorStyles.boldLabel);
            GUILayout.BeginVertical("box");
            {
                foreach (var pair in missingItems)
                {
                    GUILayout.BeginHorizontal("box");
                    {
                        GUILayout.Label(pair.Value);
                        GUILayout.Label("Id was: "+pair.Key.ToString());
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
    }
}
