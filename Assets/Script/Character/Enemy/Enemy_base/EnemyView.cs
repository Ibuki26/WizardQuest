using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Color damageColor; //�_���[�W���󂯂��Ƃ��ɕς��F
    private SpriteRenderer sr;

    public void ManualStart()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //�_���[�W���󂯂����Ɏw�肵���F�ɕς��āA0.2�b�㌳�ɖ߂�
    public async UniTask DamageColor()
    {
        sr.color = damageColor;
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        sr.color = Color.white;
    }
}
