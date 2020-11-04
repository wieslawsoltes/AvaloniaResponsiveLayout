using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Xaml.Interactivity;

// https://docs.microsoft.com/en-us/windows/uwp/design/layout/screen-sizes-and-breakpoints-for-responsive-design
// https://docs.microsoft.com/en-us/windows/uwp/design/layout/alignment-margin-padding
// https://docs.microsoft.com/en-us/windows/uwp/design/layout/layouts-with-xaml

namespace AvaloniaResponsiveLayout
{
    public class ScreenPseudoClassesBehaviour : Behavior<Control>
    {
        public static readonly StyledProperty<double> SmallMediumLimitProperty =
            AvaloniaProperty.Register<ScreenPseudoClassesBehaviour, double>(nameof(SmallMediumLimit), 640.0);

        public static readonly StyledProperty<double> MediumLargeLimitProperty =
            AvaloniaProperty.Register<ScreenPseudoClassesBehaviour, double>(nameof(MediumLargeLimit), 1008.0);

        public double SmallMediumLimit
        {
            get => GetValue(SmallMediumLimitProperty);
            set => SetValue(SmallMediumLimitProperty, value);
        }

        public double MediumLargeLimit
        {
            get => GetValue(MediumLargeLimitProperty);
            set => SetValue(MediumLargeLimitProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AttachedToVisualTree += AssociatedObject_AttachedToVisualTree;
            AssociatedObject.PropertyChanged += AssociatedObject_PropertyChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AttachedToVisualTree -= AssociatedObject_AttachedToVisualTree;
            AssociatedObject.PropertyChanged -= AssociatedObject_PropertyChanged;
        }

        private void SetPseudoClasses()
        {
            var width = AssociatedObject.Bounds.Width;
            var smallMediumLimit = SmallMediumLimit;
            var mediumLargeLimit = MediumLargeLimit;
            var isSmallScreen = width <= smallMediumLimit;
            var isMediumScreen = width > smallMediumLimit && width < mediumLargeLimit;
            var isLargeScreen = width >= mediumLargeLimit;
            AssociatedObject.Classes.Set(":SmallScreen", isSmallScreen);
            AssociatedObject.Classes.Set(":MediumScreen", isMediumScreen);
            AssociatedObject.Classes.Set(":LargeScreen", isLargeScreen);
        }

        private void AssociatedObject_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            SetPseudoClasses();
        }

        private void AssociatedObject_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Control.BoundsProperty)
            {
                SetPseudoClasses();
            }
        }
    }

    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

#if false
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == BoundsProperty)
            {
                var width = Bounds.Width;
                var isSmallScreen = width < 640;
                var isMediumScreen = width >= 641 && width <= 1007;
                var isLargeScreen = width > 1007;
                PseudoClasses.Set(":SmallScreen", isSmallScreen);
                PseudoClasses.Set(":MediumScreen", isMediumScreen);
                PseudoClasses.Set(":LargeScreen", isLargeScreen);
            }
        }
#endif

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}