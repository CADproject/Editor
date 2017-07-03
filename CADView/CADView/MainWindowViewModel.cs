using System;
using System.Collections.Generic;
using System.ComponentModel;
using CADController;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using CADView.Dialogs;
using Application = System.Windows.Application;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;
using System.Windows.Media;

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
        private ApplicationController.Operations _operationType;

        public MenuButtonItem(string image, string hintText, ApplicationController.Operations operationType, int width = DefaultWidth, int height = DefaultHeight) : base(image, hintText, width, height)
        {
            OperationType = operationType;
            Color = Brushes.Black;
        }

        public ApplicationController.Operations OperationType
        {
            get { return _operationType; }
            set
            {
                _operationType = value;
                OnPropertyChanged();
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        public MainWindowViewModel()
        {
            Controller = new ApplicationController();
            RenderPanel.Loaded += RenderPanelOnLoad;
            RenderPanel.Resized += RenderPanelOnResize;
            RenderPanel.Rendered += RenderPanelOnRender;
            RenderPanel.MouseFired += RenderPanelOnMouseFire;
        }

        ~MainWindowViewModel()
        {
            return;
            var ids = DocumentViewModels.Select(d => d.Key).ToList();
            foreach (var id in ids)
            {
                DocumentViewModels[id].Dispose();
                Controller.finalDocument(Session, id);
            }
            Controller.finalSession(Session);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate Task ProcessDataDelegate(object[] o);

        internal ApplicationController Controller { get; private set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public void Init()
        {
            return;
            Session = Controller.initSession();
            _inited = true;

            _timer = new DispatcherTimer(DispatcherPriority.Normal, Application.Current.Dispatcher);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _timer.Tick += delegate
            {
                if (DocumentViewModels.Count == 0) return;
                Controller.draw(Session, ActiveDocument.DocumentID);
            };
            _timer.Start();
        }

        public void CreateDocument()
        {
            var model = new DocumentModel(new LayerModel(true));
            var host = new WindowsFormsHost()
            {
                Child = new RenderPanel(model),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            TabItem tab = new TabItem()
            {
                Content = host,
                DataContext = model,
            };
            DocumentViewModelsTabs.Add(tab);
            _tabsDocuments[tab] = model;
            _selectedDocumentIndex = DocumentViewModelsTabs.Count - 1;
            OnPropertyChanged("SelectedDocumentIndex");
        }

        public int SelectedDocumentIndex
        {
            get { return _selectedDocumentIndex; }
            set
            {
                _selectedDocumentIndex = value;
                OnPropertyChanged();

                if (SelectedDocumentIndex < 0 || SelectedDocumentIndex > DocumentViewModelsTabs.Count) return;

                var size = ((WindowsFormsHost) DocumentViewModelsTabs[SelectedDocumentIndex].Content)
                    .Child.Size;
                Controller.activateDocement(Session, ActiveDocument.DocumentID, size.Width,
                    size.Height);
            }
        }

        public ObservableCollection<TabItem> DocumentViewModelsTabs
        {
            get { return _documentViewModelsTabs; }
            set
            {
                _documentViewModelsTabs = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<uint, Document> DocumentViewModels
        {
            get { return Controller.Documents; }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                OnPropertyChanged();
            }
        }

        public Document ActiveDocument
        {
            get
            {
                if (SelectedDocumentIndex < 0) return null;
                return _tabsDocuments[DocumentViewModelsTabs[SelectedDocumentIndex]];
            }
            set
            {
                SelectedDocumentIndex =
                    DocumentViewModelsTabs.IndexOf(_tabsDocuments.FirstOrDefault(i => i.Value == value).Key);
            }
        }

        #region UI Buttons

        public ObservableCollection<BaseMenuElement> UIMenuElements { get; } =
            new ObservableCollection<BaseMenuElement>( new BaseMenuElement[]
                {
                    new MenuExpanderItem("Icons/Home.png", "Документ", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Новый_документ.png", "Новый документ", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Открыть_документ.png", "Открыть документ", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Сохранить.png", "Сохранить", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_вперед.png", "Шаг вперёд", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_назад.png", "Шаг назад", ApplicationController.Operations.OpCircleCreate),

                    }, Brushes.Gray),
                    new MenuExpanderItem("Icons/Paint Brush.png", "Рисование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Карандаш.png", "Карандаш", ApplicationController.Operations.OpCircleCreate),
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия",
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия", ApplicationController.Operations.OpCircleCreate),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_2.png", "Линия под прямым углом", ApplicationController.Operations.OpCircleCreate),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_3.png", "Параллельная линия", ApplicationController.Operations.OpCircleCreate),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_4.png", "Линия под углом", ApplicationController.Operations.OpCircleCreate),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга", new []
                        {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга по двум точкам и центру", ApplicationController.Operations.OpCircleCreate),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_2.png", "Дуга по трём точкам", ApplicationController.Operations.OpCircleCreate),
                        }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность", new []
                        {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность по центру и точке", ApplicationController.Operations.OpCircleCreate),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Окружность_2.png", "Окружность по трём точкам", ApplicationController.Operations.OpCircleCreate),
                        }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Ломаная.png", "Ломаная", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Сплайн.png", "Сплайн", ApplicationController.Operations.OpCircleCreate),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Oscilloscope.png", "Отображение", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Подвинуть_вид.png", "Подвинуть изображение", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Приблизить_вид.png", "Увеличить изображение", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Отдалить_вид.png", "Уменьшить изображение", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Показать_все.png", "Показать всё", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Узлы.png", "Показать узлы", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Вспомогательная_сетка.png", "Показать сетку", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Темы.png", "Сменить тему", ApplicationController.Operations.OpSetTheme),

                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Question 4.png", "Справка", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель СПРАВКА/Справка.png", "Справка", ApplicationController.Operations.OpCircleCreate, 61),
                        new MenuButtonItem("", "О программе", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("", "Документация", ApplicationController.Operations.OpCircleCreate),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Edit.png", "Редактирование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Ластик.png", "Ластик", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Триммирование.png", "Триммирование", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Продление.png", "Продление", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Сшить_ломаную.png", "Создать ломаную", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Разрезать_ломаную.png", "Разрушить ломаную", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Корректировать_узел.png", "Корректировка", ApplicationController.Operations.OpCircleCreate),
                        new MenuSubItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Узлы", new[]
                        {
                            new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Добавить узел", ApplicationController.Operations.OpCircleCreate),
                            new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Удалить_узел.png", "Удалить узел", ApplicationController.Operations.OpCircleCreate),
                        }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Линейка.png", "Линейка", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Транспортир.png", "Транспортир", ApplicationController.Operations.OpCircleCreate),
                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Слои", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель СЛОИ/Добавить_слой.png", "Добавить слой", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель СЛОИ/Удалить_слой.png", "Удалить слой", ApplicationController.Operations.OpCircleCreate),
                        new MenuButtonItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Менеджер слоёв", ApplicationController.Operations.OpCircleCreate),
                    }, Brushes.DarkSlateGray),
                });

        #endregion

        #endregion

        #region Protected

