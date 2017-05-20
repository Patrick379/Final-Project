using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    private GameObject characterToMove;
    private bool playersTurn;
    private int numPlayers;
    private int numEnemies;
    private Dictionary<Vector2, GameObject> chars;
    // Use this for initialization
    void Start()
    {
        playersTurn = true;
        chars = new Dictionary<Vector2, GameObject>();
        numEnemies = 5;
        numPlayers = 5;
        Vector3 temp;
        int n = 1;
        GameObject character = GameObject.Find("Character " + n++);
        for (int i = 0; i < 10; i++)
        {

            for (int j = 0; j < 10; j++)
            {
                temp = new Vector2(4.5f - (i * 1), -4.5f + (j * 1));
                chars.Add(temp, null);

            }
        }
        chars[new Vector2(4.5f, -4.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(4.5f, -3.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(4.5f, -2.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(4.5f, -1.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(4.5f, -0.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(-4.5f, 0.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(-4.5f, 1.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(-4.5f, 2.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(-4.5f, 3.5f)] = character;
        character = GameObject.Find("Character " + n++);
        chars[new Vector2(-4.5f, 4.5f)] = character;

    }
    private Vector2 getCoord(Vector3 v)
    {
        Vector2 ret = new Vector2(((int)v.x), ((int)v.z));

        if (ret.x < 0)
            ret.x -= 0.5f;
        else
            ret.x += 0.5f;
        if (ret.y < 0)
            ret.y -= 0.5f;
        else
            ret.y += 0.5f;
        if (v.x < 0 && v.x > -1)
            ret.x = -0.5f;
        if (v.x > 0 && v.x < 1)
            ret.x = 0.5f;
        if (v.z < 0 && v.z > -1)
            ret.y = -0.5f;
        if (v.z > 0 && v.z < 1)
            ret.y = 0.5f;

        return ret;
    }

    IEnumerator DoSomething()
    {

        yield return new WaitForSeconds(20);

    }
    // Update is called once per frame
    void Update()
    {
        if (numPlayers == 0)
        {
            SceneManager.LoadScene("Lose");
        }
        if (numEnemies == 0)
        {
            Debug.Log("win");
            SceneManager.LoadScene("Win");
        }

        if (Input.GetKey("r"))
        {

            SceneManager.LoadScene("big_scene");
        }
        if (Input.GetKey("q"))
        {
            Application.Quit();
        }
       

        if (Input.GetMouseButtonDown(0) && playersTurn)
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var v3 = Input.mousePosition;
                v3.z = 8.9f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                //v3.x += 3.7f;
                v3.z += 0.1f;

                if (characterToMove == null && chars[getCoord(v3)] != null && chars[getCoord(v3)].GetComponent<EnemyScript>() == null)
                {
                    characterToMove = chars[getCoord(v3)];

                }
                else if (characterToMove != null)
                {
                    Vector2 v2 = getCoord(v3);
                    GameObject tile = chars[v2];
                    if (tile != null && tile.GetComponent<CharacterScript>() == null) //Attack
                    {

                        characterToMove.GetComponent<CharacterScript>().Attack(tile);
                        
                        tile.GetComponent<EnemyScript>().hits -= 1;

                        if (tile.GetComponent<EnemyScript>().hits == 0)
                        {
                            GameObject.Destroy(tile);
                            numEnemies--;
                        }
                        else if (tile.GetComponent<EnemyScript>().hits < 2)
                        {
                            tile.GetComponent<Renderer>().material.color = Color.magenta;
                        }
                        else if (tile.GetComponent<EnemyScript>().hits < 3)
                        {
                            tile.GetComponent<Renderer>().material.color = Color.cyan;
                        }

                        characterToMove = null;
                        playersTurn = !playersTurn;

                    }
                    if (tile == null) // Move
                    {
                        chars[new Vector2(characterToMove.transform.position.x, characterToMove.transform.position.z)] = null;

                        characterToMove.GetComponent<Rigidbody>().MovePosition(new Vector3(v2.x, 0.25f, v2.y));
                        chars[v2] = characterToMove;

                        characterToMove = null;
                        playersTurn = !playersTurn;
                    }



                }

            }

        }
        else if (!playersTurn)//Enemy turn
        {
            StartCoroutine(DoSomething());
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                rand = Random.Range(6, 10);
                GameObject enemy = GameObject.Find("Character " + rand);
                rand = Random.Range(1, 5);
                GameObject player = GameObject.Find("Character " + rand);
                if (enemy != null && player != null)
                {
                    enemy.GetComponent<EnemyScript>().Attack(player);

                    
                    player.GetComponent<CharacterScript>().hits -= 1;
                    if (player.GetComponent<CharacterScript>().hits == 0)
                    {
                        GameObject.Destroy(player);
                        numPlayers--;
                        
                    }
                    else if (player.GetComponent<CharacterScript>().hits < 2)
                    {
                        player.GetComponent<Renderer>().material.color = Color.cyan;                        
                    }
                    else if (player.GetComponent<CharacterScript>().hits < 3)
                    {
                        player.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                    playersTurn = !playersTurn;
                }
            }
            else
            {

                int randx = Random.Range(0, 4);
                int randy = Random.Range(0, 4);
                rand = Random.Range(6, 10);
                GameObject enemy = GameObject.Find("Character " + rand);
                Vector2 v2 = new Vector2(randx + 0.5f, randy + 0.5f);
                GameObject tile = chars[v2];
                if (tile == null && enemy != null) // Move
                {
                    chars[new Vector2(enemy.transform.position.x, enemy.transform.position.z)] = null;

                    enemy.GetComponent<Rigidbody>().MovePosition(new Vector3(v2.x, 0.25f, v2.y));
                    chars[v2] = enemy;

                    enemy = null;
                    playersTurn = !playersTurn;
                }
            }
        }

    }
}

