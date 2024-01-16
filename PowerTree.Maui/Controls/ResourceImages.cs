using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Controls
{
    public class ResourceImage : Image
    {
        public static readonly BindableProperty ResourceProperty = BindableProperty.Create(nameof(Resource), typeof(string), typeof(string), null, BindingMode.OneWay, null, ResourceChanged);

        private static void ResourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var resourceString = (string)newvalue;
            var imageControl = (Image)bindable;

            imageControl.Source = ImageSource.FromFile(resourceString);
        }

        public string Resource
        {
            get => (string)GetValue(ResourceProperty);
            set => SetValue(ResourceProperty, value);
        }
    }
}
