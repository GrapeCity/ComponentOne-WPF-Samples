C1BindingDemo
----------------------------------------------------------------------------------------
Shows how to use C1Binding to implement rich binding without converter classes.

The sample binds a TextBlock to a customer's name and uses two C1Bindings with
conditional expressions to show the text in bold and blue if the customer is
active, and as normal and red if the customer is not active:

<code>
	<TextBlock 
		Text="{c1:C1Binding Name}" 
        FontWeight="{c1:C1Binding 'if(Active, |Bold|, |Normal|)'}"
        Foreground="{c1:C1Binding 'if(Active, |Blue|, |Red|)'}" 
	/>
</code>

Notice that in order to accomplish this type of binding without C1Binding objects,
you would need to create two converter classes in code (C# or VB), then instantiate
the converters in the XAML file and specify them in the bindings. Without C1Binding, 
the XAML would look like this:

<code>
	<App.Resources>
		<!-- instantiate converters (implemented in C# elsewhere)
		<local:MyFontWeightConverter x:Key="_cvtFontWeight" />
		<local:MyForegroundConverter x:Key="_cvtForegound" />
	</App.Resources>

	<TextBlock 
		Text="{Binding Name}" 
        FontWeight="{Binding Active, Converter={StaticResource _cvtFontWeight}}"
        Foreground="{Binding Active, Converter={StaticResource _cvtForegound}}"
	/>
</code>