#if OLDDOTNET
        protected virtual void OnPropertyChanged(string propertyName)
#else
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
#endif
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private

        private uint _session;
        bool _inited;
        private bool _isActive;
        private RelayCommand _documentWorkCommand;
        private RelayCommand _controllerWorkCommand;
        private RelayCommand _controllerDialogCommand;
        private RelayCommand _closeDocumentCommand;
        private ObservableCollection<TabItem> _documentViewModelsTabs = new ObservableCollection<TabItem>();
        private int _selectedDocumentIndex = -1;
        private DispatcherTimer _timer;
        private double _windowWidth;
        private double _windowHeight;
        private readonly List<DocumentModel> _documentViewModels = new List<DocumentModel>();
        private RelayCommand _closeApplicationCommand;
        private readonly Dictionary<TabItem, Document> _tabsDocuments = new Dictionary<TabItem, Document>(); 

        private uint Session
        {
            get
            {
                if (!_inited)
                    throw new ApplicationException("No active session.");
                return _session;
            }
            set { _session = value; }
        }

        private void ProcessDocumentWork(object obj)
        {
            CreateDocument();
        }

        private void RenderPanelOnLoad(IntPtr hwnd, int w, int h, Document model)
        {
            var activeDocument = Controller.initDocument(Session, hwnd, model);
            Controller.activateDocement(Session, activeDocument, w, h);
            ActiveDocument.Title = "Document # " + activeDocument;
            ActiveDocument.DocumentID = activeDocument;
            IsActive = true;
        }

        private void RenderPanelOnResize(int w, int h)
        {
            Controller.resizeDocument(Session, ActiveDocument.DocumentID, w, h);
        }

        private void RenderPanelOnRender()
        {
            if (DocumentViewModels.Count == 0) return;
            Controller.draw(Session, ActiveDocument.DocumentID);
        }

        private void RenderPanelOnMouseFire(MouseEventArgs args)
        {
            Controller.eventHendling(ActiveDocument.DocumentID, (int) args.Button, args.X,
                args.Y, args.Delta);
        }

        private async Task<bool> ProcessControllerWork(ApplicationController.Operations type, object data)
        {
            try
            {
                IsActive = false;
                await Task.Run(delegate
                {
                    Controller.procOperation(Session, ActiveDocument.DocumentID, type,
                        (object[]) data);
                });
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception");
                return false;
            }
            finally
            {
                IsActive = true;
            }
        }

        private async void ProcessControllerRaiseDialog(object obj)
        {
            foreach (var element in BaseMenuElement.CreatedUIElements.OfType<MenuSubItem>())
            {
                element.IsExpanded = false;
            }
            //ResourceDictionary rd = new ResourceDictionary();
            //DataTemplate t = new DataTemplate(typeof(MenuExpanderItem));
            //rd.Add("OpenHand", t);
            //Application.Current.Resources.MergedDictionaries.Add(rd);

            return;
            ApplicationController.Operations type = (ApplicationController.Operations) obj;

            Window modalWindow = null;
            Window separatedWindow = null;

            switch (type)
            {
                case ApplicationController.Operations.OpPointCreate:
                    modalWindow = new OnePointDialog();
                    modalWindow.Title = "Create Point";
                    break;
                case ApplicationController.Operations.OpLineCreate:
                    modalWindow = new TwoPointDialog();
                    modalWindow.Title = "Create Line";
                    break;
                case ApplicationController.Operations.OpCircleCreate:
                    modalWindow = new TwoPointDialog();
                    modalWindow.Title = "Create Circle";
                    break;
                case ApplicationController.Operations.OpContourCreate:
                    modalWindow = new ElementIdInputDialog();
                    modalWindow.Title = "Create Contour";
                    break;
                case ApplicationController.Operations.OpDeleteObject:
                    modalWindow = new DestroyObjectDialog();
                    modalWindow.Title = "Delete element";
                    break;
                case ApplicationController.Operations.OpDestroyContour:
                    modalWindow = new DestroyContourDialog();
                    modalWindow.Title = "Destroy Contour";
                    break;
                case ApplicationController.Operations.OpSetBackgroundColor:
                    separatedWindow = Dialogs.ColorDialog.Instance;
                    separatedWindow.Title = "Colors";
                    break;
                case ApplicationController.Operations.OpSetLayersToShow:
                    separatedWindow = new LayersDialog(Controller, ActiveDocument.DocumentID);
                    separatedWindow.Title = "Layers manager";
                    break;
            }

            if (modalWindow is IDataDialog)
            {
                modalWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (modalWindow.ShowDialog() == true)
                {
                    {
                        var data = ((IDataDialog) modalWindow).Data.ToArray();
                        await ProcessControllerWork(type, data);
                    }

                }
            }
            if (separatedWindow is ICallbackDialog)
            {
                separatedWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ((ICallbackDialog) separatedWindow).DataChanged += o => ProcessControllerWork(type, o);
                separatedWindow.ShowDialog();
            }
        }

        #endregion

        #region Commands

        public RelayCommand DocumentWorkCommand
        {
            get
            {
                return _documentWorkCommand ?? (_documentWorkCommand = new RelayCommand(ProcessDocumentWork,
                    o => (DocumentViewModels.Count > 0 && IsActive) || DocumentViewModels.Count == 0));
            }
        }

        public RelayCommand ControllerWorkCommand
        {
            get
            {
                return _controllerWorkCommand ?? (_controllerWorkCommand = new RelayCommand(async delegate(object o)
                {
                    await ProcessControllerWork((ApplicationController.Operations) o, null);
                }));
            }
        }

        public RelayCommand ControllerDialogCommand
        {
            get
            {
                return _controllerDialogCommand ??
                       (_controllerDialogCommand = new RelayCommand(ProcessControllerRaiseDialog));
            }
        }

        public RelayCommand CloseApplicationCommand
        {
            get
            {
                return _closeApplicationCommand ??
                       (_closeApplicationCommand = new RelayCommand(o => App.Current.Shutdown()));
            }
        }

        public RelayCommand CloseDocumentCommand
        {
            get
            {
                return _closeDocumentCommand ??
                       (_closeDocumentCommand = new RelayCommand(delegate(object i)
                       {
                           string name = i as string;
                           if(string.IsNullOrEmpty(name)) return;

                           var tab = DocumentViewModelsTabs.ToList().Find(d => d.Header.Equals(name));
                           var document = _tabsDocuments[tab];

                           if(document == null) return;

                           var host = (WindowsFormsHost) tab.Content;
                           tab.Content = null;
                           tab.DataContext = null;
                           DocumentViewModelsTabs.Remove(tab);
                           _tabsDocuments.Remove(tab);
                           host.Child.Dispose();
                           host.Dispose();
                           document.Dispose();
                           Controller.finalDocument(Session, document.DocumentID);
                           SelectedDocumentIndex = SelectedDocumentIndex;
                           IsActive = DocumentViewModels.Count > 0;
                       }));
            }
        }

        #endregion
    }
}
