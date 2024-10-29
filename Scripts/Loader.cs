using System;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private static Action onLoaderCallBack;

    public enum Scenes //All the scenes names
    {
        Interior,
        Exterior,
        MainMenu,
        Loading
    }

    //Method to load scenes
    public static void Load(Scenes scene)
    {
        SceneManager.LoadScene(Scenes.Loading.ToString());
        onLoaderCallBack = () => SceneManager.LoadScene(scene.ToString());
    }

    public static void LoaderCallBack()
    {
        if (onLoaderCallBack != null)
        {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }
    }
}
