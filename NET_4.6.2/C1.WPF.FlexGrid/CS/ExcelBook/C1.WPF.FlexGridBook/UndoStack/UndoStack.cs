using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Generic Undo/Redo stack.
    /// </summary>
    public class UndoStack
	{
        //-------------------------------------------------------------------------
        #region ** fields

        List<IUndoableAction> _stack = new List<IUndoableAction>();
        int _ptr = -1;

        private const int MAX_STACK_SIZE = 500;

        #endregion

        //-------------------------------------------------------------------------
        #region ** ctor
        
        /// <summary>
        /// Initializes a new instance of an <see cref="UndoStack"/>.
        /// </summary>
        public UndoStack() { }

        #endregion

        //-------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Clears the undo/redo stack.
        /// </summary>
		public virtual void Clear()
		{
			_stack.Clear();
			_ptr = -1;
            OnStateChanged(EventArgs.Empty);
        }
        /// <summary>
        /// Gets a value that indicates whether the stack contains actions that can be undone.
        /// </summary>
		public bool CanUndo
		{
			get { return _ptr > -1 && _ptr < _stack.Count; }
		}
        /// <summary>
        /// Gets a value that indicates whether the stack contains actions that can be re-done.
        /// </summary>
        public bool CanRedo
        {
            get { return _ptr + 1 > -1 && _ptr + 1 < _stack.Count; }
        }
        /// <summary>
        /// Executes an Undo command.
        /// </summary>
        public void Undo()
		{
            if (CanUndo)
            {
                _stack[_ptr].Undo();
                _ptr--;
                OnStateChanged(EventArgs.Empty);
            }
		}
        /// <summary>
        /// Executes a Redo command.
        /// </summary>
        public void Redo()
		{
            if (CanRedo)
            {
                _ptr++;
                _stack[_ptr].Redo();
                OnStateChanged(EventArgs.Empty);
            }
        }
        /// <summary>
        /// Adds an action to the undo/redo stack.
        /// </summary>
        public void AddAction(IUndoableAction action)
        {
            // trim stack
            while (_stack.Count > 0 && _stack.Count > _ptr + 1)
            {
                _stack.RemoveAt(_stack.Count - 1);
            }
            while (_stack.Count >= MAX_STACK_SIZE)
            {
                _stack.RemoveAt(0);
            }

            // update pointer and add action to stack
            _ptr = _stack.Count;
            _stack.Add(action);

            // done
            OnStateChanged(EventArgs.Empty);
        }
        /// <summary>
        /// Occurs when the status of the undo stack changes.
        /// </summary>
        public event EventHandler StateChanged;
        /// <summary>
        /// Raises the <see cref="StateChanged"/> event.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that contains the event parameters.</param>
        protected virtual void OnStateChanged(EventArgs e)
        {
            if (StateChanged != null)
                StateChanged(this, e);
        }

        #endregion
    }
}
