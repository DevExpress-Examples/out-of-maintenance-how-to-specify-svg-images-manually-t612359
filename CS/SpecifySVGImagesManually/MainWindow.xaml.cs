using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SpecifySVGImagesManually
{
    public partial class MainWindow{
        public MainWindow() {
            InitializeComponent();
            DataContext = new ObservableCollection<String>() { "Images/First.svg", "Images/Last.svg" };
        }
    }
    public class SvgImageSourceConverterExtension : MarkupExtension, IValueConverter {
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
            return WpfSvgRenderer.CreateImageSource(absoluteUri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}