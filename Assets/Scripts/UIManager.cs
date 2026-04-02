using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI castleHealthText;

void Update()
{
    if (waveText == null) Debug.Log("WaveText not assigned");
    if (killText == null) Debug.Log("KillText not assigned");
    if (castleHealthText == null) Debug.Log("CastleHealthText not assigned");

    waveText.text = "Wave: " + GameManager.instance.wave;
    killText.text = "Goblins Killed: " + GameManager.instance.goblinsKilled;
    castleHealthText.text = "Castle Health: " + Castle.instance.health;
}
}