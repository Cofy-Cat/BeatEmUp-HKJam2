using NUnit.Framework;
using UnityEngine;

public class CharacterStatusCardElement: UGUIDataElement<HealthRecord>
{
    [SerializeField] private Transform hpBarFill;

    private void Awake()
    {
        Assert.IsNotNull(hpBarFill);
    }

    public override void SetData(HealthRecord data)
    {
        var percentage = Mathf.Max(0f, data.current / data.max);
        hpBarFill.localScale = new Vector2(percentage, hpBarFill.localScale.y);
    }
}