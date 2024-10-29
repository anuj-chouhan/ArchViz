using UnityEngine;
using NaughtyAttributes;

public class FPSManager : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = targetFrameRate;
    }

#if UNITY_EDITOR

    [SerializeField] private bool displayerFps = true;
    [ShowIf("displayerFps")][SerializeField] private int textSize = 30;
    private float deltaTime = 0.0f;



    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        if (displayerFps)
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = (h * 2 / 100) + textSize;
            style.normal.textColor = Color.white;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }

#endif
}
