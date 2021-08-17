Imports DevExpress.Xpf.Core.Native
Imports System
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Markup

Namespace SpecifySVGImagesManually
	Partial Public Class MainWindow
		Public Sub New()
			InitializeComponent()
			DataContext = New ObservableCollection(Of String)() From {"Images/First.svg", "Images/Last.svg"}
		End Sub
	End Class
	Public Class SvgImageSourceConverterExtension
		Inherits MarkupExtension
		Implements IValueConverter

		Private ReadOnly baseUri As Uri
		Private ReadOnly uriConverter As UriTypeConverter

		Public Sub New()
			Me.New(Nothing)
		End Sub
		Public Sub New(ByVal baseUri As Uri)
			Me.baseUri = baseUri
			uriConverter = New UriTypeConverter()
		End Sub

		Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
			Return New SvgImageSourceConverterExtension((TryCast(serviceProvider.GetService(GetType(IUriContext)), IUriContext)).BaseUri)
		End Function

		Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
			Dim uri As Uri = TryCast(uriConverter.ConvertFrom(value), Uri)
			If uri Is Nothing Then
				Return Nothing
			End If
			Dim absoluteUri = If(uri.IsAbsoluteUri, uri, New Uri(baseUri, uri))
			Return WpfSvgRenderer.CreateImageSource(absoluteUri)
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace
