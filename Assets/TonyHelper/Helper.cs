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
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] references = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                references[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return references;
        }
    }
}

/*
- easy to use abstract finite state machine (general T class that extends from abstract context class?)
    - have a textfile with example code
    - have a abstract script that acts as a Context for a state machine.
- write some abstract scripts that can be used like a singleton script
- Simple SpriteRenderer Animation. Have speed and frames.
- write a better abstract button adding to script thing.
- write the editor helper file with a readme and documentation. Drop the shortcut use.
- write some sort of player connector and singleton class that is also in this package of code.
    - script will have booleans weather or not to clearout singleton.
    - singleton MUST have a reset functionality.
    - player data must be encapsulated into a single object
    - when joining players back in its probably best to assign player ids to keep things consistent. Use encapsulated player controller data, player data etc when joining players in to assign player ids.
        - caveat: check if one of the players somehow dropped out? UNLIKELy but maybe.

- Every Scene should have a scene initializer script that nearly does functional programming. Can initialze every damn script in the awake call the proper way or even the start call. Every script can have an InitFunction and it can be nested in a way ??
- Write a Tony namespace with Helper class that has useful things but the namespace might contain bonus items
    - Can write an abstract singleton so I can make some really quick
    - contain whats currently in my helper
    - can write some sceneInitializer abstract classes
    - Can have my FiniteStateMachine crap in it and it can take a general T type for the Context that way the implementer can do what they want with it.
- sceneInitializer abstract class and I should favor composition over inheritance. With this I can make it by default initialize a FSM and write states for certain scenes. (expecially if making multiple games)
- goonna want multiple contexts with the above note so general T type for FSM is important
- context can contain functions that do common things (disable all player’s inputs)
    - context might also grab all players and store it locally.
    - massive high coupling class but then decouples and simplifies rest of code. Probably wont need to change much either.
*/


