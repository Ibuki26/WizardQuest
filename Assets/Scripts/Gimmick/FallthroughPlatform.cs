using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class FallthroughPlatform : MonoBehaviour
{
    private PlatformEffector2D effctor;

    public void ManualStart()
    {
        effctor = GetComponent<PlatformEffector2D>();
    }

    public async UniTask Fallthrough()
    {
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        effctor.colliderMask &= ~playerLayerMask;
        gameObject.layer = LayerMask.NameToLayer("Default");

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());

        effctor.colliderMask |= playerLayerMask;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}
