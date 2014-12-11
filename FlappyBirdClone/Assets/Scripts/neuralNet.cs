using UnityEngine;
using System.Collections;

public class neuralNet : MonoBehaviour {

	public float wy;
	public float wx;

	// Use this for initialization
	void Start () {
		wy += Random.Range(-.01f,0.1f);
		wx += Random.Range(-.01f,0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool compute(float x_input, float y_input){

		float y_out = func_y(y_input);
		float x_out = func_x(x_input);

		if(x_out * wx + y_out * wy > 0.2f){
			return true;
		}
		else{
			return false;
		}

	}

	float func_x(float input){
		if(input > 1){
			return 1;
		}
		else{
			return 0;
		}
	}

	float func_y(float input){


		if(input > 0.2f){
			return 1;
		}
		else{
			return 0;
		}

	}
	

}
