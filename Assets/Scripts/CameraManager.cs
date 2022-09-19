using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.IO;
using UnityEngine.UI;
using TMPro;
public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera overlayCamera;
    [SerializeField] Camera renderCamera;
    [SerializeField] Camera uiCamera;
    [SerializeField] List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    [SerializeField] List<KeyCode> cameraKeys = new List<KeyCode>();
    [SerializeField] Canvas canvas;
    [SerializeField] Toggle trasparencyToggle;
    [SerializeField] Toggle folderToggle;
    [SerializeField] TMP_InputField imageName;
    [SerializeField] List<LayerMask> masks;
    string screensPath;
    // Start is called before the first frame update
    void Start()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = uiCamera;
        screensPath = Application.persistentDataPath + "/screenshots/";
    }

    public void SwitchCamera(int n)
    {
        foreach (var camera in cameras)
        {
            camera.Priority = 0;
        }
        cameras[n].Priority = 10;
        Invoke(nameof(CopyMain), 0.1f);
    }

    void CopyMain()
    {
        overlayCamera.Render();
        overlayCamera.orthographic = mainCamera.orthographic;
        overlayCamera.orthographicSize = mainCamera.orthographicSize;
        renderCamera.orthographic = mainCamera.orthographic;
        renderCamera.orthographicSize = mainCamera.orthographicSize;
        uiCamera.orthographic = mainCamera.orthographic;
        uiCamera.orthographicSize = mainCamera.orthographicSize;
    }

    public void SaveImage()
    {
        renderCamera.backgroundColor = trasparencyToggle.isOn ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 1);
        renderCamera.cullingMask = trasparencyToggle.isOn ? masks[0] : masks[1];
        var width = 1920;
        var height = 1080;
        renderCamera.gameObject.SetActive(true);
        CopyMain();
        var renderTexture = new RenderTexture(width, height, 16);
        var texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);

        var target = renderCamera.targetTexture;
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        renderCamera.targetTexture = target;

        var active = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        RenderTexture.active = active;

        texture2D.Apply();
        FileInfo file = new FileInfo(screensPath + imageName.text + ".png");
        file.Directory.Create();
        File.WriteAllBytes(file.FullName, texture2D.EncodeToPNG());
        renderCamera.gameObject.SetActive(false);
        if (folderToggle.isOn) OpenFolder();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPressedKey();
    }

    void CheckPressedKey()
    {
        int i = 0;
        foreach (var key in cameraKeys)
        {
            if (Input.GetKeyDown(key))
            {
                SwitchCamera(i);
            }
            i++;
        }
    }

    public void OpenFolder()
    {
        Extensions.OpenFolder(screensPath);
    }
}
