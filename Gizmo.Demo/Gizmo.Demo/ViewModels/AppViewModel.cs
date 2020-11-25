using Gizmo.WPF;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gizmo.Demo
{
    public enum TestEnum
    {
        GoToStart = 0xe91a,
        Previous = 0xe919,
        Play = 0xe91c,
        Pause = 0xe91d,
        Stop = 0xe91e,
        PlayPause = 0xe91f,
        Next = 0xe918,
        GoToEnd = 0xe91b
    }

    public class SearchValue
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public SearchValue()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
        }
        public static SearchValue GetValue(string name, string description) => new SearchValue() { Name = name, Description = description };
    }

    public class AppViewModel : UIBaseViewModel
    {
        private ObservableCollection<SearchValue> searchList = new ObservableCollection<SearchValue>();
        private ObservableCollection<SearchValue> resultList = new ObservableCollection<SearchValue>();
        private string searchText01 = string.Empty;
        private bool searchEnabled01 = false;

        private string searchText02 = string.Empty;
        private bool searchEnabled02 = false;


        private bool filterItems = false;

        private ObservableCollection<Enum> testEnumValues = new ObservableCollection<Enum>();
        public ObservableCollection<Enum> TestEnumValues
        {
            get => testEnumValues;
            set
            {
                if (testEnumValues == value) return;
                testEnumValues = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SearchValue> SearchList
        {
            get => searchList;
            set
            {
                if (searchList == value) return;
                searchList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SearchValue> ResultList
        {
            get => resultList;
            set
            {
                if (resultList == value) return;
                resultList = value;
                OnPropertyChanged();
            }
        }

        public string SearchText01
        {
            get => searchText01;
            set
            {
                if (searchText01 == value) return;
                searchText01 = value;
                if (SearchEnabled01 && searchText01 != string.Empty)
                {
                    ResultList = new ObservableCollection<SearchValue>(SearchList.Where(x => x.Name.ToLower().Contains(searchText01.ToLower()) || x.Description.ToLower().Contains(searchText01.ToLower())));
                }
                else
                {
                    ResultList.Clear();
                }
                OnPropertyChanged();
            }
        }

        public bool SearchEnabled01
        {
            get => searchEnabled01;
            set
            {
                if (searchEnabled01 == value) return;
                searchEnabled01 = value;
                if (!searchEnabled01)
                {
                    SearchText01 = string.Empty;
                    ResultList.Clear();
                }
                OnPropertyChanged();
            }
        }

        public string SearchText02
        {
            get => searchText02;
            set
            {
                if (searchText02 == value) return;
                searchText02 = value;
                if (SearchEnabled02 && searchText02 != string.Empty)
                {
                    ResultList = new ObservableCollection<SearchValue>(SearchList.Where(x => x.Name.ToLower().Contains(searchText02.ToLower()) || x.Description.ToLower().Contains(searchText02.ToLower())));
                }
                else
                {
                    ResultList.Clear();
                }
                OnPropertyChanged();
            }
        }

        public bool SearchEnabled02
        {
            get => searchEnabled02;
            set
            {
                if (searchEnabled02 == value) return;
                searchEnabled02 = value;
                if (!searchEnabled02)
                {
                    SearchText02 = string.Empty;
                    ResultList.Clear();
                }
                OnPropertyChanged();
            }
        }
        public bool FilterItems
        {
            get => filterItems;
            set
            {
                if (filterItems == value) return;
                filterItems = value;
                OnPropertyChanged();
            }
        }

        public AppViewModel()
        {
            SearchList.Add(SearchValue.GetValue("Red", "Colors"));
            SearchList.Add(SearchValue.GetValue("Blue", "Colors"));
            SearchList.Add(SearchValue.GetValue("White", "Colors"));
            SearchList.Add(SearchValue.GetValue("Orange", "Colors"));
            SearchList.Add(SearchValue.GetValue("Apple", "Fruits"));
            SearchList.Add(SearchValue.GetValue("Banana", "Fruits"));
            SearchList.Add(SearchValue.GetValue("Orange", "Fruits"));
            SearchList.Add(SearchValue.GetValue("Lime", "Fruits"));

            TestEnumValues = new ObservableCollection<Enum>() { TestEnum.GoToEnd, TestEnum.Previous, TestEnum.Stop, TestEnum.Play, TestEnum.Pause, TestEnum.Next, TestEnum.GoToStart };
        }
    }
}
