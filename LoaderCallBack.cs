using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    //For Loading Screen
    private bool isFirstUpdate = true;
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallBack();
        }
    }
}
