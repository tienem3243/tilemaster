using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TilePicker : MonoBehaviour
{
    public Transform tileCollectionAnchor;
    public List<Tile> tileContainer;
    public Vector3 spacing;
    public int tileContainerCap = 6;
    public UnityEvent OnSelectTile;
    public UnityEvent OnReachContainerCap;
    private Sequence sequence;
    [Tooltip("Round/second")]
    [SerializeField]private float rotationSpeed=1;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < tileContainerCap; i++)
        {
            Vector3 posGo = tileCollectionAnchor.position + spacing * i;
            Gizmos.DrawCube(posGo, Vector3.one);
        }
    }
    private void Start()
    {
        sequence= DOTween.Sequence();
    }
    private void Update()
    {
        //swing selection
        foreach (var item in tileContainer)
        {
            item.transform.Rotate(Vector3.forward, rotationSpeed*360 * Time.deltaTime);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hits;
            if (Physics.Raycast(ray, out hits))
            {
                GameObject clickedObject = hits.collider.gameObject;
                if (!clickedObject.CompareTag("Tile")) return;
                Debug.Log("ray");
                if (tileContainer.Count == tileContainerCap)
                {
                    //gamerulecheck
                    Debug.Log("rule check");
                    OnReachContainerCap.Invoke();
                    return;

                }
                var pickedTile = clickedObject.GetComponent<Tile>();
                //reset and prevent collision 
                pickedTile?.NomalizeTile();
                //find right position on container
                int posGo;
                if (tileContainer.Exists(x => x.tileName == pickedTile.tileName))
                {
                    posGo = tileContainer.FindLastIndex(x => x.tileName == pickedTile.tileName)+1;
                    Debug.Log(pickedTile.tileName + " already have in " + posGo + "]");
                    tileContainer.Insert(posGo, pickedTile);
                }
                else
                {
                    posGo = tileContainer.Count;
                    Debug.Log(pickedTile.tileName + "new in" + posGo + "]");
                    tileContainer.Add(pickedTile);

                }

               

                Vector3 des = tileCollectionAnchor.position + spacing * posGo;

                ReCallculateContainer();
               var tween= pickedTile.transform.DOMove(des, 1).onComplete += () =>
                {
                    OnSelectTile.Invoke();
                };
           

            }
        }
    }
    [ContextMenu("Recallculate")]
    private void ReCallculateContainer()
    {
        for (int i = 0; i < tileContainer.Count; i++)
        {
           
            tileContainer[i].transform.DOMove(tileCollectionAnchor.position + spacing * i, 1);
        }
    }
}
