LabResults
--------------------------------------------------------------------------------------
Shows how you can build grouped, filtered views of your data in an MVVM pattern.

The sample creates a ViewModel class that contains a list of test results. The results
are filtered by patient and grouped by date, and shown on a C1FlexGrid.

When the user selects a patient on the combo box, the list of test results is
automatically filtered and the results are updated on the grid below.