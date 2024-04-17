using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Utility Component to make Scene loading easy.
/// <br>
/// Provide <see cref="_sceneLoadButtons"/> with UI button references
/// through the Inspector to load a scene by Build Index
/// </summary>
public class SceneSelector : MonoBehaviour
{
    [Serializable]
    public class SceneLoadButton
    {
        public Button Button;
        public int SceneIndex;
    }

    [SerializeField]
    private List<SceneLoadButton> _sceneLoadButtons;

    private void OnEnable()
    {
        EnableAllButtons();
    }

    private void OnDisable()
    {
        DisableAllButtons();
    }

    private void DisableAllButtons()
    {
        foreach (var SceneLoad in _sceneLoadButtons)
        {
            SceneLoad.Button.onClick.RemoveAllListeners();
            SceneLoad.Button.interactable = false;
        }
    }

    private void EnableAllButtons()
    {
        foreach (var SceneLoad in _sceneLoadButtons)
        {
            SceneLoad.Button.onClick.AddListener(()=> LoadSceneAt(SceneLoad.SceneIndex));
            SceneLoad.Button.interactable = true;
        }
    }

    private void LoadSceneAt(int index)
    {
        //Check if scene is loaded first
        var sceneToLoad = SceneManager.GetSceneByBuildIndex(index);
        if(sceneToLoad.isLoaded)
        {
            Debug.LogError($"Scene with Index: {index} is already loaded");
            return;
        }

        //Disable all buttons so no repeated calls to LoadSceneAsync are made
        DisableAllButtons();
        SceneManager.LoadSceneAsync(index);
    }
}
