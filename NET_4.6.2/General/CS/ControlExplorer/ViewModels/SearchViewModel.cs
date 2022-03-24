using System;
using System.Linq;
using System.Collections.Generic;
using ControlExplorer.Common;

namespace ControlExplorer
{
    public class SearchViewModel : BindableBase
    {
        private static SearchViewModel _instance = null;
        private bool _showSearchList = false;
        private string _searchCriteria = "";

        private SearchViewModel()
        {

        }

        public static SearchViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SearchViewModel();
                return _instance;
            }
        }

        public string SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
                _showSearchList = SearchList.Count > 0;
                OnPropertyChanged("SearchCriteria");
                OnPropertyChanged("SearchList");
                OnPropertyChanged("ShowSearchList");
            }
        }

        public bool ShowSearchList
        {
            get
            {
                return _showSearchList;
            }
            set
            {
                SetProperty(ref _showSearchList, value, "ShowSearchList");
            }
        }

        public IList<SearchResult> SearchList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchCriteria))
                    return new List<SearchResult>();

                var searchKeys = _searchCriteria.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return (from c in MainViewModel.Instance.Controls
                        where searchKeys.Any(key => c.Contains(key))
                        select new SearchResult
                         {
                             Control = c,
                         }).ToList();
            }
        }
    }

    public class SearchResult
    {
        public ControlDescription Control { get; set; }
    }
}
