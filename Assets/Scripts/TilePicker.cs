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
                //exception out
                if (lastPick == clickedObject) return;
                lastPick = clickedObject;
                if (!clickedObject.CompareTag("Tile")) return;
                if (tileContainer.Count == tileContainerCap)
                {

                    goto conditionCheck;

                }
                //remove occupiedPosition to creat more tile if necesary
                TileManager.Instance.occupiedPositions?.Remove(clickedObject.transform);
                //init sequence
                MainSequence = DOTween.Sequence();
                MainSequence.OnComplete(() => canPick = true);
                MainSequence.PrependCallback(() => canPick = false);

                //get tile component
                var pickedTile = clickedObject.GetComponent<Tile>();
                //reset and prevent collision of tile
                DeformTile(pickedTile);
                //find right position on container
                int posGo;

                if (tileContainer.Exists(x => x.tileName == pickedTile.tileName))
                {
                    posGo = tileContainer.FindLastIndex(x => x.tileName == pickedTile.tileName) + 1;

                    tileContainer.Insert(posGo, pickedTile);
                }
                else
                {

                    tileContainer.Add(pickedTile);
                    
                }
                Recallculate();
            //rule handler and other
            conditionCheck:
             
             
                EndPickHandler();
            }

        }
    }

    private void EndPickHandler()
    {
        OnSelectTile?.Invoke(tileContainer);
        int requireTileAmount = rule.GetParameter()[0];
        int[] res = new int[requireTileAmount];
        bool result = rule.RuleCheck(tileContainer.ToArray(), out res);
        Sequence validSequence = DOTween.Sequence();
     
        if (result)
        {
     
            foreach (var item in res)
            {
                //remove tile valid
                var trans = tileContainer[item].transform;

                validSequence.Join(trans.DOMove(tileCollectionAnchor.position + spacing * item + Vector3.down * 10, 0.6f));
                validSequence.onComplete += () => trans.gameObject.SetActive(false);
                TileManager.Instance.TotalTileCount --;
            }

            validSequence.AppendCallback(() =>
            {
                tileContainer.RemoveRange(res[0], requireTileAmount);
            });
       
            //recalculate tile positon after remove some tile
            validSequence.AppendCallback(() => Recallculate());
          
        }
        //end phase handler

        validSequence.AppendCallback(() =>
        {
           
            //check is win or lose
            if (TileManager.Instance.TotalTileCount == 0) GameManager.Instance.Win();

        });

        validSequence.AppendCallback(() =>
        {

            bool resl = rule.RuleCheck(tileContainer.ToArray(), out int[] a);

            if (tileContainer.Count == tileContainerCap && !resl)
                GameManager.Instance.Lose();

        });
        if(!MainSequence.IsActive()||MainSequence==null)
        MainSequence=DOTween.Sequence();
        
        MainSequence.Append(validSequence);

       
    }

    private void DeformTile(Tile pickedTile)
    {
        pickedTile.rb.useGravity = false;
        pickedTile.col.enabled = false;
        pickedTile.transform.DORotate(Vector3.zero, 0.3f);
        pickedTile.transform.DOScale(Vector3.one / 2, 0.3f).SetEase(scaleCurve);

    }

   
   
    public void Recallculate()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < tileContainer.Count; i++)
        {
           
            sequence.Join(tileContainer[i].transform.DOMove(tileCollectionAnchor.position + spacing * i, 0.3f));
        }
        if (!MainSequence.IsActive() || MainSequence == null)
            MainSequence = DOTween.Sequence();

        MainSequence.Append(sequence);

    }
    public void Reset()
    {
        tileContainer.Clear();
    }
}
