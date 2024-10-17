using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class KnockBackState: CharacterState
{
    public class Param : StateParam
    {
        public float KnockBackDistance;
        public Vector2 Force;
        public float Gravity;
    }

    private HashSet<CharacterStateId> blacklist = new()
        { CharacterStateId.Idle,  CharacterStateId.Attack, CharacterStateId.Dash, CharacterStateId.Move, CharacterStateId.KnockBack };

    public override CharacterStateId[] stateBlacklist => blacklist.ToArray();
    public override CharacterStateId Id => CharacterStateId.KnockBack;

    private float _startPosition = float.MaxValue;
    [SerializeField] private float airSpeed = 0f;
    private CharacterStateMachine _sm;
    private Param _param;

    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        _param = param as Param;
        _sm = sm;
        Assert.IsNotNull(_param);

        _startPosition = sm.Controller.MainCharacter.position.x;
        airSpeed = _param.Force.y;
        sm.Controller.Rigidbody.linearVelocityX = _param.Force.x;
    }
    
    public override void _Update()
    {
        base._Update();
        
        var mainCharacter = _sm.Controller.MainCharacter;
        
        if (Mathf.Abs(mainCharacter.position.x - _startPosition) >= _param.KnockBackDistance)
        {
            _sm.Controller.Rigidbody.linearVelocityX = 0;

            if (mainCharacter.localPosition.y <= 0)
            {
                setCharacterY(0);
                blacklist.Remove(CharacterStateId.Idle);
                _sm.Controller.Command.ExecuteCommand(new IdleCommand());
                blacklist.Add(CharacterStateId.Idle);
                return;
            }
        }

        var newY = mainCharacter.localPosition.y + airSpeed * Time.deltaTime;
        airSpeed -= _param.Gravity * Time.deltaTime;
        setCharacterY(newY);

        void setCharacterY(float y)
        {
            mainCharacter.localPosition = new Vector2(mainCharacter.localPosition.x, y);
        }
    }
}