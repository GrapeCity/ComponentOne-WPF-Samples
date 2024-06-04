using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Defines the methods required to implement undoable action objects.
    /// </summary>
    public interface IUndoableAction
    {
        /// <summary>
        /// Reverses the effect of the last action.
        /// </summary>
        void Undo();
        /// <summary>
        /// Restores the effect of the last undone action.
        /// </summary>
        void Redo();
        /// <summary>
        /// Saves the state after the action (the state before should be saved in the constructor).
        /// </summary>
        bool SaveNewState();
    }
}
