using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    [Header("Content")]
    public GameObject cardPrefabs;      //カードprefab
    public int cardMax = 5;             //カードを引く枚数
    public int cardSpacing = 25;        //カードの間隔
    public bool isMyTurn = true;
    public EnemyCPU enemyCPU;

    [Header("Text")]
    public Text turnText;
    public Text buffText;
    public Text useText;
    public Text HpText;

    [Header("Result")]
    public GameObject resultPanel;
    public Text resultText;

    [Header("Player")]
    public long playerHp = 100;
    public long playerAt = 0;
    public long playerDf = 0;

    //private変数
    private List<GameObject> handCards = new List<GameObject>();
    private int turnCount = 0;
    private long buffNum = 0;
    private bool isEndGame = false;

    //外部呼び出し用関数
    public bool GetMyTurn()
    {
        return isMyTurn;
    }
    public bool GetEndGame()
    {
        return isEndGame;
    }
    public void EnemyDead()
    {
        isEndGame = true;
        resultPanel.SetActive(true);
        resultText.text = "You Win!!";
    }
    public void EnemyAttack(long power)
    {
        power = power - playerDf;
        if (power < 0) return;
        playerHp -= power;
        HpText.text = "HP: " + playerHp;
        if (playerHp <= 0) Dead();
    }
    public void EndTurn()
    {
        Debug.Log("Your Turn");
        isMyTurn = true;
        turnCount++;
        DrowCard();
        if (turnCount % 6 == 0)
        {
            buffNum = 0;
            buffText.text = "Buff: " + buffNum.ToString();
        }
        turnText.text = "TurnCount: " + turnCount.ToString();
        Debug.Log($"Turn Count: {turnCount}");
    }
    public void Retry()
    {
        Init();
    }

    //====================================================================
    private void Start()
    {   
        Init();
    }
    private void Init()
    {
        resultPanel.SetActive(false);
        resultText.text = "";
        isMyTurn = true;
        isEndGame = false;
        playerHp = 100;
        playerAt = 0;
        playerDf = 0;
        turnCount = 1;
        buffNum = 0;
        buffText.text = "Buff: " + buffNum;
        useText.text = "Power: " + 0;
        HpText.text = "HP: " + playerHp;
        turnText.text = "TurnCount: " + turnCount;
        SetCards();
        enemyCPU.Init();
    }
    private long SetPower(int turn)
    {
        int tDiv5 = turn / 5;
        int tXor = turn ^ tDiv5;
        return (Random.Range(0 + tXor, 9 + tXor) * turn ^ turn ^ tXor) * tXor;
    }
    public void SetCards()
    {
        //handCardsにモノが入っていれば削除
        if (handCards.Count > 0)
        {
            foreach (var card in handCards)
            {
                Destroy(card);
            }
            handCards.Clear();
        }

        //Cardの要素を決めつつ5枚Instantiateする
        for (int i = 0; i < cardMax; i++)
        {
            Vector3 pos = new Vector3 (transform.position.x + cardSpacing * i, transform.position.y, transform.position.z + 0.1f * i);
            GameObject card = Instantiate(cardPrefabs, pos, Quaternion.identity,this.transform);
            handCards.Add(card);
            card.GetComponent<CardContentCon>().SetCardContent(Random.Range(0,3), Random.Range(0, 2), SetPower(turnCount));
        }
        Debug.Log($"Set Cards");
    }
    void ReLayoutHand()
    {
        // Destroy済み(null)を除去
        handCards.RemoveAll(c => c == null);

        // 左から詰める
        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 pos = new Vector3(
                transform.position.x + cardSpacing * i,
                transform.position.y,
                transform.position.z + 0.1f * i
            );

            handCards[i].transform.position = pos;
        }
    }
    public void DrowCard()
    {
        // まず空白を詰める
        ReLayoutHand();

        // 新しいカードの位置
        Vector3 pos = new Vector3(
            transform.position.x + cardSpacing * handCards.Count,
            transform.position.y,
            transform.position.z + 0.1f * handCards.Count
        );

        GameObject card =
            Instantiate(cardPrefabs, pos, Quaternion.identity, this.transform);

        handCards.Add(card);

        card.GetComponent<CardContentCon>().SetCardContent(Random.Range(0, 3), Random.Range(0, 2), SetPower(turnCount));
    }
    public void UseCard(int typeid, int cardid, long power)
    {
        long endPower = 0;
        switch (cardid)
        {
            case 0:
                if (buffNum == 0)
                {
                    endPower = power;
                }
                else
                {
                    endPower = buffNum * power;
                }
                switch (typeid)
                {
                    case 0:
                        playerAt += endPower;
                        enemyCPU.AttackPlayer(playerAt);
                        break;
                    case 1:
                        playerDf += endPower;
                        break;
                    case 2:
                        playerHp += endPower;
                        break;
                }
                useText.text = "Power: " + endPower;
                HpText.text = "HP: " + playerHp;
                break;
            case 1:
                buffNum += power;
                buffText.text = "Buff: " + buffNum.ToString();
                break;
        }
        NextTurn();
    }
    private void NextTurn()
    {
        isMyTurn = false;
        enemyCPU.Move(Random.Range(0, 3), Random.Range(0, 2), SetPower(turnCount), turnCount);
    }
    private void Dead()
    {
        isEndGame = true;
        resultPanel.SetActive(true);
        resultText.text = "You Lose...";
        Debug.Log("Your Dead");
    }
}
