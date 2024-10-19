using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterStatusCardElement: UGUIDataElement<ControllerRecord>
{
    [SerializeField] private Transform hpBarFill;
    [SerializeField] private ImageAnimation iconAnimation;
    [SerializeField] private Image cardIcon;

    private void Awake()
    {
        Assert.IsNotNull(hpBarFill);
        Assert.IsNotNull(iconAnimation);
        Assert.IsNotNull(cardIcon);
    }

    private void Start()
    {
        if (iconAnimation != null)
        {
            iconAnimation.Play("Talk");
        }
    }

    public override void SetData(ControllerRecord data)
    {
        if (data.iconSprite != null)
        {
            cardIcon.sprite = data.iconSprite;
        }
        var percentage = Mathf.Max(0f, data.health.current / data.health.max);
        hpBarFill.localScale = new Vector2(percentage, hpBarFill.localScale.y);
    }
}