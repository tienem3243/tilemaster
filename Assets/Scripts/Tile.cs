using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    public string tileName;
    public SpriteRenderer Sprite;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<Collider>();
    }
  
    public void NomalizeTile()
    {
        rb.useGravity = false;
        col.enabled = false;
        transform.eulerAngles= Vector3.zero;
    }
    
}
