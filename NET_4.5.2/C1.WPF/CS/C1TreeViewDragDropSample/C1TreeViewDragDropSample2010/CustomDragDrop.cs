using System.Collections;
using System.Windows;
using System.Windows.Controls;
using C1.WPF;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Represents a Helper class used to implement the custom Drag&Drop functionality
    /// for a C1TreeViewItem.
    /// </summary>
    public class CustomDragDrop
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="CustomDragDrop"/> object.
        /// </summary>
        /// <param name="tree"></param>
        public CustomDragDrop(C1TreeView tree)
        {
            Tree = tree;
            Mark = new DropMark();
            Effect = DragDropEffect.Move;
        }

        /// <summary>
        /// The C1TreeView that will use this custom Drag&Drop implementation.
        /// </summary>
        private C1TreeView Tree { get; set; }

        /// <summary>
        /// A custom visual element used to indicate what the target of the drop will be.
        /// </summary>
        private DropMark Mark { get; set; }

        /// <summary>
        /// The desired effect for the drop action (move or copy elements)
        /// </summary>
        public DragDropEffect Effect { get; set; }

        /// <summary>
        /// Used to handle the DragOver event.
        /// Adds the mark to indicate the drop target
        /// Determines if a given node of the tree is a valid drop location
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="e">The event arguments</param>
        public void HandleDragOver(object source, DragDropEventArgs e)
        {
            C1DragDropManager dragDropManager = source as C1DragDropManager;
            if (Mark.Parent == null)
                dragDropManager.Canvas.Children.Add(Mark);
            Mark.Visibility = Visibility.Collapsed;
            e.Effect = DragDropEffect.None;

            // Check if is a valid drop action
            C1TreeViewItem target = Tree.GetNode(e.GetPosition(null));
            if (IsValidDrop(e.DragSource as C1TreeViewItem, target))
            {
                // get target with respect to drag/drop canvas
                Point p = target.C1TransformToVisual(dragDropManager.Canvas).Transform(new Point());
                Point pos = e.GetPosition(target);
                if (pos.Y > target.RenderSize.Height / 2)
                {
                    p.Y += target.RenderSize.Height;
                }
                Mark.SetValue(Canvas.LeftProperty, p.X);
                Mark.SetValue(Canvas.TopProperty, p.Y);
                Mark.Visibility = Visibility.Visible;
                e.Effect = DragDropEffect.Move;
            }
        }

        /// <summary>
        /// Used to handle the DragDrop event.
        /// Determines the target and fires the corresponding action (move or copy).
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="e">The event arguments</param>
        public void HandleDragDrop(object source, DragDropEventArgs e)
        {
            if (e.Effect != DragDropEffect.None)
            {
                // make the move...
                C1TreeViewItem item = e.DragSource as C1TreeViewItem;
                if (item != null)
                {
                    // find target node
                    C1TreeViewItem target = Tree.GetNode(e.GetPosition(null));
                    if (target != null)
                    {
                        Point p = e.GetPosition(target);
                        bool insertAfter = (p.Y > target.RenderSize.Height / 2);
                        if (Effect == DragDropEffect.Move)
                        {
                            DoMove(item, target, insertAfter);
                        }
                        else
                        {
                            DoCopy(item, target, insertAfter);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the target node is a valid drop location.
        /// For this sample, a valid target node is the one that:
        ///     Represents an employee (departments are not valid)
        ///     Is not being copied to the same department it is assigned
        ///     Is not being moved to a department it is already assigned
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidDrop(C1TreeViewItem source, C1TreeViewItem target)
        {
            bool isValidTarget = false;
            // For this sample we will allow dragging of employees only
            if ((source != null) && (target != null) && (source.Header is Employee) && (target.Header is Employee))
            {
                // Cannot copy employees in the same department
                Employee sourceDataObject = (Employee)source.Header;
                bool isAlreadyInDepartment = ((IList)target.ParentItemsSource).Contains(sourceDataObject);
                if ((isAlreadyInDepartment) && ((Effect == DragDropEffect.Copy) || (source.Parent != target.Parent)))
                {
                    isValidTarget = false;
                }
                else
                {
                    isValidTarget = (!source.IsAncestorOf(target) && !(source == target));
                }
            }
            return isValidTarget;
        }

        /// <summary>
        /// Copies a node to the same collection the target node belongs to.
        /// </summary>
        /// <param name="source">The source node to copy</param>
        /// <param name="target">The target node</param>
        /// <param name="copyAfter">Indicates if the source node will be copied after
        /// (larger index) the target </param>
        private void DoCopy(C1TreeViewItem source, C1TreeViewItem target, bool copyAfter)
        {
            int targetIndex = target.Index;
            IList targetCollection = (IList)target.ParentItemsSource;

            // adjust target index if necessary (insert after or before the target)
            if (copyAfter)
                targetIndex++;

            // copy source node and insert
            Employee sourceCopy = ((Employee)source.Header).Clone();
            targetCollection.Insert(targetIndex, sourceCopy);
        }

        /// <summary>
        /// Moves a node to the same collection the target node belongs to.
        /// </summary>
        /// <param name="source">The source node to move</param>
        /// <param name="target">The target node</param>
        /// <param name="copyAfter">Indicates if the source node will be moved after
        /// (larger index) the target </param>
        private void DoMove(C1TreeViewItem source, C1TreeViewItem target, bool moveAfter)
        {
            // get source collection/index
            int sourceIndex = source.Index;
            int targetIndex = target.Index;
            IList sourceCollection = (IList)source.ParentItemsSource;
            IList targetCollection = (IList)target.ParentItemsSource;

            // adjust target index if necessary (insert after or before the target)
            if (moveAfter)
                targetIndex++;
            if (sourceCollection == targetCollection && sourceIndex < targetIndex)
                targetIndex--;

            // remove node from old position, insert into new
            sourceCollection.RemoveAt(sourceIndex);
            targetCollection.Insert(targetIndex, source.Header);
        }
    }
}
