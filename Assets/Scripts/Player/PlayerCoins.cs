using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public int coinCount = 0;

    void Start()
    {
        coinCount = PlayerPrefs.GetInt("Coin", 0);
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        PlayerPrefs.SetInt("Coins", coinCount);
        PlayerPrefs.Save();
        //Debug.Log("Coins: " + coinCount);
    }
}
