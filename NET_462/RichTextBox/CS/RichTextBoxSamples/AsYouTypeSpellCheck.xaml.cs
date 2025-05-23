﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.SpellChecker;
using System.Reflection;
using C1.WPF.Theming;
using C1.WPF.Theming.Office2016;

namespace RichTextBoxSamples
{
    public partial class AsYouTypeSpellCheck : UserControl
    {
        public AsYouTypeSpellCheck()
        {
            InitializeComponent();

            rtb.Text = @"Some facts about Mark Twain (spelling errors intentional ;-)

A steambat pilot neded a vast knowldege of the ever-chaging river to be able to stop at any of the hundreds of ports and wood-lots along the river banks. Twain meticulosly studied 2,000 miles (3,200 km) of the Mississipi for more than two years before he received his steamboat pilot license in 1859.

While training, Samuel covinced his younger brother Henry to work with him. Heny was killed on June 21, 1858, when the steamboat he was working on, the Pennsylvania, exploded. Twain had foreseen this death in a detailed dream a month earlier, [13] which inspired his interest in parasychology; he was an early member of the Society for Psychical Research.[14] Twain was guilt-strcken over his brother's death and held himself responsible for the rest of his life. However, he continued to work on the river and served as a river pilot until the American Civil War broke out in 1861 and traffic along the Mississipi was curtaled.

Missouri was a slave state and considered by many to be part of the South, and was represented in both the Confederate and Federal governments during the Civil War. When the war began, Twain and his friends formed a Confederate militia (depcted in an 1885 short story, ""The Private History of a Campaign That Failed""), which drilled for only two weeks before disbanding.[15] Twain joinded his brother, Orion, who had been apointed secretary to the teritorial govenor of Nevada, James W. Nye, and headed west.
 
Twain then traveled to San Francisco, California, where he comtinued as a journalist and began lecturing. He met other writers such as Bret Harte, Artemus Ward and Dan DeQuille. An assignment in Hawaii became the basis for his first lectures.[18] In 1867, a local newspaper funded a trip to the Mediterrannean. During his tohr of Europe and the Midddle East, he wrote a popular colection of travel letters wich were compled as The Inocents Abroad in 1869.";

            toolbar.RichTextBox = rtb;

            var spell = new C1SpellChecker();
            rtb.SpellChecker = spell;
            spell.MainDictionary.Load(Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/C1Spell_en-US.dct", UriKind.Relative)).Stream);

            var theme = new C1ThemeOffice2016White();
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                // this will aplly theme to everything displayed in adorner, including any C1Window instances
                C1Theme.ApplyTheme(adornerLayer, theme);
            }
        }
    }
}
