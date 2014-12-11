using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cameraZ = transform.position.z;
	}

    float cameraZ;


	void Update () {
		if(GameStateManager.GameState == GameState.Intro){
			transform.position = new Vector3(Player.transform.position.x + 0.5f, 0, cameraZ);
		}
		else{
			//transform.position = new Vector3(this.transform.position.x + 0.009f, 0, cameraZ);
			transform.position += new Vector3(Time.deltaTime * Player.XSpeed, 0, 0);
		}
        
	}

    
    public FlappyScript Player;
}
