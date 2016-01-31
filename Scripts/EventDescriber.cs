using UnityEngine;
using System.Collections;
using System.Linq;

public class EventDescriber{

	//public float[] elements = new float[4];
	//elements will define the effectiveness against each biome and humans/animals/buildings
	//public string[] elementQualifiers = {"ether", "air", "fire", "water", "light", "dark", "critter/insect", "beast"};
	//each series of two strings indicate the element and its opposite.

	public EventComponents linkToCalc;
	public float[] absMax = new float[2];
	public int[] absMaxIndexes = new int[2];

	public EventDescriber(float[] input){
		linkToCalc = new EventComponents (input);
		absMaxIndexes = linkToCalc.getIndexOfTwoAbsMax ();
		absMax = linkToCalc.getValueOfMaxes ();
	}
		
	//GetEnergySubType --> first need to get energy Type

	//public float getEnergyPowerFromIndex(int index){
	//
	//}

	public string[] getDescribers(){
		//Globals.energyTypes et = 0;
		string[] tempString = new string[2];
		tempString[0] = retMainDescriptors ((Globals.energyTypes) absMaxIndexes[0], absMax[0]);
		tempString[1] = retSecondaryDescriptors ((Globals.energyTypes) absMaxIndexes[1], absMax[1]);
		return tempString;
	}

	//note as power decreases from index 0 to 3. Need 3 adjectives for each.

	//primaties
    public static string[] etherMainDescriber = { "thick black fog", "lake of smoke" };
    public static string[] airMainDescriber = { "hurricane", "squall" };
    public static string[] fireMainDescriber = { "fire Storm", "wild fire" };
    public static string[] waterMainDescriber = { "torrential downpour", "thunderstrom" };
    public static string[] lightMainDescriber = { "devine presence", "brilliant aura" };
    public static string[] darkMainDescriber = { "diabolical presence", "unholy aura" };
    public static string[] critterMainDescriber = { "plague of Locusts", "swarm of Rats" };
    public static string[] beastMainDescriber = { "pack of hungry Wolves", "roaming herd of Buffalo" };

	//secondaries
	public static string[] etherAdjectives = {"volotile", "strangly powerful", "etherreal"};
	public static string[] airAdjectives = {"gale force", "howling", "windy" };
	public static string[] fireAdjectives = { "infernal", "ardent", "heated" };
	public static string[] waterAdjectives = { "drenching", "moist", "damp" };
	public static string[] lightAdjectives = {"blinding", "holy", "bright"};
	public static string[] darkAdjectives = {"jet black", "unholy" ,"dim"};
	public static string[] critterAdjectives = { "teaming", "crawling", "pesky"};
	public static string[] beastAdjectives = { "rampaging", "stampeding", "beastial" };

	public string OutputStringRepresentingEvent(){
		string[] qualifiers = getDescribers ();
		string statement = "A " + qualifiers[1] + " " + qualifiers[0] + "has descended upon our village!";
		return statement;
	}


	public string[][] secondaryAdjectives = {
		etherAdjectives,
		airAdjectives,
		fireAdjectives,
		waterAdjectives,
		lightAdjectives,
		darkAdjectives,
		critterAdjectives,
		beastAdjectives
	};

	public string[][] mainDescriptors = {
		etherMainDescriber,
		airMainDescriber,
		fireMainDescriber,
		waterMainDescriber,
		lightMainDescriber,
		darkMainDescriber,
		critterMainDescriber,
		beastMainDescriber
	};



	public string retMainDescriptors(Globals.energySubTypes type, float power){
		int intType = (int)type; 
		power = Mathf.Abs (power);
		if (power > 20) {
			power = 0;
		}else{
			power = 1;
		}
		return mainDescriptors[intType][(int)power];
	}

	public string retMainDescriptors(Globals.energyTypes eng, float power){
		return retMainDescriptors(Globals.GetEnergySubType(eng,power),power);
	}

	public string retSecondaryDescriptors(Globals.energySubTypes type, float power){
		int intType = (int)type; 
		power = Mathf.Abs (power);
		if (power > 20) {
			power = 0;
		}else if(power > 10){
			power = 1;
		}else{
			power = 2;
		}
		return secondaryAdjectives[intType][(int)power];
	}

	public string retSecondaryDescriptors(Globals.energyTypes eng, float power){
		return retSecondaryDescriptors (Globals.GetEnergySubType (eng, power),power);
	}



	//(1) when player summon a storm(thing) have it described
	//(2) have npc describe something to player in witch hut scene
	//(3) 

	//public EventComponents workingEvent = new EventComponents ();
}
