using System.Collections;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // Asigná tu SpriteRenderer en el inspector
    [SerializeField] private float blinkDuration = 0.5f;    // Tiempo que tarda cada parpadeo
    [SerializeField] private int blinkCount = 3;            // Cantidad de parpadeos

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // Cambia el alpha a 0
            SetAlpha(0f);
            yield return new WaitForSeconds(blinkDuration / 2);

            // Cambia el alpha a 1
            SetAlpha(1f);
            yield return new WaitForSeconds(blinkDuration / 2);
        }

        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
