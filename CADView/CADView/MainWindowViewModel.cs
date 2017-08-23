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
using System.Windows.Threading;
using CADView.Dialogs;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CADView
{
    public enum ButtonsCommands
    {
        //Core Operations
        //Controller Operations
        //View Operations
        NewDocument = 0,
        OpenDocument,
        SaveDocument,
        StepBackward,
        StepForward,
        Pen,
        Line1,
        Line2,
        Line3,
        Line4,
        Arc1,
        Arc2,
        Circle1,
        Circle2,
        BrokenLine,
        Spline,
        MoveView,
        EnlargeView,
        DiminishView,
        ShowAll,
        ShowNodes,
        ShowGrid,
        SetTheme,
        Console,
        Eraser,
        Trimming,
        EnlargeElement,
        LinkLines,
        DestroyLine,
        Correct,
        CreateNode,
        DeleteNode,
        Measure,
        Protractor,
        AddLayer,
        DeleteLayer,
        LayersManager,
        Help,
        Statistics,
        Properties,

        Count,
    }

    public class LayerModel : Layer, INotifyPropertyChanged
    {
        public LayerModel(bool visible = false) : base(visible)
        {
        }

        public LayerModel(int id, bool visible = false) : base(id, visible)
        {
        }

        public LayerModel(Layer layer) : base(layer)
        {

        }

        public override int Id
        {
            get { return base.Id; }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                OnPropertyChanged();
                VisibleChangedStatic?.Invoke(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public static event PropertyChangedEventHandler VisibleChangedStatic;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        Grid _helperSpace;

        public MainWindowViewModel() : this(null, null)
        {

        }

        public MainWindowViewModel(Window owner, Grid space)
        {
            _helperSpace = space;
            _owner = owner;
            //Controller = new ApplicationController();
            RenderPanel.Loaded += RenderPanelOnLoad;
            RenderPanel.Resized += RenderPanelOnResize;
            RenderPanel.Rendered += RenderPanelOnRender;
            RenderPanel.MouseFired += RenderPanelOnMouseFire;
        }

        ~MainWindowViewModel()
        {
            var ids = DocumentViewModels.Select(d => d.Key).ToList();
            foreach (var id in ids)
            {
                DocumentViewModels[id].Dispose();
                //Controller.finalDocument(Session, id);
            }
            //Controller.CloseSession();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate Task ProcessDataDelegate(object[] o);

        internal IController Controller { get; private set; }

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
            //Controller.OpenSession();
            _inited = true;

            _timer = new DispatcherTimer(DispatcherPriority.Normal, Application.Current.Dispatcher);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _timer.Tick += delegate
            {
                if (DocumentViewModels.Count == 0) return;
                //Controller.draw(Session, ActiveDocument.DocumentID);
            };
            _timer.Start();
        }

        public void CreateDocument()
        {
            var model = new DocumentModel(new LayerModel(true)) { DocumentID = (uint)DocumentViewModelsTabs.Count };
            model.Title = "Document #" + model.DocumentID;
            //var host = new WindowsFormsHost()
            //{
            //    Child = new RenderPanel(model),
            //    VerticalAlignment = VerticalAlignment.Stretch,
            //    HorizontalAlignment = HorizontalAlignment.Stretch,
            //};

            //TabItem tab = new TabItem()
            //{
            //    Content = host,
            //    DataContext = model,
            //};
            DocumentViewModelsTabs.Add(model);
            //_tabsDocuments[tab] = model;
            _selectedDocumentIndex = DocumentViewModelsTabs.Count - 1;
            OnPropertyChanged(nameof(SelectedDocumentIndex));
            OnPropertyChanged(nameof(InfoVisible));
        }

        public int SelectedDocumentIndex
        {
            get { return _selectedDocumentIndex; }
            set
            {
                _selectedDocumentIndex = value;
                OnPropertyChanged();

                if (SelectedDocumentIndex < 0 || SelectedDocumentIndex > DocumentViewModelsTabs.Count) return;

                //Controller.SetActiveDocument(ActiveDocument.DocumentID);
            }
        }

        public ObservableCollection<DocumentModel> DocumentViewModelsTabs
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
            get;
            //get { return Controller.Documents; }
        } = new Dictionary<uint, Document>();

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
                return DocumentViewModelsTabs[SelectedDocumentIndex];
            }
            set
            {
                SelectedDocumentIndex =
                    DocumentViewModelsTabs.IndexOf(DocumentViewModelsTabs.FirstOrDefault(i => i.DocumentID == value.DocumentID));
            }
        }

        public ObservableCollection<string> TabMenuCollection { get; set; } =
            new ObservableCollection<string>(new[] { "123", "456" });

        #region UI Buttons

        public ObservableCollection<BaseMenuElement> UIMenuElements { get; } =
            new ObservableCollection<BaseMenuElement>(new BaseMenuElement[]
                {
                    new MenuExpanderItem("Icons/Home.png", "Документ", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Новый_документ.png", "Новый документ", ButtonsCommands.NewDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Открыть_документ.png", "Открыть документ", ButtonsCommands.OpenDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Сохранить.png", "Сохранить", ButtonsCommands.SaveDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_назад.png", "Шаг назад", ButtonsCommands.StepBackward),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_вперед.png", "Шаг вперёд", ButtonsCommands.StepForward),
                    }, Brushes.Gray),
                    new MenuExpanderItem("Icons/Paint Brush.png", "Рисование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Карандаш.png", "Карандаш", ButtonsCommands.Pen),
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия", ButtonsCommands.Line1,
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_2.png", "Линия под прямым углом", ButtonsCommands.Line2),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_3.png", "Параллельная линия", ButtonsCommands.Line3),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_4.png", "Линия под углом", ButtonsCommands.Line4),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга по двум точкам и центру", ButtonsCommands.Arc1,
                            new []
                            {
                                    new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_2.png", "Дуга по трём точкам", ButtonsCommands.Arc2),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность по точке и центру", ButtonsCommands.Circle1,
                            new []
                            {
                                    new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Окружность_2.png", "Окружность по трём точкам", ButtonsCommands.Circle2),
                            }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Ломаная.png", "Ломаная", ButtonsCommands.BrokenLine),
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Сплайн.png", "Сплайн", ButtonsCommands.Spline),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Oscilloscope.png", "Отображение", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Подвинуть_вид.png", "Подвинуть изображение", ButtonsCommands.MoveView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Приблизить_вид.png", "Увеличить изображение", ButtonsCommands.EnlargeView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Отдалить_вид.png", "Уменьшить изображение", ButtonsCommands.DiminishView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Показать_все.png", "Показать всё", ButtonsCommands.ShowAll),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Узлы.png", "Показать узлы", ButtonsCommands.ShowNodes),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Вспомогательная_сетка.png", "Показать сетку", ButtonsCommands.ShowGrid),
                        new MenuSubItem("Icons/Панель ОТОБРАЖЕНИЕ/Темы.png", "Сменить тему", null,
                            new MenuTextButtonItem[]
                            {
                                new MenuTextButtonItem("Оранжевая", null, 80) {Color = Brushes.DarkSeaGreen, IsSelected = true},
                                new MenuTextButtonItem("Синяя", null, 80),
                                new MenuTextButtonItem("Годная", null, 80),
                            }) {Color = Brushes.DimGray},
                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Edit.png", "Редактирование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Ластик.png", "Ластик", ButtonsCommands.Eraser),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Триммирование.png", "Триммирование", ButtonsCommands.Trimming),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Продление.png", "Продление", ButtonsCommands.EnlargeElement),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Сшить_ломаную.png", "Создать ломаную", ButtonsCommands.LinkLines),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Разрезать_ломаную.png", "Разрушить ломаную", ButtonsCommands.DestroyLine),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Корректировать_узел.png", "Корректировка", ButtonsCommands.Correct),
                        new MenuSubItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Добавить узел", ButtonsCommands.CreateNode,
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Удалить_узел.png", "Удалить узел", ButtonsCommands.DeleteNode),
                            }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Линейка.png", "Линейка", ButtonsCommands.Measure),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Транспортир.png", "Транспортир", ButtonsCommands.Protractor),
                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Слои", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель СЛОИ/Добавить_слой.png", "Добавить слой", ButtonsCommands.AddLayer),
                        new MenuButtonItem("Icons/Панель СЛОИ/Удалить_слой.png", "Удалить слой", ButtonsCommands.DeleteLayer),
                        new MenuButtonItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Менеджер слоёв", ButtonsCommands.LayersManager),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Question 4.png", "Справка", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Консоль.png", "Консоль", ButtonsCommands.Console),
                        new MenuButtonItem("Icons/Панель СПРАВКА/Статистика.png", "Статистика", ButtonsCommands.Statistics),
                        new MenuSubItem(null, "Справка", ButtonsCommands.Help,
                            new[]
                            {
                                new MenuTextButtonItem("О программе", ButtonsCommands.Help, 100),
                                new MenuTextButtonItem("Документация", ButtonsCommands.Help, 100),
                            }, 56) {Description = "Справка"},
                    }, Brushes.DarkSlateGray, Visibility.Hidden),
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

        private readonly Window _owner;
        private uint _session;
        bool _inited;
        private bool _isActive;
        private RelayCommand _documentWorkCommand;
        private RelayCommand _controllerWorkCommand;
        private RelayCommand _controllerDialogCommand;
        private RelayCommand _closeDocumentCommand;
        private ObservableCollection<DocumentModel> _documentViewModelsTabs = new ObservableCollection<DocumentModel>();
        private int _selectedDocumentIndex = -1;
        private DispatcherTimer _timer;
        private double _windowWidth;
        private double _windowHeight;
        private readonly List<DocumentModel> _documentViewModels = new List<DocumentModel>();
        private RelayCommand _closeApplicationCommand;
        private readonly Dictionary<TabItem, Document> _tabsDocuments = new Dictionary<TabItem, Document>();
        private Visibility _consoleVisible = Visibility.Collapsed;
        private double _consoleHeight = 8;
        private double _lastConsoleHeight = 150;
        private const double _minConsoleHeight = 25;
        private const double _minDocumentHeight = 152.5;
        private Thickness _documentMargin = new Thickness(0, 0, 0, 8);

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
            //Controller.SetActiveDocument();
            //var activeDocument = Controller.initDocument(Session, hwnd, model);
            //Controller.activateDocement(Session, activeDocument, w, h);
            //ActiveDocument.Title = "Document # " + activeDocument;
            //ActiveDocument.DocumentID = activeDocument;
            //IsActive = true;
        }

        private void RenderPanelOnResize(int w, int h)
        {
            //Controller.resizeDocument(Session, ActiveDocument.DocumentID, w, h);
        }

        private void RenderPanelOnRender()
        {
            if (DocumentViewModels.Count == 0) return;
            //Controller.draw(Session, ActiveDocument.DocumentID);
        }

        private void RenderPanelOnMouseFire(MouseEventArgs args)
        {
            //Controller.eventHendling(ActiveDocument.DocumentID, (int) args.Button, args.X,
            //    args.Y, args.Delta);
        }

        private async Task<bool> ProcessControllerWork(ApplicationController.Operations type, object data)
        {
            try
            {
                IsActive = false;
                await Task.Run(delegate
                {
                    //Controller.procOperation(Session, ActiveDocument.DocumentID, type,
                    //    (object[]) data);
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

            if (!(obj is ButtonsCommands))
            {

            }
            else
            {
                var button = BaseMenuElement.CreatedUIElements.Find(
                    element => element is MenuButtonItem && ((IButtonOperation)element).Parameter == obj);
                var parent = BaseMenuElement.CreatedUIElements.Find(element =>
                    element is MenuSubItem && ((BaseExpanderItem)element).SubItems
                        .ToList().Contains(button)) as BaseExpanderItem;
                if (parent != null)
                {
                    string tmp;
                    tmp = button.Image;
                    button.Image = parent.Image;
                    parent.Image = tmp;

                    tmp = button.HintText;
                    button.HintText = parent.HintText;
                    parent.HintText = tmp;

                    tmp = button.Description;
                    button.Description = parent.Description;
                    parent.Description = tmp;

                    var brush = button.Color;
                    button.Color = parent.Color;
                    parent.Color = brush;

                    parent.IsExpanded = false;
                }

                ButtonsCommands buttonCommand = (ButtonsCommands)obj;
                switch (buttonCommand)
                {
                    case ButtonsCommands.NewDocument:
                        CreateDocument();
                        break;
                    case ButtonsCommands.OpenDocument:
                        new OpenFileDialog().ShowDialog();
                        break;
                    case ButtonsCommands.SaveDocument:
                        new SaveFileDialog().ShowDialog();
                        break;
                    case ButtonsCommands.StepBackward:
                        break;
                    case ButtonsCommands.StepForward:
                        break;
                    case ButtonsCommands.Pen:
                        break;
                    case ButtonsCommands.Line1:
                        break;
                    case ButtonsCommands.Line2:
                        break;
                    case ButtonsCommands.Line3:
                        break;
                    case ButtonsCommands.Line4:
                        break;
                    case ButtonsCommands.Arc1:
                        break;
                    case ButtonsCommands.Arc2:
                        break;
                    case ButtonsCommands.Circle1:
                        break;
                    case ButtonsCommands.Circle2:
                        break;
                    case ButtonsCommands.BrokenLine:
                        break;
                    case ButtonsCommands.Spline:
                        break;
                    case ButtonsCommands.MoveView:
                        break;
                    case ButtonsCommands.EnlargeView:
                        break;
                    case ButtonsCommands.DiminishView:
                        break;
                    case ButtonsCommands.ShowAll:
                        break;
                    case ButtonsCommands.ShowNodes:
                        break;
                    case ButtonsCommands.ShowGrid:
                        break;
                    case ButtonsCommands.SetTheme:
                        break;
                    case ButtonsCommands.Eraser:
                        break;
                    case ButtonsCommands.Trimming:
                        break;
                    case ButtonsCommands.EnlargeElement:
                        break;
                    case ButtonsCommands.LinkLines:
                        break;
                    case ButtonsCommands.DestroyLine:
                        break;
                    case ButtonsCommands.Correct:
                        break;
                    case ButtonsCommands.CreateNode:
                        break;
                    case ButtonsCommands.DeleteNode:
                        break;
                    case ButtonsCommands.Measure:
                        break;
                    case ButtonsCommands.Protractor:
                        break;
                    case ButtonsCommands.AddLayer:
                        new LayersAdd { Owner = _owner }.ShowDialog();
                        break;
                    case ButtonsCommands.DeleteLayer:
                        new LayersDelete { Owner = _owner }.ShowDialog();
                        break;
                    case ButtonsCommands.LayersManager:
                        new LayersManager { Owner = _owner }.ShowDialog();
                        break;
                    case ButtonsCommands.Help:
                        break;
                    case ButtonsCommands.Console:
                        ConsoleVisible = ConsoleVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        break;
                    case ButtonsCommands.Statistics:
                        new Statistics { Owner = _owner }.ShowDialog();
                        break;
                    case ButtonsCommands.Properties:
                        new Dialogs.Properties { Owner = _owner }.ShowDialog();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand DocumentWorkCommand
        {
            get { return _documentWorkCommand ?? (_documentWorkCommand = new RelayCommand(ProcessDocumentWork, o => (DocumentViewModels.Count > 0 && IsActive) || DocumentViewModels.Count == 0)); }
        }

        public RelayCommand ControllerWorkCommand
        {
            get { return _controllerWorkCommand ?? (_controllerWorkCommand = new RelayCommand(async delegate (object o) { await ProcessControllerWork((ApplicationController.Operations)o, null); })); }
        }

        public RelayCommand ControllerDialogCommand
        {
            get { return _controllerDialogCommand ?? (_controllerDialogCommand = new RelayCommand(ProcessControllerRaiseDialog)); }
        }

        public RelayCommand CloseApplicationCommand
        {
            get { return _closeApplicationCommand ?? (_closeApplicationCommand = new RelayCommand(o => Application.Current.Shutdown())); }
        }

        public RelayCommand CloseDocumentCommand
        {
            get
            {
                return _closeDocumentCommand ?? (_closeDocumentCommand = new RelayCommand(delegate (object i)
                {
                    string name = i as string;
                    if (string.IsNullOrEmpty(name)) return;

                    var tab = DocumentViewModelsTabs.ToList().Find(d => d.Title.Equals(name));
                    //var document = _tabsDocuments[tab];

                    //if (document == null) return;

                    //var host = (WindowsFormsHost) tab.Content;
                    //tab.Content = null;
                    //tab.DataContext = null;
                    DocumentViewModelsTabs.Remove(tab);
                    //_tabsDocuments.Remove(tab);
                    //host.Child.Dispose();
                    //host.Dispose();
                    tab.Dispose();
                    //Controller.finalDocument(Session, document.DocumentID);
                    SelectedDocumentIndex = SelectedDocumentIndex;
                    IsActive = DocumentViewModels.Count > 0;
                }));
            }
        }

        #endregion

        #region ScreenInfo

        #region Console height helpers

        public GridLength ConsoleHeight
        {
            get { return new GridLength(_consoleHeight, GridUnitType.Pixel); }
            set
            {
                if (ConsoleVisible == Visibility.Visible && value.Value < _minConsoleHeight)
                    _consoleHeight = _minConsoleHeight;
                else if (ConsoleVisible == Visibility.Visible && this._helperSpace.ActualHeight - value.Value < _minDocumentHeight)
                    _consoleHeight = this._helperSpace.ActualHeight - _minDocumentHeight;
                else
                    _consoleHeight = value.Value;
                OnPropertyChanged();
            }
        }

        public Thickness DocumentMargin
        {
            get { return _documentMargin; }
            set
            {
                _documentMargin = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public Visibility ConsoleVisible
        {
            get { return _consoleVisible; }
            set
            {
                _consoleVisible = value;
                OnPropertyChanged();
                if (value != Visibility.Visible)
                {
                    _lastConsoleHeight = ConsoleHeight.Value;
                    ConsoleHeight = new GridLength(8, GridUnitType.Pixel);
                    DocumentMargin = new Thickness(0, 0, 0, 8);
                    //DocumentsHeight = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    ConsoleHeight = new GridLength(_lastConsoleHeight, GridUnitType.Star);
                    DocumentMargin = new Thickness(0);
                    //DocumentsHeight = new GridLength(1, GridUnitType.Pixel);
                }
            }
        }

        public Visibility InfoVisible
        {
            get { return DocumentViewModelsTabs.Count > 0 ? Visibility.Visible : Visibility.Hidden; }
        }

        public string StatusBarTextFirst => "имя документа";
        public string StatusBarTextSecond => "Активный слой: имя активного слоя";

        public string MainInfoText => "Главное инфо:";

        public string AdditionalInfoText => "Дополнительное инфо";

        public string CoordinatesTextX => "X = 50";
        public string CoordinatesTextY => "Y = 100";

        #endregion
    }
}
