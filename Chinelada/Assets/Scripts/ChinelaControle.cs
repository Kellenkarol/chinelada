using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

// using Microsoft.CodeAnalysis.CSharp.Scripting;

// COMO ADICIONAR NOVO CHINELO ---------------------------------------------------------------------------------------- 
// 
// Para adicionar um novo chinelo, primeiro crie o seu prefab e o coloque em 'Assets/Chinelas'. Em seguida crie
// um novo gameObject em   Hierarchy->GamePlay->Canvas->Chinelas->Lista   (use Crtl+D para copiar um item já na lista), 
// após isso coloque o gameObject na posição que desejar (entre a BordaSuperior e BordaInferior), e por último 
// renomeie-o com o mesmo nome usado no seu prefab 
// 
// OBS: o script e o prefab tem que ter o mesmo nome
// 
// -------------------------------------------------------------------------------------------------------------------- 


public class ChinelaControle : MonoBehaviour
{
	
	public Transform spawnPoint;
	public GameObject buttonShot, buttonGas;
    public GameObject winDisplay, loseDisplay;

	private  Dictionary<string, GameObject> ChinelasPrefab = new Dictionary<string, GameObject>(){};
	[HideInInspector] public string CurrentChinelaName; //inicia em 'CreateScene.cs'
	private Chinela CurrentChinela;
	private bool buttonPressed;
    private GameObject SetaPontoDeRotacao;
	
	[HideInInspector]
	public List<GameObject> chinelas = new List<GameObject>();

	public static ChinelaControle Instance;

    bool IsShoting;
    float angulo;

    void Start()
    {
    	Instance = this; // deixa todo o script estático, para acessar qualquer variável ou função é só usar 'ChinelaControle.Instance'
        SetaPontoDeRotacao = spawnPoint.gameObject;


		UpdateChinelasPrefab();
        StartFirstChinela();
    }
    

    // void Update()
    // {

    // }


    // instancia todas as chinelas que vão ser usadas e salva em uma lista
    // chinelas é inicado em 'CreateScene.cs'
    public void UpdateChinelasPrefab()
    {
        foreach (GameObject chinela in chinelas)
        {
        	string tmpName = chinela.name;
        	ChinelasPrefab[tmpName] = Instantiate(chinela);
	        ChinelasPrefab[tmpName].SetActive(false);
        }
    }


    // muda o estado de 'simulated' do 'Rigidbody2D' da chinela atual
    private void SetChinelaSimulated(bool value)
    {
    	CurrentChinela.GetComponent<Rigidbody2D>().simulated = value;
    }


    // img é a imagem que mostra a quantidade de gás
    public void UseGas(Image img)
    {
    	CurrentChinela.UseGas(img);
    }


    public void SetThrowed(bool v)
    {
        CurrentChinela.SetThrowed(v);
    }


    // mira para cima (está sendo chamando em 'HoldButton.cs')
    public void Up()
    {	
        float anguloMax = -70; // máximo que o angulo superior pode atingir a partir do seu angulo inicial (-90°)
        if(angulo >= anguloMax)
        {
            angulo--; 
            SetaPontoDeRotacao.transform.Rotate(0,0,1); // sentido anti-horário
        }
    }


    // mira para baixo (está sendo chamando em 'HoldButton.cs')
    public void Down()
    {	
        float anguloMin = 45; // máximo que o angulo inferior pode atingir a partir do seu angulo inicial (-90°)
        if(angulo <= anguloMin) 
        {
            angulo++; 
            SetaPontoDeRotacao.transform.Rotate(0,0,-1); // sentido horário
        }
    }

    public Vector3 GetChinelaPosition()
    {
    	return CurrentChinela.GetPosition();
    }


    // inicia a primeira chinela que aparece para o jogador
    void StartFirstChinela()
    {
        CurrentChinela = ChinelasPrefab[CurrentChinelaName].GetComponent<Chinela>(); 
        CurrentChinela.gameObject.SetActive(true);
        CurrentChinela.transform.position = spawnPoint.position;
    	// CurrentChinela.GetComponent<Chinela>().TurnOn(); //obsoleto, isso já está sendo feito no 'Awake'
        SetChinelaSimulated(false);
    }


    // usado para trocar de chinela
    // está sendo chamado pelos botões na lista de chinelas
    public void ChangeChinela(GameObject gb)
    {
        // ShotsCount.Instance.RemainingShots() = quantidade de tiros restantes
        if(!IsShoting && ShotsCount.Instance.RemainingShots() > 0)
        {
        	string name = gb.name;
        	// CurrentChinela.GetComponent<Chinela>().TurnOff();
        	CurrentChinela.gameObject.SetActive(false);
        	CurrentChinelaName = name;
        	ChinelasPrefab[name].SetActive(true);
            ChinelasPrefab[name].transform.position = spawnPoint.position;
            CurrentChinela = ChinelasPrefab[name].GetComponent<Chinela>(); 
        	// CurrentChinela.TurnOn();
            SetChinelaSimulated(false);
    		buttonShot.SetActive(true);  
    		buttonGas.SetActive(false);
        }
    }

    public void Shot()
    {
        // só é permitido atirar quando a chinela está no 'spawnPoint.position'
        if(!IsShoting)
        {
            IsShoting = true;
        	// Chinela aux = ChinelasPrefab[CurrentChinelaName].GetComponent<Chinela>();
        	
            bool isChinelaFoguete = CurrentChinelaName == "ChinelaFoguete"; // checa se a chinela é 'ChinelaFoguete'
    		buttonGas.SetActive(isChinelaFoguete); // ativa o botão de gás caso seja a chinela 'ChinelaFoguete'
            buttonShot.SetActive(!isChinelaFoguete); // desativa o botão de shot caso seja a chinela 'ChinelaFoguete'

            Vector2 direc = Magnetismo.GetDirOfAngle(90+angulo);
            direc /= Magnetismo.CalculateDistance(direc, new Vector2(0,0));
            CurrentChinela.Shot(direc); 
        }
    }


    public void ResetChinela()
    {

    }


    public void EndChinela()
    {
        buttonGas.GetComponent<HoldButton>().isPressed = false;
        buttonGas.SetActive(false); 
        buttonShot.SetActive(true);
        if(ShotsCount.Instance.RemainingShots() > 0)
        {
            // é necessário para que 'CurrentChinela.ResetChinela()' saiba a posição para fazer o reset
            CurrentChinela._startPos = spawnPoint.position;
            CurrentChinela.ResetChinela();
        }
        else if(false) //if win
        {

        }
        else //if not win
        {
            GameOver();
        }

        IsShoting = false;
    }


    private void GameOver()
    {
        print("GameOver");
        loseDisplay.SetActive(true);
    }


    public void GameWin()
    {
        print("GameWin");
        winDisplay.SetActive(true);
    }

    public void Reset()
    {
    	chinelas = new List<GameObject>();
    }

}