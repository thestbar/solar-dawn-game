using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToObject : MonoBehaviour
{
    public Material mat;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = mat;
    }
}
