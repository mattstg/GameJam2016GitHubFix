using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class VillageCenter : MonoBehaviour {
	//LINKS TO OTHER SCRIPTS
	public Biome[] biomes;
	public WitchHut witchLink;
	public Population population;
    public float worldTime = 0;
    public Listener TheListener;

    public bool wasConstructed = false; //worse fix ever
	public bool toggler = true;
	public bool availableFoodToggler = true;
	
    public void Update(){
		if (toggler == false) {
			toggler = true;
			Cycle ();
		}
        worldTime += (Time.deltaTime>3)?.16f:Time.deltaTime; //If a pause causing a large value on unpause, then this will prevent that
        if (worldTime > Globals.lengthOfScenario)
        {
            Cycle();
            worldTime = 0; //just in case that weird loop thing
        }

	}

	//WORKING VARIABLES & INFORMATIVE VARIABLES
	public int currentHouses;

	//RESOURCE STORAGE SYSTEM
	public Dictionary<Globals.product,int> resourceStorage = new Dictionary<Globals.product, int>();
	public void startStorage(){
		//should initialize the start state of the Resource Storage System
		resourceStorage.Add(Globals.product.Fish, 10);
		resourceStorage.Add(Globals.product.Carrot, 10);
		resourceStorage.Add (Globals.product.Wood, 50);
	}

    public void Initialization()
    {
        TheListener = new Listener();
        startStorage();        
        //initialize population object
        population = new Population();
        population.center = this;

        witchLink = WitchHut.Instance;
        witchLink.linkToVillageCenter = this;

        //need to initialize population & houses
        currentHouses = Globals.startHouses;
        //need to fill biome array
        biomes = new Biome[Globals.numberOfBiomes];
        for (int counter = 0; counter < biomes.Length; counter++)
        {
            //Debug.Log ("Biome Initialized: " + counter);
            biomes[counter] = new Biome(counter);
            
            biomes[counter].center = this;
        }
        wasConstructed = true;        
    }

	void Cycle(){
		//Debug.Log ("Cycle Start: VillageCenter.");
		//keep track of population loss for last turn, and reset population loss this cycle
		//will proceed with next game cycle
		//calculating yeild for each biome in biomes[]
		//Debug.Log ("About to Cycle Biomes.");
		foreach (Biome biome in biomes){
			biome.Cycle(); //this calculates the produce and adds it to village store
		}
		//now we need to use wood/ore to build houses? if village needs more houses.
		houseConstruction();

		//calculate population consumption, and update working population variable in VillageCenter
		int popToAdd = population.Cycle();

		//now we need to send the surplus to the witch's coffer
		giveResourcesToWitch();

        //Debug.Log("The population values before sum: " + population);
        //now get the new pop stats
        GetAveragePopulationStatus();
       // Debug.Log("The population values now: " + population);
        population.currentPopulation += popToAdd;

        //End the scene
        EndScene();

		//should add print contents of VillageStorage
		/*Debug.Log("******************VILLAGE*STOREAGE*****************");
		foreach (KeyValuePair<Globals.product, int> resource in resourceStorage) {
			Debug.Log("Resource: "+ resource.Key.ToString() + "...    Quantity:"  +resource.Value.ToString());
		}
		Debug.Log("**********************END**********************");*/
	}

	public void addResourceToStorage(Globals.product resource, int amount){
		//check first if it is in the dictionary
			//if not, then add with amount = 0 then proceed to avoid error

		if (!resourceStorage.ContainsKey (resource)) {
			//then we need to initialize it first with amount we want to add!
			resourceStorage.Add (resource, amount);
			//Debug.Log ("Successfully added " + amount + "x " + resource + " from VillageStorage! (From zero)");
		} else {
			//else, it exists in storage and we can add resource to resourceStorage, adding amount to add with current amount in storage
			resourceStorage[resource] = amount + resourceStorage[resource];
			//Debug.Log ("Successfully added " + amount + "x " + resource + " from VillageStorage!");
		}
	}

	public bool subtractResourceFromStorage(Globals.product resource, int amount){
		//check first if resource has amount in storage
		if (resourceStorage.ContainsKey (resource) && resourceStorage [resource] >= amount && amount != 0) {
			//then we have enough to take the amount we want of said resource
			resourceStorage[resource] = resourceStorage[resource] - amount;
			//Debug.Log ("Successfully subtracted  " + amount + "x " + resource + " from VillageStorage!");
			return true;
		} else {
			//we either dont have enough of resource, or resource isn't contained (meaning we have 0 of said resource)...
			//Debug.Log("Trying to subtract " + amount + "x "+ resource + " from VillageStorage, but there is not enough");
			return false;
		}
	}

	public void giveResourcesToWitch(){
		//for each loop which goes through each resources in storage and allocates a certain percent to the witch
		for(int counter = 0; counter < resourceStorage.Count; counter++){
			//if resource is 0, then we dont care about it
			//Debug.Log("Starting with " + resourceStorage.ElementAt (counter).Value + "x " + resourceStorage.ElementAt (counter).Key + " in Village Store");
			//global variable percentGivenToWitch is used to calculate amount given
			int amountToGive = Mathf.FloorToInt (resourceStorage.ElementAt(counter).Value * Globals.percentGivenToWitch);
			//Debug.Log (resourceStorage.ElementAt (counter).Key + " <-- Key ... Value -->" + amountToGive);
			if (resourceStorage.ElementAt (counter).Value > Globals.minimumTaxableAmountOfProduce && amountToGive != 0) {
				//need to subtract amoulnt given from resourceStorage
				if (subtractResourceFromStorage (resourceStorage.ElementAt (counter).Key, amountToGive)) {
					//Debug.Log ("resourceStorage.ElementAt(counter).Key  " + resourceStorage.ElementAt (counter).Key + " amountToGive: " + amountToGive);
					//need to actually give amounts to witch
					witchLink.addToWitchsCoffer (resourceStorage.ElementAt (counter).Key, amountToGive);
					//Debug.Log ("after to add to witch's coffer.");
				} else {
					//we dont have enough to give, therefore do not take from storage or give to witch
					//Debug.Log ("Error: attempting to give resources from VillageCenter to WitchCoffer, when none of said resource is stored in VillageCenter.");
				}
			} else {
				//Debug.Log ("Insufficient amount to give resources away.");
			}
		}
	}

	public int amountOfAvailableFood(){
		int totalFoodCount = 0;
		foreach (KeyValuePair<Globals.product, int> resource in resourceStorage) {
			//if the resource type is a type which appears in array foodTypeProduce, then it is a food and we will add the quantity of food to totalFoodCount
			if (Globals.foodTypeProduce.Contains((int) resource.Key)) {
				totalFoodCount += resource.Value;
			}
		}
		return totalFoodCount;
	}

	public void houseConstruction(){
		//need to know demand for houses, which is based current population and current amount of houses
		//desired houses = totalPop / popPerHouses + floating Houses (value is 1, the amount of houses to be built before population actually needs
		int desiredHouses = Globals.floatingHouses + Mathf.CeilToInt(population.currentPopulation / Globals.residentsPerHouse);
		if (desiredHouses > 0) { 		//then we have homeless population, and need to build houses
			//how many houses can we build? check storage to see how much wood, and consult global variables for how much wood per house
			int buildableHousesThisCycle = Mathf.FloorToInt(resourceStorage[Globals.product.Wood] / Globals.woodPerHouse);
			while (desiredHouses > 0 && buildableHousesThisCycle > 0) {
				buildHouse ();
				desiredHouses--;
				buildableHousesThisCycle--;
			}
		}
	}

	public void buildHouse(){
		if (resourceStorage [Globals.product.Wood] > Globals.woodPerHouse) {
			subtractResourceFromStorage(Globals.product.Wood, Globals.woodPerHouse);
			currentHouses++;
			//Debug.Log ("Building House, current house count at " + currentHouses);
		} else {
			//Debug.Log ("Warning: calling buildHouse() function in VillageCenter, when you have insufficient wood");
		}
	}

    public void DestroyHouse(Vector2 houseDestroyed)
    {
        currentHouses--;
        GetComponent<WorldLoader>().RemoveHouse(houseDestroyed);
    }

    public void GetAveragePopulationStatus()
    {
        Population pop = new Population(0,0,0,0);
        Villager[] villagers = GameObject.FindObjectsOfType<Villager>();
        foreach (Villager v in villagers)
        {
            v.UpdateStats();
            pop.averageHappiness += v.happiness;
            pop.averageHealthiness += v.healthiness;
            pop.averagePercentLifePoints += v.hp;
        }
        int curPop = villagers.Length;
        TheListener.RecordValue("Average Happiness", population.averageHappiness, pop.averageHappiness / curPop);
        TheListener.RecordValue("Average Healthiness", population.averageHealthiness, pop.averageHealthiness / curPop);
        TheListener.RecordValue("Average hitpoints", population.averagePercentLifePoints, pop.averagePercentLifePoints / curPop);
        TheListener.RecordValue("Population", population.currentPopulation, curPop);
        population.averageHappiness =  pop.averageHappiness/curPop;
        population.averageHealthiness =  pop.averageHealthiness/curPop;
        population.averagePercentLifePoints = pop.averagePercentLifePoints / curPop;
        pop.currentPopulation = curPop;
    }

    public void EndScene()
    {
        foreach (Biome bi in biomes)
            TheListener.RecordFinalBiomeHp(bi.biomeType.ToString(), bi.health);
        if (GameObject.FindObjectOfType<EventLauncher>())
            Destroy(GameObject.FindObjectOfType<EventLauncher>().gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("WitchHut");
        this.enabled = false;
    }

    public void BeginVillageScenario()
    {
        this.enabled = true;
        GameObject.FindObjectOfType<WorldLoader>().LoadAll(population,currentHouses);
        foreach (Biome bi in biomes)
            TheListener.RecordInitialBiomeHp(bi.biomeType.ToString(), bi.health);
		Villain.Instance.createEventFromBluePrint();
    }

    public void BiomeModHp(int bioNumber, float amt)
    {
        biomes[bioNumber].health += amt;
        if (biomes[bioNumber].health > 1)
            biomes[bioNumber].health = 1;
        if (biomes[bioNumber].health <= 0)
            biomes[bioNumber].health = 0;
    }

    public float GetBiomeHp(int bioNumber)
    {
        return biomes[bioNumber].health;
    }
}