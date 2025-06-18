using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionAssetController : SingletonMonoBehaviour<InputActionAssetController>
{
    [SerializeField] private InputActionAsset asset;

    public void DisableAsset()
    {
        asset.Disable();
    }
}
