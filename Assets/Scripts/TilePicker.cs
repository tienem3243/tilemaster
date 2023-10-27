using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sequence = DG.Tweening.Sequence;

public class TilePicker : MonoBehaviour
{
    public Transform tileCollectionAnchor;
    public List<Tile> tileContainer;
    public Vector3 spacing;
    public int tileContainerCap = 6;
    public UnityEvent<List<Tile>> OnSelectTile;
    public UnityEvent OnReachContainerCap;
    private Sequence MainSequence;

    private Gamerule rule;
    private bool canPick=true;
    private GameObject lastPick=default;
    public AnimationCurve scaleCurve;
    public AnimationCurve moveToContenerCurve;
    private void OnDrawGizmos()
    {
        for (int i = 0; i < tileContainerCap; i++)
        {
            Vector3 posGo = tileCollectionAnchor.position + spacing * i;
            Gizmos.DrawCube(posGo, Vector3.one/2);
        }
    }
    private void Start()
    {
        MainSequence = DOTween.Sequence();
        if (OnSelectTile == null) OnSelectTile = new();
        if (OnReachContainerCap == null) OnReachContainerCap = new();
        //init rule
        rule = new FirstDuplicate(3);
    }
    private void Update()
    {
      /*  //swing selection
        foreach (var item in tileContainer)
        {
            item.transform.Rotate(Vector3.forward, rotationSpeed * 360 * Time.deltaTime);
        }*/

        if (Input.GetMouseButtonDown(0)&&canPick&&!EventSystem.current.IsPointerOverGameObject())
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hits;
            if (Physics.Raycast(ray, out hits))
            {
                GameObject clickedObject = hits.collider.gameObject;
                if (lastPick == clickedObject) return;
                lastPick = clickedObject;
                if (!clickedObject.CompareTag("Tile")) return;
                Debug.Log("ray");
             
                TileManager.Instance.occupiedPositions.Remove(clickedObject.transform);
                MainSequence = DOTween.Sequence();
                MainSequence.OnComplete(() => canPick = true);
                MainSequence.PrependCallback(() => canPick = false);

                var pickedTile = clickedObject.GetComponent<Tile>();
                //reset and prevent collision 
                DeformTile(pickedTile);
                //find right position on container
                int posGo;

                if (tileContainer.Exists(x => x.tileName == pickedTile.tileName))
                {
                    posGo = tileContainer.FindLastIndex(x => x.tileName == pickedTile.tileName) + 1;
                    Debug.Log("insert");

                    InserTile(pickedTile, posGo);
                }
                else
                {
                    Debug.Log("add");
                    AddTile(pickedTile);
                    return;
                }
                //rule handler and other
                OnSelectTile?.Invoke(tileContainer);
                int requireTileAmount = rule.GetParameter()[0];
                int[] res = new int[requireTileAmount];
                bool result = rule.RuleCheck(tileContainer.ToArray(), out res);
                if (result)
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.Pause();
                    foreach (var item in res)
                    {
                        var trans = tileContainer[item].transform;

                        sequence.Join(trans.DOMove(tileCollectionAnchor.position + spacing * item + Vector3.down * 10, 0.5f));
                    }

                    sequence.AppendCallback(() =>
                    {
                        tileContainer.RemoveRange(res[0], requireTileAmount);
                    });
                    MainSequence.Append(sequence);
                    MainSequence.AppendCallback(() => Recallculate());
                    MainSequence.AppendCallback(() =>
                    {
                        TileManager.Instance.TotalTileCount -= requireTileAmount;
                        //check is win or lose
                        if (TileManager.Instance.TotalTileCount == 0) GameManager.Instance.Win();
                        if (tileContainer.Count == tileContainerCap)
                        {
                            //gamerulecheck
                            GameManager.Instance.Lose();
                            OnReachContainerCap.Invoke();
                            return;

                        }
                    });
                }

            }

        }
    }

    private void DeformTile(Tile pickedTile)
    {
        pickedTile.rb.useGravity = false;
        pickedTile.col.enabled = false;
        pickedTile.transform.DORotate(Vector3.zero, 0.5f);
        pickedTile.transform.DOScale(Vector3.one / 2, 0.5f).SetEase(scaleCurve);
    }

    [ContextMenu("Recallculate")]
    private void InserTile(Tile tile, int index)
    {
        tileContainer.Insert(index, tile);
        Sequence sequence = DOTween.Sequence();
      
        for (int i = 0; i < tileContainer.Count; i++)
        {
            if (i == index) continue;
            sequence.Join(tileContainer[i].transform.DOMove(tileCollectionAnchor.position + spacing * i, 0.2f));
        }

        sequence.Join(tile.transform.DOMove(tileCollectionAnchor.position + spacing * index, 0.3f))
            .SetEase(moveToContenerCurve);
        sequence.Pause();
        MainSequence.Append(sequence);
    }
    private void AddTile(Tile tile)
    {
       
       Tween tween= tile.gameObject.transform.DOMove(tileCollectionAnchor.position + spacing * tileContainer.Count, 0.2f)
                    .SetEase(moveToContenerCurve);
        tileContainer.Add(tile);
        tween.Pause();
        MainSequence.Append(tween);
    }
    public void Recallculate()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < tileContainer.Count; i++)
        {
            sequence.Join(tileContainer[i].transform.DOMove(tileCollectionAnchor.position + spacing * i, 0.2f));
        }
    
    }
    private void Reset()
    {
        
    }
}
