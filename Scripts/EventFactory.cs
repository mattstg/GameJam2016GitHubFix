using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventFactory : MonoBehaviour
{
    #region singleton
    private static EventFactory instance;

   private EventFactory() { }

   public static EventFactory Instance
   {
      get 
      {
         if (instance == null)
         {
             instance = new EventFactory();
         }
         return instance;
      }
   }
    #endregion


   public string CreateEvent(Vector2 mousePos,Dictionary<Globals.energyTypes, float> _temp)
   {
       /*Dictionary<Globals.energyTypes, float>  energyStored = new Dictionary<Globals.energyTypes, float>();
       foreach (KeyValuePair<Globals.energyTypes, float> kv in _temp)
       {



       }
		*/
		float[] tempArr = new float[4];
		foreach(KeyValuePair<Globals.energyTypes, float> kv in _temp){
			tempArr [(int)kv.Key] = kv.Value;
		}


		Dictionary<Globals.energySubTypes, float> subtypeEnergy = SeperateIntoSubtypes(_temp);
       //Debug.Log("here");
       //Now make event!
	   
       EventBrain evBrain = (Instantiate(Resources.Load("EventPrefab"), mousePos, Quaternion.identity) as GameObject).GetComponent<EventBrain>();
       evBrain.Initialize(subtypeEnergy);
	   
		string descriptionOfEvent = new EventDescriber (tempArr).OutputStringRepresentingEvent();
		GameObject.FindObjectOfType<VillageCenter> ().TheListener.RecordString (descriptionOfEvent);

		//EventDescriber temp = new EventDescriber ();
		return descriptionOfEvent;
   }

   private Dictionary<Globals.energySubTypes, float> SeperateIntoSubtypes(Dictionary<Globals.energyTypes, float> energyDict)
   {
       Dictionary<Globals.energySubTypes, float> toReturn = new Dictionary<Globals.energySubTypes,float>();
       List<KeyValuePair<Globals.energySubTypes, float>> result;
       foreach(KeyValuePair<Globals.energyTypes,float> kv in energyDict)
       {
           result = Globals.SplitIntoEnergySubTypes(kv);
           toReturn.Add(result[0].Key,Mathf.Abs(result[0].Value));
           toReturn.Add(result[1].Key,Mathf.Abs(result[1].Value));
       }
       return toReturn;
   }



	public string CreateRandomEvent(float[] bluePrint, float power){
		Dictionary<Globals.energyTypes, float> randomDict = new Dictionary<Globals.energyTypes, float>();
		//need to generate float[4] and interpret into energyTypes and add to dictionary
		float individualPower = power/4;
		float[] temp = {
			Random.Range (-individualPower, individualPower),
			Random.Range (-individualPower, individualPower),
			Random.Range (-individualPower, individualPower),
			Random.Range (-individualPower, individualPower)
		}; 

		EventDescriber eventManipulator = new EventDescriber (bluePrint);
		for (int c = 0; c < 4; c++) {
			randomDict.Add ((Globals.energyTypes)c, bluePrint [c]);
		}
		//x range {-15 --> 15} y range {8 --> -8}
		return CreateEvent (new Vector2(Random.Range(-15,15), Random.Range(8,-8)), randomDict);
	}

	public string CreateEventFromFloat(float[] bluePrint){
		Dictionary<Globals.energyTypes, float> randomDict = new Dictionary<Globals.energyTypes, float>();
		EventDescriber eventManipulator = new EventDescriber (bluePrint);
		for (int c = 0; c < 4; c++) {
			randomDict.Add ((Globals.energyTypes)c, bluePrint [c]);
		}
		//x range {-15 --> 15} y range {8 --> -8}
		return CreateEvent (new Vector2(Random.Range(-15,15), Random.Range(8,-8)), randomDict);
	}
}
