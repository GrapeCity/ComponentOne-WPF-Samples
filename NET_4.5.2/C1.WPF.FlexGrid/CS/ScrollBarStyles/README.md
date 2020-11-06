## Scrollbar Styles
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.FlexGrid\CS\ScrollBarStyles)
____
#### Shows how you can customize the appearance of the scrollbars within a C1FlexGrid.
____
The C1FlexGrid uses standard scrollbar controls in its layout, and relies on the
styling mechanism provided by Silverlight and WPF. This ensures that the grid 
scrollbars have an appearance that is consistent with the rest of your application.

For example, if you apply a theme to a  in your application, the scrollbars
in the C1FlexGrid will automatically honor the theme, and their appearance will
be consistent with the rest of the app.

If you want to apply a custom style to the scrollbars in a C1FlexGrid, you have
to create the Style and add it to the grid's resources. For example:

```
   <c1:C1FlexGrid.Resources>

                <Style TargetType="ScrollBar">
                    <Setter Property="MinWidth" Value="17"/>
                    <Setter Property="MinHeight" Value="17"/>
                    <Setter Property="IsTabStop" Value="False"/>
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate TargetType="ScrollBar">
                                <Grid x:Name="Root" Background="{TemplateBinding Background}" >
                                    <!-- Vertical Template -->
                                    <Grid x:Name="VerticalRoot" Background="#10000000" >
                                        <Grid.RowDefinitions>
                                            <!-- ************ 3 rows instead of 5 ************-->
                                            <RowDefinition MaxHeight="{DynamicResource {x:Static SystemParameters.VerticalScrollBarButtonHeightKey}}"/>
                                            <RowDefinition Height="0.00001*"/>
                                            <RowDefinition MaxHeight="{DynamicResource {x:Static SystemParameters.VerticalScrollBarButtonHeightKey}}"/>
                                        </Grid.RowDefinitions>
                                        <RepeatButton x:Name="VerticalSmallDecrease" Foreground="{TemplateBinding Foreground}" Grid.Row="0" Height="16" IsTabStop="False" Interval="50" Template="{StaticResource VerticalDecrementTemplate}" Margin="1" />
                                        <!-- ************ Thumb inside Track element called "PART_Track" ************-->
                                        <Track x:Name="PART_Track" IsDirectionReversed="true" IsEnabled="{TemplateBinding IsMouseOver}" Grid.Row="1">
                                            <Track.DecreaseRepeatButton>
                                                <RepeatButton Command="{x:Static ScrollBar.PageUpCommand}" Style="{StaticResource VerticalScrollBarPageButton}"/>
                                            </Track.DecreaseRepeatButton>
                                            <Track.IncreaseRepeatButton>
                                                <RepeatButton Command="{x:Static ScrollBar.PageDownCommand}" Style="{StaticResource VerticalScrollBarPageButton}"/>
                                            </Track.IncreaseRepeatButton>
                                            <Track.Thumb>
                                                <Thumb x:Name="VerticalThumb" Background="{TemplateBinding Background}" MinHeight="18" Height="18" Grid.Row="1" Template="{StaticResource VerticalThumbTemplate}" />
                                            </Track.Thumb>
                                        </Track>
                                        <RepeatButton x:Name="VerticalSmallIncrease" Foreground="{TemplateBinding Foreground}" Grid.Row="2" Height="16" IsTabStop="False" Interval="50" Template="{StaticResource VerticalIncrementTemplate}" Margin="1" />
                                    </Grid>
                                </Grid>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                    <Style.Triggers>
                        <!-- ************ Horizontal Orientation defined as a Trigger" ************-->
                        <Trigger Property="Orientation" Value="Horizontal">
                            <Setter Property="Template">
                                <Setter.Value>
                                    <ControlTemplate TargetType="{x:Type ScrollBar}">
                                        <!-- Horizontal Template -->
                                        <Grid x:Name="HorizontalRoot" Background="Silver" >
                                            <Grid.ColumnDefinitions>
                                                <!-- ************ 3 columns instead of 5 ************-->
                                                <ColumnDefinition MaxWidth="{DynamicResource {x:Static SystemParameters.HorizontalScrollBarButtonWidthKey}}"/>
                                                <ColumnDefinition Width="0.00001*"/>
                                                <ColumnDefinition MaxWidth="{DynamicResource {x:Static SystemParameters.HorizontalScrollBarButtonWidthKey}}"/>
                                            </Grid.ColumnDefinitions>
                                            <RepeatButton x:Name="HorizontalSmallDecrease" Foreground="{TemplateBinding Foreground}" Grid.Column="0" Width="16" IsTabStop="False" Interval="50" Template="{StaticResource HorizontalDecrementTemplate}" Margin="1" />
                                            <!-- ************ Thumb inside Track element called "PART_Track" ************-->
                                            <Track x:Name="PART_Track" Grid.Column="1" IsEnabled="{TemplateBinding IsMouseOver}">
                                                <Track.DecreaseRepeatButton>
                                                    <RepeatButton Command="{x:Static ScrollBar.PageLeftCommand}" Style="{StaticResource HorizontalScrollBarPageButton}"/>
                                                </Track.DecreaseRepeatButton>
                                                <Track.IncreaseRepeatButton>
                                                    <RepeatButton Command="{x:Static ScrollBar.PageRightCommand}" Style="{StaticResource HorizontalScrollBarPageButton}"/>
                                                </Track.IncreaseRepeatButton>
                                                <Track.Thumb>
                                                    <Thumb x:Name="HorizontalThumb" Background="{TemplateBinding Background}" MinWidth="18" Width="100" HorizontalAlignment="Left" Grid.Column="0" Grid.ColumnSpan="3" Template="{StaticResource HorizontalThumbTemplate}" />
                                                </Track.Thumb>
                                            </Track>
                                            <RepeatButton x:Name="HorizontalSmallIncrease" Foreground="{TemplateBinding Foreground}" Grid.Column="2" Width="16" IsTabStop="False" Interval="50" Template="{StaticResource HorizontalIncrementTemplate}" Margin="1" />
                                        </Grid>
                                    </ControlTemplate>
                                </Setter.Value>
                            </Setter>
                        </Trigger>
                    </Style.Triggers>
                </Style>

            </c1:C1FlexGrid.Resources>
```
