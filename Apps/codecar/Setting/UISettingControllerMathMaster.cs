using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


// enum SettingItemTag
// {
//     TAG_SETTING_COMMENT = 0,
//     TAG_SETTING_VERSION,
//     TAG_SETTING_LANGUAGE,
//     TAG_SETTING_BACKGROUND_MUSIC,
//     TAG_SETTING_NOAD,
//     TAG_SETTING_RESTORE_IAP
// }
public class UISettingControllerMathMaster : UISettingControllerBase, ITableViewDataSource
{
    const int TAG_HOWTOPLAY = (int)SettingItemTag.TAG_SETTING_LAST + 0;
    public override void UpdateItem()
    {
        base.UpdateItem();
        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString("STRING_HOWTOPLAY_TITLE0");
            info.tag = TAG_HOWTOPLAY;
            listItem.Add(info);
        }
    }
    public override void OnClickItem(UICellItemBase item)
    {
        switch (item.tagValue)
        {
            case TAG_HOWTOPLAY:
                {
                    
                }
                break;


        }
    }
}
