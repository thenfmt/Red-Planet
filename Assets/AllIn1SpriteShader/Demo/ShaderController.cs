using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField]
    private Sprite _sprite;
	private SpriteRenderer[] renderers;
	
    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            r.sprite = _sprite;
        }
    }
}
