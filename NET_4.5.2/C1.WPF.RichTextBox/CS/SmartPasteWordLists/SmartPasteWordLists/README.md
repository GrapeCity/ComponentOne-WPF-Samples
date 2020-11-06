## SmartPasteWordLists
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.RichTextBox\CS\SmartPasteWordLists\SmartPasteWordLists)
____
#### Customize clipboard content to improve list pasting.
____
When you copy lists from Microsoft Word to the clipboard, it creates
a custom HTML document that contains text formatted to look like 
lists, but without list semantics (ol/ul/li tags).

If you paste this custom HTML document into a C1RichTextBox, or
into any HTML editor, the lists will look correct, but you won't
be able to insert or append items to the list as you would expect.

This sample handles the ClipboardPasting event to inspect the content
of the clipboard and to replace Word's custom lists with actual
HTML lists.

When this modified content is pasted into the editor, you get 
lists that are semantically correct and can be edited as
lists. For example, pressing enter after item 2 will create item 3 
and renumber all items below.

