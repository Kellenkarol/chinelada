using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Index;

public class Chinela : MonoBehaviour
{
	[HideInInspector] public Rigidbody2D  _rb; //Rigidbody who's taking Force
	[HideInInspector] public Vector2      _startPos;
	[HideInInspector] public float        _mass; //Mass of affected Object's Rigidbody (_rb)
    [HideInInspector] public bool         _throwed;
	[HideInInspector] public bool         _rotate;


    public virtual void ResetRotation()
    {
    	transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    public virtual void SetThrowed(bool v)
    {
        _throwed = v;
    }


    public virtual void TurnOff()
    {
        StopAllCoroutines();
    }


    public virtual void Shot(Vector2 direction)
    {
        ShotsCount.Instance.RemoveLast(); // atualiza os tiros restantes
        _rb.velocity = Vector3.zero; // não lembro a necessidade dessa linha
    	SetThrowed(true);
        _rb.simulated = true; // liga a simulação do Rigidbody
        _rb.AddForce(direction * ForceBar.Instance.GetForce()); // adiciona uma força em uma direção
        _rb.AddTorque(-30-(ForceBar.Instance.GetForceInPercent()/4)); // adiciona uma força de rotação
    }


    public virtual Vector3 GetPosition()
    {
    	return transform.position;
    }


    public virtual void ResetChinela()
    {
        StartCoroutine(Reset(0));
    }


    public virtual IEnumerator Reset(float time)
    {
    	yield return new WaitForSeconds(time);

        SetThrowed(false);
        ResetRotation();
        _rb.angularVelocity = 0; // reseta a força de rotação (torque)
        print("_rb.angularVelocity:  "+_rb.angularVelocity);
        _rb.simulated = false;
    	transform.position = _startPos; // muda a posição da chinela para a posição inicial ('ChinelaControle' define o  valor de '_startPos')
        transform.eulerAngles = Vector3.zero;
    
    }


    public virtual void UseGas(Image img)
    {
    	// é implementado em 'ChinelaFoguete.cs'
    }


    // não é mais útil
    public virtual void SetAllVariables()
    {
        // if(!_wasUpdate)
        // {
        //     // print("Variables was set");
        //     _startPos = transform.position;
        //     _rb = GetComponent<Rigidbody2D>();
        //     _rb.simulated = false;
        //     // _mass = _rb.mass;
        //     _rb.mass = _mass;
        //     _wasUpdate = true;
        // }

    }


    // Detecta quando o chinelo colide com algo
    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.layer == 6) // Bed
    	{
    		// Do something
    	}

    }


}
