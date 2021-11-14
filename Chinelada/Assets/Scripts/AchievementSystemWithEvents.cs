using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class AchievementSystemWithEvents : MonoBehaviour
{

    private Dictionary<string, Dictionary<string,List<string>>> conquistas;

    // Start is called before the first frame update
    private void Start()
    {

    	// Start Test ------------------------------------------------------------------------------------------

	    	TextAsset aa = Resources.Load<TextAsset>("Achievements/Achievements") as TextAsset;
	    	
            if(aa == null)
	    	{
	    		print("Não achei nada ----------");
	    	}
    		else
    		{
    			print("achei alguma coisa ----------");
    			print(aa.ToString());
    		}
			
            conquistas = 
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string,List<string>>>>(aa.text);	

    	// End Test --------------------------------------------------------------------------------------------


        // PlayerPrefs.DeleteAll();

        PointOfInterestWithEvents.OnPointOfInterestEntered += 
        	PointOfInterestWithEvents_OnPointOfInterestEntered;
    }

    private void PointOfInterestWithEvents_OnPointOfInterestEntered(string poiName)
    {
        string achievementKey = "achievement-"+poiName;

    	if(PlayerPrefs.GetInt(achievementKey, 0) == 1)
    		return;

    	PlayerPrefs.SetInt(achievementKey, 1);
    	print("Unlocked " + poiName);
    }


    private void Shots(string chinelaName)
    {
    	string achievementKey   = "achievement-"+chinelaName;
        PlayerPrefs.SetInt(achievementKey,PlayerPrefs.GetInt(achievementKey,0)+1);
        int _shots              = PlayerPrefs.GetInt(achievementKey+"-shots",0);
        int niveisCount         = conquistas[achievementKey]["niveis"].Count;

        for(int c = niveisCount-1; c>=0; c--)
        {
            if(int.Parse(conquistas[achievementKey]["niveis"][c]) < _shots)
            {
                PlayerPrefs.GetInt(achievementKey+"-nivel",c);

                print("Unlocked " + achievementKey+"-nivel: "+c);

                break;
            }
        }

    }
}
