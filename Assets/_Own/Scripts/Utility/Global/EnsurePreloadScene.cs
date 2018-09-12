using UnityEngine;
using UnityEngine.SceneManagement;

/// Ensures the "__preload" scene has been loaded.
/// Put on vital GameObjects such as the player to make it easy.
public class EnsurePreloadScene : MonoBehaviour
{
    private static bool wasPreloadLoaded;

    void Awake()
    {
        if (GameObject.Find("__app")) return;
        SceneManager.LoadScene(SceneNames.preload, LoadSceneMode.Additive);
    }
}