using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using C1.WPF;

namespace BasicControls
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
            Effect = DragDropEffect.Move;
            tree.DragDrop += HandleDragDrop;
            tree.DragStart += HandleDragStart;
        }

        /// <summary>
        /// The C1TreeView that will use this custom Drag&Drop implementation.
        /// </summary>
        private C1TreeView Tree { get; set; }

        /// <summary>
        /// The desired effect for the drop action (move or copy elements)
        /// </summary>
        public DragDropEffect Effect { get; set; }

        /// <summary>
        /// Handles DragStart event;
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="e">The event arguments</param>
        private void HandleDragStart(object source, DragDropEventArgs e)
        {
            if (e.DragSource is C1TreeViewItem && ((C1TreeViewItem)e.DragSource).DataContext is Department)
            {
                // don't allow to drag department nodes
                e.Effect = DragDropEffect.None;
            }
            else
            {
                e.Effect = Effect;
            }
        }

        /// <summary>
        /// Handles the DragDrop event.
        /// Determines the target and fires the corresponding action (move or copy).
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="e">The event arguments</param>
        private void HandleDragDrop(object source, DragDropEventArgs e)
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
                        bool insertAfter = e.GetPosition(target).Y > 4;
                        if (target.HasItems)
                        {
                            // if target is department, insert new item inside
                            target = target.FirstNode;
                            insertAfter = false;
                        }
                        if (Effect == DragDropEffect.Move)
                        {
                            DoMove(item, target, insertAfter);
                        }
                        else
                        {
                            DoCopy(item, target, insertAfter);
                        }

                        Tree.ItemsSource = Tree.ItemsSource;
                    }
                }
            }
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

            var sourceTreeViewItem = source as C1TreeViewItem;
            var targetTreeViewItem = target as C1TreeViewItem;

            var empSource = sourceTreeViewItem.DataContext as Employee;
            var departmentId = 0;
            if (targetTreeViewItem.DataContext is Employee)
            {
                departmentId = ((Employee)targetTreeViewItem.DataContext).DepartmentID;
            }
            else
            {
                departmentId = ((Department)targetTreeViewItem.DataContext).DepartmentID;
            }
            if (departmentId == 0 || empSource.DepartmentID == departmentId) return;
            foreach (var dep in Tree.ItemsSource)
            {
                var department = dep as Department;
                if (department.DepartmentID != departmentId) continue;

                var empToAdd = empSource.Clone();
                empToAdd.DepartmentID = departmentId;
                if (!department.Employees.Any(x => x.EmployeeID == empToAdd.EmployeeID))
                {
                    if (!department.Employees.Any())
                    {
                        department.Employees.Add(empToAdd);
                    }
                    else
                    {
                        department.Employees.Insert(targetIndex, empToAdd);
                    }
                }
                break;
            }
        }

        /// <summary>
        /// Moves a node to the same collection the target node belongs to.
        /// </summary>
        /// <param name="source">The source node to move</param>
        /// <param name="target">The target node</param>
        /// <param name="moveAfter">Indicates if the source node will be moved after (larger index) the target </param>
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


            var sourceTreeViewItem = source as C1TreeViewItem;
            var targetTreeViewItem = target as C1TreeViewItem;

            var empSource = sourceTreeViewItem.DataContext as Employee;
            var departmentId = 0;
            if (targetTreeViewItem.DataContext is Employee)
            {
                departmentId = ((Employee)targetTreeViewItem.DataContext).DepartmentID;
            }
            else
            {
                departmentId = ((Department)targetTreeViewItem.DataContext).DepartmentID;
            }
            if (departmentId == 0 || empSource.DepartmentID == departmentId) return;
            foreach (var dep in Tree.ItemsSource)
            {
                var department = dep as Department;
                if (department.DepartmentID != departmentId) continue;

                if (!department.Employees.Any(x => x.EmployeeID == empSource.EmployeeID))
                {
                    if (!department.Employees.Any())
                    {
                        department.Employees.Add(empSource);
                    }
                    else
                    {
                        department.Employees.Insert(targetIndex, empSource);
                    }
                }
                break;
            }
            // remove node from old position
            if (sourceIndex >= 0 && empSource.DepartmentID != departmentId)
            {
                empSource.DepartmentID = departmentId;
                sourceCollection.RemoveAt(sourceIndex);
            }
        }
    }
}
