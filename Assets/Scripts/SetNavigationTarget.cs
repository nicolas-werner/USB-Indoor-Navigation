using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
public class SetNavigationTarget : MonoBehaviour
{
 
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;

    private bool lineToggle = false;

    private void Start() {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }


    private void Update() {
        if (lineToggle && targetPosition != Vector3.zero) {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            // Calculatea and prints the length of the path
            float pathLength = CalculatePathLength(path);
            Debug.Log("Path Length: " + pathLength);
        }
    }


    public void SetCurrentNavigationTarget(int selectedValue) {
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null) {
            targetPosition = currentTarget.PositionObject.transform.position;
        }
    }

    public void ToggleVisibility() {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }


float CalculatePathLength(NavMeshPath path)
{
    float length = 0.0f;
    if (path.corners.Length < 2) // Not enough points to form a path
        return length;

    for (int i = 0; i < path.corners.Length - 1; i++)
    {
        length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
    }

    return length;
}
}