using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Globals : MonoBehaviour {
	public static float biomeProductivityCoefficient = 5f;
	public static float populationYeildBonus = .02f; //productivity boost per population, so at 50 popProdBonus = 1, 100 = 2, 150 = 3 (150 is max)
	public static int maxPopulation = 150;
	public static float percentGivenToWitch = 0.1f; 
	public static int startPopulation = 30; 
	public static float startAverageHappiness = 0.7f; //range from 0 - 1 (0 being very sad, 1 being very happy)
	public static float startAverageHealthiness = 0.7f; //.7  range 0 - 1 (0 being very ill, 1 being very well)
	public static float startAverageLifePoints = 0.9f; //range 0 - 1 (0 being dead, 1 being full Life Points)
	public static int maximumVillagerHealthPoints = 10;
	public static float residentsPerHouse = 5; //can function with fractions
	public static int startHouses = 10; 
	public static int woodPerHouse = 5;
	public static int floatingHouses = 1; //amount of houses population will try to have excess
	public static float foodConsumptionPerPerson = 0.25f; //amount of food consumed per person, rounded down in formula
	public static float percentOfStarvingWhoDie = 0.5f; //percent of starving population who die
	public static float maxPercentPopulationIncrease = 0.2f; //20% increase in population maximum
    public static int MaxElementPower = 5;
    public static int MaxElementsPerIngredient = 3;
    public static int[] PeoplePerCircleLevel = { 6, 12, 16, 18 };
    public static float ItemPlacementRangeX = 1;
    public static float ItemPlacementRangeY = 4;
    public static float ItemPlacementRangeGrowth = 1;
	public static int minimumTaxableAmountOfProduce = 6;
    public static float PopulationMutationRate = .2f;
    public static float mapRadiusY = 4;
    public static float mapRadiusX = 7;
    public static float mapRadiusYforHousePlacement = 4;
    public static float mapRadiusXforHousePlacement = 4;
    public static float contentThreshold = .7f; //a villager at 80% happy is content
    public static float contentExcessMultiplier = .5f; //per .1 pass threshold, be affected by .05
    public static float worldDamageReduction = .1f;
    public static int daysBetweenEventSpawn = 2;
    public static float powerIncremenetPerDay = 1;
    public static float basePower = 5;


    
    public static readonly float lengthOfScenario = 20f;
    private static readonly float villagerUpdateCyclesPerScenario = 2;
    public static float villageUpdateCounterMax =  lengthOfScenario / villagerUpdateCyclesPerScenario;

	//******************************* Event Globals
	public static float eventDecay = 0.15f; //percent decay every Time.delta
	public static float eventSwerveVariance = 0.5f; //max percent direction change on x and y place for event movement
	public static float minimumPower = 0.5f;
    

    public enum worshipperStates { Dance, Chant, COUNT };

	public enum product {Mushrooms = 0, Wood = 1, Daisy = 2, Rot = 3, StinkWeed = 4, Frog = 5, Potatoe = 6, Carrot = 7, Bean = 8, Cow = 9, Chicken = 10, Manure = 11,
		Fish = 12, Seaweed = 13, WaterLilly = 14, MountainHerb = 15, Stone = 16, Gold = 17};
	public static int numberOfProduct = 18;
	public static int[] foodTypeProduce = {6,7,8,9,10,12};
	public static int[] herbTypeProduce = {0,2,4,14,15};
    public enum energyTypes { ether_air = 0, fire_water = 1, dark_light = 2, critter_beast = 3 };
    public enum energySubTypes { ether = 0, air = 1, fire = 2, water = 3,dark = 4,light = 5, critter = 6, beast = 7};

	public enum biome {Forest = 0, Bog = 1,  Farmland = 2, Ranch = 3, Lake = 4, Mountain = 5};
	public static int numberOfBiomes = 6;
	public static product[] retBiomeResources(biome biome, int startPosition){ 
		product[] toRet = new product[3];
		//Debug.Log ("Biome being initialized: " + biome);
		//Debug.Log ("start position: " + startPosition);
		for (int counter = startPosition; counter < startPosition + 3; counter++) {
			//Debug.Log("counter = " + counter + "|||||| startPosition " + startPosition);
			toRet [counter - startPosition] = (product) counter;
		}
		return toRet;
	}

    public static energySubTypes GetEnergySubType(energyTypes et, float pwr)
    {
        if (pwr < 0)
            return (energySubTypes)((int)et * 2);
        else
            return (energySubTypes)((int)et * 2 + 1);
    }

    public static List<KeyValuePair<energySubTypes, float>> SplitIntoEnergySubTypes(KeyValuePair<energyTypes, float> t)
    {
        List<KeyValuePair<energySubTypes, float>> toReturn = new List<KeyValuePair<energySubTypes, float>>();
        if (t.Value == 0)
        {
            KeyValuePair<energySubTypes, float> positiveOne = new KeyValuePair<energySubTypes, float>(GetEnergySubType(t.Key, 10), 0);
            KeyValuePair<energySubTypes, float> zeroOne = new KeyValuePair<energySubTypes, float>(GetEnergySubType(t.Key, -10), 0);
            toReturn.Add(positiveOne);
            toReturn.Add(zeroOne);
        }
        else
        {
            KeyValuePair<energySubTypes, float> positiveOne = new KeyValuePair<energySubTypes, float>(GetEnergySubType(t.Key, t.Value), t.Value);
            KeyValuePair<energySubTypes, float> zeroOne = new KeyValuePair<energySubTypes, float>(GetEnergySubType(t.Key, t.Value * -1), 0);
            toReturn.Add(positiveOne);
            toReturn.Add(zeroOne);
        }
        return toReturn;
    }

    public static void limit(ref float value,float lowLim,float highLim)
    {
        if (value < lowLim)
            value = lowLim;
        if (value > highLim)
            value = highLim;
    }
    

    public static Vector2 AddVec(Vector3 v1,Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }

    public static bool CompareVec(Vector3 v1, Vector2 v2)
    {
        return (v1.x == v2.x) && (v1.y == v2.y);
    }

    public static string PrintDictionary<K, V>(Dictionary<K, V> toPrint, string name = "Dictionary")
    {
        string toRet = name + " contents: ";
        try
        {
            foreach (KeyValuePair<K, V> kv in toPrint)
                toRet += "[" + kv.Key.ToString() + "," + kv.Value.ToString() + "],";
        }
        catch
        {
            toRet = "The dictionary you have tried to print " + name + " could not be printed, it is assumed it's types are not castable into strings";
        }
        return toRet;

    }
}