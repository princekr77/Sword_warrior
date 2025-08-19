using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;
    public AudioSource source4;
    public AudioSource source5;

    public void AttackAudio()
    {
        source1.Play();
    }

    public void KillAudio()
    {
        source2.Play();
    }

    public void WinAudio()
    {
        source3.Play();
    }

    public void GameOverAudio()
    {
        source4.Play();
    }

    public void TrackSound()
    {
        source5.Stop();
    }
}

