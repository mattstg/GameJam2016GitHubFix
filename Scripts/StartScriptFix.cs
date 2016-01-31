using UnityEngine;
using System.Collections;

public class StartScriptFix : MonoBehaviour {

	//an odd fix
	void Start () {
        if(!GameObject.FindObjectOfType<VillageCenter>().wasConstructed)
            GameObject.FindObjectOfType<VillageCenter>().Initialization();
        GameObject.FindObjectOfType<VillageCenter>().enabled = true;
        GameObject.FindObjectOfType<VillageCenter>().BeginVillageScenario();        
	}
	
	
}
