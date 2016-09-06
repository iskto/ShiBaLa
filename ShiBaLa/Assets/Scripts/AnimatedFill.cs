using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(GUITexture))]
public class AnimatedFill : MonoBehaviour
{
    [Range(0f, 1f)]
    public float fillAmount = 1f;

    UITexture mTexture;

    void Awake() { mTexture = GetComponent<UITexture>(); }
    void Update() { mTexture.fillAmount = fillAmount;}
}
