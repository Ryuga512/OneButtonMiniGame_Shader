using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    private Color _color;
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
    }

    private void Update()
    {
        // この時点ですでにほかのスクリプトなどからMaterialPropertyBlockが
        // セットされているかもしれないので、まずは取得する
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        // MaterialPropertyBlockに対して色をセットする
        _materialPropertyBlock.SetColor("_Color", _color);
        
        // MaterialPropertyBlockをセットする
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
