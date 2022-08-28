using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] SoldierCard meeleSoldierCard; //for ai
    [SerializeField] ScriptableInt playerElixirCount;
    [SerializeField] ScriptableInt aiElixirCount;
    public bool IsGameStarted { get; private set; }

    [SerializeField] TextMeshProUGUI elixirText;

    [SerializeField] Tower[] playerTowers;
    [SerializeField] Tower[] aiTowers;

    [SerializeField] GameObject gameFinishedPanel;
    [SerializeField] TextMeshProUGUI gameFinishedText;

    [SerializeField] Material enemyMaterial;

    private void Awake()
    {
        Instance = this;
        IsGameStarted = true;
        playerElixirCount.Value = 0;
        aiElixirCount.Value = 0;
        StartCoroutine(ElixirIncreaser());
    }

    private IEnumerator ElixirIncreaser()
    {
        while (IsGameStarted)
        {
            yield return new WaitForSeconds(1);
            if(playerElixirCount.Value < 10)
            {
                playerElixirCount.Value++;
                UpdateElixirText();

                aiElixirCount.Value++;
                if(aiElixirCount.Value >= 3)
                {
                    SpawnEnemySoldier();
                }
            }
        }
    }

    private void UpdateElixirText()
    {
        elixirText.text = $"{playerElixirCount.Value} / 10";
    }

    public void SoldierPlaced(SoldierCard card, Vector3 pos)
    {
        if(card.soldierType == SoldierType.Meele)
        {
            SoldierBase soldier = Instantiate(card.prefab, pos, Quaternion.identity).GetComponent<SoldierBase>();
            playerElixirCount.Value -= card.elixirAmount;
            UpdateElixirText();
            soldier.IsPlayer = true;
            soldier.Initialize();
        }
    }

    public void SpawnEnemySoldier()
    {
        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-4.25f, 4.25f), 0.85f, UnityEngine.Random.Range(18f, 12f));
        SoldierCard card = meeleSoldierCard;

        SoldierBase soldier = Instantiate(card.prefab, randomPos, Quaternion.identity).GetComponent<SoldierBase>();
        aiElixirCount.Value -= card.elixirAmount;
        soldier.SetMaterial(enemyMaterial);
        soldier.Initialize();
    }

    public bool CheckIfElixirAmountEnough(int value)
    {
        return playerElixirCount.Value - value >= 0;
    }

    public bool CheckCanAttackBaseTower(bool isPlayer)
    {
        Tower[] lookTowers = isPlayer ? aiTowers : playerTowers;
        if (lookTowers[0] == null || lookTowers[1] == null)
            return true;
        return false;
    }
    public Transform GetClosestTower(bool isPlayer, Vector3 currentPos)
    {
        Tower[] lookTowers = isPlayer ? aiTowers : playerTowers;

        if (lookTowers[0] == null && lookTowers[1] == null)
            return lookTowers[2].transform;
        else
        {
            if (lookTowers[0] != null && lookTowers[1] != null)
                return CheckDistance(lookTowers[0], lookTowers[1], currentPos).transform;
            else if(lookTowers[0] == null)
                return CheckDistance(lookTowers[1], lookTowers[2], currentPos).transform;
            else
                return CheckDistance(lookTowers[0], lookTowers[2], currentPos).transform;
        }
    }
    private Tower CheckDistance(Tower tower1, Tower tower2, Vector3 pos)
    {
        float distance1 = Vector3.Distance(pos, tower1.transform.position);
        float distance2 = Vector3.Distance(pos, tower2.transform.position);

        if (distance1 < distance2)
            return tower1;
        else
            return tower2;
    }

    public void GameFinished(bool isPlayer)
    {
        Time.timeScale = 0;
        gameFinishedPanel.SetActive(true);
        string winner = isPlayer ? "AI" : "Player";
        gameFinishedText.text = $"Game finished. Winner: {winner}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }


}


