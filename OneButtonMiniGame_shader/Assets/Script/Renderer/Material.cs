using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Material : MonoBehaviour
{
    [SerializeField]
    private Color color_1;
    [SerializeField]
    private Color color_2;
    [SerializeField]
    public int render_queue_offset;
    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    //Material material;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        //_renderer.material.shader = Shader.Find("SpotLightAddShader.shader");
        //material = GetComponent<Material>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _renderer.material.renderQueue = (int)RenderQueue.Transparent + render_queue_offset;

        //_color = _renderer.color;

    }

    private void Update()
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        float size_x = _renderer.bounds.size.x/2;
        float size_y = _renderer.bounds.size.y/2;
        _renderer.material.SetFloat("half_size_x", size_x);
        _renderer.material.SetFloat("half_size_y", size_y);
        // _renderer.material.SetFloat("localPos_x", transform.localPosition.x);
        // _renderer.material.SetFloat("localPos_y", transform.localPosition.y);
        _materialPropertyBlock.SetColor("color_1", color_1);
        _materialPropertyBlock.SetColor("color_2", color_2);
        
        // MaterialPropertyBlockをセットする
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
