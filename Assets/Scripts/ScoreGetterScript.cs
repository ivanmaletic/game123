using UnityEngine;
using UnityEngine.UI;
public class ScoreGetterScript : MonoBehaviour
{
    public Text scoreText;
    void Update()
    {
        scoreText.text ="Score: "+ GameObject.Find("Coin1").GetComponent<GameplayScript>().score.ToString();
    }
}
