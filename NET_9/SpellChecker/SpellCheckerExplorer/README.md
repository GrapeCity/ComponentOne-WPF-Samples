## C1SpellChecker Sample
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_9/SpellChecker/SpellCheckerExplorer)
____
#### Shows how to use the C1SpellChecker object to check TextBox and C1RichTextBox controls.
____
The sample shows how to load spell dictionaries, how to spell-check strings (CheckText), how to
spell-check controls using a spell dialog (CheckControl), and how to enable as-you-type spell
checking on a C1RichTextBox.

C1SpellChecker uses two types of spell dictionaries. The main dictionary is a large, read-only, compressed
word list that is distributed with the control. ComponentOne includes spell dictionaries for over 15 
languages. The sample loads the main dictionary (C1Spell_en-US.dct) using this code:

	// load main dictionary from resources
    _c1SpellChecker.MainDictionary.Load(Application.GetResourceStream(new Uri("/" + 
	new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/C1Spell_en-US.dct", UriKind.Relative)).Stream);

If you don't load the main dictionary, you won't be able to spell-check.

Besides the main dictionary, C1SpellChecker has a 'user' dictionary that is a smaller, read-write word list
customizable by the developer and by users (through the "Add" button in the Spell Dialog). The user dictionary
is optional. It is commonly used to add terms that make sense for particular users, such as names, technical 
terms, etc.

The sample overrides the default behavior of a TextBox when focus is lost. By handling the LostFocus event
you can maintain highlighting text even when the control does lose focus.

```
// keep text selected while spell-checking dialog is open
private void _plainTextBox_LostFocus(object sender, RoutedEventArgs e)
{
    e.Handled = true;
}
```




