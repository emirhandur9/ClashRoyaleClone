using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Camera mainCam;
    [SerializeField] Canvas canvas;
    [SerializeField] Transform soldierPreview;
    [SerializeField] TextMeshProUGUI elixirText;
    [SerializeField] TextMeshProUGUI soldierTypeText;

    [SerializeField] LayerMask rayLayer;
    [SerializeField] SoldierCard soldierCard;

    private RectTransform rectT;
    private Image img;
    private HorizontalLayoutGroup layout;

    private bool isSoldierPlaced;
    private bool canDraggable;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        rectT = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        layout = GetComponentInParent<HorizontalLayoutGroup>();
        soldierPreview.gameObject.SetActive(false);
        elixirText.text = soldierCard.elixirAmount.ToString();
        soldierTypeText.text = soldierCard.soldierType.ToString();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isSoldierPlaced = false;
        canDraggable = GameManager.Instance.CheckIfElixirAmountEnough(soldierCard.elixirAmount);

        if(canDraggable)
        {
            layout.enabled = false;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDraggable) return;


        rectT.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if(rectT.anchoredPosition.y > 125)
        {
            img.color = new Color(0, 0, 0, 0);
            elixirText.color = new Color(0, 0, 0, 0);
            soldierTypeText.color = new Color(0, 0, 0, 0);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLayer))
            {
                Vector3 pos = Vector3.zero;
                if(rectT.anchoredPosition.y < 700)
                {
                    pos = new Vector3(hit.point.x, soldierPreview.position.y, hit.point.z);
                }
                else
                {
                    pos = new Vector3(hit.point.x, soldierPreview.position.y, 8); //goz hesabi xd
                }

                soldierPreview.transform.position = pos;

            }
            isSoldierPlaced = true;
            soldierPreview.gameObject.SetActive(true);
        }
        else
        {
            img.color = new Color(0, 0, 0, 1);
            elixirText.color = new Color(1, 1, 1, 1);
            soldierTypeText.color = new Color(1, 1, 1, 1);
            isSoldierPlaced = false;
            soldierPreview.gameObject.SetActive(false);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        img.color = new Color(0, 0, 0, 1);
        elixirText.color = new Color(1, 1, 1, 1);
        soldierTypeText.color = new Color(1, 1, 1, 1);
        layout.enabled = true;
        soldierPreview.gameObject.SetActive(false);
        if (isSoldierPlaced)
        {
            GameManager.Instance.SoldierPlaced(soldierCard, soldierPreview.position);
        }
    }


}
