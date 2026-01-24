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
    int cardtype;
    long cardPower;

    GameDrector gameDirector;

    private void Start()
    {
        //CardConを持っているObjectをさがしてCardConのScriptを取得する
        gameDirector = GameObject.Find("GameDrector").GetComponent<GameDrector>();
    }

    //CardのContentを設定する
    public void SetCardContent(int typeid, int cardid, int power)
    {
        foreach (GameObject card in cardMeshs)
        {
            card.SetActive(false);
        }

        name = "none";

        switch (cardid)
        {
            case 0:
                switch (typeid)
                {
                    case 0:
                        name = "けん";
                        break;
                    case 1:
                        name = "たて";
                        break;
                    case 2:
                        name = "かいふく";
                        break;
                }
                break;
            case 1:
                name = "バフ";
                break;
        }

        cardId = typeid;
        cardtype = cardid;
        cardPower = power;

        cardMeshs[typeid].SetActive(true);
        nameText.text = name;
        numText.text = power.ToString();
    }

    //カードが使われたら削除してcardConの関数を呼び出す
    private void OnMouseDown()
    {
        Destroy(this.gameObject);
        gameDirector.UseCard(cardId, cardtype, cardPower);
    }
}
