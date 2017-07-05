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
        Help
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        public MainWindowViewModel()
        {
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
            Controller.CloseSession();
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
                Controller.SetActiveDocument(ActiveDocument.DocumentID);
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
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Новый_документ.png", "Новый документ", ButtonsCommands.NewDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Открыть_документ.png", "Открыть документ", ButtonsCommands.OpenDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Сохранить.png", "Сохранить", ButtonsCommands.SaveDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_назад.png", "Шаг назад", ButtonsCommands.StepBackward),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_вперед.png", "Шаг вперёд", ButtonsCommands.StepForward),
                    }, Brushes.Gray),
                    new MenuExpanderItem("Icons/Paint Brush.png", "Рисование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Карандаш.png", "Карандаш", ButtonsCommands.Pen),
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия",
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия", ButtonsCommands.Line1),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_2.png", "Линия под прямым углом", ButtonsCommands.Line2),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_3.png", "Параллельная линия", ButtonsCommands.Line3),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_4.png", "Линия под углом", ButtonsCommands.Line4),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга", new []
                        {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга по двум точкам и центру", ButtonsCommands.Arc1),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_2.png", "Дуга по трём точкам", ButtonsCommands.Arc2),
                        }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность", new []
                        {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность по центру и точке", ButtonsCommands.Circle1),
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
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Темы.png", "Сменить тему", ButtonsCommands.SetTheme),

                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Edit.png", "Редактирование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Ластик.png", "Ластик", ButtonsCommands.Eraser),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Триммирование.png", "Триммирование", ButtonsCommands.Trimming),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Продление.png", "Продление", ButtonsCommands.EnlargeElement),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Сшить_ломаную.png", "Создать ломаную", ButtonsCommands.LinkLines),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Разрезать_ломаную.png", "Разрушить ломаную", ButtonsCommands.DestroyLine),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Корректировать_узел.png", "Корректировка", ButtonsCommands.Correct),
                        new MenuSubItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Узлы", new[]
                        {
                            new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Добавить узел", ButtonsCommands.CreateNode),
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
                        new MenuButtonItem("Icons/Панель СПРАВКА/Справка.png", "Справка", ButtonsCommands.Help, 61),
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

            ButtonsCommands button = (ButtonsCommands) obj;
            switch (button)
            {
                case ButtonsCommands.NewDocument:
                    this.CreateDocument();
                    break;
                case ButtonsCommands.OpenDocument:
                    break;
                case ButtonsCommands.SaveDocument:
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
                    break;
                case ButtonsCommands.DeleteLayer:
                    break;
                case ButtonsCommands.LayersManager:
                    break;
                case ButtonsCommands.Help:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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
                    //separatedWindow = new LayersDialog(Controller, ActiveDocument.DocumentID);
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
            get { return _documentWorkCommand ?? (_documentWorkCommand = new RelayCommand(ProcessDocumentWork, o => (DocumentViewModels.Count > 0 && IsActive) || DocumentViewModels.Count == 0)); }
        }

        public RelayCommand ControllerWorkCommand
        {
            get { return _controllerWorkCommand ?? (_controllerWorkCommand = new RelayCommand(async delegate(object o) { await ProcessControllerWork((ApplicationController.Operations) o, null); })); }
        }

        public RelayCommand ControllerDialogCommand
        {
            get { return _controllerDialogCommand ?? (_controllerDialogCommand = new RelayCommand(ProcessControllerRaiseDialog)); }
        }

        public RelayCommand CloseApplicationCommand
        {
            get { return _closeApplicationCommand ?? (_closeApplicationCommand = new RelayCommand(o => App.Current.Shutdown())); }
        }

        public RelayCommand CloseDocumentCommand
        {
            get
            {
                return _closeDocumentCommand ?? (_closeDocumentCommand = new RelayCommand(delegate(object i)
                {
                    string name = i as string;
                    if (string.IsNullOrEmpty(name)) return;

                    var tab = DocumentViewModelsTabs.ToList().Find(d => d.Header.Equals(name));
                    var document = _tabsDocuments[tab];

                    if (document == null) return;

                    var host = (WindowsFormsHost) tab.Content;
                    tab.Content = null;
                    tab.DataContext = null;
                    DocumentViewModelsTabs.Remove(tab);
                    _tabsDocuments.Remove(tab);
                    host.Child.Dispose();
                    host.Dispose();
                    document.Dispose();
                    //Controller.finalDocument(Session, document.DocumentID);
                    SelectedDocumentIndex = SelectedDocumentIndex;
                    IsActive = DocumentViewModels.Count > 0;
                }));
            }
        }

        #endregion
    }
}
