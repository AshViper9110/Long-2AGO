using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardContentCon : MonoBehaviour
{
    [Header("Content")]
    public List<GameObject> cardMeshs = new List<GameObject>();     //カードのテクスチャがついたobject
    public Text nameText;                                           //名前のText
    public Text numText;                                            //カードパワーのText

    //カードの保持用変数
    int cardId;
    string cardName;
    int cardPower;

    CardCon cardCon;

    private void Start()
    {
        //CardConを持っているObjectをさがしてCardConのScriptを取得する
        cardCon = GameObject.Find("CardCon").GetComponent<CardCon>();
    }

    //CardのContentを設定する
    public void SetCardContent(int id, string name, int power)
    {
        foreach (GameObject card in cardMeshs)
        {
            card.SetActive(false);
        }
        cardId = id;
        cardName = name;
        cardPower = power;

        cardMeshs[id].SetActive(true);
        nameText.text = name;
        numText.text = power.ToString();
    }

    //カードが使われたら削除してcardConの関数を呼び出す
    private void OnMouseDown()
    {
        Destroy(this.gameObject);
        cardCon.NextTurn();
        Debug.Log($"== Card Touth ==\nID: {cardId} Name: {cardName} Power: {cardPower}");
    }
}
