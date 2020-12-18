using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinCount : MonoBehaviour
{
    //total count of all coins collected
    public int totalCoins;
    public int minimumCoinsCollected;

    //Where the coin count is displayed
    [SerializeField]
    TextMeshProUGUI coinCountText = null;

    [SerializeField]
    Transform tokenSpawnPoint = null;

    public GameObject token;
    private GameObject tokenClone;
    private tokenCount totalTokenCount;
    bool tokenCreated = false;

    //coin explode effect
    public ParticleSystem coinExplode;


    // Start is called before the first frame update
    void Start()
    {
        //when game is started, set the coin count to 0
        totalCoins = 0;
        tokenClone = token;


        tokenClone.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //The text is "x" with the total coins collected added. The int is changed to a string
        coinCountText.text = "x" + totalCoins.ToString();

        if(minimumCoinsCollected == totalCoins && tokenCreated == false)
        {

            Instantiate(tokenClone);
            tokenCreated = true;
            tokenClone.transform.position = tokenSpawnPoint.transform.position;
            if (tokenCreated == true)
            {
                tokenClone.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if there is a collision with a coin trigger, +1 to total coins and destroy the coin it collides with
        if(collision.GetComponent<coin>())
        {
            totalCoins += 1;
            Destroy(collision.gameObject);
            coinExplodeEffect();
        }
    }

    void coinExplodeEffect()
    {
        ParticleSystem coinExplodeEffect = Instantiate(coinExplode)
            as ParticleSystem;
        coinExplodeEffect.transform.position = transform.position;

        coinExplodeEffect.Play();

    }
}
