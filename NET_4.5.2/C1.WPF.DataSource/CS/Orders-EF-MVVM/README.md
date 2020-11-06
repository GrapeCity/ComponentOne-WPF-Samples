## Orders-EF-MVVM
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.DataSource/CS/Orders-EF-MVVM)
____
#### Orders demo application with Entity Framework and the MVVM pattern
____
This demo shows how following MVVM pattern (Model-View-ViewModel) becomes
easy using C1DataSource.

View models can be created as live views, which means with little
or no code except for simple LINQ statements. It contrasts with the
usual practice of creating view models with lots of code building
them and even more code synchronizing them with model data when
either of them is modified.  

Live views are synchronized with their sources automatically, there is
no need in change notifications and special synchronization code in view
models. All you need is to define live views that shape the source data
in the way you need them to be shown in the view.

This sample is based on the code from the well-known article by Josh Smith,
one of the authors of MVVM, "WPF Apps With The Model-View-ViewModel
Design Pattern" (http://msdn.microsoft.com/en-us/magazine/dd419663.aspx). 

Essentially all the files are the same as the originals (bar a few
cosmetic changes) except one: ViewModels\OrdersViewModel.cs.
In this file, we build the view model class using live views.
You can see how many re-shaping functions are applied to model data to
construct a view model, all done exclusively through LINQ. This made
it easy and required little code. The best part is that it synchronizes
automatically with model data when data in either of the two layers
is changed - no synchronization code was necessary. 

The fact that we only changed the way that the view model classes
themselves were created (they are still derived from the original base
class 'ViewModelBase') and made no other changes to the framework code
that Josh Smith had employed in his original sample should serve as
an example that this approach is entirely compatible with other frameworks.
You can continue to use your preferred frameworks when working with
MVVM, but now you have an additional tool to make your MVVM development
even easier.

