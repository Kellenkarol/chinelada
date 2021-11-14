using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBar : MonoBehaviour
{
	public float speed, minForce, maxForce;
	public bool startMovement, stopMovement, getForce; // apenas para teste
	public static ForceBar Instance; 

	private Transform indicator;
	private bool isMoving, movingRight=true;
	private float _min, _max;
	private float indicatorPosition;
	

    // Start is called before the first frame update
    void Start()
    {
    	Instance 		= this;
        // print("instance "+(instance));
        indicator 		= transform.GetChild(0).transform;
        _min 			= transform.GetChild(1).gameObject.transform.position.x; // limite da esqueda
        _max 			= transform.GetChild(2).gameObject.transform.position.x; // limite da direita
        StartMovement();
    }

    // Update is called once per frame
    void Update()
    {
        // Start Test ----------------------------
    	if(startMovement)
    	{
	        StartMovement();
    		startMovement = false;
    	}

    	else if(stopMovement)
    	{
	        StopMovement();
    		stopMovement = false;
    	}

    	else if(getForce)
    	{
	        print("Force: "+GetForce());
    		getForce = false;
    	}
        // End Test ------------------------------


    	if(isMoving)
    	{
    		MoveIndicator();	
    	}
    }


    public void StartMovement()
    {
    	indicator.gameObject.SetActive(true);
    	RandomPositionAndDirection();
    	isMoving = true;
    }


    public void StopMovement()
    {
    	isMoving = false;
    	indicator.gameObject.SetActive(false);
    }


    // retorna a força real
    public float GetForce()
    {
    	float aux = (indicatorPosition-_min)/(_max-_min), forceAux;

    	if(aux >= 0.5f)
    	{
    		forceAux = (1-aux)/0.5f;
    	}
    	else
    	{
    		forceAux = aux/0.5f;
    	}

    	return forceAux*(maxForce-minForce)+minForce;
    }


    // retorna a força em porcentagem
    public float GetForceInPercent()
    {
        float f         = GetForce() - minForce; // tirar a força mínima para fazer a amplitude da força começar do 0
        float percent   = f * 100 / (maxForce - minForce); // calcula a porcentagem
        return percent;
    }


    // gera a posição e a direção do 'indicator'
    private void RandomPositionAndDirection()
    {
        Vector3 oldPos      = indicator.position;
        oldPos.x            = Random.Range(_min, _max); // gera uma posição no eixo x
    	indicator.position  = oldPos; 
    	int aux             = (int)(Random.Range(0, 2)); // auxiliar para a direção inicial
    	movingRight         = new bool[]{true, false}[aux == 2 ? 1 : aux]; // define a direção inicial
    }


    // calcula o movimento do 'indicator'
    private void MoveIndicator()
    {
    	indicatorPosition = indicator.position.x + (movingRight ? +speed : -speed) * Time.deltaTime * transform.localScale.x;

		if(indicatorPosition >= _max || indicatorPosition <= _min)
		{
			indicatorPosition    = indicatorPosition >= _max ? _max : _min;
			movingRight          = !movingRight;
		}

        Vector3 oldPos     = indicator.position;
		oldPos.x           = indicatorPosition; // nova posição
        indicator.position = oldPos;

    }
}
