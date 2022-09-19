using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyArticulation : MonoBehaviour
{
    public float size = 1;
    public enum ArticulationType
    {
        normal,
        root,
    }
    public ArticulationType articulationType;
}
