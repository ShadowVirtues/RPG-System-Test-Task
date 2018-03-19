#pragma warning disable 0618

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
#if UNITY_EDITOR

//Saves the scene every time you enter Play Mode

[InitializeOnLoad]
public class Autosave
{
    static Autosave()
    {
        EditorApplication.playmodeStateChanged += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                Debug.Log("Auto-saving all open scenes...");
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        };
    }
}
#endif