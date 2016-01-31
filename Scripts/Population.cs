using UnityEngine;
using System.Collections;

public class Population {
	//LINK TO VILLAGECENTER
	public VillageCenter center;

	//current working population & happiness for village
	public int currentPopulation;
	public float averagePercentLifePoints; //range from 0 - 1 (is multiplied by Globals.maximumVillagerHealthPoints)
	public float averageHappiness; //range from 0 - 1
	public float averageHealthiness; //range from 0 - 1

	//informative variables
	public int desiredAmountOfFood = 0;
	public int netFoodAfterEating = 0; //Can be values in the negatives or positives, indicating deficit or surplus food
	public bool villageIsStarving = false; //boolean used to govern growth of populations; no growth if starving.

	public Population () {
		//when initialized pull starting population from Global Variables
        currentPopulation = Globals.startPopulation;
		averageHappiness = Globals.startAverageHappiness;
		averagePercentLifePoints = Globals.startAverageLifePoints;
		averageHealthiness = Globals.startAverageHealthiness;
	}

    public Population(int _currentPopulation, float _averagePercentLifePoints, float _averageHappiness, float _averageHealthiness)
    {
        //when initialized pull starting population from Global Variables
        currentPopulation = _currentPopulation;
        averageHappiness = _averagePercentLifePoints;
        averagePercentLifePoints = _averageHappiness;
        averageHealthiness = _averageHealthiness;
    }
	
	public int Cycle () {
		//Debug.Log ("Entering Population Cycle.");
		//people consume food, and if they dont eat, some starve.
		//Debug.Log("Current Population: " + currentPopulation);
		//********
		populationFoodConsumptionAndStarvation();
		//Debug.Log ("populationFoodConsumptionAndStarvation()");
		//********

		//we need to alter average happiness of population
		//will calculate happiness after the event scenes:
		//decreasing health or lifePoints == decrease happiness
		//increase """"" === increase happiness


		//population now needs to be able to grow. Depends upon happiness and healthiness. If they were starving, then don't have births. 


		//********
		return populationBirthController();
		//Debug.Log ("populationBirthController()");
		//********
		//Debug.Log ("Exiting Population Cycle.");
	}

	public int populationBirthController(){
		//surplus food & happiness effect population growth
		if (!villageIsStarving) {
			// if population is not starving, then population can grow
			// currently population can grow by a maximum of 20% each cycle, provided averageHappiness is perfect
			// if averageHappiness is 0.5f, then maxPercentPopulationIncrease is halved. 
			int projectedPopulationGrowth = Mathf.CeilToInt(Globals.maxPercentPopulationIncrease * currentPopulation * averageHappiness);
            int actualPopGrowth = (currentPopulation + projectedPopulationGrowth < Globals.maxPopulation)? currentPopulation += projectedPopulationGrowth: currentPopulation = Globals.maxPopulation;
            return actualPopGrowth;
			Debug.Log ("Population Grows by: " + projectedPopulationGrowth);
		} else {
			//village is starving, therefore no one wants to reproduce
            return 0;
			Debug.Log("Population is starving, and dont feel like producing offspring");
		}
	}

	public void killPopulation(int amountToKill){
		//Alert Listener that someone has died?????
		//Debug.Log ("Population that dies: " + amountToKill);
        Villager[] v = GameObject.FindObjectsOfType<Villager>();
        for(int i = 0; i < amountToKill && i < currentPopulation; ++i)
            GameObject.Destroy(v[i].gameObject);
		currentPopulation -= amountToKill;
	}

	public void populationFoodConsumptionAndStarvation(){
		villageIsStarving = false; //set to false, simply for start. If foodDemand > foodStores, then isStarving = true;
		//population should consume food
		int currentAvailableFood = Mathf.FloorToInt(center.amountOfAvailableFood() * Globals.foodConsumptionPerPerson);
		desiredAmountOfFood = Mathf.FloorToInt(currentPopulation * Globals.foodConsumptionPerPerson);
		if (desiredAmountOfFood < currentAvailableFood) {
			//should have enough food to eat, so lets calculate the food left after we eat it (because we will need it for population growth)
			netFoodAfterEating = currentAvailableFood - desiredAmountOfFood;
			//will choose food at random until no more food is required.
			int workingFoodDesire = desiredAmountOfFood;
			Debug.Log ("Desired Amount of Food: " + desiredAmountOfFood + "     actual amount of food products: " + center.amountOfAvailableFood());
			//while workingFoodDesire is still above 0, ie. population is still hungry.


			int counter = 0;
			while (workingFoodDesire > 0) {
				//at VillageCenter, subtract resource from storage, choosing randomly from all known foodTypes, one at a time.
				if (center.subtractResourceFromStorage ((Globals.product) Globals.foodTypeProduce[Random.Range (0, Globals.foodTypeProduce.Length-1)], 1)) {
					workingFoodDesire--;
				} else {
					//we were not able to subtract resource from storage due to quantity
					Debug.Log("Tried to take food type resource from storage, but we dont have that resource.");
					counter++;
					if (counter > 15)
						break;
				}
			}
		} else {
			//in event of food demand being less than actual food
			//population should starve, decrease happiness
			villageIsStarving = true;
			Debug.Log ("Village is starving. Current Available Food is " + currentAvailableFood);

			//so, mostRecentDesiredFoodConsumption > amountOfAvailableFood = foodStillWanted
					//int amountDesiredFood = mostRecentDesiredFoodConsumption - currentAvailableFood; //NOT SURE WHY I HAD THIS
			//therefore we can take safely amountOfAvaialeFood from VillageStorage
			//therefore, take what food is available in storage
			int tempAvailableFood = currentAvailableFood;
			int counter = 0;
			while (tempAvailableFood > 0) {
				//at VillageCenter, subtract resource from storage, choosing randomly from all known foodTypes, one at a time.
				if (center.subtractResourceFromStorage ((Globals.product) Globals.foodTypeProduce [Random.Range (0, Globals.foodTypeProduce.Length-1)], 1)) {
					tempAvailableFood--;
				} else {
					//we were not able to subtract resource from storage due to quantity
					//Debug.Log("Tried to take food type resource from storage, but we dont have that resource.");
					counter++;
					if (counter > 15)
						break;
				}
			}
			//so, some percent of population went without food, and so some should die	
			//currentPopulation is culled by peopleWhoWentWithoutFood * %whoDieWhenStarving
			int foodShortage = desiredAmountOfFood - currentAvailableFood;
			killPopulation(Mathf.FloorToInt(foodShortage * Globals.foodConsumptionPerPerson * Globals.percentOfStarvingWhoDie));
		}
	}

    public override string ToString()
    {
        return "Pop values [pop,avrgHp,avrgHappy,avrgHealthiness] = [" + currentPopulation + "," + averagePercentLifePoints + "," + averageHappiness + "," + averageHealthiness + "]";
    }
}
