using UnityEngine;
using System.Collections;

public class AuraTestScript : MonoBehaviour {

    public AuraEvent auraEvent;
	// Use this for initialization
	void Start () {
        auraEvent.ParentSubOrbs(3);
	}
	
	
}
