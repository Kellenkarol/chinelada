using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TRAJECTORY PROJECTION WHILE THROWING AN OBJECT
//Needs a Shape with a Collider2D, Rigidbody2D, LineRenderer
//In order to measure MousePosition easily in 2D, Camera needs to be set to Orthographic

//TLDR: Calculate Physics, then assign these points to a LineRenderer for display

//Currently does not account for 3-D, Drag, Gravity Scale, Bounces, Ground Velocity, and Existing Velocity (only launch when standing still)

public class ChinelaFoguete : Chinela
{

    [HideInInspector] public float        MaxGas;
    [HideInInspector] public float        GasForce = 1;
    
    private Image _img; // imagem do botão gás
	private float CurrentGas;
    private Vector2 GasDir = Vector2.right; // direção da força do gás
	// private Vector2 GasDir = new Vector2(1,0); // direção da força do gás

    void Awake()
    {
        // _startPos = transform.position; // _startPos está sendo iniciado em 'ChinelaControle.cs'
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
        _rb.mass = _mass;
        CurrentGas = MaxGas;
        _throwed = false;
    }

    void Start()
    {

    }

    void Update()
    {
		// Throwed(); 
    }


    // usa o gás e faz a chinela se mover apenas para direita
    public override void UseGas(Image img)
    {
        _img               = img;
    	float fill         = CalculateGasFill();
    	img.fillAmount     = fill;

    	if(fill > 0)
    	{
	    	_rb.velocity = GasDir*GasForce;
    	}
    }


    // retorna o gás restante entre 0 e 1
    private float CalculateGasFill()
    {
    	CurrentGas -= Time.deltaTime;
    	return CurrentGas / MaxGas;
    }

    // reseta o gás para seu valor inicial
    public void ResetGas()
    {
    	CurrentGas = MaxGas;
    }

    // reseta a quantidade de gás para mostrar em 100%
    public void ResetFill()
    {
        _img.fillAmount = 1;
    }


    public override void ResetChinela()
    {
        ResetGas();
        if(_img)
            ResetFill();
        StartCoroutine(Reset(0));
    }


}