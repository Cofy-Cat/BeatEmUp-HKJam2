using UnityEngine;

public class BossEnemyAttackState : AttackState
{
    [SerializeField] GameObject AAPrefab;
    [SerializeField] GameObject AAAPrefab;
    [SerializeField] GameObject comboBBPrefab;
    [SerializeField] GameObject comboBBBPrefab;
    [SerializeField] GameObject comboCCPrefab;
    [SerializeField] GameObject comboDPrefab;
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
                attackPosition.y += 2.5f;
                AA aa = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAALeft":
                attackPosition.y += 2.5f;
                AA aaL = Instantiate(AAPrefab, attackPosition, Quaternion.identity).GetComponent<AA>();
                break;
            case "AttackAAARight":
                attackPosition.x -= 7f;
                AAA aaa = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                aaa.enabled = true;
                aaa.gameObject.SetActive(true);
                break;
            case "AttackAAALeft":
                attackPosition.x -= 7f;
                AAA aaaL = Instantiate(AAAPrefab, attackPosition, Quaternion.identity).GetComponent<AAA>();
                aaaL.enabled = true;
                aaaL.gameObject.SetActive(true);
                break;
                //     case "AttackRightBB":
                //         BB bb = Instantiate(comboBBPrefab, attackPosition, Quaternion.identity).GetComponent<Bullet>();
                //         break;
                //     case "AttackRightCC":
                //         CC cc = Instantiate(comboCCPrefab, attackPosition, Quaternion.identity).GetComponent<Bullet>();
                //         break;
                //     case "AttackRightBBB":
                //         BBB bbb = Instantiate(comboBBBPrefab, attackPosition, Quaternion.identity).GetComponent<Bullet>();
                //         break;
                //     case "AttackRightD":
                //         D d = Instantiate(comboDPrefab, attackPosition, Quaternion.identity).GetComponent<Bullet>();
                //         break;
        }

        base.StartContext(sm, param);
    }
}