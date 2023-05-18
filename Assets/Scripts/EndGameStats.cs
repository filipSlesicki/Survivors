using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameStats : MonoBehaviour
{
    public GameObject endGameScreen;
    public TextMeshProUGUI killerText;
    public TextMeshProUGUI timeText;
    public Transform weaponStatsParent;
    public WeaponEndGameStatsUI weaponStatsUIPrefab;

    private void OnEnable()
    {
        Player.OnPlayerDead += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        Player.OnPlayerDead -= ShowGameOverScreen;
    }

    void ShowGameOverScreen(GameObject killer)
    {
        Time.timeScale = 0;
        killerText.text = "Killed by "+ killer.name;
        timeText.text = "Survived " + GameTime.GetTimeString();
        endGameScreen.SetActive(true);
        var weapons = PlayerSkills.Instance.weapons.Values;
        foreach (var weapon in weapons)
        {
            WeaponEndGameStatsUI weaponStat = Instantiate(weaponStatsUIPrefab,weaponStatsParent);
            weaponStat.Set(weapon);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
