using NUnit.Framework;
using UnityEngine;

public class CharacterStatusCardElement: UGUIDataElement<HealthRecord>
{
    [SerializeField] private Transform hpBarFill;
    [SerializeField] private ImageAnimation iconAnimation;

    private void Awake()
    {
        Assert.IsNotNull(hpBarFill);
        Assert.IsNotNull(iconAnimation);
    }

    private void Start()
    {
        iconAnimation.Play("Talk");
    }

    public override void SetData(HealthRecord data)
    {
        var percentage = Mathf.Max(0f, data.current / data.max);
        hpBarFill.localScale = new Vector2(percentage, hpBarFill.localScale.y);
    }
}