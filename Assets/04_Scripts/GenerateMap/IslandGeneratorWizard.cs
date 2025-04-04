// using UnityEditor;
// using UnityEngine;

// public class IslandGeneratorWizard : ScriptableWizard
// {
//     public int width = 50;
//     public int height = 50;
//     public Tilemap tilemap;

//     [MenuItem("Tools/Generate Island")]
//     static void CreateWizard()
//     {
//         DisplayWizard<IslandGeneratorWizard>("Generate Island", "Create");
//     }

//     void OnWizardCreate()
//     {
//         GameObject go = new GameObject("IslandGenerator");
//         IslandGenerator generator = go.AddComponent<IslandGenerator>();
//         generator.tilemap = tilemap;
//         generator.width = width;
//         generator.height = height;
//         generator.GenerateIsland();
//     }
// }