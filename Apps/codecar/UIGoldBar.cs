
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIGoldBar : UIView
{
    public Image imageBg;
    public Image imageGold;
    public Text textGold;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textGold.text = Common.gold.ToString();

        LayOutChild();
    }


    void LayOutChild()
    {
        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rctran = imageGold.GetComponent<RectTransform>();
            rctran.anchoredPosition = new Vector2(-this.frame.width / 4, 0);
        }
        {
            RectTransform rctran = textGold.GetComponent<RectTransform>();
            rctran.anchoredPosition = new Vector2(this.frame.width / 4, 0);
        }

    }

    public void UpdateGold(int gold)
    {
        textGold.text = gold.ToString();
    }

    public void OnClickGold()
    {
        ShopViewController.main.Show(null,null);
    }


}
