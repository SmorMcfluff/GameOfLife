using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static int maxTrailFadeTime = 30;
    public static int secondGradientStartTime = 10;

    static Color aliveColor = Color.white;

    static Color trailStartColor = Color.yellow;
    static Color trailMidColor = Color.red;
    static Color trailEndColor = Color.black;

    static Color trailColor;


    public static void UpdateColor(Cell cell)
    {
        SpriteRenderer sprite = cell.GetSpriteRenderer();

        if (cell.isAlive)
        {
            if (!sprite.enabled)
            {
                sprite.enabled = true;
            }

            sprite.color = aliveColor;

            return;
        }
        else if (cell.trailFadeTime > 0)
        {
            TrailGradient(cell);
        }
        else
        {
            sprite.enabled = false;
        }
    }


    static void TrailGradient(Cell cell)
    {
        SpriteRenderer sr = cell.GetSpriteRenderer();

        if (cell.trailFadeTime == maxTrailFadeTime)
        {
            trailColor = trailStartColor;
            sr.color = trailColor;
        }
        else if (cell.trailFadeTime > secondGradientStartTime)
        {
            float trailFadeRate = (float)cell.trailFadeTime / maxTrailFadeTime;
            sr.color = Color.Lerp(trailMidColor, trailStartColor, trailFadeRate);
        }
        else
        {
            float trailFadeRate = (float)cell.trailFadeTime / secondGradientStartTime;
            sr.color = Color.Lerp(trailEndColor, trailMidColor, trailFadeRate);
        }

        cell.trailFadeTime--;
    }
}
