using UnityEngine;
using System.Collections;

public class neuralNet : MonoBehaviour {

	public float wy; //Y DISTANCE FROM SWEETSPOT TO PIPE, AKA HEIGHT OF PIPE - HEIGHT OF SWEETSPOT)
	public float wx; //X DISTANCE FROM SWEETSPOT TO PIPE, AKA LENGTH FROM PIPE TO BIRD - LENGTH FROM SWEETSPOT TO BIRD)

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//returns 1 if the horizontal distance from bird to sweetspot is less than
	//the horizontal distance from bird to pipe, 0 otherwise
	//if distance from bird to sweepspot is negative, though, always return 0
	public int closerSweetSpot(float x_input){
		float bird_to_spot = x_input - wx;
		if (bird_to_spot < 0)
			return 0;
		else{
			return 1;
		}
	}
	// 
	public int isLowerThanPipe(float y_input){
		if (y_input > 0)
			return 1;
		else
			return 0;
	}

	public int isLowerThanSweetSpot(float y_input){
		if (y_input - wy > 0)
			return 1;
		else
			return 0;
	}

	public bool SweetSpotOrPipe(int LowerPipe, int LowerSweetSpot, int closer){
		return ((LowerSweetSpot * closer) + (LowerPipe * (1 - closer)) > 0);
		}

	public bool compute(float x_input, float y_input){
		int LowerPipe = isLowerThanPipe (y_input);
		int LowerSweetSpot = isLowerThanSweetSpot (y_input);
		int closer = closerSweetSpot (x_input);

		return SweetSpotOrPipe (LowerPipe, LowerSweetSpot, closer);
		//return LowerPipe > 0;
		//return y_input > 0;

	}
}
