using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Tony
{
    public static class Helper
    {

        /* Author: Anthony D'Alesandro
            * 
            * Quits the application if neccesary.
            */
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /* Author: Anthony D'Alesandro
            * 
            * Load scene of string name
            */
        public static void LoadScene(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }

        /* Author: Anthony D'Alesandro
            * 
            * Try get scripts of type T
            */
        public static bool TryGetScripts<T>(out T[] matches, out int count)
        {
            T[] foundScripts = GameObject.FindObjectsOfType(typeof(T), true) as T[];

            if (foundScripts != null)
            {
                count = foundScripts.Length;
                matches = foundScripts;
                return true;
            }
            else
            {
                count = 0;
                matches = null;
                return false;
            }
        }

        /* Author: Anthony D'Alesandro
            * 
            * Gets items from assets folder of type T
            */
        public static T[] GetAssets<T>() where T : ScriptableObject
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] references = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                references[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return references;
#else
            return null;
#endif
        }
    }
}


