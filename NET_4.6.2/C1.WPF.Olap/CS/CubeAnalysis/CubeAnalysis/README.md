## CubeAnalysis
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.Olap/CS/CubeAnalysis/CubeAnalysis)
____
#### Shows how to use C1Olap to analyze Cube data.
____
Basic steps to setup SSAS:

1) Install full version of SQL server.
2) Download Adventure works data base.
http://msftdbprodsamples.codeplex.com/releases
3) Install the Adventure works data base in SQL server.

To connect with a cube, you should use the ConnectCube method. This method accepts two parameters: 
the connection string to a SQL Server with Analysis Services installed, and the name of the cube.

For example:
```
	string connectionString = @"Data Source=myServerAddress;Provider=msolap;Initial Catalog=myDataBase";
        string cubeName = "Adventure Works";
	_c1OlapPage.ConnectCube(cubeName, connectionString);

        try
        {
            _c1OlapPage.OlapPanel.ConnectCube(cubeName, connectionString);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
```
If you have more than one Microsoft OLE DB providers for OLAP installed you may need to specify the version of the provider in the connection string. 
For example, setting the Provider to MSOLAP will use the latest version of OLE DB for OLAP installed on your system.

For example:
Provider=MSOLAP;Data Source=myServerAddress;Initial Catalog=myDataBase;
