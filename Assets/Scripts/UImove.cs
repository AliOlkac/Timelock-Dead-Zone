using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RotateWithDOTween : MonoBehaviour
{
    public Movement takeDamage; // Health kontrolü için referans
    public RectTransform uiElement; // Hedef UI öğesi (HUV)
    public float rotationAmount = 90f; // Her seferde dönülecek açı
    public Image uiElementColor; // UI öğesi (örneğin, HUV Image)

    private void Update()
    {
        // Tuşa basıldığında 90 derece döndür
        if (Input.GetButtonDown("Fire1"))
        {
            uiElement.DORotate(new Vector3(0, 0, rotationAmount), 0.001f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear); // Sabit hızda dönüş
        }
    }
}