using UnityEngine;
using System.Collections;

public class speeder : MonoBehaviour {

	public int inc;
	public controller central;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){

			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(this.collider2D == Physics2D.OverlapPoint(pos)){
				if(central.speed < 9 && inc > 0){
					central.speed += inc;
					Time.timeScale = central.speed;
				}

				if(central.speed > 1 && inc < 0){
					central.speed += inc;
					Time.timeScale = central.speed;
				}

			}
				
		}
	}
}
