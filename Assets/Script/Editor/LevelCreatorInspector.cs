// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(LevelCreator))]
// [ExecuteInEditMode]
// public class LevelCreatorInspector : Editor
// {
//     Dictionary<ElementTypes, Texture> textureHolder = new Dictionary<ElementTypes, Texture>();
//     private void OnEnable()
//     {
//         textureHolder.Add(ElementTypes.Empty, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/empty.png"));
//         textureHolder.Add(ElementTypes.Baba, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/baba.png"));
//         textureHolder.Add(ElementTypes.Wall, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/wall.png"));
//         textureHolder.Add(ElementTypes.Rock, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/rock.png"));
//         textureHolder.Add(ElementTypes.Flag, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/flag.png"));
//         textureHolder.Add(ElementTypes.Goop, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/goop.png"));
//         textureHolder.Add(ElementTypes.IsWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/IsWord.png"));
//         textureHolder.Add(ElementTypes.BabaWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/BabaWord.png"));
//         textureHolder.Add(ElementTypes.WallWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/wallword.png"));
//         textureHolder.Add(ElementTypes.FlagWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/flagword.png"));
//         textureHolder.Add(ElementTypes.RockWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/rockword.png"));
//         textureHolder.Add(ElementTypes.YouWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/youword.png"));
//         textureHolder.Add(ElementTypes.PushWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/pushword.png"));
//         textureHolder.Add(ElementTypes.WinWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/winword.png"));
//         textureHolder.Add(ElementTypes.StopWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/stopword.png"));
//         textureHolder.Add(ElementTypes.GoopWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/goopword.png"));
//         textureHolder.Add(ElementTypes.SinkWord, (Texture)EditorGUIUtility.Load("Assets/EditorDefaultResources/sinkword.png"));
//     }
//     ElementTypes currentSelected = ElementTypes.Empty;
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         GUILayout.Label("Current Selected : " + currentSelected.ToString());

//         LevelCreator levelCreator = (LevelCreator)target;
//         int rows = (int)Mathf.Sqrt(levelCreator.level.Count);
//         GUILayout.BeginVertical();
//         for(int r = rows-1; r >=0; r--)
//         {

//             GUILayout.BeginHorizontal();
//             for(int c = 0; c < rows; c++)
//             {
//                 if (GUILayout.Button(textureHolder[levelCreator.level[c+((rows)*r)]],GUILayout.Width(50),GUILayout.Height(50)))
//                 {
//                     levelCreator.level[c + ((rows) * r)] = currentSelected;
//                 }
//             }
//             GUILayout.EndHorizontal();
//         }
//         GUILayout.EndVertical();

//         GUILayout.Space(20);
//         GUILayout.BeginVertical();
//         GUILayout.BeginHorizontal();
//         int count = 0;
//         foreach(KeyValuePair<ElementTypes,Texture> e in textureHolder)
//         {
//             count++;
//             if (GUILayout.Button(e.Value, GUILayout.Width(50), GUILayout.Height(50)))
//             {
//                 currentSelected = e.Key;
//             }
//             if (count % 4 == 0)
//             {
//                 GUILayout.EndHorizontal();
//                 GUILayout.BeginHorizontal();
//             }
//         }
        
//         GUILayout.EndVertical();
//     }
// }