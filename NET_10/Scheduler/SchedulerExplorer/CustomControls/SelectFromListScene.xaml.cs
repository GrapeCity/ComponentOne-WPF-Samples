using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using C1.Schedule;
using C1.WPF;
using C1.WPF.Core;
using C1.WPF.Docking;

namespace SchedulerExplorer
{
    /// <summary>
    /// Interaction logic for SelectFromListScene.xaml
    /// </summary>
	public partial class SelectFromListScene : UserControl
	{
		#region ** fields
		private IList _sourceCollection;
		private IList _sourceCollectionEdited;
		private IList _targetCollection;
		private IList _targetCollectionEdited;
		private Type _itemType;
        private bool _allowEdit = true;
        private bool _allowMultipleSelection = true;
        private bool _cleaningExtraItems = false;
        private CollectionView _sortedSourceView;
		private bool _isLoaded = false;
		private ContentControl _parentWindow = null;
		#endregion

		#region ** initializing
		/// <summary>
		/// Initializes the new instance of the <see cref="SelectFromListScene"/> control.
		/// </summary>
        public SelectFromListScene()
        {
            InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;
            if (!System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted)
            {
                _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(Window));
            }
            else
            _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(C1Window));
            if (_parentWindow != null)
			{
                sourceListBox.ItemsSource = SortedSourceView;
                sourceListBox.SelectedIndex = -1;
                sourceListBox.Focus();
                if (ItemType != typeof(Category))
                {
                    resetButton.Visibility = Visibility.Collapsed;
                }
                UpdateSelectedValues();
                var allowEdit = TryFindResource("AllowEdit");
                if (allowEdit != null)
                {
                    AllowEdit = (bool)allowEdit;
                }
                var allowMultipleSelection = TryFindResource("AllowMultipleSelection");
                if (allowMultipleSelection != null)
                {
                    AllowMultipleSelection = (bool)allowMultipleSelection;
                }
            }
		}

		private void SourceItemSelectedCB_Loaded(object sender, RoutedEventArgs e)
		{
			CheckBox chk = sender as CheckBox;
			if (chk == null)
			{
				return;
			}
            if (sourceListBox.SelectedIndex < 0)
            {
                sourceListBox.SelectedIndex = 0;
                chk.Focus();
            }
			UpdateCheckBoxes();
		}
		#endregion

		#region ** public interface
        /// <summary>
        /// Gets or sets boolean value determining whether dialog should allow adding or deleting items.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool AllowEdit
        {
            get { return _allowEdit; }
            set
            {
                _allowEdit = value;
                if (!_allowEdit && _isLoaded)
                {
                    resetButton.Visibility = 
                        addButton.Visibility = 
                        newItemText.Visibility =
                        deleteButton.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Gets or sets boolean value determining whether multiple selection is allowed.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool AllowMultipleSelection
        {
            get { return _allowMultipleSelection; }
            set
            {
                _allowMultipleSelection = value;
                if (!_allowMultipleSelection && _isLoaded)
                {
                    sourceListBox.SelectionMode = SelectionMode.Single;
                }
            }
        }

		/// <summary>
		/// Gets the reference to the source collection.
		/// </summary>
		public IList SourceCollection
		{
			get
			{
				_sourceCollection = _sourceCollection ?? TryFindResource("SourceCollection") as IList;
				return _sourceCollection;
			}
		}

		/// <summary>
		/// Gets the reference to the target collection.
		/// </summary>
		public IList TargetCollection
		{
			get
			{
				_targetCollection = _targetCollection ?? TryFindResource("TargetCollection") as IList;
				return _targetCollection;
			}
		}

		/// <summary>
		/// Gets the reference to the edited copy of the target collection.
		/// </summary>
		public IList TargetCollectionEdited
		{
			get
			{
                if (_targetCollectionEdited == null)
                {
                    _targetCollectionEdited = new BaseObjectList();
                    foreach (object o in TargetCollection)
                    {
                        _targetCollectionEdited.Add(o);
                    }
                }
                return _targetCollectionEdited;
            }
		}

		/// <summary>
		/// Gets the reference to the edited copy of the source collection.
		/// </summary>
		public IList SourceCollectionEdited
		{
			get
			{
				if (_sourceCollectionEdited == null )
				{
					if (SourceCollection is ResourceCollection)
					{
						_sourceCollectionEdited = new ResourceCollection();
					}
					else if (SourceCollection is CategoryCollection)
					{
						_sourceCollectionEdited = new CategoryCollection();
						_sourceCollectionEdited.Clear();
					}
					else if (SourceCollection is ContactCollection)
					{
						_sourceCollectionEdited = new ContactCollection();
					}
					else
					{
						_sourceCollectionEdited = new System.Collections.ObjectModel.ObservableCollection<object>();
					}
					foreach (object o in SourceCollection)
					{
						_sourceCollectionEdited.Add(o);
					}
				}
				return _sourceCollectionEdited;
			}
		}

		/// <summary>
		/// Gets the type of items in collections.
		/// </summary>
		public Type ItemType
		{
			get
			{
				_itemType = _itemType ?? TryFindResource("ItemType") as Type;
				return _itemType;
			}
		}

		/// <summary>
		/// Gets the sorted view of SourceCollectionEdited to display in the list of available objects.
		/// </summary>
        public CollectionView SortedSourceView
		{
			get
			{
				if (_sortedSourceView == null)
				{
                    _sortedSourceView = new ListCollectionView(SourceCollectionEdited);
					_sortedSourceView.SortDescriptions.Add(new SortDescription("Text", ListSortDirection.Ascending));
				}
				return _sortedSourceView;
			}
		}
		#endregion

		#region ** private stuff
		private void UpdateSelectedValues()
		{
			selectedValues.Text = TargetCollectionEdited.ToString();
		}

		private object TryFindResource(string key)
		{
			if (_parentWindow != null)
			{
				if (_parentWindow.Resources.Contains(key))
				{
					return _parentWindow.Resources[key];
				}
			}
			return null;
		}

        private void SourceItemSelectedCB_GotFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement uiItem = e.OriginalSource as FrameworkElement;
            if (uiItem == null)
                return;
            object item = uiItem.DataContext;
            // the second check is required for WPF 4
            if (item == null || !item.GetType().IsSubclassOf(typeof(BasePersistableObject)))
                return;
            sourceListBox.SelectedItem = item;
        }

        //[Obfuscation(Exclude = true)]
        private void SourceItemSelectedCB_Checked(Object sender, RoutedEventArgs e)
        {
            AddOrRemoveItem(true, e);
        }

        //[Obfuscation(Exclude = true)]
        private void SourceItemSelectedCB_Unchecked(Object sender, RoutedEventArgs e)
        {
            AddOrRemoveItem(false, e);
        }


		private void AddOrRemoveItem(bool add, RoutedEventArgs e)
		{
			if (!_isLoaded || _cleaningExtraItems || TargetCollection == null)
				return;
			FrameworkElement uiItem = e.OriginalSource as FrameworkElement;
			if (uiItem == null)
				return;
			AddOrRemoveItem(add, uiItem.DataContext);
		}

		private void AddOrRemoveItem(bool add, object item)
		{
			if (!_isLoaded || _cleaningExtraItems || TargetCollection == null)
				return;
			if (item == null)
				return;
			if (add)
			{
				AddItemToTargetCollection(item);
			}
			else
			{
				TargetCollectionEdited.Remove(item);
			}
			UpdateCheckBoxes();
			UpdateSelectedValues();
		}

		private void UpdateCheckBoxes()
		{
			if (!_isLoaded || _cleaningExtraItems || TargetCollection == null)
				return;
			try
			{
				_cleaningExtraItems = true;

				foreach (object obj in this.sourceListBox.Items)
				{
					ListBoxItem container = this.sourceListBox.ItemContainerGenerator.ContainerFromItem(obj) as ListBoxItem;
					if (container != null)
					{
						IList<DependencyObject> list = new List<DependencyObject>();
						VTreeHelper.GetChildrenOfType(container, typeof(CheckBox), ref list);
						if (list.Count > 0)
						{
							((CheckBox)list[0]).IsChecked = TargetCollectionEdited.Contains(obj);
						}
					}
				}
			}
			finally
			{
				_cleaningExtraItems = false;
			}
		}

		private static object FindItemWithText(IList list, string text)
		{
			if (list == null || text == null || text.Trim().Length == 0)
				return null;
			PropertyInfo textProp = null;
			foreach (object curItem in list)
			{
				if (curItem != null)
				{
					textProp = textProp ?? curItem.GetType().GetProperty("Text");
					if (textProp == null)
						return null;
					string curText = textProp.GetValue(curItem, null) as string;
					if (string.Compare(text.ToLower(), curText.ToLower()) == 0)
						return curItem;
				}
			}

			return null;
		}

		private void addButton_Click(object sender, RoutedEventArgs e)
		{
			TextBox tb = newItemText;
			string str = tb.Text;
			if (!string.IsNullOrEmpty(str))
			{
				object obj = FindItemWithText(SourceCollectionEdited, str);
				if (obj == null)
				{
					if (ItemType == typeof(Category))
					{
						Category cat = new Category(str);
						cat.MenuCaption = str;
						obj = cat;
					}
					else if (ItemType == typeof(Contact))
					{
						Contact cnt = new Contact();
						cnt.Text = str;
						cnt.MenuCaption = str;
						obj = cnt;
					}
					else if (ItemType == typeof(Resource))
					{
						Resource res = new Resource();
						res.Text = str;
						res.MenuCaption = str;
						obj = res;
					}
					else if (ItemType == typeof(C1.Schedule.Label))
					{
						obj = new C1.Schedule.Label(str, str);
					}
					else if (ItemType == typeof(Status))
					{
						obj = new Status(str, str);
					}
				}
				if (obj != null)
				{
					SourceCollectionEdited.Add(obj);
					AddOrRemoveItem(true, obj);
				}
				tb.ClearValue(TextBox.TextProperty);
			}
		}

		private void AddItemToTargetCollection(object item)
		{
			if (item != null)
			{
				if (!AllowMultipleSelection)
				{
					TargetCollectionEdited.Clear();
				}
				if (!TargetCollectionEdited.Contains(item))
				{
					TargetCollectionEdited.Add(item);
				}
			}
		}

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			object o = sourceListBox.SelectedItem;
			if (o != null)
			{
				SourceCollectionEdited.Remove(o);
				TargetCollectionEdited.Remove(o);
			}
			UpdateCheckBoxes();
			UpdateSelectedValues();
		}

		private void resetButton_Click(object sender, RoutedEventArgs e)
		{
			CategoryCollection coll = SourceCollectionEdited as CategoryCollection;
			if (coll != null)
			{
				coll.LoadDefaults();
				for (int i = TargetCollectionEdited.Count - 1; i >= 0; i--)
				{
					Category category = TargetCollectionEdited[i] as Category;
					if (!coll.Contains(category))
					{
						TargetCollectionEdited.RemoveAt(i);
					}
				}
			}
			UpdateCheckBoxes();
			UpdateSelectedValues();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			for (int i = SourceCollection.Count - 1; i >= 0; i--)
			{
				object o = SourceCollection[i];
				if (!SourceCollectionEdited.Contains(o))
				{
					SourceCollection.RemoveAt(i);
				}
			}
			foreach (object o in SourceCollectionEdited)
			{
				if (!SourceCollection.Contains(o))
				{
					SourceCollection.Add(o);
				}
			}
			for (int i = TargetCollection.Count - 1; i >= 0; i--)
			{
				object o = TargetCollection[i];
				if (!TargetCollectionEdited.Contains(o))
				{
					TargetCollection.RemoveAt(i);
				}
			}
			foreach (object o in TargetCollectionEdited)
			{
				if (!TargetCollection.Contains(o))
				{
					TargetCollection.Add(o);
				}
			}
			cancelButton_Click(sender, e);
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
            if (_parentWindow == null)
            {
                return;
            }
            if (_parentWindow is Window)
            {
                ((Window)_parentWindow).Close();
            }
            else
            if (_parentWindow is C1Window)
			{
                ((C1Window)_parentWindow).Close();
			}
		}
        private void accessKeyPressed(object sender, System.Windows.Input.AccessKeyPressedEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Alt)
            {
                e.Scope = sender;
                e.Handled = true;
            }
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DependencyObject parentControl = VTreeHelper.GetParentOfType((DependencyObject)e.OriginalSource, GetType());
                if (parentControl != null)
                {
                    cancelButton_Click(this, null);
                }
            }
            base.OnPreviewKeyDown(e);
        }
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            base.OnMouseWheel(e);
        }
        #endregion
	}


    public class BaseObjectList : ObservableCollection<BaseObject>
    {
        public BaseObjectList()
            : base()
        {
        }

        /// <summary>
        /// Overrides the default behavior.
        /// Returned text will be displayed in selectedValues TextBox.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (BaseObject item in Items)
            {
                sb.Append(item.ToString() + "; ");
            }
            return sb.ToString();
        }

    }

}