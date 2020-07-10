using UnityEngine;
using Fungus;

[CommandInfo("Narrative",
                 "MarisaSay",
                 "Marisa's message")]
public class SayMarisa : Say
{
    public MarisaExpression expression;
    public override void OnEnter()
    {
        character = GameObject.FindGameObjectWithTag("MarisaCharacter").GetComponent<Character>();
        portrait = GetCharacterPortrait();
        character.SetSayDialog.CharacterImage.CrossFadeAlpha(1, 0.25f, true);
        float waitTime = storyText.Length > 70 ? 9 : 4.5f;
        if (GameObject.FindGameObjectWithTag("MessageTimer") != null)
        {
            Timer tmr = GameObject.FindGameObjectWithTag("MessageTimer").GetComponent<Timer>();
            tmr.timerReset(waitTime, Continue);
        }
        base.OnEnter();
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }

    private Sprite GetCharacterPortrait()
    {
        switch (expression)
        {
            case MarisaExpression.HAPPY:
                GetFlowchart().SetStringVariable("MarisaExpression", "HAPPY");
                return character.Portraits[0];
            default:
                return null;
        }
    }

    public override void Continue()
    {
        if (GameObject.FindObjectOfType<GameFlow>() != null)
        {
            GameObject.FindObjectOfType<GameFlow>().HandleContinue(this, BaseContinue);
        }
        else
        {
            base.Continue();
        }
    }

    private void BaseContinue()
    {
        base.Continue();
    }
}
