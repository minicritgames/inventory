using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Minikit.Inventory.Internal;

namespace Minikit.Inventory.Editor
{
    [CustomEditor(typeof(MKItemDefinitionScriptableObject))]
    public class MKItemDefinitionEditor : UnityEditor.Editor
    {
        static int shardTypeOption = 0;
        static int shardContainerOption = 0;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MKItemDefinitionScriptableObject itemDefinition = target as MKItemDefinitionScriptableObject;

            EditorGUILayout.Space(20f);

            List<string> nativeShardTypes = MKShardReflector.GetNativelyDefinedShardTypes();
            if (nativeShardTypes.Count == 0)
            {
                nativeShardTypes.Add("Native Shards Not Found!"); // You need to add the [MKNativeShard] attribute to your shards that inherit from MKShard
            }

            List<string> shardContainers = new List<string>() { "Static", "Dynamic" };

            shardTypeOption = EditorGUILayout.Popup("Shard Type:", shardTypeOption, nativeShardTypes.ToArray());
            shardContainerOption = EditorGUILayout.Popup("Shard Container:", shardContainerOption, shardContainers.ToArray());
            if (GUILayout.Button($"Add {shardContainers[shardContainerOption]} {nativeShardTypes[shardTypeOption]}"))
            {
                Type shardType = MKShardReflector.GetRegisteredShardType(nativeShardTypes[shardTypeOption]);
                if (shardType == null)
                {
                    Debug.LogError($"Failed to get valid Type from selected Native Shard Type ({nativeShardTypes[shardTypeOption]})");
                    return;
                }

                MKShard shard = Activator.CreateInstance(shardType) as MKShard;
                if (shard == null)
                {
                    Debug.LogError($"Failed to create valid Shard from selected Type ({shardType.Name})");
                    return;
                }

                if (shardContainerOption == 0)
                {
                    itemDefinition.Editor_AddStaticShard(shard);
                }
                else if (shardContainerOption == 1)
                {
                    itemDefinition.Editor_AddDynamicShard(shard);
                }

                EditorUtility.SetDirty(itemDefinition);
            }
        }
    }
} // Minikit.Inventory.Editor namespace
