using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using CADController;

namespace CADView
{
    /// <summary>
    /// Базовый класс для элементов меню.
    /// </summary>
    public abstract class BaseMenuElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _image;
        private string _hintText;
        private int _width;
        private int _height;
        private Brush _color = Brushes.LightSlateGray;
        private string _description;

        public const int DefaultWidth = 40;
        public const int DefaultHeight = 40;

        public BaseMenuElement(string image, string hintText, int width, int height)
        {
            Image = image;
            HintText = hintText;
            Width = width;
            Height = height;

            CreatedUIElements.Add(this);
        }

        public static List<BaseMenuElement> CreatedUIElements { get; } = new List<BaseMenuElement>();

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public string HintText
        {
            get { return _hintText; }
            set
            {
                _hintText = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Media.Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Базовый класс для экспандеров
    /// </summary>
    public abstract class BaseExpanderItem : BaseMenuElement
    {
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public int Index { get; private set; }

        private static int _counter = 0;
        private bool _isExpanded;

        public BaseExpanderItem(string image, string hintText, int width, int height) : base(image, hintText, width, height)
        {
            Index = _counter++;
        }
    }

    /// <summary>
    /// Панель-экспандер с элементами
    /// </summary>
    public class MenuExpanderItem : BaseExpanderItem
    {
        private IEnumerable<BaseMenuElement> _subItems;

        public IEnumerable<BaseMenuElement> SubItems
        {
            get { return _subItems; }
            set
            {
                _subItems = value;
                OnPropertyChanged();
            }
        }

        public MenuExpanderItem(string image, string hintText, IEnumerable<BaseMenuElement> subItems, Brush color, int width = 0, int height = 0) : base(image, hintText, width, height)
        {
            SubItems = subItems;
            Color = color;
        }
    }

    /// <summary>
    /// Вертикальный экспандер с элементами
    /// </summary>
    public class MenuSubItem : BaseExpanderItem
    {
        private IEnumerable<BaseMenuElement> _subItems;

        public MenuSubItem(string image, string hintText, IEnumerable<BaseMenuElement> subItems,
            int width = DefaultWidth + 14, int height = DefaultHeight) : base(image, hintText, width, height)
        {
            SubItems = subItems;
            Image = image;
            HintText = hintText;
        }

        public IEnumerable<BaseMenuElement> SubItems
        {
            get { return _subItems; }
            set
            {
                _subItems = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Кнопка в меню
    /// </summary>
    public class MenuButtonItem : BaseMenuElement
    {
        private ButtonsCommands _operationType;

        public MenuButtonItem(string image, string hintText, ButtonsCommands operationType, int width = DefaultWidth, int height = DefaultHeight) : base(image, hintText, width, height)
        {
            OperationType = operationType;
            Color = Brushes.Black;
        }

        public ButtonsCommands OperationType
        {
            get { return _operationType; }
            set
            {
                _operationType = value;
                OnPropertyChanged();
            }
        }
    }
}
