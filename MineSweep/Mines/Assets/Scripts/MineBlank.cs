using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBlank : MonoBehaviour {

    private Renderer rend;
    public int mineNumber;        // 周边雷数
    public bool hasBoom;        // 是否有雷  
    public bool opend = false;  // 是否打开
    public bool isFlag = false;//是否被标记
	// Use this for initialization
	void Start () 
    {
        rend = GetComponent<Renderer>();

	}
	// Update is called once per frame
	void Update () 
    {
        
	}
    void OnMouseEnter() 
    {
        if (isFlag)
        {
            rend.material.color = Color.white;
        }
        else 
        {
            rend.material.color = Color.red;
        }
    }
    void OnMouseExit() 
    {
        rend.material.color = Color.white;
    }
}
