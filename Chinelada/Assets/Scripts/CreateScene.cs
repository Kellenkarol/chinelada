using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreateScene : MonoBehaviour
{
	public bool _new;
	private SceneScriptable sceneScriptable;
	public Transform Chinelas_Lista;
	public ForceBar forceBar;
	public ShotsCount shotsCount;
	public TimeCount timeCount;
	public ChinelaControle chinelaControle;
    public Image displayChinelaSelected;

	// public GameObject ChinelaNormal, ChinelaPesada, ChinelaFoguete, ChinelaMetal;

	private int currentSceneLevel;
	private GameObject[] prefabs;
	private Dictionary<string, GameObject> Chinelas = new Dictionary<string, GameObject>();
    private string[] exceptChinelas = {"ChinelaFoguete", "ChinelaPesada"}; // chinelas que precisam atributos a mais
    private int IndexOfChinelaOnTop;
    private Sprite[] chinelasImages; // textura das chinelas  -  Resources > ChinelasImages


    // Start is called before the first frame update
    void Start()
    {
        
    	GameObject[] chinelas = Resources.LoadAll<GameObject>("Chinelas");
        chinelasImages = Resources.LoadAll<Sprite>("ChinelasImages"); //  carrega os sprites
        chinelaControle.CurrentChinelaName = null;
        IndexOfChinelaOnTop = chinelas.Length;

        foreach (GameObject chinela in chinelas)
        {
        	string name = chinela.name;
        	Chinelas[name] = chinela;
        }

        // currentSceneLevel = LoadLevel.Instance.currentSceneLevel; // CurrentScene é definido em LoadLevel.cs ao selecionar o nível
        
        currentSceneLevel = LoadLevel.Instance.GetCurrentLevel();
        SetCurrentScriptable();
        CreateCurrentSceneLevel(); 
    }


    // Update is called once per frame
    void Update()
    {	
        // teste
    	if(_new)
    	{
    		_new = false;
    		// New();
    	}
        
    }


    // retorna o index da chinela na lista de chinelas
    int GetIndexInLista(string name)
    {

        for(int c=1; c<Chinelas_Lista.childCount-1; c++)
        {
            Transform child = Chinelas_Lista.GetChild(c);
            if(child.name == name)
            {
                return c;
            }
        }

        return -1;

    }



    // desativa todos os gameObejects na 'Chinelas_Lista'
    void DeactivateLista()
    {
        for(int c=1; c<Chinelas_Lista.childCount-1; c++)
        {
            Chinelas_Lista.GetChild(c).gameObject.SetActive(false);
        }
    }


    public void SetCurrentScriptable()
    {
        sceneScriptable = LoadLevel.Instance.levels[currentSceneLevel];
    }


    // cria a fase atual salva em 'CurrentSceneLevel'
	public void CreateCurrentSceneLevel()
	{

		ResetAll();
        DeactivateLista(); //desativa para ativar posteriormente
        CreateChinelas(); // cria a lista de chinelas e atualiza seus atributos

    	shotsCount.MaxShots 		= sceneScriptable.MaxShot;
    	 timeCount.Count 			= sceneScriptable.MaxTime;
    	  forceBar.maxForce 		= sceneScriptable.MaxForce;
    	  forceBar.minForce 		= sceneScriptable.MinForce;
    	  forceBar.speed 			= sceneScriptable.ForceIndicatorSpeed;

        Instantiate(sceneScriptable.Tiles);
	}


	private void ResetAll()
	{
		chinelaControle.chinelas = new List<GameObject>();
		shotsCount.Reset();
	}


    // cria as chinelas que vão ser usadas nas fase
    void CreateChinelas()
    {
        foreach(GameObject chin in sceneScriptable.AllChinelas)
        {
            CreateChinela(chin.name); // cria a chinela sem atributos adicionais

            // verfica se é uma chinela sem atributos adicionais
            if(ExceptChinela(chin.name))
            {
                // adiciona mais atributos
                switch(chin.name)
                {
                    case "ChinelaFoguete":
                        AddChinelaFogueteAttributes();
                        break;
                    case "ChinelaPesada":
                        AddChinelaPesadaAttributes();
                        break;
                    // case "OutraChinela":
                        // 
                        // break;
                    // ...

                }
            }
        }

        SetNameOfCurrentChinelaInChinelaControle(GetNameInListByIndex(IndexOfChinelaOnTop));
        SetCurrentChinelaSelected(IndexOfChinelaOnTop-1);
    }


    // mostra qual chinela está selecionada
    public void SetCurrentChinelaSelected(int idx)
    {
        displayChinelaSelected.sprite = chinelasImages[idx];
    }


    // cria uma chinela
    void CreateChinela(string name)
    {
        Chinelas[name].GetComponent<Chinela>()._mass             = sceneScriptable.MassPadrao;
        chinelaControle.chinelas.Add(Chinelas[name]); 
        Chinelas_Lista.GetChild(GetIndexInLista(name)).gameObject.SetActive(true);
        
        if(GetIndexInLista(name) < IndexOfChinelaOnTop)
        {
            IndexOfChinelaOnTop = GetIndexInLista(name); // salva qual é a chinela mais no topo
        }

        // chinelaControle.CurrentChinelaName = chinelaControle.CurrentChinelaName == null ? name:chinelaControle.CurrentChinelaName;
    }


    // sem utilidade
    // string GetNameOfFirstChinelaOnList()
    // {
    //     for(int c=1; c<Chinelas_Lista.childCount-1; c++)
    //     {
    //         if(Chinelas_Lista.GetChild(c).gameObject.active)
    //         {
    //             return Chinelas_Lista.GetChild(c).gameObject.name;
    //         }
    //     }

    //     return "";
    // }


    string GetNameInListByIndex(int idx)
    {
        return Chinelas_Lista.GetChild(idx).gameObject.name;
    }

    void SetNameOfCurrentChinelaInChinelaControle(string _name)
    {
        print("_name ----------- "+_name);
        chinelaControle.CurrentChinelaName = _name;
    }


    bool ExceptChinela(string _name)
    {

        foreach(string name in exceptChinelas)
        {
            if(_name == name)
                return true;
        }

        return false;
    }


    void AddChinelaFogueteAttributes()
    {
        Chinelas["ChinelaFoguete"].GetComponent<ChinelaFoguete>().MaxGas    = sceneScriptable.MaxGas;
        Chinelas["ChinelaFoguete"].GetComponent<ChinelaFoguete>().GasForce  = sceneScriptable.GasForce;
    }


    void AddChinelaPesadaAttributes()
    {
        Chinelas["ChinelaPesada"].GetComponent<Chinela>()._mass             = sceneScriptable.ChinelaPesadaMass;
    }


    public void NextLevel()
    {
        // currentSceneLevel++;
        LoadLevel.Instance.SetCurrentLevel(currentSceneLevel+1);
        Loading.Instance.StartTheLoad(2); // gameplay
    }


    public void ReplayLevel()
    {
        // currentSceneLevel++;
        LoadLevel.Instance.SetCurrentLevel(currentSceneLevel);
        Loading.Instance.StartTheLoad(2); // gameplay
    }

}
