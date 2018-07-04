using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public int x;                   // 记录x坐标  
    public int y;                // 记录y坐标
    public int mineNumber;        // 周边雷数
 
    public bool hasBoom;        // 是否有雷  
    public bool opend = false;  // 是否打开
    public bool isFlag = false;

    public GameObject mine;

}
