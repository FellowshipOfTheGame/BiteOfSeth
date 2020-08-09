using UnityEngine.UI;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        string score = ServiceLocator.Get<GameManager>().GetLevelScore().ToString();
        scoreText.text = $"x{score}";
    }
}
