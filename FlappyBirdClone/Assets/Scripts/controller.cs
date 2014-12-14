using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

	public FlappyScript[] flappies;
	public float mutation_level;
	public Transform[] anchors;
	public GameObject explo;

	public float[] Xweights;
	public float[] Yweights;
	public float[] Hweights;

	public int speed;

	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		//causes flappies to ignore each other
		Physics2D.IgnoreLayerCollision(8,8,true);
		Xweights = new float[flappies.Length];
		Yweights = new float[flappies.Length];
		Hweights = new float[flappies.Length];
	}
	
	// Update is called once per frame
	void Update () {

		if(GameStateManager.GameState == GameState.Playing){
			bool check_dead = true;

			for(int i = 0; i < flappies.Length; i++){
				check_dead = check_dead && (flappies[i].player_state == 2);
			}

			if(check_dead){

				GameStateManager.GameState = GameState.Remaking;
				Invoke ("respawn", 6f);

			}

		}
	}

	void respawn(){
		//GameStateManager.GameState = GameState.Remaking;
		//print ("1");
		//yield return new WaitForSeconds(5);
		//print ("2");
		int tot_score = 0;
		for (int i = 0; i < flappies.Length; i++){
			tot_score += flappies[i].own_score;
		}


		for(int j = 0; j < flappies.Length; j++){
			int rand_val = Random.Range(0,tot_score);
			int sum = 0;
			neuralNet flappy_parent = null;

			for(int k = 0; k < flappies.Length; k++){
				sum += flappies[k].own_score;
				if(sum > rand_val){
					flappy_parent = flappies[k].nn;
					break;
				}
			}
			Xweights[j] = flappy_parent.wx;
			if (Random.Range(0, 10) >= 8){
				float n_wx = flappy_parent.wx + Random.Range(-mutation_level, mutation_level);
				Xweights[j] = n_wx;
			}
			Yweights[j] = flappy_parent.wy;
			if (Random.Range(0, 10) >= 8){
				float n_wy = flappy_parent.wy + Random.Range(-mutation_level, mutation_level);
				Yweights[j] = n_wy;
			}
			Hweights[j] = flappy_parent.wh;
			if (Random.Range(0,10) >= 8){
				float n_wh = flappy_parent.wh + Random.Range(-mutation_level, mutation_level);
				Hweights[j] = n_wh;
			}
		}

		//cross stuff
		for (int v = 0; v < flappies.Length; v += 2){
			int r1 = Random.Range(0,4);

			if(r1 >= 3){
				int r2 = Random.Range (0,3);
				if(r2 == 0){
					float old_weightX = Xweights[v];
					Xweights[v] = Xweights[v+1];
					Xweights[v+1] = old_weightX;

					float old_weightY = Yweights[v];
					Yweights[v] = Yweights[v+1];
					Yweights[v+1] = old_weightY;
				}
				else if (r2 == 1){
					float old_weightY = Yweights[v];
					Yweights[v] = Yweights[v+1];
					Yweights[v+1] = old_weightY;

					float old_weightH = Hweights[v];
					Hweights[v] = Hweights[v+1];
					Hweights[v+1] = old_weightH;
				}
				else{
					float old_weightX = Xweights[v];
					Xweights[v] = Xweights[v+1];
					Xweights[v+1] = old_weightX;

					float old_weightH = Hweights[v];
					Hweights[v] = Hweights[v+1];
					Hweights[v+1] = old_weightH;
				}

			}

			else if (r1 >= 2){
				int r2 = Random.Range (0,3);
				if(r2 == 0){
					float old_weightX = Xweights[v];
					Xweights[v] = Xweights[v+1];
					Xweights[v+1] = old_weightX;
				}
				else if (r2 == 1){
					float old_weightY = Yweights[v];
					Yweights[v] = Yweights[v+1];
					Yweights[v+1] = old_weightY;

				}
				else{
					float old_weightH = Hweights[v];
					Hweights[v] = Hweights[v+1];
					Hweights[v+1] = old_weightH;
				}

			}	

		}

		for (int k = 0; k < flappies.Length; k++){
			FlappyScript fs = flappies[k];
			fs.own_score = 1;
			fs.nn.wx = Xweights[k];
			fs.nn.wy = Yweights[k];
			fs.nn.wh = Hweights[k];

			sr = fs.GetComponent<SpriteRenderer>();
			sr.color = new Color(fs.nn.wx, fs.nn.wy, fs.nn.wh);

			fs.next_pipe = fs.pipe_placeholder;
			fs.transform.position = anchors[k].position;
			fs.player_state = 1;
			Instantiate(explo, fs.transform.position, Quaternion.identity);
		}
		ScoreManagerScript.Score = 0;
		GenManager.Score += 1;
		GameStateManager.GameState = GameState.Playing;
	}
}
