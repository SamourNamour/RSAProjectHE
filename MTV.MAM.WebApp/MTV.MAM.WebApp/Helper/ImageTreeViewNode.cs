using System;
//using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

public class ImageTreeViewNode : TreeNode
{

    // property to hold our icon data
    private ListDictionary m_StatusIcons;
    public ListDictionary StatusIcons
    {
        get
        {
            return m_StatusIcons;
        }

        set
        {
            m_StatusIcons = value;
        }
    }

    protected override void RenderPostText(HtmlTextWriter writer)
    {
        if (m_StatusIcons != null)
        {
            // loop thru each item in our icon data and write out an img tag
            foreach (DictionaryEntry statusIcon in m_StatusIcons)
            {
                writer.AddAttribute("src", "Common/" + statusIcon.Value);
                writer.AddAttribute("alt", statusIcon.Key.ToString());
                writer.AddAttribute("title", statusIcon.Key.ToString());
                writer.AddAttribute("style", "padding-left:5px");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();

            }
        }
        base.RenderPostText(writer);
    }

}

