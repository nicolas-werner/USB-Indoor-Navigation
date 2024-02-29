using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material material1; // First material.
    public Material material2; // Second material.
    private bool isMaterial1 = true; // Track which material is currently applied.
    private GameObject[] objectsToToggle; // Now private, as it will be set in Start()

    void Start()
    {
        // Automatically find and assign objects on the "Wand" layer
        int layer = LayerMask.NameToLayer("Wand");
        objectsToToggle = FindObjectsOnLayer(layer);
    }

    GameObject[] FindObjectsOnLayer(int layer)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        var layerObjects = new System.Collections.Generic.List<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.layer == layer)
            {
                layerObjects.Add(obj);
            }
        }
        return layerObjects.ToArray();
    }

    // Function to toggle the material for all objects.
    public void ToggleAllMaterials()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = isMaterial1 ? material2 : material1;
            }
        }
        isMaterial1 = !isMaterial1;
    }
}
