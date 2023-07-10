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

    public void ShowGameOverScreen(DeathInfo deathInfo)
    {
        Time.timeScale = 0;
        killerText.text = "Killed by "+ deathInfo.killer.name;
        timeText.text = "Survived " + GameTime.GetTimeString();
        endGameScreen.SetActive(true);
        var weapons = PlayerSkills.Instance.weapons.Values;
        foreach (var weapon in weapons)
        {
            WeaponEndGameStatsUI weaponStat = Instantiate(weaponStatsUIPrefab,weaponStatsParent);
            weaponStat.Set(weapon);
        }
    }
    //TODO: Move to other class
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
