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
                attackPosition.y += 2f;
                AA aa = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAALeft":
                attackPosition.y += 2f;
                AA aaL = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAAARight":
                attackPosition.x -= 6f;
                AAA aaa = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                aaa.enabled = true;
                aaa.gameObject.SetActive(true);
                break;
            case "AttackAAALeft":
                attackPosition.x -= 6f;
                AAA aaaL = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                aaaL.enabled = true;
                aaaL.gameObject.SetActive(true);
                break;
            case "AttackBBRight":
                attackPosition.x -= 2.5f;
                AAA bb = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bb.enabled = true;
                bb.gameObject.SetActive(true);
                break;
            case "AttackBBLeft":
                attackPosition.x -= 2.5f;
                AAA bbL = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bbL.enabled = true;
                bbL.gameObject.SetActive(true);
                break;
            case "AttackBBBRight":
                attackPosition.x -= 5f;
                AAA bbb = Instantiate(comboBPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                bbb.enabled = true;
                bbb.gameObject.SetActive(true);
                break;
            case "AttackBBBLeft":
                attackPosition.x -= 5f;
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