using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour{

    public List<Starlane> starlanes = new List<Starlane>();
    public List<Transform> lineRenderers = new List<Transform>();
    public bool printDistance = false;
    public OWNER owner = OWNER.NONE;
    public Transform starlane;

    public Button btnCorvette;

    public void Awake()
    {
        btnCorvette = GameObject.Find("btn_corvette").GetComponent<Button>();
    }

    public bool Equals(Planet b)
    {
        if (Vector3.Equals(this.transform.position, b.transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetOwner(OWNER owner)
    {
        this.owner = owner;
        Transform glowplane = transform.GetChild(0).transform.GetChild(0);
        if (owner == OWNER.PLAYER)
        {
            glowplane.gameObject.GetComponent<Renderer>().material.color  = Color.cyan;
        }
        else if(owner == OWNER.AI1)
        {
            glowplane.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        foreach(Starlane sl in starlanes)
        {
            sl.UpdateColors();
        }
    }
    // Use this for initialization
    public static Planet Create(Transform starPrefab)
    {
        GameObject obj = new GameObject("Star-" + Random.Range(0,200).ToString(), typeof(Planet));
        obj.tag = "Star";
        Transform star = Transform.Instantiate(starPrefab);
        float scale = Random.Range(1f, 4.5f);
        star.transform.localScale = new Vector3(scale, scale, scale);
        star.transform.parent = obj.transform;
        return obj.GetComponent<Planet>();
    }
}
