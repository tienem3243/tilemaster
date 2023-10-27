using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Rigidbody rb { private set; get; }
    public Collider col { private set; get; }
    public string tileName;
    public SpriteRenderer Sprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<Collider>();
    }



}
