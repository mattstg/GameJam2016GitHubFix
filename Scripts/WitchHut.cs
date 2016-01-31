using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WitchHut {
	
    #region singleton
    private static WitchHut instance;

    private WitchHut() { DEBUG_InitializeCoffer(); Debug.Log(DEBUG_OutputCofferContents()); }

    public static WitchHut Instance
   {
      get 
      {
         if (instance == null)
         {
             instance = new WitchHut();
         }
         return instance;
      }
   }
    #endregion
    public VillageCenter linkToVillageCenter;
    Dictionary<Globals.product, int> witchsCoffer = new Dictionary<Globals.product,int>();

    public Dictionary<Globals.product, int> GetWitchesCoffer()
    {
        return witchsCoffer;
    }

	public void addToWitchsCoffer(Globals.product resource, int amount){
		//will add amount passed into witch's coffer
		if (!witchsCoffer.ContainsKey (resource)) {
			//then we need to initialize it first with amount we want to add!
			witchsCoffer.Add (resource, amount);
		} else {
			//else, it exists in storage and we can add resource to resourceStorage, adding amount to add with current amount in storage
			witchsCoffer[resource] = amount + witchsCoffer[resource];
		}
	}

    public void RemoveFromWitchesCoffer(Dictionary<Globals.product, int> toRemove)
    {
        foreach (KeyValuePair<Globals.product, int> kv in toRemove)
        {
            RemoveFromWitchesCoffer(kv.Key, kv.Value);
        }
    }

	public bool RemoveFromWitchesCoffer(Globals.product resource, int amount){
		//check first if resource has amount in storage
		if (witchsCoffer.ContainsKey (resource) && witchsCoffer [resource] >= amount) {
			//then we have enough to take the amount we want of said resource
			witchsCoffer[resource] -= amount;
			return true;
		} else {
			//we either dont have enough of resource, or resource isn't contained (meaning we have 0 of said resource)...
			Debug.LogError("Error: attempting to take resource " + resource + " from Witch's Coffer, when none of said resource is stored");
			return false;
		}
	}

	public void giveResourceBackToVillageCenter(Globals.product resource, int amount){
		//checking if we have enough of said resource to give
		if (RemoveFromWitchesCoffer (resource, amount)) {
			linkToVillageCenter.addResourceToStorage (resource, amount);
		} else {
            Debug.Log("Warning: trying to remove more resources than are stored in Witch's Coffer, cannot give back to VillageCenter");
		}
	}

    public int GetCountOfItem(Globals.product item)
    {
        if (witchsCoffer.ContainsKey(item))
            return witchsCoffer[item];
        return 0;
    }

    public string DEBUG_OutputCofferContents()
    {
        string toRet = "In witches coffer = ";
        foreach (KeyValuePair<Globals.product, int> kv in witchsCoffer)
        {
            toRet += "[" + kv.Key + "," + kv.Value + "], ";
        }
        return toRet;
    }

    public void DEBUG_InitializeCoffer()
    {
        foreach (Globals.product ingr in System.Enum.GetValues(typeof(Globals.product)))
        {
            addToWitchsCoffer(ingr, Random.Range(1, 3));
        }
        //addToWitchsCoffer(Globals.product.Bean, 5);
        //addToWitchsCoffer(Globals.product.Carrot, 5);
        //addToWitchsCoffer(Globals.product.Chicken, 5);
    }
}
