using System;
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
using C1.WPF;
using System.Diagnostics;
using System.Windows.Markup;
using C1.WPF.Core;

namespace CoreExplorer
{
    public partial class DemoListBox : UserControl
    {
        C1DragDropManager _dd;

        public DemoListBox()
        {
            InitializeComponent();
            Tag = "This demo shows how you can drag and drop between two UI controls using C1DragDropManager.";

            // create drag/drop manager
            _dd = new C1DragDropManager();

            // register drop targets
            foreach (ListBox lb in new ListBox[] { _list1, _list2 })
            {
                _dd.RegisterDropTarget(lb, true);
                foreach (Person p in Person.Generate(5))
                {
                    var personElement = new ContentPresenter();
                    personElement.Content = p;
                    personElement.ContentTemplate = (DataTemplate)Resources["StudentTemplate"];

                    lb.Items.Add(personElement);
                    _dd.RegisterDragSource(personElement, DragDropEffect.Move, ModifierKeys.None);
                    personElement.MouseDown += (s, e) =>
                    {
                        // _dd is the first to get the event.
                        // Then we mark it handled to inhibit ListBox selection stealing _dd's mouse capture.
                        e.Handled = true;
                    };
                }
            }

            // handle the drops
            _dd.DragDrop += _dd_DragDrop;
        }

        // perform drop
        void _dd_DragDrop(object source, DragDropEventArgs e)
        {
            // get object being dragged
            UIElement sourceElement = e.DragSource;

            // get source parent, target listboxes
            ListBox sourceParent = _list1.Items.Contains(sourceElement) ? _list1 : _list2;
            ListBox target = e.DropTarget as ListBox;

            // get index into target
            int index = GetDropIndex(e, target);

            // adjust index
            if (sourceParent == target)
            {
                int sourceIndex = sourceParent.Items.IndexOf(sourceElement);
                if (index > sourceParent.Items.IndexOf(sourceElement))
                {
                    index--;
                }
                if (index == sourceIndex)
                    return;
            }

            // remove from original position, insert into new position
            sourceParent.Items.Remove(sourceElement);
            target.Items.Insert(index, sourceElement);
        }

        #region ** utility

        // get drop location within a ListBox
        public static int GetDropIndex(DragDropEventArgs e, ListBox target)
        {
            int index = 0;
            foreach (UIElement child in target.Items)
            {
                Point p = e.GetPosition(child);
                if (p.Y - child.DesiredSize.Height / 2 < 0) break;
                index++;
            }
            return index;
        }

        #endregion
    }
}
