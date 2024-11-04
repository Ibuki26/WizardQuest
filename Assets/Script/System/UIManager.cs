using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player player; // Player�̃C���X�^���X
    [SerializeField] private Magic[] magics; // Magic�̃C���X�^���X
    [SerializeField] private Image hitPointBar; // �̗̓o�[��Image�R���|�[�l���g
    [SerializeField] private Image bButton; // B�{�^����Image�R���|�[�l���g
    [SerializeField] private Image yButton; // Y�{�^����Image�R���|�[�l���g
    [SerializeField] private TextMeshProUGUI scoreText; // �X�R�A�\���p��Text�R���|�[�l���g
    [SerializeField] private GameObject clearWindow; //�Q�[���N���A���ɉf���I�u�W�F�N�g
    [SerializeField] private GameObject overWindow; //�Q�[���I�[�o�[���ɉf���I�u�W�F�N�g
    [SerializeField] private GameObject optionWindow; //���j���[��� 
    private int score; // �X�R�A�̕ϐ�
    private float coolTime1 = 0;
    private float coolTime2 = 0;

    private void Start()
    {
        Cursor.visible = false;
        UpdateHitPointBar(player.GetHitPoint()); // �����̗̑̓o�[�̍X�V
        score = 0; // �X�R�A������
        UpdateScoreDisplay(); // �X�R�A�\���̍X�V
    }

    
    private void Update()
    {
        if (magics[0].IsCooling())
            coolTime1 += Time.deltaTime;
        else
            coolTime1 = 0;

        if (magics[1].IsCooling())
            coolTime2 += Time.deltaTime;
        else
            coolTime2 = 0;

        UpdateHitPointBar(player.GetHitPoint()); // Get�֐����g�p���Č��݂̗̑͂��擾
        UpdateMagicCooldown(magics[0], bButton, coolTime1); // B�{�^���̃N�[���^�C�����X�V
        UpdateMagicCooldown(magics[1], yButton, coolTime2); // Y�{�^���̃N�[���^�C�����X�V
    }

    // �̗̓o�[�̍X�V
    private void UpdateHitPointBar(int currentHitPoint)
    {
        float fillAmount = (float)currentHitPoint / player.GetMaxHitPoint(); // Get�֐����g�p
        hitPointBar.fillAmount = fillAmount;
    }

    // ���@�̃N�[���^�C�����X�V
    private async void UpdateMagicCooldown(Magic magic, Image buttonImage, float coolTime)
    {
        if (magic.IsCooling()) // �N�[���^�C���������m�F
        {
            buttonImage.fillAmount = 0; // FillAmount��0�ɂ���
            buttonImage.fillAmount = coolTime/magic.GetCoolTime();
            await UniTask.Delay((int)(magic.GetCoolTime() * 1000)); // �N�[���^�C���̑ҋ@
            buttonImage.fillAmount = 1; // FillAmount��1�ɖ߂�
        }
    }

    // �X�R�A�̉��Z����
    public void AddScore(int value)
    {
        score += value; // �X�R�A���Z
        UpdateScoreDisplay(); // �X�R�A�\�����X�V
    }

    // �X�R�A�\�����X�V
    private void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString(); // �X�R�A��\��
    }

    public void SetMagic(Magic magic1, Magic magic2)
    {
        if (magic1 == null || magic2 == null) return;
        magics[0] = magic1;
        magics[1] = magic2;
    }

    public void ActiveWindow(string name)
    {
        if (name == "clear")
            clearWindow.SetActive(true);
        else if (name == "over")
            overWindow.SetActive(true);
        else if (name == "option")
            optionWindow.SetActive(true);
        else if (name == "mouse")
        {
            Cursor.visible = true;
        }
    }

    public void DisappearWindow(string name)
    {
        if (name == "option")
            optionWindow.SetActive(false);
        else if (name == "mouse")
        {
            Cursor.visible = false;
        }
    }

    public int GetScore()
    {
        return score;
    }
}
