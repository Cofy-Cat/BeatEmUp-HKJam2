using cfEngine.Util;
using UnityEngine;

public class BossEnemyAttackState : AttackState
{
    [SerializeField] GameObject AAPrefab;
    [SerializeField] GameObject AAAPrefab;
    [SerializeField] GameObject comboBPrefab;
    [SerializeField] GameObject comboCPrefab;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (Param)param;

        var controller = sm.Controller;
        string animationName = AnimationName.GetComboDirectional(AnimationName.Attack, p.Combo, controller.LastFaceDirection);
        Vector3 attackPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Debug.Log(animationName);
        switch (animationName)
        {
            case "AttackAARight":
                attackPosition.x -= 1.75f;
                attackPosition.y += 1f;
                AA aa = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAALeft":
                attackPosition.x -= 1.75f;
                attackPosition.y += 1f;
                AA aaL = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAAARight":
                attackPosition.x -= 7f;
                AAA aaa = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                break;
            case "AttackAAALeft":
                attackPosition.x -= 7f;
                AAA aaaL = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                break;
            case "AttackBBRight":
                attackPosition.x -= 4f;
                AAA bb = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bb.enabled = true;
                bb.gameObject.SetActive(true);
                break;
            case "AttackBBLeft":
                attackPosition.x -= 4f;
                AAA bbL = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bbL.enabled = true;
                bbL.gameObject.SetActive(true);
                break;
            case "AttackBBBRight":
                attackPosition.x -= 6.5f;
                AAA bbb = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bbb.enabled = true;
                bbb.gameObject.SetActive(true);
                break;
            case "AttackBBBLeft":
                attackPosition.x -= 6.5f;
                AAA bbbL = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bbbL.enabled = true;
                bbbL.gameObject.SetActive(true);
                break;
            case "AttackCCRight":
                attackPosition.y += 3f;
                AA cc = Instantiate(comboCPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                cc.enabled = true;
                cc.gameObject.SetActive(true);
                break;
            case "AttackCCLeft":
                attackPosition.y += 3f;
                AA ccL = Instantiate(comboCPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                ccL.enabled = true;
                ccL.gameObject.SetActive(true);
                break;
        }

        base.StartContext(sm, param);
    }
}