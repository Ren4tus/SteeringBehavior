using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    #region Singleton Definition
    private static ObjectManager instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            ReCalculationObjectArray();
        }
    }
    public static ObjectManager Instance
    {
        get
        {
            if (null == instance)
            {
                return instance;
            }
            return instance;
        }
    }


    #endregion
    [HideInInspector]
    public Transform InvertTransform;
    [HideInInspector]
    public List<GameObject> ObjectCollection;

    int IDCount = 0;
    [HideInInspector]
    public int PlayerID = 0;
    [HideInInspector]
    public int BossID;
    [HideInInspector]
    public List<GameObject> Obstacles;
    void Start()
    {
        
        
    }

    public void ReCalculationObjectArray()
    {
        ObjectCollection.Clear();
        IDCount = 0;
        ObjectCollection.Add(GameObject.Find("Player"));
        ObjectCollection[0].GetComponent<BaseGameEntity>().ID = 0;
        IDCount++;
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ObjectCollection.Add(GO);
            GO.GetComponent<BaseGameEntity>().ID = IDCount;
            if (GO.gameObject.name == "Boss")
                BossID = IDCount;
            IDCount++;
        }

        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("objObstacle"))
        {
            Obstacles.Add(GO);
        }


    }
    
}
