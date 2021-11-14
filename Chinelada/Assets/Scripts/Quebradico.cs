using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quebradico : MonoBehaviour
{

	// ainda não existe animação/animator para esse gameObject
	private Animator anim; 


	void Start()
	{
		// anim = GetComponent<Animator>();  // pega o Animator
	}


    void DestroyAnimation()
    {
    	// anim.Play("Destroy");  // começa a animação
    }


    // está sendo chamado no final da animação  ------  (quando houver uma animação)
    void Destroy()
    {
    	gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "Chinela")
    	{
    		if(col.gameObject.layer == 9)  // layer 9 = ChinelaPesada
    		{
    			DestroyAnimation();
    		}
    		else
    		{
    			// destroi/resta a chinela
    			ChinelaControle.Instance.EndChinela();
    		}
    	}
    
    }
}
