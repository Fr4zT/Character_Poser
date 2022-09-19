using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3 ToVector(this SavedVector value)
    {
        return new Vector3(value.x, value.y, value.z);
    }

    public static void OpenFolder(string folder)
    {
        var itemPath = folder.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/open," + itemPath);
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector3 ToWorld(this Vector3 mousePos)
    {
        var screenPoint = mousePos;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
