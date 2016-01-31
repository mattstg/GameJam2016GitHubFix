using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchBag : MonoBehaviour {
    public Dictionary<Globals.product, int> witchsCoffer;
	// Use this for initialization
	void Awake () {
        
	}

    public void Start()
    {
        witchsCoffer = new Dictionary<Globals.product, int>();
        DontDestroyOnLoad(gameObject);
    }

    public void FillWitchBag(Dictionary<Globals.product, int> itemsBringing)
    {
        foreach (KeyValuePair<Globals.product, int> kv in itemsBringing)
        {
            witchsCoffer.Add(kv.Key, kv.Value);
        }
    }

   
	
	
}
