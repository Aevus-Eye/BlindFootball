using UnityEngine;

public class OnlyEnabledInEditor : MonoBehaviour
{
    private void Awake() => gameObject.SetActive(Application.isEditor);
}
