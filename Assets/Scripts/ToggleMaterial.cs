using UnityEngine;

public class ToggleMaterial : MonoBehaviour
{
    public Material[] materials; // Array to hold the materials.
    private int currentMaterialIndex = 0; // Track the current material.

    // Function to toggle the material.
    public void ChangeMaterial()
    {
        currentMaterialIndex++; // Move to the next material in the array.
        if (currentMaterialIndex >= materials.Length) currentMaterialIndex = 0; // Loop back to the first material if at the end.

        GetComponent<Renderer>().material = materials[currentMaterialIndex]; // Apply the new material.
    }
}
