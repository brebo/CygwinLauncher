using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Brebo.CygwinLauncher.Config
{
    class ValidationErrorBehavior : Behavior<DependencyObject>
    {
        private int errorCount;

        public bool HasViewError
        {
            get { return (bool)GetValue(HasViewErrorProperty); }
            set { SetValue(HasViewErrorProperty, value); }
        }

        public static readonly DependencyProperty HasViewErrorProperty =
            DependencyProperty.Register("HasViewError", typeof(bool), typeof(ValidationErrorBehavior), new UIPropertyMetadata(false));
        
        protected override void OnAttached()
        {
            base.OnAttached();
            Validation.AddErrorHandler(this.AssociatedObject, this.ErrorHandler);
        }

        protected override void OnDetaching()
        {
            Validation.RemoveErrorHandler(this.AssociatedObject, this.ErrorHandler);
            base.OnDetaching();
        }

        private void ErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                errorCount++;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                errorCount--;
            }
            this.HasViewError = this.errorCount != 0;
        }
    }
}
