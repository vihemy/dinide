using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private int numScenes; // number of scenes in build settings
    [SerializeField] private float sceneTransitionDuration = 1.0f;
    [SerializeField] private Color sceneTransitionColor = Color.black;

    public void LoadNextScene()
    {
        numScenes = GetScenesTotalAmount();
        int currentSceneIndex = GetCurrentSceneBuildIndex();
        int nextSceneIndex = currentSceneIndex + 1 < numScenes ? currentSceneIndex + 1 : 0; // finds next scene in index. If no more scenes left, load first scene in index
        string nextSceneName = NameOfSceneByBuildIndex(nextSceneIndex);
        PerformTransition(nextSceneName);
    }

    public void ReloadCurrentScene()
    {
        int currentSceneIndex = GetCurrentSceneBuildIndex();
        string currentSceneName = NameOfSceneByBuildIndex(currentSceneIndex);
        PerformTransition(currentSceneName);
    }

    public void LoadSceneByIndex(int buildIndex)
    {
        string sceneName = NameOfSceneByBuildIndex(buildIndex);
        PerformTransition(sceneName);
    }

    public void LoadSceneByIndexWithoutAnalytics(int buildIndex)
    {
        string sceneName = NameOfSceneByBuildIndex(buildIndex);
        PerformTransition(sceneName);
    }


    public static string NameOfSceneByBuildIndex(int buildIndex) // gets name of scene by build index - needed in order to use PerformTransition (from Ultimate Clean GUI Pack)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    public void PerformTransition(string scene)
    {
        print("Loading scene: " + scene);
        Transition.LoadLevel(scene, sceneTransitionDuration, sceneTransitionColor);
    }

    public int GetScenesTotalAmount()
    {
        if (Application.isEditor) // checks if in editor. Sets numScenes accordingly
        {
            int scenesTotal = SceneManager.sceneCountInBuildSettings;
            return scenesTotal;
        }
        else
        {
            int scenesTotal = SceneManager.sceneCountInBuildSettings;  // get number of scenes in build settings
            return scenesTotal;
        }
    }

    public int GetCurrentSceneBuildIndex()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        return currentBuildIndex;
    }

    public string GetCurrentSceneName()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        return currentSceneName;
    }
}
