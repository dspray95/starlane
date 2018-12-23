using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour{

    public List<Transform> connections = new List<Transform>();
    public List<Transform> lineRenderers = new List<Transform>();
    bool lineRendererSet = false;
    public bool printDistance = false;
    OWNER owner = OWNER.NONE;
    public Transform starlane;

    void Start()
    {
        //Temporary, scale will be decided by star class along a distribution range
        float scale = Random.Range(1f, 4.5f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Update()
    {
        if (!lineRendererSet)
        {
            int connectedCount = 0;
            for (int i = 0; i < connections.Count; i++)
            {
                Transform tStarlane = Instantiate(starlane);
                tStarlane.parent = this.transform;
                LineRenderer lr = tStarlane.GetComponent<LineRenderer>();
                
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, connections[i].position);
                lineRenderers.Add(tStarlane);
            }
            lineRendererSet = true;
        }

        if (printDistance)
        {
            foreach(Transform connection in connections)
            {
                Debug.Log(Vector3.Distance(transform.position, connection.position));
            }
        }
    }

}
