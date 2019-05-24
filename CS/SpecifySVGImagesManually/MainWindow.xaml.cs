using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SpecifySVGImagesManually {    
    public partial class MainWindow{
        public MainWindow() {
            InitializeComponent();
            DataContext = new List<String>() { "Images/First.svg", "Images/Last.svg" };
        }
    }
    public class SvgImageSourceConverterExtension : MarkupExtension, IValueConverter {
        class UriStreamHelper : SvgImageSourceExtension {
            public static Stream GetStream(Uri uri) {
                return Application.GetResourceStream(uri).Stream;
            }
        }

        readonly Uri baseUri;
        readonly UriTypeConverter uriConverter;
        
        public SvgImageSourceConverterExtension() : this(null) { }
        public SvgImageSourceConverterExtension(Uri baseUri) { 
            this.baseUri = baseUri;
            uriConverter = new UriTypeConverter();
        }
        
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return new SvgImageSourceConverterExtension((serviceProvider.GetService(typeof(IUriContext)) as IUriContext).BaseUri);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var uri = uriConverter.ConvertFrom(value) as Uri;
            if (uri == null)
                return null;
            var absoluteUri = uri.IsAbsoluteUri ? uri : new Uri(baseUri, uri);
            using(var stream = UriStreamHelper.GetStream(absoluteUri)) {
                var image = DevExpress.Utils.Svg.SvgLoader.LoadFromStream(stream);
                return WpfSvgRenderer.CreateImageSource(image, 1d, null, null, true);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
