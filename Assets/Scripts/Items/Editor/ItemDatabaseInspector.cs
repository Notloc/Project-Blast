using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseInspector : Editor
{
    private ItemDatabase database;
    private ItemMap registeredItems;
    private StringMap nameMap;

    private void OnEnable()
    {
        database = (ItemDatabase)target;
        registeredItems = database.GetItemMap();
        nameMap = database.GetNameMap();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Update Database"))
        {
            database.UpdateDatabase();
            registeredItems = database.GetItemMap();
            nameMap = database.GetNameMap();
        }

        base.OnInspectorGUI();

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
}
