using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




[CreateAssetMenu(fileName = "Novo Mapa")]
public class SceneScriptable : ScriptableObject
{
   
    // public enum Chinela {Normal, Pesada, Foguete, Metal}

	public GameObject[] AllChinelas;

	public bool ChinelaNormal;
	public bool ChinelaPesada;
	public bool ChinelaFoguete;
	public bool ChinelaMetal;

	public float ChinelaPesadaMass;
	public float MaxGas;
	// public float GasUsePerSecond;
	public float GasForce;

	public float MassPadrao;
	public int   MaxShot;
	public int   MaxTime;
	public float MinForce;
	public float MaxForce;
	public float ForceIndicatorSpeed;
	public GameObject Tiles;

}




 // [CustomEditor(typeof(SceneScriptable))]
 // public class MyScriptEditor : Editor
 // {
 //   public override void OnInspectorGUI()
 //   {
	// SceneScriptable myScript = target as SceneScriptable;
 
	// myScript.ChinelaNormal 			= EditorGUILayout.Toggle("Chinela Normal", myScript.ChinelaNormal);
	// myScript.ChinelaPesada 			= EditorGUILayout.Toggle("Chinela Pesada", myScript.ChinelaPesada);
	// myScript.ChinelaFoguete 		= EditorGUILayout.Toggle("Chinela Foguete", myScript.ChinelaFoguete);
	// myScript.ChinelaMetal 			= EditorGUILayout.Toggle("Chinela De Metal", myScript.ChinelaMetal);
     
	// myScript.MaxShot 				= EditorGUILayout.IntField("Quantidade De Tiros", myScript.MaxShot);
	// myScript.MaxTime 				= EditorGUILayout.IntField("Duração Do Jogo", myScript.MaxTime);
	// myScript.MinForce 				= EditorGUILayout.IntField("Força Mínima", myScript.MinForce);
	// myScript.MaxForce 				= EditorGUILayout.IntField("Força Máxima", myScript.MaxForce);

	// EditorGUI.BeginDisabledGroup(!myScript.ChinelaFoguete);
	// 	myScript.MaxGas 			= EditorGUILayout.IntField("Gás Máximo", myScript.MaxGas);
	// 	myScript.GasForce 			= EditorGUILayout.IntField("Uso Do Gás Por Segundo", myScript.GasForce);
	// 	EditorGUI.EndDisabledGroup();

	// myScript.ForceIndicatorSpeed 	= EditorGUILayout.IntField("Velocidade Do Indicador De Força", myScript.ForceIndicatorSpeed);

 //     // else
 //     // {
 //     // 	EditorGUI.BeginDisabledGroup(myScript.MaxGas);
 //     // }
 
 //   }
 // }










