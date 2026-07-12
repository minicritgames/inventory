using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Minikit.Inventory.Internal;

namespace Minikit.Inventory.Editor
{
    [CustomEditor(typeof(MKInventoryComponent))]
    public class MKInventoryComponentEditor : UnityEditor.Editor
    {
        static int bagTypeOption = 0;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MKInventoryComponent inventoryComponent = target as MKInventoryComponent;

            EditorGUILayout.Space(20f);

            List<Type> nativeBagTypes = MKBagReflector.GetNativelyDefinedBagTypes();
            List<string> nativeBagNames = new();
            if (nativeBagTypes.Count == 0)
            {
                nativeBagNames.Add("Native Bags Not Found!");
            }
            else
            {
                foreach (Type nativeBagType in nativeBagTypes)
                {
                    nativeBagNames.Add(nativeBagType.Name);
                }
            }

            bagTypeOption = EditorGUILayout.Popup("Bag Type:", bagTypeOption, nativeBagNames.ToArray());
            if (GUILayout.Button($"Add {nativeBagNames[bagTypeOption]}"))
            {
                Type bagType = nativeBagTypes[bagTypeOption];
                if (bagType == null)
                {
                    Debug.LogError($"Failed to get valid Type from selected Native Bag Type ({nativeBagNames[bagTypeOption]})");
                    return;
                }

                if (Activator.CreateInstance(bagType) is not MKBag bag)
                {
                    Debug.LogError($"Failed to create valid Bag from selected Type ({bagType.Name})");
                    return;
                }

                inventoryComponent.Editor_AddBag(bag);

                EditorUtility.SetDirty(inventoryComponent);
            }
        }
    }
} // Minikit.Inventory.Editor namespace
