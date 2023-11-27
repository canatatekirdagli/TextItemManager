using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TextItemManager.Data;
using TextItemManager.Models;

namespace TextItemManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<TextItem> _textItems;
        private TextItem _selectedTextItem;
        private DataContext _context;
        private string _newTextItemContent;
        private string _originalTextForEdit;
        private bool _isEditing;
        private bool _isSaveEnabled;

        public MainViewModel()
        {
            _context = new DataContext();
            LoadTextItems();
        }

        public ObservableCollection<TextItem> TextItems
        {
            get => _textItems;
            set
            {
                _textItems = value;
                OnPropertyChanged(nameof(TextItems));
            }
        }

        public TextItem SelectedTextItem
        {
            get => _selectedTextItem;
            set
            {
                _selectedTextItem = value;
                OnPropertyChanged(nameof(SelectedTextItem));
                OriginalTextForEdit = _selectedTextItem?.Content;
                TextBoxTextChangedCommand.Execute(null);
            }
        }

        public string NewTextItemContent
        {
            get => _newTextItemContent;
            set
            {
                _newTextItemContent = value;
                OnPropertyChanged(nameof(NewTextItemContent));
            }
        }

        public string OriginalTextForEdit
        {
            get => _originalTextForEdit;
            set
            {
                _originalTextForEdit = value;
                OnPropertyChanged(nameof(OriginalTextForEdit));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsSaveEnabled
        {
            get => _isSaveEnabled;
            set
            {
                _isSaveEnabled = value;
                OnPropertyChanged(nameof(IsSaveEnabled));
            }
        }

        public ICommand AddCommand => new RelayCommand(AddTextItem, CanAddTextItem);
        public ICommand EditCommand => new RelayCommand(StartEditing, CanEditOrRemove);
        public ICommand RemoveCommand => new RelayCommand(RemoveTextItem, CanEditOrRemove);
        public ICommand SaveEditingCommand => new RelayCommand(SaveEditing);
        public ICommand CancelEditingCommand => new RelayCommand(CancelEditing);
        public ICommand TextBoxTextChangedCommand => new RelayCommand(TextBoxTextChanged);

        private void LoadTextItems()
        {
            TextItems = new ObservableCollection<TextItem>(_context.TextItems.ToList());
        }

        private void AddTextItem()
        {
            var newItem = new TextItem { Content = NewTextItemContent };
            _context.TextItems.Add(newItem);
            _context.SaveChanges();
            LoadTextItems();
            NewTextItemContent = string.Empty;
        }


        private void StartEditing()
        {
            IsEditing = true;
        }

        private void SaveEditing()
        {
            if (SelectedTextItem != null)
            {
                string trimmedContent = SelectedTextItem.Content?.Trim();

                if (OriginalTextForEdit != SelectedTextItem.Content)
                {
                    _context.Entry(SelectedTextItem).State = EntityState.Modified;
                    _context.SaveChanges();
                    LoadTextItems();
                }
            }

            OriginalTextForEdit = SelectedTextItem?.Content;
            IsEditing = false;
        }


        private void CancelEditing()
        {
            if (SelectedTextItem != null)
            {
                if (SelectedTextItem.Content != _originalTextForEdit)
                {
                    SelectedTextItem.Content = _originalTextForEdit;
                    OnPropertyChanged(nameof(SelectedTextItem));
                    LoadTextItems();
                }

                IsEditing = false;
            }
        }

        private void RemoveTextItem()
        {
            if (SelectedTextItem != null)
            {
                _context.TextItems.Remove(SelectedTextItem);
                _context.SaveChanges();
                LoadTextItems();
            }
        }

        private bool CanEditOrRemove()
        {
            return SelectedTextItem != null;
        }

        private bool CanAddTextItem()
        {
            return !string.IsNullOrWhiteSpace(NewTextItemContent);
        }

        private void TextBoxTextChanged()
        {
            IsSaveEnabled = !string.IsNullOrWhiteSpace(SelectedTextItem?.Content);
            OnPropertyChanged(nameof(IsSaveEnabled));
        }
    }
}
