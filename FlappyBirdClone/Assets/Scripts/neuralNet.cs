using UnityEngine;
using System.Collections;

public class neuralNet : MonoBehaviour {

	public float wy; //Y DISTANCE FROM SWEETSPOT TO PIPE, AKA HEIGHT OF PIPE - HEIGHT OF SWEETSPOT)
	public float wx; //X DISTANCE FROM SWEETSPOT TO PIPE, AKA LENGTH FROM PIPE TO BIRD - LENGTH FROM SWEETSPOT TO BIRD)

	public float wh; //Y 

	// Use this for initialization
	void Start () {
	/*	wx = wx + Random.Range(-0.1f, 0.1f);
		wy = wy + Random.Range(-0.1f, 0.1f);
		wh = wh + Random.Range(-0.1f, 0.1f);*/

		wx = Random.Range(-0.5f, 1f);
		wy = Random.Range(-0.5f, 1f);
		wh = Random.Range(-0.5f, 1f);
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
		if (y_input - wh > 0)
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

	public int tooCloseToFloor(float h_input){
		if(h_input < -1.2f){
			return 1;
		}
		else{
			return 0;
		}

	}

	public int closeToPipe(float x_input, float y_input){
		if(x_input - y_input/wh - wx > 0){
		//if(x_input - wx > 0){
			return 1;
		}
		else{
			return 0;
		}

	}

	public bool SweetOrFloor(int floor_in, int sweet_in, int close_in){
		return ((floor_in + (sweet_in * (1 - close_in))) >= 0.5);

	}

	public bool SweetSpotOrPipe(int LowerPipe, int LowerSweetSpot, int closer){
		return ((LowerSweetSpot * closer) + (LowerPipe * (1 - closer)) > 0);
		}

	public bool compute(float x_input, float y_input, float h_input){
		//int LowerPipe = isLowerThanPipe (y_input);
		//int LowerSweetSpot = isLowerThanSweetSpot (y_input);
		//int closer = closerSweetSpot (x_input);

		//return SweetSpotOrPipe (LowerPipe, LowerSweetSpot, closer);


		int floor = tooCloseToFloor(h_input);
		int sweet = isLowerThanSweetSpot(y_input);
		int close = closeToPipe(x_input, y_input);

		return SweetOrFloor(floor, sweet, close);
	
	}
}
