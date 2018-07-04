using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour {
    public GameObject blankPrefab;
    public GameObject minePrefab;
    public GameObject flagPrefab;
    public GameObject[] number;
    public GameObject EndUI;
    public GameObject VictoryUI;
    public Text UITime;
    [HideInInspector]
    public GameObject blank;
    [HideInInspector]
    public MineBlank[,] map;
    public int row;    // 行数  
    public int column; // 列数
    public int mineQuantity;//雷数
    private int blankCount = 0;//格子打开计数
    private bool over = false;// 游戏是否结束

    void CreateMap ()  
    {  
        map = new MineBlank[row, column];    // 创建初始化数组  
        for (int i = 0; i < row; i++) 
        {  
            for (int j = 0; j < column; j++) 
            {  
                blank=Instantiate(blankPrefab, new Vector3(i*5, 0, j*5), Quaternion.identity);
                map[i, j] = blank.GetComponent<MineBlank>();
            }  
        }
    }  
  
    void ComputerNumber () // 遍历数组，查找每个格子周围雷的个数  
    {  
        for (int i = 0; i < row; i++) 
        {  
            for (int j = 0; j < column; j++) 
            {  
                map [i, j].mineNumber = CountBoom (i, j);  
            }  
        }  
    }  
  
    int CountBoom (int x, int y)  // 计算每个方格的周围雷数（九宫格形状，除去中心位置的）  
    {  
        int count = 0;  
        for (int i = -1; i <= 1; i++) {  
            for (int j = -1; j <= 1; j++) {  
                if (!(i == 0 && j == 0)) {  
                    int n = x + i;  
                    int m = y + j;  
                    if (n >= 0 && n < row && m >= 0 && m < column) { // 如果是雷，计数加1  
                        if (map [n, m].hasBoom) {  
                            count++;  
                        }  
                    }  
                }  
            }  
        }  
        return count;  
    }  
  
    void Open (int x, int y,GameObject gameObj)  // 打开雷数为0的格子  
    {  
        map [x, y].opend = true;
        blankCount++;
        if (VictoryCheck())
        {
            GameManager.Instance.Win();
            VictoryUI.SetActive(true);
            GameObject obj = VictoryUI.transform.Find("TimeCostText").gameObject;
            Text victoryText = obj.GetComponent<Text>();
            victoryText.text = "你花了" + ((int)Time.time).ToString() + "秒";

        }
        Destroy(gameObj);
        if (map[x, y].mineNumber == 0 && !map[x, y].hasBoom && !map[x, y].isFlag)
        {
            Destroy(gameObj);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        int n = x + i;
                        int m = y + j;
                        if (n >= 0 && n < row && m >= 0 && m < column && map[n, m].opend == false)
                        {
                            Open(n, m, map[n, m].gameObject);
                        }
                    }
                }
            }
        }
        else if(map[x,y].hasBoom)
        {
            GameManager.Instance.Failed();
            EndUI.SetActive(true);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (map[i, j].hasBoom)
                    {
                        Instantiate(minePrefab, map[i, j].gameObject.transform.position, Quaternion.identity);
                        Destroy(map[i, j].gameObject);
                    }
                }
            }  
        }
        else if (!map[x, y].isFlag)
        {
            Destroy(gameObj);
            Instantiate(number[map[x, y].mineNumber-1], map[x, y].gameObject.transform.position, Quaternion.identity);
        }
    }  
  
    void addMine (int boomCount)  // 为地图中添加的雷数  
    {  
        for (int i = 0; i < boomCount; i++) {
            int r = UnityEngine.Random.Range(0, row);
            int c = UnityEngine.Random.Range(0, column);
            if (!map[r, c].hasBoom)
                map[r, c].hasBoom = true;
            else
                boomCount++;
        }
    }
    void ShowTime() 
    {
        float timeSpend = Time.time;
        int hour = (int)timeSpend / 3600;
        int minute = ((int)timeSpend - hour * 3600) / 60;
        int second = (int)timeSpend - hour * 3600 - minute * 60;
        UITime.text = string.Format("时间：{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
    }
    void clickBlankCheck()
    {
        //鼠标点击检测
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线             
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject obj = hitInfo.collider.gameObject;
                int x = (int)obj.transform.position.x;
                int y = (int)obj.transform.position.z;
                if (!map[x / 5, y / 5].opend && !map[x / 5, y / 5].isFlag)
                {
                    Open(x / 5, y / 5, map[x / 5, y / 5].gameObject);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线             
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject obj = hitInfo.collider.gameObject;
                int x = (int)obj.transform.position.x;
                int y = (int)obj.transform.position.z;
                if (!map[x / 5, y / 5].isFlag )
                {
                    GameObject chid = Instantiate(flagPrefab, map[x / 5, y / 5].transform.position, Quaternion.identity);
                    chid.transform.parent = map[x / 5, y / 5].gameObject.transform;
                    map[x / 5, y / 5].isFlag = true;
                }
                else
                {
                    map[x / 5, y / 5].isFlag = false;
                    Destroy(map[x / 5, y / 5].gameObject.transform.GetChild(0).gameObject);
                }
            }
        }
    }
   public void Stop() 
    {
        over = true;
    }
   bool VictoryCheck()
   {
       if (blankCount == ((column * row) - mineQuantity))
       {
           return true;
       }
       else 
       {
           return false;
       }
   }
    void Start ()
   {
        row = Convert.ToInt32(Global.GCloumn);
        column = Convert.ToInt32(Global.GRow);
        mineQuantity = Convert.ToInt32(Global.GMineNumber); 
        CreateMap();// 初始化地图
        addMine(mineQuantity);//埋雷
        ComputerNumber();//计算雷数
        GameManager.Instance.mine = this;
    }  
  
    void Update ()
    {
        if (!over)
        {
            clickBlankCheck();
        }
        ShowTime();
    }  
}
