using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        Level0,
        Level1,
        Level2,
        MainMenu,
        Credits,
        LoadingScene
    }

    private static Action onLoaderCallback;


    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
