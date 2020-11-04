﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace AvaloniaResponsiveLayout.Behaviors
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
}