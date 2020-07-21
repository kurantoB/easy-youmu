using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float maxTime;
    private float timeLeft = 5f;
    private bool disabled = false;

    public Image progressCircle;

    public delegate void ContinueCmd();
    private ContinueCmd cc;

    void Update()
    {
        if (disabled)
        {
            return;
        }
        //timeLeft -= Time.deltaTime;
        progressCircle.fillAmount = timeLeft / maxTime;

        if (timeLeft <= 0)
        {
            cc();
        }
    }

    public void timerReset(float messageTime, ContinueCmd cc)
    {
        maxTime = messageTime;
        timeLeft = messageTime;
        this.cc = cc;
    }

    public void DisableTimer()
    {
        disabled = true;
    }

    public void EnableTimer()
    {
        disabled = false;
    }
}
