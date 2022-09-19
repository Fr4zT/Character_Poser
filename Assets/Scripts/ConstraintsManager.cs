using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;
public class ConstraintsManager : MonoBehaviour
{
    [SerializeField] List<GameObject> gizmoPoints = new List<GameObject>();
    [SerializeField] GameObject gizmoPrefab;
    [SerializeField] RigBuilder rig;
    [SerializeField] PoseManager poseManager;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var point in gizmoPoints)
        {
            var newGizmo = Instantiate(gizmoPrefab);
            newGizmo.transform.position = point.transform.position;
            newGizmo.transform.parent = point.transform.parent;
            point.transform.parent = newGizmo.transform;
            var articulation = point.GetComponent<BodyArticulation>();
            var gizmoSize = articulation.size;
            newGizmo.GetComponent<BoxCollider>().size = new Vector3(gizmoSize * 0.1f, gizmoSize * 0.1f, gizmoSize * 0.1f);
            newGizmo.transform.GetChild(0).transform.localScale = new Vector3(0.1f * gizmoSize, 0.1f * gizmoSize, 0.1f * gizmoSize);
            poseManager.gizmos.Add(newGizmo.transform);
        }
        rig.Build();
    }

    public void ToggleGizmos()
    {
        foreach(var gizmo in poseManager.gizmos)
        {
            gizmo.GetComponent<Gizmo>().Toggle();
        }
    }
}
