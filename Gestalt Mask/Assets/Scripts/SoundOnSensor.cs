using UnityEngine;

public class SoundOnSensor : BetterMonoBehaviour
{
    public SimpleMMSoundPlayer SoundPlayer;
    public int LimitedPlay = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;
        if (LimitedPlay <= 0) { return; }

        LimitedPlay -= 1;
        SoundPlayer?.PlayClip();
    }
}
