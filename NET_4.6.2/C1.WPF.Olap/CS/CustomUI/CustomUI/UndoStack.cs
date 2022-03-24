using C1.WPF.Olap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CustomUI
{
    /// <summary>
    /// Undo/Redo stack for the C1OlapPage.
    /// </summary>
    public class OlapPanelUndoStack : UndoStack
    {
        // fields
        C1OlapPanel _panel;
        Button _btnUndo, _btnRedo;
        string _viewDef;

        /// <summary>
        /// Initializes a new instance of an <see cref="OlapPageUndoStack"/>.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="btnUndo"></param>
        /// <param name="btnRedo"></param>
        public OlapPanelUndoStack(C1OlapPanel panel, Button btnUndo, Button btnRedo)
        {
            _panel = panel;
            _btnUndo = btnUndo;
            _btnRedo = btnRedo;

            _btnUndo.Click += _btnUndo_Click;
            _btnRedo.Click += _btnRedo_Click;

            UpdateButtonState();

            _panel.ViewDefinitionChanged += _page_ViewDefinitionChanged;
            _panel.DataSourceChanged += _page_DataSourceChanged;
            _viewDef = _panel.ViewDefinition;
        }

        // handle button events
        void _btnUndo_Click(object sender, EventArgs e)
        {
            if (_btnUndo.Visibility == Visibility.Visible)
            {
                Undo();
                UpdateButtonState();
            }
        }
        void _btnRedo_Click(object sender, EventArgs e)
        {
            if (_btnRedo.Visibility == Visibility.Visible)
            {
                Redo();
                UpdateButtonState();
            }
        }

        // handle page events
        void _page_DataSourceChanged(object sender, EventArgs e)
        {
            Clear();
            UpdateButtonState();
        }
        void _page_ViewDefinitionChanged(object sender, EventArgs e)
        {
            SaveState();
            UpdateButtonState();
            _viewDef = _panel.ViewDefinition;
        }

        // enable/disable buttons
        void UpdateButtonState()
        {
            _btnUndo.IsEnabled = CanUndo;
            _btnRedo.IsEnabled = CanRedo;
        }

        /// <summary>
        /// Gets an object that represents the current state.
        /// </summary>
        public override object GetCurrentState()
        {
            return _viewDef;
        }
        /// <summary>
        /// Applies a saved state.
        /// </summary>
        public override void ApplySavedState(object state)
        {
            try
            {
                _panel.ViewDefinition = (string)state;
            }
            catch { }
        }
        /// <summary>
        /// Overridden to update button state after clearing the undo stack.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            UpdateButtonState();
        }
    }
}
