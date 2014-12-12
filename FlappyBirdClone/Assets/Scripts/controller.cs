using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

	public FlappyScript[] flappies;
	public float mutation_level;
	public Transform[] anchors;
	public GameObject explo;

	public float[] Xweights;
	public float[] Yweights;

	// Use this for initialization
	void Start () {
		//causes flappies to ignore each other
		Physics2D.IgnoreLayerCollision(8,8,true);
		Xweights = new float[flappies.Length];
		Yweights = new float[flappies.Length];
	}
	
	// Update is called once per frame
	void Update () {

		if(GameStateManager.GameState == GameState.Playing){
			bool check_dead = true;

			for(int i = 0; i < flappies.Length; i++){
				check_dead = check_dead && (flappies[i].player_state == 2);
			}

			if(check_dead){
				print ("respawning");
				GameStateManager.GameState = GameState.Remaking;
				Invoke ("respawn", 5f);

			}

		}
	}

	void respawn(){
		//GameStateManager.GameState = GameState.Remaking;
		//print ("1");
		//yield return new WaitForSeconds(5);
		print ("2");
		int tot_score = 0;
		for (int i = 0; i < flappies.Length; i++){
			tot_score += flappies[i].own_score;
		}


		for(int j = 0; j < neural_nets.Length; j++){
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
			neural_nets[j] = new neuralNet();
			float n_wx = flappy_parent.wx + Random.Range(-mutation_level, mutation_level);
			float n_wy = flappy_parent.wy + Random.Range(-mutation_level, mutation_level);
			neural_nets[j].wx = n_wx;
			neural_nets[j].wy = n_wy;

		}

		for (int k = 0; k < flappies.Length; k++){
			FlappyScript fs = flappies[k];
			fs.own_score = 1;
			fs.nn.wx = neural_nets[k].wx;
			fs.nn.wy = neural_nets[k].wy;
			fs.next_pipe = fs.pipe_placeholder;
			fs.transform.position = anchors[k].position;
			fs.player_state = 1;

			Instantiate(explo, fs.transform.position, Quaternion.identity);
		}

		GameStateManager.GameState = GameState.Playing;
	}
}
