using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private WizardPresenter player;
    [SerializeField] private float max_x;
    private Rigidbody2D playerRD;

    void Start()
    {
        playerRD = player.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if(playerRD.velocity.x != 0)
        {
            float x = player.transform.position.x;

            x = Mathf.Clamp(x, 0, max_x);
            Move(new Vector3(x, transform.position.y, transform.position.z));
        }
    }

    public void Move(Vector3 vec)
    {
        transform.position = vec;
    }
}
