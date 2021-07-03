using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHoverColor : MonoBehaviour
{
    public Renderer wireRenderer;
    public GameObject wire;
    [SerializeField] public Color hoverColor;
    [SerializeField] public Color initialColor;


    // Start is called before the first frame update
    void Start()
    {
        wireRenderer = wire.GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaterial()
    {
        wireRenderer.material.color = hoverColor;
    }

    public void ResetMaterial()
    {
        wireRenderer.material.color = initialColor;
    }
}
