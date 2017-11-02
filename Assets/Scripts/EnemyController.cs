using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour {

    public float moveSpeed;

    private Rigidbody2D myRigidBody;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private bool moving;

    private Vector2 moveDirection;

    public float waitToReload;

    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            //This will clamp how far up/down/left/right we can go in LOCAL space
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -29, 29), Mathf.Clamp(transform.position.y, -15, -6));

            timeToMoveCounter -= Time.deltaTime;
            myRigidBody.velocity = moveDirection;

            if (timeToMoveCounter < 0f)
            {
                moving = false;
                //timeBetweenMoveCounter = timeBetweenMove;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }
        }
        else
        {
            myRigidBody.velocity = Vector2.zero;
            timeBetweenMoveCounter -= Time.deltaTime;

            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                //timeToMoveCounter = timeToMove;
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

                moveDirection = new Vector2(Random.Range(-0.5f, 0.5f) * moveSpeed, Random.Range(-0.5f, 0.5f) * moveSpeed);
            }
        }

    }

}