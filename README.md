# quickstarts-dotnet
This code shows ADO.NET, XEP, Native, multi-model, and Entity Framework access. 
It is required for the .NET QuickStart which can be found here: 
[.NET QuickStart](https://gettingstarted.intersystems.com/language-quickstarts/net-quickstart/). 

## Contents

* `adoNETplaystocks.cs` to see how to store and retrieve data relationally
* `xepplaystocks.cs` to see how to quickly store objects
* `nativeplaystocks.cs` to see how to run methods within InterSystems IRIS
* `multiplay.cs` to see multi-model access using ADO.NET, XEP, and Native API
* `Program.cs` to see how to use Entity Framework, a third-party tool to work with objects

## Configuration files

* `config.txt`: contains connection details for **ADO.NET**, **XEP**, **Native API** and **multi-model**.
* `App.config`: located in EFPlay folder, contains connections details, parameters and initial settings for **Entity Framework**. 

## How to Run

1.  Verify you have an [<span class="urlformat">instance of InterSystems IRIS</span>](https://learning.intersystems.com/course/view.php?name=Get%20InterSystems%20IRIS), and an IDE that supports Node.js (such as **Visual Studio Code**). If you are using AWS, Azure, or GCP, that you have followed the steps to [change the password for InterSystems IRIS](https://docs.intersystems.com/irislatest/csp/docbook/DocBook.UI.Page.cls?KEY=ACLOUD#ACLOUD_interact).

2.  If you are using AWS, GCP, or Microsoft Azure, load the sample stock data into InterSystems IRIS:  
    `$ iris load http://github.com/intersystems/Samples-Stock-Data`  
    If you are using InterSystems Labs, the sample stock data is already loaded. You can skip to the next step.

3. Clone the repo and open it in your IDE.

4. With Microsoft Visual Studio:

    * Select **File** → **Open** → **Project/Solution**. Choose the `quickstarts-dotnet.sln` file you recently cloned. 
    * Select **View** → **Solution Explorer** to view project structure.
    * Right click on Solution. Select **Add** → **Existing item**. Choose `config.txt`.
    * Open `config.txt` file and modify the `IP` and `password` to be the correct values for your InterSystems IRIS instance. 
`Port` and `username` are most likely the defaults but you can verify those as well.
    * Due to its complexity, **Entity Framework** requires a few more steps to set up the IDE and configuration file. 
Please follow our instructions in section 6 - "Use Entity Framework, an example of a third-party API, to store objects to InterSystems IRIS", 
of [.NET QuickStart](https://gettingstarted.intersystems.com/language-quickstarts/net-quickstart/).

You should now have several classes for **ADO.NET**, **XEP**, **Native API**, **multi-model** and **Entity Framework**. 

Detailed instructions are included on the [.NET QuickStart page](https://gettingstarted.intersystems.com/language-quickstarts/net-quickstart/).
