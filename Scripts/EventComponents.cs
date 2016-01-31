using UnityEngine;
using System.Collections;

public class EventComponents{

	//public Globals.energyTypes[] elemets = new Globals.energyTypes[4];
	public float[] power = new float[4];
	// [0] eather - air, -10 eather and if 10 air

	public EventComponents(float[] powerIn){
		power = powerIn;
	}

	public Globals.energyTypes retEnergyTypeFromIndex(int index){
		return (Globals.energyTypes)index;
	}
		

	public float[] retAbsolutePower(){
		float[] tempArr = new float[4];
		for (int count = 0; count < 4; count++) {
			tempArr [count] = Mathf.Abs (power [count]);
		}
		return tempArr;
	}

	public int[] getIndexOfTwoAbsMax(){
		float[] tempArr = retAbsolutePower ();
		int indexOfAbsMax = 0;
		int indexOfSecAbsMax = 1;
		float absMax = tempArr [0];
		float secAbsMax = tempArr [1];
		for(int count = 2; count < 4; count++){
			if (tempArr [count] > absMax) {
				if (absMax > secAbsMax) {
					secAbsMax = absMax;
					indexOfSecAbsMax = indexOfAbsMax;
				}
				absMax = tempArr [count];
				indexOfAbsMax = count;
			} else {
				if (tempArr [count] > secAbsMax)
					secAbsMax = tempArr[count];
					indexOfSecAbsMax = count;
			}
		}
		int[] absMaxes = new int[2];
		absMaxes [0] = indexOfAbsMax;
		absMaxes [1] = indexOfSecAbsMax;
		return absMaxes;
	}

	public float[] getValueOfMaxes(){
		int[] tempIndex = getIndexOfTwoAbsMax ();
		float[] valueOfMaxes = new float[2];
		valueOfMaxes[0] = power[tempIndex[0]];
		valueOfMaxes [1] = power [tempIndex [1]];
		return valueOfMaxes;
	}


}
