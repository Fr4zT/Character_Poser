using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using TMPro;
public class PoseManager : MonoBehaviour
{
    public List<Transform> gizmos = new List<Transform>();
    [SerializeField] Camera thumnailCamera;

    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject poseButtonPrefab;

    public GameObject poseWindow;
    string posesPath;
    string thumbsPath;
    // Start is called before the first frame update
    void Start()
    {
        posesPath = Application.persistentDataPath + "/poses/";
        thumbsPath = Application.persistentDataPath + "/thumbnails/";
        Directory.CreateDirectory(posesPath); // returns a DirectoryInfo object
    }

    public void SavePose(TMP_InputField inputField)
    {
        if (inputField.text == "") return;
        var poseName = inputField.text;
        Pose pose = new Pose(poseName, gizmos);
        BinaryFormatter bf = new BinaryFormatter();
        FileInfo fileRef = new FileInfo(posesPath + poseName + ".frzPose");
        fileRef.Directory.Create();
        FileStream file = File.Create(fileRef.FullName);
        SaveThumbnail(poseName);
        bf.Serialize(file, pose);
        file.Close();
    }

    public void OpenLoadDialog()
    {
        foreach(Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }
        var info = new DirectoryInfo(posesPath);
        var fileInfo = info.GetFiles("*.frzPose");
        foreach (var file in fileInfo)
        {
            var newButton = Instantiate(poseButtonPrefab, scrollContent);
            var button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => LoadPose(file));
            button.onClick.AddListener(() => poseWindow.SetActive(false));
            var fileName = file.Name.Replace(".frzPose", "");
            var img = LoadImage(fileName);
            Sprite sprite = Sprite.Create(img, new Rect(0.0f, 0.0f, img.width, img.height), new Vector2(0.5f, 0.5f));
            newButton.GetComponent<Image>().sprite = sprite;
            newButton.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = fileName;
        }
    }

    Texture2D LoadImage(string name)
    {
        Texture2D tex = null;
        byte[] fileData;
        var path = thumbsPath + name + ".jpg";
        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public void LoadPose(FileInfo fileInfo)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(fileInfo.FullName, FileMode.Open);
        Pose loadedPose = (Pose)bf.Deserialize(file);
        file.Close();
        for(int i=0;i<loadedPose.savedVectors.Count;i++)
        {
            gizmos[i].localEulerAngles = loadedPose.savedVectors[i].ToVector();
        }
    }

    void SaveThumbnail(string thumbnailName)
    {
        var width = 384;
        var height = 216;
        var renderTexture = new RenderTexture(width, height, 16);
        var texture2D = new Texture2D(width, height);

        var target = thumnailCamera.targetTexture;
        thumnailCamera.targetTexture = renderTexture;
        thumnailCamera.Render();
        thumnailCamera.targetTexture = target;

        var active = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        RenderTexture.active = active;

        texture2D.Apply();
        FileInfo file = new FileInfo(thumbsPath + thumbnailName + ".jpg");
        file.Directory.Create();
        File.WriteAllBytes(file.FullName, texture2D.EncodeToJPG());
    }

    public void OpenFolder()
    {
        Extensions.OpenFolder(posesPath);
    }
}

[System.Serializable]
public class Pose
{
    public string poseName;
    public List<SavedVector> savedVectors = new List<SavedVector>();
    public Pose(string n, List<Vector3> vectors)
    {
        poseName = n;
        foreach (var vector in vectors)
        {
            savedVectors.Add(new SavedVector(vector));
        }
    }

    public Pose() { }

    public Pose(string n, List<Transform> transforms)
    {
        poseName = n;
        foreach (var transform in transforms)
        {
            savedVectors.Add(new SavedVector(transform.localEulerAngles));
        }
    }
}

[System.Serializable]
public struct SavedVector
{
    public float x;
    public float y;
    public float z;
    public SavedVector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SavedVector(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }
}


