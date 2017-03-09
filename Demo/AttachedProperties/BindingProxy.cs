using System.ComponentModel;
using System.Windows;

namespace Demo.AttachedProperties
{
    public static class BindingProxy
    {

        #region Source
        //Represents the source object that the data is coming from
        public static object GetSource(DependencyObject obj) => obj.GetValue(SourceProperty);

        public static void SetSource(DependencyObject obj, object value) => obj.SetValue(SourceProperty, value);

        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
        #endregion

        #region SourceProperty
        //Represents the name of property on the source object that the data is coming from.  
        public static string GetSourceProperty(DependencyObject obj) => (string)obj.GetValue(SourcePropertyProperty);

        public static void SetSourceProperty(DependencyObject obj, string value) => obj.SetValue(SourcePropertyProperty, value);

        // Using a DependencyProperty as the backing store for SourceProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourcePropertyProperty = DependencyProperty.RegisterAttached("SourceProperty", typeof(string), typeof(BindingProxy), new PropertyMetadata(null));
        #endregion

        #region TargetProperty
        //This is the property on the attached control that we are binding to (e.g. Text, SelectedValue, etc.)
        public static object GetTargetProperty(DependencyObject obj) => obj.GetValue(TargetPropertyProperty);

        public static void SetTargetProperty(DependencyObject obj, object value) => obj.SetValue(TargetPropertyProperty, value);

        public static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof(object), typeof(BindingProxy), new PropertyMetadata(OnTargetPropertyChanged));
        #endregion

        #region BindingInitialized
        //Used internally to indicate if the binding has been initialized

        public static bool GetBindingProxyInitialized(DependencyObject obj) => (bool)obj.GetValue(BindingProxyInitializedProperty);

        public static void SetBindingProxyInitialized(DependencyObject obj, bool value) => obj.SetValue(BindingProxyInitializedProperty, value);

        public static readonly DependencyProperty BindingProxyInitializedProperty = DependencyProperty.RegisterAttached("BindingProxyInitialized", typeof(bool), typeof(BindingProxy), new PropertyMetadata(false));

        #endregion

        private static void OnTargetPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var initialized = GetBindingProxyInitialized(target);
            if (!initialized)
            {
                //Don't run the first time through because binding flow has not yet been established
                SetBindingProxyInitialized(target, true);
                return;
            }

            //Get the target element
            var frameworkElement = target as FrameworkElement;
            if (frameworkElement == null) return;

            //Get the TargetProperty type descriptor
            var dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromName(TargetPropertyProperty.Name, typeof(BindingProxy), target.GetType());
            if (dependencyPropertyDescriptor == null) return;

            //Get the binding expression for the target property
            var dependencyProperty = dependencyPropertyDescriptor.DependencyProperty;
            var bindingExpression = frameworkElement.GetBindingExpression(dependencyProperty);

            if (bindingExpression?.ResolvedSource == null || bindingExpression.ResolvedSourcePropertyName == null) return;

            //Get the source and source property bindings
            var source = GetSource(target);
            var sourceProperty = GetSourceProperty(target);

            if (source == null || sourceProperty == null) return;

            //Get the property info for the source property
            var propertyInfo = source.GetType().GetProperty(sourceProperty);
            if (propertyInfo == null) return;

            //Get the currently bound value from the target property
            var targetValue = frameworkElement.GetValue(bindingExpression.TargetProperty);
            var typedVal = targetValue;

            //If there is a converter, execute ConvertBack
            var converter = bindingExpression.ParentBinding.Converter;
            if (converter != null) typedVal = converter.ConvertBack(typedVal, propertyInfo.PropertyType, null, null);

            //Set the value via reflection on the Source object
            propertyInfo.SetValue(source, typedVal);
        }
    }
}
