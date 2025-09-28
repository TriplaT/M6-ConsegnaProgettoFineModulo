using UnityEngine;

public class ArchTriggerParticles : MonoBehaviour
{
    [SerializeField] private CoinCollection playerCoinCollection;
    [SerializeField] private int coinsNeeded = 1;
    [SerializeField] private ParticleSystem fireworksEffect;

    private bool hasPlayed = false;

    void Update()
    {
        if (!hasPlayed && playerCoinCollection.GetCoinCount() >= coinsNeeded)
        {
            if (fireworksEffect != null)
                fireworksEffect.Play();
            hasPlayed = true;
        }
    }
}
