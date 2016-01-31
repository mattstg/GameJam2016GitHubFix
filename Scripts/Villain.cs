using UnityEngine;
using System.Collections;

public class Villain{
	#region singleton
	private static Villain instance;

	public static Villain Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Villain();
			}
			return instance;
		}
	}
	#endregion

	/*createEvent(){

	}*/

	float[] bluePrintForEvent = new float[4];
	string eventDescription;
	int day;
	float power;
	bool willSpawnEventToday;

	public Villain(){
		willSpawnEventToday = false;
		day = 1;
		power = Globals.basePower;
	}

	public void incrementDay(){
		day++;
		power += Globals.powerIncremenetPerDay;
		if (day % Globals.daysBetweenEventSpawn == 0) {
			createEventBluePrint ();
			willSpawnEventToday = true;
			GameObject.FindObjectOfType<VillageCenter> ().TheListener.RecordString(eventDescription);
		} else {
			willSpawnEventToday = false;
		}
	}

	public string createEventBluePrint(){
		for (int c = 0; c < 4; c++) {
			bluePrintForEvent [c] = Random.Range (-power / 4, power / 4);
		}
		EventDescriber eventManipulator = new EventDescriber (bluePrintForEvent);
		eventDescription = eventManipulator.OutputStringRepresentingEvent();
		return eventDescription;
	}

	public string createEventFromBluePrint(){
		//needs to call eventFactory and pass them a value
		return EventFactory.Instance.CreateEventFromFloat(bluePrintForEvent);
	}
}
