using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCPU : MonoBehaviour
{
    [Header("Content")]
    public GameObject gameDirector;

    [Header("Enemy")]
    public long enemyHp = 100;
    public long enemyDf = 0;

    [Header("Text")]
    public Text buffText;
    public Text useText;
    public Text HpText;

    private long buffNum = 0;

    public void Init()
    {
        enemyHp = 100;
        enemyDf = 0;
        buffNum = 0;
        buffText.text = "Buff: " + buffNum;
        useText.text = "Power: " + 0;
        HpText.text = "HP: " + enemyHp;
    }
    public void AttackPlayer(long power)
    {
        power = power - enemyDf;
        if (power < 0) return;
        enemyHp -= power;
        if (enemyHp <= 0) Dead();
    }
    public void Move(int typeid, int cardid, long power, int turn)
    {
        if (turn % 6 == 0) buffNum = 0;
        if (turn % 5 == 0) cardid = 0;
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
                        Debug.Log($"Attack Enemy: " + endPower);
                        gameDirector.GetComponent<GameDirector>().EnemyAttack(endPower);
                        break;
                    case 1:
                        Debug.Log($"Block Enemy: " + endPower);
                        enemyDf += endPower;
                        break;
                    case 2:
                        Debug.Log($"Heal Enemy: " + endPower);
                        enemyHp += endPower;
                        break;
                }
                break;
            case 1:
                buffNum += power;
                Debug.Log($"Buff Up Enemy: " + buffNum);
                break;
        }
        buffText.text = "Buff: " + buffNum;
        useText.text = "Power: " + endPower;
        HpText.text = "HP: " + enemyHp;

        gameDirector.GetComponent<GameDirector>().EndTurn();
    }
    private void Dead()
    {
        gameDirector.GetComponent<GameDirector>().EnemyDead();
        Debug.Log("Enemy Dead");
    }
}
