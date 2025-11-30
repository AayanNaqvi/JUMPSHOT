using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointCollector : MonoBehaviour
{
    public float totalPoints;
    public float kills;
    public TextMeshProUGUI pointText;
    public GameObject player;

    private PlayerMovement pm;
    public GameObject playerMove;

    public float airTime;

    private int lastBonusKillCount = 0;
    private ThrowingUtil tU;

    private float displayedPoints = 0f;

    private void Start()
    {
        tU = player.GetComponent<ThrowingUtil>();
        pm = playerMove.GetComponent<PlayerMovement>();
    }

    public void addPoints(float p)
    {
        totalPoints += p + Mathf.RoundToInt(10f * airTime);
        kills += 1f;

        if ((int)kills % 5 == 0 && (int)kills != lastBonusKillCount)
        {
            tU.numUtil1 += 2f;
            tU.numUtil2 += 2f;
            lastBonusKillCount = (int)kills;
        }
    }

    public void Update()
    {
        displayedPoints = Mathf.Lerp(displayedPoints, totalPoints, 7.5f * Time.deltaTime);


        pointText.SetText("" + Mathf.RoundToInt(displayedPoints));
        if(pm.grounded)
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.deltaTime;
        }
    }
}
