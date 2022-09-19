using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UsefulFunctions
{
    public static float minX;
    public static float maxX;

    public static void SetBounds()
    {
        minX = new Vector3(80, 0, 0).ToWorld().x;
        maxX = new Vector3(Screen.width-80, 0, 0).ToWorld().x;
    }

    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
