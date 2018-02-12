using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour {

	// Use this for initialization
    enum LevelType
    {
        Base,
        Structure,
        Fortune
    }
    // All Base level
    List<string> BaseL = new List<string>();
    // Current Base level
    List<string> Current_BaseL = new List<string>();
    // All Structure level
    List<string> StructureL = new List<string>();
    // Current Structure level
    List<string> Current_StructureL = new List<string>();
    List<string> FortuneL = new List<string>();
	void Start () {
        NotificationCenter.DefaultCenter().AddObserver(this, "Level");
        NotificationCenter.DefaultCenter().AddObserver(this, "UnloadEnemy");
    }
	
	void Level(Notification noti)
    {
        Collider a = (Collider)noti.data;
       
        if (a.tag.Equals("BaseLevel"))
        {
            BS(a, BaseL, Current_BaseL);
        }
        else if(a.tag.Equals("StructureLevel"))
        {
            BS(a, StructureL, Current_StructureL);
        }
        else if (a.tag.Equals("FortuneLevel"))
        {
            FL(a);
        }
    }
    void BS(Collider a,List<string> Level,List<string> CurrentLevel)
    {
        string name = a.name;
        int indexOfAll = 0;
        //Debug.Log(name);
        //将即将加载的场景放到大的场景管理里面
        if (!Level.Contains(name))
        {
            Level.Add(name);
            
        }
        //找到即将要加载的场景位于所有场景的位置
        for(int i = Level.Count - 1; i >= 0; i--)
        {
            if (Level[i] == name) { indexOfAll = i; break; }
        }
        int LeftIndex = indexOfAll - 1;
        int RightIndex = indexOfAll + 1;
        if (LeftIndex < 0) LeftIndex = 0;
        if (RightIndex >= Level.Count) RightIndex = Level.Count - 1;


        if (CurrentLevel.Contains(name))
        {

        }
        else
        {
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            CurrentLevel.Add(name);
            List<string> UnloadIndex = new List<string>();
            for(int i=0;i<CurrentLevel.Count;i++)
            {
                if (CurrentLevel[i] != Level[LeftIndex] && CurrentLevel[i] != Level[indexOfAll] && CurrentLevel[i] != Level[RightIndex])
                {
                    UnloadIndex.Add(CurrentLevel[i]);
                    SceneManager.UnloadSceneAsync(CurrentLevel[i]);
                }
            }
            for(int i = 0; i < UnloadIndex.Count; i++)
            {
                CurrentLevel.Remove(UnloadIndex[i]);   
            }
            //防止重复访问
            UnloadIndex = null;
        }
    }
    void FL(Collider a)
    {
        if (FortuneL.Contains(a.name))
        {
            //we dont load that scene
        }
        else
        {
            FortuneL.Add(a.name);
            SceneManager.LoadScene(a.name,LoadSceneMode.Additive);
        }
    }
    void UnloadEnemy(Notification noti)
    {
        string name = (string)noti.data;
        SceneManager.UnloadSceneAsync(name);
    }
}
