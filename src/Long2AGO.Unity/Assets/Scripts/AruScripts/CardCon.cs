using System.Collections.Generic;
using UnityEngine;


public class CardCon : MonoBehaviour
{
    [Header("Content")]
    public GameObject cardPrefabs;      //カードprefab
    public int cardMax = 5;             //カードを引く枚数
    public int cardSpacing = 25;         //カードの間隔

    public List<GameObject> handCards = new List<GameObject>();
    private int turnCount = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        turnCount = 1;
        SetCards();
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
            card.GetComponent<CardContentCon>().SetCardContent(Random.Range(0,3),"けん", Random.Range(0, 9) + turnCount * (turnCount / 5));
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

        card.GetComponent<CardContentCon>().SetCardContent(Random.Range(0, 3), "けん", Random.Range(0, 9) + turnCount * (turnCount / 5));
    }


    public void NextTurn()
    {
        turnCount++;
        DrowCard();
        Debug.Log($"Turn Count: {turnCount}");
    }
}
