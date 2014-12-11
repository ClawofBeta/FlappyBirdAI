using UnityEngine;
using System.Collections;

/// <summary>
/// Spritesheet for Flappy Bird found here: http://www.spriters-resource.com/mobile_phone/flappybird/sheet/59537/
/// Audio for Flappy Bird found here: https://www.youtube.com/watch?v=xY0sZUJWwA8
/// </summary>
public class FlappyScript : MonoBehaviour
{

    public AudioClip FlyAudioClip, DeathAudioClip, ScoredAudioClip;
    public Sprite GetReadySprite;
    public float RotateUpSpeed = 1, RotateDownSpeed = 1;
    public GameObject IntroGUI, DeathGUI;
    public Collider2D restartButtonGameCollider;
	public float VelocityPerJump;
    public float XSpeed;

	public int player_state; //0 = start 1 = playing 2 = dead
	public neuralNet nn;
	public GameObject next_pipe;
	public GameObject pipe_placeholder;
	public int own_score;

    // Use this for initialization
    void Start()
    {
		XSpeed = 1.3f;
		VelocityPerJump = 2.0f;
		own_score = 1;
    }

    FlappyYAxisTravelState flappyYAxisTravelState;

    enum FlappyYAxisTravelState
    {
        GoingUp, GoingDown
    }

    Vector3 birdRotation = Vector3.zero;
    // Update is called once per frame
    void Update()
    {	
		if(next_pipe.Equals(null)){
			get_next_pipe();
		}
		else if (next_pipe.Equals(pipe_placeholder)){
			get_next_pipe();
		}
		else{
			float x = next_pipe.transform.position.x - this.transform.position.x;
			if (x < 0){
				get_next_pipe();
			}
		}


        //handle back key in Windows Phone
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (player_state == 0)
        {
            MoveBirdOnXAxis();
            if (WasTouchedOrClicked())
            {
				GameStateManager.GameState = GameState.Playing;
				BoostOnYAxis();
				player_state = 1;
                IntroGUI.SetActive(false);
                ScoreManagerScript.Score = 0;
            }
        }

        else if (player_state == 1)
        {
            MoveBirdOnXAxis();
            //if (WasTouchedOrClicked())
			if(next_pipe.Equals(null)){
				float px = pipe_placeholder.transform.position.x - this.transform.position.x;
				float py = pipe_placeholder.transform.position.y - this.transform.position.y;
				if(nn.compute(px,py))
				{
					BoostOnYAxis();
				}
			}

			else {
				float dx = next_pipe.transform.position.x - this.transform.position.x;
				float dy = next_pipe.transform.position.y - this.transform.position.y;
				if(nn.compute(dx,dy)){
                BoostOnYAxis();
				}
            }

        }

        else if (player_state == 2)
        {
            Vector2 contactPoint = Vector2.zero;

            if (Input.touchCount > 0)
                contactPoint = Input.touches[0].position;
            if (Input.GetMouseButtonDown(0))
                contactPoint = Input.mousePosition;

            //check if user wants to restart the game
            if (restartButtonGameCollider == Physics2D.OverlapPoint
                (Camera.main.ScreenToWorldPoint(contactPoint)))
            {
                GameStateManager.GameState = GameState.Intro;
                Application.LoadLevel(Application.loadedLevelName);
            }
        }

    }


    void FixedUpdate()
    {
        //just jump up and down on intro screen
        if (GameStateManager.GameState == GameState.Intro)
        {
            if (rigidbody2D.velocity.y < -1) //when the speed drops, give a boost
                rigidbody2D.AddForce(new Vector2(0, rigidbody2D.mass * 5500 * Time.deltaTime)); //lots of play and stop 
                                                        //and play and stop etc to find this value, feel free to modify
        }
        else if (player_state == 1 || player_state == 2)
        {
            FixFlappyRotation();
        }
    }

	void get_next_pipe(){
		GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipeblank");
		if(pipes.Length > 0){

			GameObject pipe_comp = pipe_placeholder;
			float dx = pipe_comp.transform.position.x - this.transform.position.x;

			for(int i = 0; i < pipes.Length; i++){
				GameObject pipe = pipes[i];
				if(!next_pipe.Equals(pipe)){
					float n_dx = pipe.transform.position.x - this.transform.position.x;
					if ((n_dx > 0) && (n_dx < dx)){
						pipe_comp = pipe;
						dx = n_dx;
					}
				}
			}

			if(!pipe_comp.Equals(pipe_placeholder)){
				next_pipe = pipe_comp;
			}
		}

	}

    bool WasTouchedOrClicked()
    {
        if (Input.GetButtonUp("Jump") || Input.GetMouseButtonDown(0) || 
            (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended))
            return true;
        else
            return false;
    }

    void MoveBirdOnXAxis()
    {
        transform.position += new Vector3(Time.deltaTime * XSpeed, 0, 0);
    }

    void BoostOnYAxis()
    {
        rigidbody2D.velocity = new Vector2(0, VelocityPerJump);
        audio.PlayOneShot(FlyAudioClip);
    }



    /// <summary>
    /// when the flappy goes up, it'll rotate up to 45 degrees. when it falls, rotation will be -90 degrees min
    /// </summary>
    private void FixFlappyRotation()
    {
        if (rigidbody2D.velocity.y > 0) flappyYAxisTravelState = FlappyYAxisTravelState.GoingUp;
        else flappyYAxisTravelState = FlappyYAxisTravelState.GoingDown;

        float degreesToAdd = 0;

        switch (flappyYAxisTravelState)
        {
            case FlappyYAxisTravelState.GoingUp:
                degreesToAdd = 6 * RotateUpSpeed;
                break;
            case FlappyYAxisTravelState.GoingDown:
                degreesToAdd = -3 * RotateDownSpeed;
                break;
            default:
                break;
        }
        //solution with negative eulerAngles found here: http://answers.unity3d.com/questions/445191/negative-eular-angles.html

        //clamp the values so that -90<rotation<45 *always*
        birdRotation = new Vector3(0, 0, Mathf.Clamp(birdRotation.z + degreesToAdd, -90, 45));
        transform.eulerAngles = birdRotation;
    }

    /// <summary>
    /// check for collision with pipes
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (player_state == 1)
        {
            if (col.gameObject.tag == "Pipeblank") //pipeblank is an empty gameobject with a collider between the two pipes
            {
                audio.PlayOneShot(ScoredAudioClip);
                ScoreManagerScript.Score++;
				//get_next_pipe();
				this.own_score++;
            }
            else if (col.gameObject.tag == "Pipe")
            {
                FlappyDies();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (player_state == 1)
        {
            if (col.gameObject.tag == "Floor")
            {
                FlappyDies();
            }
        }
    }

    void FlappyDies()
    {
		player_state = 2;
        //DeathGUI.SetActive(true);
        audio.PlayOneShot(DeathAudioClip);
    }

}
