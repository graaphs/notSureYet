using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tokenCount : MonoBehaviour
{
    //total count of all tokens collected
    public int totalTokens;

    //Where the token count is displayed
    [SerializeField]
    TextMeshProUGUI TokenCountText = null;

    //token explode effect
    public ParticleSystem tokenExplode;

    // Start is called before the first frame update
    void Start()
    {
        //when game is started, set the token count to 0
        totalTokens = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //The text is "x" with the total tokens collected added. The int is changed to a string
        TokenCountText.text = "x" + totalTokens.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if there is a collision with a token trigger, +1 to total tokens and destroy the token it collides with
        if (collision.GetComponent<token>())
        {
            totalTokens += 1;
            Destroy(collision.gameObject);
            tokenExplodeEffect();
        }
    }

    void tokenExplodeEffect()
    {
        ParticleSystem tokenExplodeEffect = Instantiate(tokenExplode)
            as ParticleSystem;
        tokenExplodeEffect.transform.position = transform.position;

        tokenExplodeEffect.Play();

    }
}
