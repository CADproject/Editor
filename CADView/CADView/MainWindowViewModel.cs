using System;
using System.Collections.Generic;
using System.ComponentModel;
using CADController;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using CADView.Dialogs;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CADView
{
    public class MouseEventArgsExtended
    {
        public enum PressedState
        {
            Released,
            Pressed,
        }

        public enum MouseButtons
        {
            Empty,
            Left,
            Right,
            Middle,
        }

        public PressedState State { get; private set; }
        public MouseButtons Button { get; private set; }
        public bool DoubleClick { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Wheel { get; private set; }

        public MouseEventArgsExtended(MouseButtons button, PressedState state, bool doubleClick, double x, double y, double wheel)
        /*: base(args.MouseDevice, args.Timestamp, args.StylusDevice)*/
        {
            Button = button;
            State = state;
            DoubleClick = doubleClick;
            X = x;
            Y = y;
            Wheel = wheel;
        }
    }

    public enum UniversalInputEvents
    {
        //общие события
        ok_event,
        cancel_event,
        //события мыши
        mouse_right_button,
        mouse_left_button_pressed,
        mouse_left_button_released,
        mouse_left_button_double_click,
        mouse_middle_button,
        mouse_wheel,
        //события клавиатуры
        key_ctrl_pressed,
        key_ctrl_released,
        key_shift_pressed,
        key_shift_released,

        Count,
    }

    public enum UniversalCommands
    {
                                                                ////операции с документами
        NewDocument = 0,                                        //create_document,
        OpenDocument,                                           //open_document,
        SaveDocument,                                           //save_document,
        rename_document,                                        //rename_document,
        copy_document,                                          //copy_document,
        close_document,                                         //close_document,
        close_all_docs,                                         //close_all_docs,
        StepBackward,                                           //undo,
        StepForward,                                            //redo,
                                                                ////геометрические операции
        Pen,                                                    //draw_curve,
        Line1,                                                  //draw_line,
        Line2,                                                  //draw_line_parallel,
        Line3,                                                  //draw_line_normal,
        Line4,                                                  //draw_line_at_angle,
        Arc1,                                                   //draw_arc_center_two_points,
        Arc2,                                                   //draw_arc_three_points,
        Circle1,                                                //draw_circle_center_point,
        Circle2,                                                //draw_circle_three_points,
        BrokenLine,                                             //draw_polygonal,
        Spline,                                                 //draw_spline,
                                                                ////операции с объектами
        Eraser,                                                 //delete_object,
        Trimming,                                               //trim_object,
        EnlargeElement,                                         //lengthen_object,
        LinkLines,                                              //sew_polygonal,
        DestroyLine,                                            //destroy_polygonal,
        Correct,                                                //correct_object,
        CreateNode,                                             //add_node_to_object,
        DeleteNode,                                             //remove_node_from_object,
        Measure,                                                //measure_distance,
        Protractor,                                             //measure_angle,
        copy_object,                                            //copy_object,
        cut_object,                                             //cut_object,
        paste_object,                                           //paste_object,
        move_object,                                            //move_object,
        rotate_object,                                          //rotate_object,
        display_in_circle_object,                               //display_in_circle_object,      
        display_symmetyrically_object,                          //display_symmetyrically_object,
        scale_object,                                           //scale_object,
                                                                ////операции с видом
        MoveView,                                               //move_camera_position,
        EnlargeView,                                            //zoom_in_camera,
        DiminishView,                                           //zoom_out_camera,
        ShowAll,                                                //show_all_objects,
                                                                ////операции со слоями
        AddLayer,                                               //add_layer,
        rename_layer,                                            //rename_layer,
        DeleteLayer,                                            //remove_layer,
        LayersManager,
        set_active_layer,                                       //set_active_layer,
        set_visible_layers,                                     //set_visible_layers,
        set_invisible_layers,                                   //set_invisible_layers,
                                                                ////операции оформления
        ShowNodes,                                              //show_hide_nodes,
        ShowGrid,                                               //show_hide_mesh,               
        SetTheme,                                               //change_theme,         
        change_object_proporties,                               //change_object_proporties,

                                                                ////общие операции
        Help,                                                   //show_help_info,
        Statistics,
        Properties,
        Console,
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

    public interface IRenderer: IDisposable
    {
        void UpdateGeometry(double[] points, int[] color, double size, bool clearPrevious);
    }

    public class MainWindowViewModel : INotifyPropertyChanged, IViewCallback
    {
        #region Public

        Grid _helperSpace;

        public MainWindowViewModel() : this(null, null)
        {

        }

        public MainWindowViewModel(Window owner, Grid space)
        {
            _helperSpace = space;
            _helperSpace.SizeChanged += delegate
            {
                ConsoleHeight = ConsoleHeight;
            };
            _owner = owner;
            Controller = FakeController.CreateController();
            WpfRenderPanel.Created += RenderPanelOnLoad;
            WpfRenderPanel.Resized += RenderPanelOnResize;
            WpfRenderPanel.Rendered += RenderPanelOnRender;
            WpfRenderPanel.MouseFired += RenderPanelOnMouseFire;
        }

        ~MainWindowViewModel()
        {
            var ids = DocumentViewModels.Select(d => d.Key).ToList();
            foreach (var id in ids)
            {
                DocumentViewModels[id].Dispose();
            }
            Controller.Operation((int)UniversalCommands.close_all_docs);
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
            Controller.OpenSession(((IViewCallback) this).ConsoleLog, ((IViewCallback) this).DrawGeometry,
                ((IViewCallback) this).FirstString, ((IViewCallback) this).SecondString);
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
            Controller.Operation((int)UniversalCommands.NewDocument);
            //Заметка: мы обновляем весь список документов после создания нового по событию?

            var model = new DocumentModel(new LayerModel(true)) { DocumentID = (uint)DocumentViewModelsTabs.Count };
            model.Title = "Document #" + model.DocumentID;
            DocumentViewModelsTabs.Add(model);
            _selectedDocumentIndex = DocumentViewModelsTabs.Count - 1;
            OnPropertyChanged(nameof(SelectedDocumentIndex));
            OnPropertyChanged(nameof(InfoVisible));
        }

        public void CloseDocument(int id)
        {
            Controller.Operation((int) UniversalCommands.close_document);
            Controller.SendInt(id);
            //Заметка: потому что мы можем закрыть и не активный документ
        }

        public int SelectedDocumentIndex
        {
            get { return _selectedDocumentIndex; }
            set
            {
                _selectedDocumentIndex = value;
                OnPropertyChanged();

                if (SelectedDocumentIndex < 0 || SelectedDocumentIndex > DocumentViewModelsTabs.Count) return;

                Controller.SetActiveDocument(DocumentViewModelsTabs[value].DocumentID);
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

        public DocumentModel ActiveDocument
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
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Новый_документ.png", "Новый документ", UniversalCommands.NewDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Открыть_документ.png", "Открыть документ", UniversalCommands.OpenDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Сохранить.png", "Сохранить", UniversalCommands.SaveDocument),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_назад.png", "Шаг назад", UniversalCommands.StepBackward),
                        new MenuButtonItem("Icons/Панель ОБЩЕЕ/Шаг_вперед.png", "Шаг вперёд", UniversalCommands.StepForward),
                    }, Brushes.Gray),
                    new MenuExpanderItem("Icons/Paint Brush.png", "Рисование", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Карандаш.png", "Карандаш", UniversalCommands.Pen),
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Отрезок_1.png", "Линия", UniversalCommands.Line1,
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_2.png", "Линия под прямым углом", UniversalCommands.Line2),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_3.png", "Параллельная линия", UniversalCommands.Line3),
                                new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Отрезок_4.png", "Линия под углом", UniversalCommands.Line4),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Дуга_1.png", "Дуга по двум точкам и центру", UniversalCommands.Arc1,
                            new []
                            {
                                    new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Дуга_2.png", "Дуга по трём точкам", UniversalCommands.Arc2),
                            }) {Color = Brushes.DimGray},
                        new MenuSubItem("Icons/Панель РИСОВАНИЕ/Окружность_1.png", "Окружность по точке и центру", UniversalCommands.Circle1,
                            new []
                            {
                                    new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Окружность_2.png", "Окружность по трём точкам", UniversalCommands.Circle2),
                            }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Ломаная.png", "Ломаная", UniversalCommands.BrokenLine),
                        new MenuButtonItem("Icons/Панель РИСОВАНИЕ/Сплайн.png", "Сплайн", UniversalCommands.Spline),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Oscilloscope.png", "Отображение", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Подвинуть_вид.png", "Подвинуть изображение", UniversalCommands.MoveView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Приблизить_вид.png", "Увеличить изображение", UniversalCommands.EnlargeView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Отдалить_вид.png", "Уменьшить изображение", UniversalCommands.DiminishView),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Показать_все.png", "Показать всё", UniversalCommands.ShowAll),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Узлы.png", "Показать узлы", UniversalCommands.ShowNodes),
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Вспомогательная_сетка.png", "Показать сетку", UniversalCommands.ShowGrid),
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
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Ластик.png", "Ластик", UniversalCommands.Eraser),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Триммирование.png", "Триммирование", UniversalCommands.Trimming),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Продление.png", "Продление", UniversalCommands.EnlargeElement),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Сшить_ломаную.png", "Создать ломаную", UniversalCommands.LinkLines),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Разрезать_ломаную.png", "Разрушить ломаную", UniversalCommands.DestroyLine),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Корректировать_узел.png", "Корректировка", UniversalCommands.Correct),
                        new MenuSubItem("Icons/Панель РЕДАКТИРОВАНИЕ/Добавить_узел.png", "Добавить узел", UniversalCommands.CreateNode,
                            new[]
                            {
                                new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Удалить_узел.png", "Удалить узел", UniversalCommands.DeleteNode),
                            }) {Color = Brushes.DimGray},
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Линейка.png", "Линейка", UniversalCommands.Measure),
                        new MenuButtonItem("Icons/Панель РЕДАКТИРОВАНИЕ/Транспортир.png", "Транспортир", UniversalCommands.Protractor),
                    }, Brushes.DarkGray),
                    new MenuExpanderItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Слои", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель СЛОИ/Добавить_слой.png", "Добавить слой", UniversalCommands.AddLayer),
                        new MenuButtonItem("Icons/Панель СЛОИ/Удалить_слой.png", "Удалить слой", UniversalCommands.DeleteLayer),
                        new MenuButtonItem("Icons/Панель СЛОИ/Менеджер_слоев.png", "Менеджер слоёв", UniversalCommands.LayersManager),
                    }, Brushes.DarkSlateGray),
                    new MenuExpanderItem("Icons/Question 4.png", "Справка", new BaseMenuElement[]
                    {
                        new MenuButtonItem("Icons/Панель ОТОБРАЖЕНИЕ/Консоль.png", "Консоль", UniversalCommands.Console),
                        new MenuButtonItem("Icons/Панель СПРАВКА/Статистика.png", "Статистика", UniversalCommands.Statistics),
                        new MenuSubItem(null, "Справка", UniversalCommands.Help,
                            new[]
                            {
                                new MenuTextButtonItem("О программе", UniversalCommands.Help, 100),
                                new MenuTextButtonItem("Документация", UniversalCommands.Help, 100),
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
        private RelayCommand _changeThemeCommand;
        private readonly Dictionary<TabItem, Document> _tabsDocuments = new Dictionary<TabItem, Document>();
        private Visibility _consoleVisible = Visibility.Collapsed;
        private double _consoleHeight = 8;
        private double _lastConsoleHeight = 150;
        private const double _minConsoleHeight = 25;
        private const double _minDocumentHeight = 152.5;
        private Thickness _documentMargin = new Thickness(0, 0, 0, 8);
        private string _consoleText = "Test";
        private string _mainInfoText = "Главное инфо:";
        private string _additionalInfoText = "Дополнительное инфо";
        private string _coordinatesTextX = "X = 50";
        private string _coordinatesTextY = "Y = 100";

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

        private void RenderPanelOnLoad(IRenderer sender)
        {
            this.ActiveDocument.Renderer = sender;
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

        private void RenderPanelOnMouseFire(MouseEventArgsExtended args)
        {
            UniversalInputEvents ev = UniversalInputEvents.Count;
            switch (args.Button)
            {
                case MouseEventArgsExtended.MouseButtons.Empty:
                    break;
                case MouseEventArgsExtended.MouseButtons.Left:
                    ev = args.State == MouseEventArgsExtended.PressedState.Pressed
                        ? UniversalInputEvents.mouse_left_button_pressed
                        : UniversalInputEvents.mouse_left_button_released;
                    if (args.DoubleClick)
                        ev = UniversalInputEvents.mouse_left_button_double_click;
                    break;
                case MouseEventArgsExtended.MouseButtons.Right:
                    if(args.State == MouseEventArgsExtended.PressedState.Released)
                        ev = UniversalInputEvents.mouse_right_button;
                    break;
                case MouseEventArgsExtended.MouseButtons.Middle:
                    if (args.State == MouseEventArgsExtended.PressedState.Released)
                        ev = UniversalInputEvents.mouse_middle_button;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (args.Wheel > 0)
                ev = UniversalInputEvents.mouse_wheel;

            if (ev != UniversalInputEvents.Count)
                Controller.Event((int)ev);

            if (args.Wheel > 0)
                Controller.SendDouble(args.Wheel);

            Controller.MouseMove(args.X, args.Y);
            CoordinatesTextX = args.X.ToString("F");
            CoordinatesTextY = args.Y.ToString("F");
        }

        private async Task<bool> ProcessControllerWork(ApplicationController.Operations type, object data)
        {
            try
            {
                IsActive = false;
                await Task.Run(delegate
                {
                    Controller.Operation(0);
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

            if (!(obj is UniversalCommands))
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

                UniversalCommands buttonCommand = (UniversalCommands)obj;
                switch (buttonCommand)
                {
                    case UniversalCommands.NewDocument:
                        CreateDocument();
                        break;
                    case UniversalCommands.OpenDocument:
                        new OpenFileDialog().ShowDialog();
                        break;
                    case UniversalCommands.SaveDocument:
                        new SaveFileDialog().ShowDialog();
                        break;
                    case UniversalCommands.StepBackward:
                        break;
                    case UniversalCommands.StepForward:
                        break;
                    case UniversalCommands.Pen:
                        break;
                    case UniversalCommands.Line1:
                        break;
                    case UniversalCommands.Line2:
                        break;
                    case UniversalCommands.Line3:
                        break;
                    case UniversalCommands.Line4:
                        break;
                    case UniversalCommands.Arc1:
                        break;
                    case UniversalCommands.Arc2:
                        break;
                    case UniversalCommands.Circle1:
                        break;
                    case UniversalCommands.Circle2:
                        break;
                    case UniversalCommands.BrokenLine:
                        break;
                    case UniversalCommands.Spline:
                        break;
                    case UniversalCommands.MoveView:
                        break;
                    case UniversalCommands.EnlargeView:
                        break;
                    case UniversalCommands.DiminishView:
                        break;
                    case UniversalCommands.ShowAll:
                        break;
                    case UniversalCommands.ShowNodes:
                        break;
                    case UniversalCommands.ShowGrid:
                        break;
                    case UniversalCommands.SetTheme:
                        break;
                    case UniversalCommands.Eraser:
                        break;
                    case UniversalCommands.Trimming:
                        break;
                    case UniversalCommands.EnlargeElement:
                        break;
                    case UniversalCommands.LinkLines:
                        break;
                    case UniversalCommands.DestroyLine:
                        break;
                    case UniversalCommands.Correct:
                        break;
                    case UniversalCommands.CreateNode:
                        break;
                    case UniversalCommands.DeleteNode:
                        break;
                    case UniversalCommands.Measure:
                        break;
                    case UniversalCommands.Protractor:
                        break;
                    case UniversalCommands.AddLayer:
                        new LayersAdd { Owner = _owner }.ShowDialog();
                        break;
                    case UniversalCommands.DeleteLayer:
                        new LayersDelete { Owner = _owner }.ShowDialog();
                        break;
                    case UniversalCommands.LayersManager:
                        new LayersManager { Owner = _owner }.ShowDialog();
                        break;
                    case UniversalCommands.Help:
                        break;
                    case UniversalCommands.Console:
                        ConsoleVisible = ConsoleVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        break;
                    case UniversalCommands.Statistics:
                        new Statistics { Owner = _owner }.ShowDialog();
                        break;
                    case UniversalCommands.Properties:
                        new Dialogs.Properties { Owner = _owner }.ShowDialog();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region View callback functions

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), System.Security.SuppressUnmanagedCodeSecurity]
        private static extern unsafe void* CopyMemory(void* dest, void* src, ulong count);

        unsafe void IViewCallback.DrawGeometry(CallbackValues value)
        {
            int* color = (int*) value.pInt.ToPointer();
            double* points = (double*) value.pDouble.ToPointer();
            double[] arrayPoints = new double[value.size * 3];
            fixed (double* p = arrayPoints)
                CopyMemory(p, points, (ulong) (value.size * 3 * sizeof(double)));
            ActiveDocument?.Renderer?.UpdateGeometry(arrayPoints, new[] {color[0], color[1], color[2]}, value.thickness,
                value.flag == 1);
        }

        void IViewCallback.Background(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.DrawNodes(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.DrawMesh(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetCameraPosition(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.FirstString(CallbackValues value)
        {
            this.MainInfoText = value.line;
        }

        void IViewCallback.SecondString(CallbackValues value)
        {
            this.AdditionalInfoText = value.line;
        }

        void IViewCallback.ConsoleLog(CallbackValues value)
        {
            ConsoleText += Environment.NewLine + value.line;
        }

        void IViewCallback.LayersList(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.VisibleLayers(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetActiveLayer(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetDocName(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.DocsList(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetDocState(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetDocStatistics(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.SetActiveTheme(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        void IViewCallback.ThemesList(CallbackValues value)
        {
            throw new NotImplementedException();
        }

        #endregion

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
                    CloseDocument((int) tab.DocumentID);

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

        public RelayCommand ChangeThemeCommand
        {
            get
            {
                return _changeThemeCommand ?? (_changeThemeCommand = new RelayCommand(
                           delegate(object o)
                           {
                               ResourceDictionary resources;
                               ResourceDictionary second;
                               List<ResourceDictionary> resourcesToChange = new List<ResourceDictionary>();
                               resources = this._owner.Resources;
                               resources = App.Instance.Resources;
                               second = resources.MergedDictionaries[1];
                               resourcesToChange.Add(second);
                               var isLight = !(bool) o;
                               if (isLight)
                               {
                                   foreach (var res in resourcesToChange)
                                   {
                                       res["Theme_TelegramButtonColor"]        = res["ThemeL_TelegramButtonColor"];
                                       res["Theme_PressedTelegramButtonColor"] = res["ThemeL_PressedTelegramButtonColor"];
                                       res["Theme_StandartBackgroundColor"]    = res["ThemeL_StandartBackgroundColor"];
                                       res["Theme_StandartButtonColor"]        = res["ThemeL_StandartButtonColor"];
                                   }
                               }
                               else
                               {
                                   foreach (var res in resourcesToChange)
                                   {
                                       res["Theme_TelegramButtonColor"]        = res["ThemeD_TelegramButtonColor"];
                                       res["Theme_PressedTelegramButtonColor"] = res["ThemeD_PressedTelegramButtonColor"];
                                       res["Theme_StandartBackgroundColor"]    = res["ThemeD_StandartBackgroundColor"];
                                       res["Theme_StandartButtonColor"]        = res["ThemeD_StandartButtonColor"];
                                   }
                               }

                               this._owner.UpdateDefaultStyle();
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

        public string StatusBarTextFirst { get; set; } = "имя документа";
        public string StatusBarTextSecond { get; set; } = "Активный слой: имя активного слоя";

        public string MainInfoText
        {
            get { return _mainInfoText; }
            set
            {
                _mainInfoText = value;
                OnPropertyChanged();
            }
        }

        public string AdditionalInfoText
        {
            get { return _additionalInfoText; }
            set
            {
                _additionalInfoText = value;
                OnPropertyChanged();
            }
        }

        public string ConsoleText
        {
            get { return _consoleText; }
            set
            {
                _consoleText = value;
                OnPropertyChanged();
            }
        }

        public string CoordinatesTextX
        {
            get { return _coordinatesTextX; }
            set
            {
                _coordinatesTextX = value;
                OnPropertyChanged();
            }
        }

        public string CoordinatesTextY
        {
            get { return _coordinatesTextY; }
            set
            {
                _coordinatesTextY = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
