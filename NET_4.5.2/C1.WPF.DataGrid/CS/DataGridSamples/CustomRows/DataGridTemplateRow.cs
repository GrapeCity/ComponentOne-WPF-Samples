using System.Windows;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using System.Reflection;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    [ContentProperty("RowTemplate")]
    public class DataGridTemplateRow : DataGridRow
    {
        protected override void OnLoaded()
        {
            base.OnLoaded();

            IsEditable = false;
            RowStyle = rowStyle;
        }

        protected override object GetRowPresenterRecyclingKey()
        {
            return typeof(DataGridTemplateRowPresenter);
        }

        protected override DataGridRowPresenter CreateRowPresenter()
        {
            return new DataGridTemplateRowPresenter() {  Content = RowTemplate.LoadContent() };
        }

        protected override void BindRowPresenter(DataGridRowPresenter presenter)
        {
            presenter.DataContext = DataItem;
        }

        protected override bool HasCellPresenter(DataGridColumn column)
        {
            return false;
        }

        public DataTemplate RowTemplate { get; set; }

        private static Style rowStyle = null;

        static DataGridTemplateRow()
        {
            string styleXaml = 
            @"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
             xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
             xmlns:local=""clr-namespace:DataGridSamples;assembly=" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + @""" 
             TargetType=""local:DataGridTemplateRowPresenter"">
            <Setter Property=""Template"">
                <Setter.Value>
                    <ControlTemplate TargetType=""local:DataGridTemplateRowPresenter"" >
                        <Grid x:Name=""Root"" Margin=""{TemplateBinding RowAreaMargin}"" Background=""{TemplateBinding Background}"">
                            <VisualStateManager.VisualStateGroups>
                                <VisualStateGroup x:Name=""CommonStates"">
                                    <VisualState x:Name=""Normal""/>
                                    <VisualState x:Name=""Disabled""/>
                                    <VisualState x:Name=""MouseOver"">
                                        <Storyboard>
                                            <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""MouseOverRectangle"" Storyboard.TargetProperty=""Opacity"">
                                                <SplineDoubleKeyFrame KeyTime=""0"" Value=""1""/>
                                            </DoubleAnimationUsingKeyFrames>
                                        </Storyboard>
                                    </VisualState>
                                </VisualStateGroup>
                                <VisualStateGroup x:Name=""SelectionStates"">
                                    <VisualState x:Name=""Selected"">
                                        <Storyboard>
                                            <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SelectedRectangle"" Storyboard.TargetProperty=""Opacity"">
                                                <SplineDoubleKeyFrame KeyTime=""0"" Value=""1""/>
                                            </DoubleAnimationUsingKeyFrames>
                                        </Storyboard>
                                    </VisualState>
                                    <VisualState x:Name=""Editing""/>
                                    <VisualState x:Name=""Unselected""/>
                                </VisualStateGroup>
                            </VisualStateManager.VisualStateGroups>
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""*"" />
                                <RowDefinition Height=""Auto"" />
                            </Grid.RowDefinitions>
                            <Border x:Name=""MouseOverRectangle"" Background=""{TemplateBinding MouseOverBrush}"" Opacity=""0"" />
                            <Border x:Name=""SelectedRectangle"" Background=""{TemplateBinding SelectedBackground}""  Opacity=""0"" />
                            <ContentPresenter Content=""{TemplateBinding Content}"" />
                            <Rectangle x:Name=""HorizontalGridLine"" Grid.Row=""2"" Height=""1"" Fill=""{TemplateBinding HorizontalGridLineBrush}"" Visibility=""{TemplateBinding HorizontalGridLineVisibility}""/>
                        </Grid>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>";
            rowStyle = (Style)XamlReader.Load(XmlReader.Create(new StringReader(styleXaml)));
        }
    }
}
