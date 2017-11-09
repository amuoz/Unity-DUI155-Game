using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private PlayerController player;
    private Camera camara;

    public float speed;
    

    private float height;
    private float width;
    private float ratio;    // 16f/9f

    public float leftEdge;
    public float rightEdge;
    public float topEdge;
    public float bottomEdge;
    public float border;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        camara = GetComponent<Camera>();

        // camera aspect ratio 16:9
        ratio = camara.aspect;
        // the orthographicSize is half the size of the vertical viewing volume
        height = camara.orthographicSize * 2;
        // the horizontal size of the viewing volume depends on the aspect ratio
        width = camara.orthographicSize * ratio * 2;
    }
	
	// Update is called once per frame
	void Update () {
        
        // si el battle region esta activa la camara se queda fija en la region cuando llega al centro
        if (player.getBattleRegion() != null && player.getBattleRegion().isActive())
        {
            // si la camara ha llegado al centro de la region se debe quedar congelada
            if (transform.position.x < player.getBattleRegion().transform.position.x)
            {
                moveCamera();
            }
        }
        else
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            moveCamera();
        }

        leftEdge = transform.position.x - width / 2 + border;
        rightEdge = transform.position.x + width / 2 - border;
        bottomEdge = transform.position.y - height / 2 + border;
        topEdge = transform.position.y + height / 2 - border;
    }

    private void moveCamera()
    {
        // solo movemos la camara si vamos hacia delante, de momento mantenemos la camara bloqueada en vertical
        if (player.transform.position.x > transform.position.x)
        {
            // la camara se tiene que desplazar a la misma velocidad que el personaje
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

}
