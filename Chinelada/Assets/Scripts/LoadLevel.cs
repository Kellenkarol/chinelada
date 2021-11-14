using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


// -----------------------------------------------------------------------
// É IMPORTANTE QUE OS SCRIPTABLES ESTEJAM SALVOS COM SEU RESPECTIVO NÍVEL
// POR EXEMPLO: NIVEL 1, NIVEL 2 ETC
// O IMPORTANTE É QUE ESTEJAM NA ORDEM CORRETA
// -----------------------------------------------------------------------


public class LoadLevel : MonoBehaviour
{

	[HideInInspector] public SceneScriptable[] levels;
	[HideInInspector] public int currentScene;

	public static LoadLevel Instance;
	
	//public GameObject LoadPrefab; // não é necessário

    // Start is called before the first frame update
    void Start()
    {
    	Instance = this;
        levels = Resources.LoadAll<SceneScriptable>("ScenesScriptable"); // carrega os Scriptables
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Carrega nível
    public void Load(int level)
    {
    	SetCurrentLevel(level);
        Loading.Instance.StartTheLoad(2); // gameplay
    	// currentScene 	= level; // define o nivel que está sendo carregado (vai ser usado posteriormente em CreateScene.cs)
    	// GameObject gob 	= Instantiate(LoadPrefab) as GameObject; // instancia a animação de carregamento
    	// gob.transform.GetChild(0).GetComponent<Loading>().SetCurrentSceneToLoad(2); // diz qual tela carregar (2=GamePlay)
    }


    public int GetCurrentLevel()
    {
        if(CheckIfFileExists())
        {
            return int.Parse(File.ReadAllText(Application.dataPath + "/SaveAndLoadData/CurrentSceneLevelToLoad.txt"));
        }
        else
        {
            return 1;
        }
    }


    public void SetCurrentLevel(int lvl)
    {
    	File.WriteAllText(Application.dataPath + "/SaveAndLoadData/CurrentSceneLevelToLoad.txt", lvl.ToString());
    }


    public bool CheckIfFileExists()
    {
    	return File.Exists(Application.dataPath + "/SaveAndLoadData/CurrentSceneLevelToLoad.txt");
    }
}
