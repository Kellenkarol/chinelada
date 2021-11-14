using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinelaPadrao : Chinela
{

    void Awake()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
        _rb.mass = _mass;
        _throwed = false;
    }

    void Start()
    {
        // SetAllVariables(); //obsoleto pois já é iniciado em 'ChinelaControle.cs'
    }

    void Update()
    {
        // Throwed();
    }

    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     if(col.gameObject.tag == "Tile")
    //     {
    //         //
    //     }
    // }


}