using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LFeBlog.Core.Interfaces;

namespace LFeBlog.Core.Entities
{
    public class QueryParameters:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private const int DefaultPageSize = 10;
        private const int DefaultMaxPageSize = 100;


        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value >=0 ? value:0;
        }
       
        private int _pageSize = DefaultPageSize;
        public virtual int PageSize
        {
            get => _pageSize;
            set => SetField(ref _pageSize,value);
        }
        

        private string _orderBy;
        public string OrderBy
        {
            get => _orderBy;
            set => _orderBy = value ?? nameof(IEntity.Id);
        }
       
        private int _maxPageSize = DefaultMaxPageSize;

        protected internal  virtual int MaxPageSize
        {
            get => _maxPageSize;
            set => _maxPageSize = value;
        }


        public string Fields { get; set; }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field,T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field,value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            if (propertyName==nameof(PageSize) || propertyName==nameof(MaxPageSize))
            {
                SetPageSize();
            }
            return true;
        }

        private void SetPageSize()
        {
            if (_maxPageSize<=0)
            {
                _maxPageSize = DefaultMaxPageSize;
            }
            if (_pageSize<=0)
            {
                _pageSize = DefaultPageSize;
            }
            _pageSize = _pageSize > _maxPageSize ? _maxPageSize : _pageSize;

        }
    }
}