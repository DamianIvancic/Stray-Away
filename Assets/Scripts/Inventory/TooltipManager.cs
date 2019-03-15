using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public Transform TooltipPanel;
    public Image ItemImage;
    public Text ItemName;
    public Text ItemValue;
    public Text ItemDescription;
    public Vector3 PositionOffset;

    public static TooltipManager Instance;
	
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            DisableTooltip();
        }
        else
            Destroy(gameObject);
    }

	void Update () {
        //be careful when using different resolutions since offset is in pixels
        TooltipPanel.position = Input.mousePosition + PositionOffset;
	}

    public void DisableTooltip()
    {
        TooltipPanel.gameObject.SetActive(false);
    }

    public void EnableTooltip(Item item)
    {
        if (item == null)
            return;

        ItemImage.sprite = item.Icon;
        ItemName.text = item.ItemName;
        ItemValue.text = "value: " + item.ItemValue.ToString();
        ItemDescription.text = item.ItemDescription;

        TooltipPanel.gameObject.SetActive(true);
    }

}
