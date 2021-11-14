using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{
	public bool FirstLoad=false; // se é a primeira cena 
	public Animator anim;
    public int currentScene;
    public static Loading Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentScene = GetCurrentSceneToLoad();
        StartCoroutine(LoadYourAsyncScene());
        print("currentScene  "+currentScene);
    }


    // Update is called once per frame
    void Update()
    {
         // anim.Play("JustTest", -1, slider.normalizedValue);
    }

    public void SetCurrentScene(int _currentScene)
    {
        currentScene = _currentScene;
        StartCoroutine(LoadYourAsyncScene());
    }

    public void StartLoad(int sceneNum)
    {
        // Instance.currentScene = sceneNum;
        // print("currentScene  "+currentScene);
        SceneManager.LoadScene(0); // vai pra tela de carregamento
    }


    IEnumerator LoadYourAsyncScene(float minTime=2f)
    {
    	// yield return new WaitForSeconds(1f);

        if(GetCurrentSceneToLoad() != 1)
            minTime = 1f; // um tempo mínimo para a tela de load permancer 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene);
		asyncLoad.allowSceneActivation = false;
        float auxTime = 0;

        // Wait until the asynchronous scene fully loads
        while (true)
        {
			// anim.Play("Loading", -1, asyncLoad.progress);
			// yield return new WaitForSeconds(1);
			auxTime += Time.deltaTime;
            
            if (asyncLoad.progress >= 0.9f && auxTime/minTime >= 1)
            {
				// anim.Play("Loading", -1, 1);
                ResetSceneToLoad();
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public int GetCurrentSceneToLoad()
    {
        if(FileExists())
        {
            return int.Parse(File.ReadAllText(Application.dataPath + "/SaveAndLoadData/CurrentSceneToLoad.txt"));
        }
        else
        {
            return 1; // menu
        }
    }


    public void SetCurrentSceneToLoad(int sceneNum)
    {
        File.WriteAllText(Application.dataPath + "/SaveAndLoadData/CurrentSceneToLoad.txt", sceneNum.ToString());
    }


    public bool FileExists()
    {
        return File.Exists(Application.dataPath + "/SaveAndLoadData/CurrentSceneToLoad.txt");
    }


    public void StartTheLoad(int sceneNum)
    {
        SetCurrentSceneToLoad(sceneNum); // gameplay
        SceneManager.LoadScene(0); // load
    }


    private void ResetSceneToLoad()
    {
        SetCurrentSceneToLoad(1); // menu
    }
}
