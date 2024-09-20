using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    TMP_Text text;

    bool helpMenuOpen;


    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpMenuOpen = !helpMenuOpen;
            text.SetText(SetHelpMenuText());
        }
    }

    string SetHelpMenuText()
    {
        if (helpMenuOpen)
        {
            return
                "H - Close/Open this list\r\n" +
                "P - Pause/\r\n" +
                "E - Empty Grid\r\n" +
                "R - Randomize Grid\r\n\r\n" +
                "Arrow Keys\r\n" +
                "Up - Spawn random column\r\n" +
                "Down - Spawn center column\r\n" +
                "Left - Spawn random row\r\n" +
                "Right - Spawn center row";
        }
        else
        {
            return "Press H for Help";
        }
    }
}
