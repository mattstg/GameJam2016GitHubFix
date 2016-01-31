using UnityEngine;
using System.Collections;

public class Biome{
	public VillageCenter center;
	public Globals.biome biomeType;

	public float health;

	//has some stuff it produces
	public Globals.product[] resources = new Globals.product[3];

	//A productivity for each of the three resources
	public float[] yeildRate = new float[3];
	//between 0 - 1 usually
	//is function of population in town and the effects

/*	public Biome(Biome biomeToCopy){
		health = biomeToCopy.health;
		center = biomeToCopy.center;
		biomeType = biomeToCopy.biomeType;
		resources = biomeToCopy.resources;
		yeildRate = biomeToCopy.yeildRate;
	} */

	public Biome(int biomeNumber){
		float health = 1f;
		biomeType = (Globals.biome)biomeNumber;
		resources = Globals.retBiomeResources(biomeType, biomeNumber*3);
		float tempTotal = 0f;
		for (int counter = 0; counter < 3; counter++) {
			yeildRate [counter] = Random.Range (.2f, 1f);
			tempTotal += yeildRate [counter];
		}
		tempTotal /= 2;
		for (int counter = 0; counter < 3; counter++) {
			yeildRate[counter] /= tempTotal/2;
			int tempRounder = (int) (yeildRate [counter] * 100);
			yeildRate [counter] = (float) tempRounder / 100;
			//Debug.Log (resources [counter] + " <-- resource ; percentYeild --> " + yeildRate [counter]);
		}
	}
	//should initialize the enum for reasources for each biome.

	public void Cycle(){
		//Debug.Log ("Cycle Start: " + biomeType + ".");
		produceResources();
	}
	//will produce resource based on productivity of resource and type of resource
	//will send to village center

	public void produceResources(){
		//uses productivity to calulate yeild
		//sends goods to resource storage
		for(int counter = 0; counter < 3; counter++) {
			if(grossProduce(yeildRate[counter]) != 0)
			giveResourceToVillage (resources [counter], grossProduce(yeildRate[counter]));
		}
	}
		
	public int grossProduce(float productivity){
		return Mathf.FloorToInt(health * center.population.currentPopulation * Globals.populationYeildBonus * productivity * Globals.biomeProductivityCoefficient);
	}
		
	public void giveResourceToVillage(Globals.product resource, int amount){
		//Debug.Log ("Giving Resources to Village: from " + biomeType + ".");
		//Debug.Log ("Adding: " + resource + " x" + amount + ".");
		center.addResourceToStorage (resource, amount);
	}

	public void isCollidedWith(){ 
		//nothing yet	
		//remove health
	}
		
	//needs to be able to heal

	//inputs event type and somehow interprets 
}