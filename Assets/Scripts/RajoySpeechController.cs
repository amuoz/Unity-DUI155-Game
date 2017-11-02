using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RajoySpeechController : MonoBehaviour {

    // array de sonidos de rajoy
    public AudioSource[] rajoyTracks;

    private SpriteRenderer sprite;

    private PlayerController player;

    public Image rajoyRunning;

    public float timeBetweenTalk;
    private float timeBetweenTalkCounter;
    private bool speedBoost;

    // tiempo de speed boost
    public float timeSpeedBoost;
    private float timeSpeedBoostCounter;

    // es el tiempo de speed boost
    private float timeToTalkCounter;

    private bool talking;
    private int currentTrack;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();

        sprite.enabled = false;
        rajoyRunning.enabled = false;

        speedBoost = false;
        talking = false;
        currentTrack = Random.Range(0, rajoyTracks.Length-1);

        timeBetweenTalkCounter = Random.Range(timeBetweenTalk * 0.75f, timeBetweenTalk * 1.25f);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (talking)
        {
            timeToTalkCounter -= Time.deltaTime;
            if (timeToTalkCounter < 0f)
            {
                talking = false;
                sprite.enabled = false;

                timeBetweenTalkCounter = Random.Range(timeBetweenTalk * 0.75f, timeBetweenTalk * 1.25f);
            }
        }
        else
        {
            timeBetweenTalkCounter -= Time.deltaTime;

            // activamos rajoy cuando pasa el tiempo aleatorio
            if (timeBetweenTalkCounter < 0f)
            {
                talking = true;

                // el tiempo hablando tiene que ser según duración del track
                timeToTalkCounter = rajoyTracks[currentTrack].clip.length+1f;

                rajoyTracks[currentTrack].Play();

                // calculamos el siguiente track
                currentTrack++;
                if (currentTrack == rajoyTracks.Length)
                {
                    currentTrack = 0;
                }
                
                // mostrar rajoy
                sprite.enabled = true;

                // aplicar boost de velocidad al personaje
                player.currentSpeed += 10f;
                timeSpeedBoostCounter = timeSpeedBoost;
                speedBoost = true;
                rajoyRunning.enabled = true;
            }

            
            if (speedBoost)
            {
                timeSpeedBoostCounter -= Time.deltaTime;
            }

            if(timeSpeedBoostCounter <= 0)
            {
                speedBoost = false;
                timeSpeedBoostCounter = timeSpeedBoost;
                player.currentSpeed = player.speed;
                rajoyRunning.enabled = false;
            }
            

        }
        
    }
}
