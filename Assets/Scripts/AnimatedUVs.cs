using UnityEngine;
using System.Collections;

public class AnimatedUVs : MonoBehaviour
{
    public int materialIndex = 0;
    public float speed;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";
    Renderer myRenderer;
    Vector2 uvOffset = Vector2.zero;
    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }
    void LateUpdate()
    {
        uvOffset -= (uvAnimationRate * Time.deltaTime * speed);
        if (myRenderer.enabled)
        {
            myRenderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}