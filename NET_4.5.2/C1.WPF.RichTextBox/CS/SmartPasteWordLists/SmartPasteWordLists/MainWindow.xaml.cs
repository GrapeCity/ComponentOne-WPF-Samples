using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace SmartPasteWordLists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var rtb = new C1.WPF.RichTextBox.C1RichTextBox();
            rtb.ClipboardPasting += rtb_ClipboardPasting;
            this.Content = rtb;

            rtb.Html = @"
<p>
    When you copy lists from Microsoft Word to the clipboard, it creates
    a custom HTML document that contains text formatted to look like 
    lists, but without list semantics (ol/ul/li tags).</p>
<p>
    If you paste this custom HTML document into a <b>C1RichTextBox</b>,
    or into any HTML editor, the lists will look correct, but you won't
    be able to insert or append items to the list as you would expect.</p>
<p>
    This sample handles the <b>ClipboardPasting</b> event to inspect the 
    content of the clipboard and to replace Word's custom lists with actual
    HTML lists. When this modified content is pasted into the editor, you 
    get lists that are semantically correct and can be edited as lists.</p>
<p>
    For example, copy a list from Word, paste it here, and then try inserting
    some new items into the list.</p>";            
        }

        void rtb_ClipboardPasting(object sender, C1.WPF.RichTextBox.Documents.ClipboardEventArgs e)
        {
            var html = Clipboard.GetText(TextDataFormat.Html);
            if (!string.IsNullOrEmpty(html))
            {
                // extract html content from clipboard 'html' string
                html = GetHtmlContent(html);

                // replace fake mso lists with real lists
                html = ReplaceOfficeLists(html);

                // update clipboard
                Clipboard.SetText(html, TextDataFormat.Html);
            }
        }

        // extract html content from clipboard 'html' string
        string GetHtmlContent(string html)
        {
            if (html.StartsWith("Version:"))
            {
                int start = GetHtmlIndex(html, "StartHtml:");
                int end = GetHtmlIndex(html, "EndHtml:");
                if (start > -1 && end > start)
                {
                    end = Math.Min(end, html.Length);
                    html = html.Substring(start, end - start).Trim();
                }
            }
            return html;
        }
        int GetHtmlIndex(string html, string tag)
        {
            var start = html.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
            if (start > -1)
            {
                var end = html.IndexOf('\r', start);
                if (end > -1)
                {
                    start += tag.Length;
                    html = html.Substring(start, end - start);
                    if (int.TryParse(html, out start))
                    {
                        return start;
                    }
                }
            }
            return -1;
        }

        // replace fake Office lists with actual Html lists
        // so user can add/delete items and the numbering will be updated
        // replace fake Office lists with actual Html lists
        // so user can add/delete items and the numbering will be updated
        string ReplaceOfficeLists(string html)
        {
            // match list items 
            // http://www.dijksterhuis.org/csharp-regular-expression-operator-cheat-sheet/
            // groups
            // 0: all
            // 1: class tag
            // 2: class name
            // 3: between p and mso-list
            // 4: level
            // 5: between level and end if p tag
            // 6: payload
            var pattern = "<p\\s+(class=([^ ]+)\\s+)?style([^>]*?)mso-list:l([0-9]+)\\s+level[0-9]+(.*?)>(.*?)</p>\\s*";
            var rx = new System.Text.RegularExpressions.Regex(pattern, RegexOptions.Singleline);
            var matches = rx.Matches(html);

            // no matches? we're done
            if (matches.Count == 0)
            {
                return html;
            }

            // stack of open list tags (lists may be nested)
            var listStack = new Stack<string>();

            // add all before first match
            var sb = new StringBuilder();
            sb.Append(html.Substring(0, matches[0].Index));

            // add each item
            for (int i = 0; i < matches.Count; i++)
            {
                // get this match
                var m = matches[i];

                // get item payload
                var payload = m.Groups[6].Value;

                // add list start
                // if (previous == null || previous.level < level) -> list start
                if (listStack.Count == 0 || GetLevel(matches[i - 1]) < GetLevel(m))
                {
                    var openTag = GetListTag(payload, true);
                    var closeTag = GetListTag(payload, false);
                    listStack.Push(closeTag);
                    sb.AppendLine(openTag);
                }

                // remove hard-wired numbers/bullets etc
                var itemStart = payload.IndexOf("<![if !supportLists]>");
                if (itemStart > -1)
                {
                    var itemEnd = payload.IndexOf("<![endif]>", itemStart);
                    if (itemEnd > itemStart)
                    {
                        payload = payload.Substring(0, itemStart) + payload.Substring(itemEnd + 10);
                    }
                }
                payload = payload.Replace("<span dir=LTR></span>", string.Empty).Replace("<o:p></o:p>", string.Empty);

                // add this item
                var item = m.Groups[2].Value.Length > 0
                    ? string.Format("<li class={0}>{1}</li>", m.Groups[2], payload)
                    : string.Format("<li>{0}</li>", payload);
                sb.AppendLine(item);

                // get stuff between this and the next item
                var endThis = m.Index + m.Length;
                var startNext = i < matches.Count - 1
                    ? matches[i + 1].Index
                    : html.Length;
                var continuation = html.Substring(endThis, startNext - endThis);

                // add list end
                // if (next == null || next.level < level) -> list end
                if (i == matches.Count - 1 || GetLevel(matches[i + 1]) < GetLevel(m) || continuation.IndexOf("<p ") > -1)
                {
                    var closeTag = listStack.Pop();
                    sb.AppendLine(closeTag);
                }

                // add stuff between this and the next item
                sb.Append(continuation);
            }

            // done
            return sb.ToString();
        }
        int GetLevel(Match m)
        {
            var level = -1;
            int.TryParse(m.Groups[4].Value, out level);
            return level;
        }
        string GetListTag(string openTag, bool open)
        {
            // assume unordered list, get type
            var tag = "ul";
            var type = "disc";
            if (openTag.Contains("font-family:Wingdings"))
            {
                type = "square";
            }
            else if (openTag.Contains("font-family:\"Courier New\""))
            {
                type = "circle";
            }

            // handle ordered lists
            var itemHeader = GetItemHeader(openTag);
            switch (itemHeader)
            {
                case '1':
                    tag = "ol";
                    type = "decimal";
                    break;
                case 'a':
                    tag = "ol";
                    type = "lower-alpha";
                    break;
                case 'A':
                    tag = "ol";
                    type = "upper-alpha";
                    break;
                case 'I':
                    tag = "ol";
                    type = "upper-roman";
                    break;
                case 'i':
                    tag = "ol";
                    type = "lower-roman";
                    break;
            }

            // done
            return open
                ? string.Format("<{0} style='list-style-type:{1}'>", tag, type)
                : string.Format("</{0}>", tag, type);
        }
        char GetItemHeader(string openTag)
        {
            var pos = openTag.IndexOf("mso-list:Ignore", StringComparison.OrdinalIgnoreCase);
            if (pos > -1)
            {
                pos = openTag.IndexOf('>', pos);
                if (pos > -1)
                {
                    var itemHeader = openTag[pos + 1];
                    if (itemHeader == '<')
                    {
                        pos = openTag.IndexOf("</span>", pos + 2);
                        if (pos > -1)
                        {
                            itemHeader = openTag[pos + 7];
                        }
                    }
                    return itemHeader;
                }
            }
            return ' ';
        }
    }
}
