# quickstarts-dotnet
This code shows ADO.NET, XEP, Native, multi-model, and Entity Framework access. 
It is required for the .NET QuickStart which can be found here: 
[.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS). 
You should use this QuickStart to run this sample code.

## Contents

### Code files

* `adoNETplaystocks.cs` to see how to store and retrieve data relationally
* `xepplaystocks.cs` to see how to quickly store objects
* `nativeplaystocks.cs` to see how to run methods within InterSystems IRIS
* `multiplay.cs` to see multi-model access using ADO.NET, XEP, and Native API
* `Program.cs` to see how to use Entity Framework, a third-party tool to work with objects

### Configuration files

* `config.txt`: contains connection details for **ADO.NET**, **XEP**, **Native API** and **multi-model**.
* `App.config`: located in EFPlay folder, contains connections details, parameters and initial settings for **Entity Framework**. 

## How to Run

To run this code:

1. Visit [Direct Access to InterSystems IRIS](https://learning.intersystems.com/course/view.php?name=Java%20Build), 
[Microsoft Azure](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/intersystems.intersystems-iris-single-node) or 
[Google Cloud Platform](https://console.cloud.google.com/marketplace/details/intersystems-launcher/intersystems-iris-community) 
marketplaces to get InterSystems IRIS instance.

2. If you use [Microsoft Azure](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/intersystems.intersystems-iris-single-node) or 
[Google Cloud Platform](https://console.cloud.google.com/marketplace/details/intersystems-launcher/intersystems-iris-community), 
you need to [load data into your instance](https://github.com/intersystems/Samples-Stock-Data). 

3. Clone the repo

4. With Microsoft Visual Studio:

* Select **File** → **Open** → **Project/Solution**. Choose the `quickstarts-dotnet.sln` file you recently cloned. 
* Select **View** → **Solution Explorer** to view project structure.
* Right click on Solution. Select **Add** → **Existing item**. Choose `config.txt`.
* Open `config.txt` file and modify the `IP` and `password` to be the correct values for your InterSystems IRIS instance. 
`Port` and `username` are most likely the defaults but you can verify those as well.
* Due to its complexity, **Entity Framework** requires a few more steps to set up the IDE and configuration file. 
Please follow our instructions in section 6 - "Use Entity Framework, an example of a third-party API, to store objects to InterSystems IRIS", 
of [.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS).

You should now have several classes for **ADO.NET**, **XEP**, **Native API**, **multi-model** and **Entity Framework**, 

Detailed instructions are included on the [.NET QuickStart page](https://learning.intersystems.com/course/view.php?name=.NET%20QS).
