using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    // referencia a prefab de la caja
    public GameObject boxPrefab;
    public int tileSize = 32;
    public float boxChance = 0.1f;

    public GameObject monsterPrefab;
    public int monsterSize = 5;

    // referencia al jugador
    public PlayerController player;

    private List<MonsterController> monsters;

    private bool killedMonster;

	// Use this for initialization
	void Start () {
        monsters = new List<MonsterController>();

        for (int y = Screen.height / 2 - tileSize; y > -Screen.height / 2 + tileSize * 2; y-= tileSize)
        {
            for(int x = Screen.width / 2 - tileSize; x > -Screen.width / 2 + tileSize; x -= tileSize)
            {
                if(Random.value < boxChance)
                {
                    GameObject boxInstance = Instantiate(boxPrefab);
                    boxInstance.transform.SetParent(transform);
                    // convert to unity units
                    boxInstance.transform.position = new Vector2((x - tileSize /2) / 100f, (y - tileSize/2) / 100f);
                }
            }
        }

        // add monsters
        MonsterController previousMonster = null;
        for(int i = 0; i < monsterSize; i++)
        {
            GameObject monsterInstance = Instantiate(monsterPrefab);
            monsterInstance.transform.SetParent(transform);
            monsterInstance.transform.position = new Vector2(
                -i * tileSize / 100f, 
                (Screen.height / 2 - tileSize / 2) / 100f);

            MonsterController monster = monsterInstance.GetComponent<MonsterController>();
            monster.OnKill += OnMonsterKill;

            if(previousMonster != null)
            {
                previousMonster.next = monster;
            } else
            {
                monsters.Add(monster);
            }

            previousMonster = monster;
        }
       
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player == null)
        {
            SceneManager.LoadScene("Game");
        }

        if(monsters.Count == 0)
        {
            SceneManager.LoadScene("Game");
        }

        killedMonster = false;
	}

    void OnMonsterKill(MonsterController monster)
    {
        if(killedMonster == true)
        {
            return;
        }

        killedMonster = true;

        MonsterController currentMonster = monster;
        if(monster.next != null)
        {
            List<MonsterController> monsterString = new List<MonsterController>();
            while(currentMonster.next != null)
            {
                monsterString.Add(currentMonster);
                currentMonster.ChangeDirection();
                currentMonster = currentMonster.next;
            }
            monsterString.Add(currentMonster);

            for(int i = monsterString.Count - 1; i > 0; i--)
            {
                monsterString[i].next = monsterString[i - 1];
            }
            monsterString[0].next = null;

            // cambiar direccion del ultimo monstruo
            currentMonster.ChangeDirection();
            monsters.Add(currentMonster);
        }

        if(monsters.IndexOf(monster) != -1)
        {
            monsters.Remove(monster);
        }

        Destroy(monster.gameObject);
    }
}
