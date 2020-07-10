using UnityEngine;
using Fungus;

[CommandInfo("Narrative",
                 "YoumuSay",
                 "Youmu's message")]
public class YoumuSay : Say
{
    public YoumuExpression expression;
    public override void OnEnter()
    {
        character = GameObject.FindGameObjectWithTag("YoumuCharacter").GetComponent<Character>();
        portrait = GetMarisaDimPortrait();
        character.SetSayDialog.CharacterImage.CrossFadeAlpha(0.5f, 0.25f, true);
        switch (expression)
        {
            case YoumuExpression.NEUTRAL:
                GetFlowchart().SetStringVariable("YoumuExpression", "NEUTRAL");
                break;
        }
        GetFlowchart().ExecuteBlock("YoumuPortrait");

        float waitTime = storyText.Length > 70 ? 9 : 4;
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

    private Sprite GetMarisaDimPortrait()
    {
        switch (GetFlowchart().GetStringVariable("MarisaExpression"))
        {
            case "HAPPY":
                return GameObject.FindGameObjectWithTag("MarisaCharacter").GetComponent<Character>().Portraits[0];
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
