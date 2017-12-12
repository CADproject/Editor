using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CADView
{
    public static class MouseDownHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
            typeof(bool), typeof(MouseDownHelper),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnNotifyPropertyChanged)));

        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(UIElement element)
        {
            return (bool) element.GetValue(IsEnabledProperty);
        }

        private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element != null && e.NewValue != null)
            {
                if ((bool) e.NewValue)
                {
                    Register(element);
                }
                else
                {
                    UnRegister(element);
                }
            }
        }

        private static void Register(UIElement element)
        {
            element.PreviewMouseDown += element_MouseDown;
            element.PreviewMouseLeftButtonDown += element_MouseLeftButtonDown;
            element.MouseLeave += element_MouseLeave;
            element.PreviewMouseUp += element_MouseUp;
        }

        private static void UnRegister(UIElement element)
        {
            element.PreviewMouseDown -= element_MouseDown;
            element.PreviewMouseLeftButtonDown -= element_MouseLeftButtonDown;
            element.MouseLeave -= element_MouseLeave;
            element.PreviewMouseUp -= element_MouseUp;
        }

        private static void element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var element = e.Source as UIElement;
            if (element != null)
            {
                SetIsMouseDown(element, true);
            }
        }

        private static void element_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var element = e.Source as UIElement;
            if (element != null)
            {
                SetIsMouseLeftButtonDown(element, true);
            }
        }

        private static void element_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var element = e.Source as UIElement;
            if (element != null)
            {
                SetIsMouseDown(element, false);
                SetIsMouseLeftButtonDown(element, false);
            }
        }

        private static void element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var element = e.Source as UIElement;
            if (element != null)
            {
                SetIsMouseDown(element, false);
                SetIsMouseLeftButtonDown(element, false);
            }
        }

        internal static readonly DependencyPropertyKey IsMouseDownPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsMouseDown",
                typeof(bool), typeof(MouseDownHelper), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsMouseDownProperty = IsMouseDownPropertyKey.DependencyProperty;

        internal static void SetIsMouseDown(UIElement element, bool value)
        {
            element.SetValue(IsMouseDownPropertyKey, value);
        }

        public static bool GetIsMouseDown(UIElement element)
        {
            return (bool) element.GetValue(IsMouseDownProperty);
        }

        internal static readonly DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsMouseLeftButtonDown",
                typeof(bool), typeof(MouseDownHelper), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsMouseLeftButtonDownProperty =
            IsMouseLeftButtonDownPropertyKey.DependencyProperty;

        internal static void SetIsMouseLeftButtonDown(UIElement element, bool value)
        {
            element.SetValue(IsMouseLeftButtonDownPropertyKey, value);
        }

        public static bool GetIsMouseLeftButtonDown(UIElement element)
        {
            return (bool)element.GetValue(IsMouseLeftButtonDownProperty);
        }
    }

    public interface IButtonOperation
    {
        object Parameter { get; set; }
    }

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
        private Brush _color = Brushes.Gray;
        private string _description;

        public const int DefaultWidth = 30;
        public const int DefaultHeight = 30;

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

        public BaseExpanderItem(string image, string hintText, IEnumerable<BaseMenuElement> subItems, int width, int height) : base(image, hintText, width, height)
        {
            Index = _counter++;
            SubItems = subItems;
        }
    }

    /// <summary>
    /// Панель-экспандер с элементами
    /// </summary>
    public class MenuExpanderItem : BaseExpanderItem
    {
        private Visibility _separatorVisibility = Visibility.Visible;
        public Visibility SeparatorVisibility
        {
            get { return _separatorVisibility; }
            set
            {
                _separatorVisibility = value;
                OnPropertyChanged();
            }
        }

        public MenuExpanderItem(string image, string hintText, IEnumerable<BaseMenuElement> subItems, Brush color, Visibility separator = Visibility.Visible, int width = 0, int height = 0) : base(image, hintText, subItems, width, height)
        {
            Color = color;
            SeparatorVisibility = separator;
        }
    }

    /// <summary>
    /// Вертикальный экспандер с элементами
    /// </summary>
    public class MenuSubItem : BaseExpanderItem, IButtonOperation
    {
        private object _parameter;

        public MenuSubItem(string image, string hintText, UniversalCommands operationType, IEnumerable<BaseMenuElement> subItems,
            int width = DefaultWidth, int height = DefaultHeight) : this(image, hintText, (object)operationType, subItems, width, height)
        {

        }

        public MenuSubItem(string image, string hintText, object parameter, IEnumerable<BaseMenuElement> subItems, 
            int width = DefaultWidth, int height = DefaultHeight) : base(image, hintText, subItems, width, height)
        {
            Image = image;
            HintText = hintText;
            Parameter = parameter;
        }

        public object Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Кнопка в меню
    /// </summary>
    public class MenuButtonItem : BaseMenuElement, IButtonOperation
    {
        private object _parameter;

        public MenuButtonItem(string image, string hintText, object parameter, int width = DefaultWidth, int height = DefaultHeight) : base(image, hintText, width, height)
        {
            _parameter = parameter;
            Color = Brushes.Black;
        }

        public MenuButtonItem(string image, string hintText, UniversalCommands operationType, int width = DefaultWidth,
            int height = DefaultHeight) : this(image, hintText, (object) operationType, width, height)
        {
        }

        public object Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged();
            }
        }
    }

    public class MenuTextButtonItem : BaseMenuElement
    {
        private object _parameter;
        private bool _isSelected;

        public MenuTextButtonItem(string hintText, object parameter, int width = DefaultWidth, int height = DefaultHeight) : 
            base(null, hintText, width, height)
        {
            _parameter = parameter;
            Color = Brushes.Black;
            Description = hintText;
        }

        public MenuTextButtonItem(string hintText, UniversalCommands operationType, int width = DefaultWidth,
            int height = DefaultHeight) : this(hintText, (object)operationType, width, height)
        {
        }

        public object Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
