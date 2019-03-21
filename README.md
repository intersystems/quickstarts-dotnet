# quickstarts-dotnet
This code shows ADO.NET, XEP, Native, and multi-model access in .NET Core. 
It is required for the .NET QuickStart which can be found here: 
[.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS). 

## Contents

* `adoNETplaystocks.cs` to see how to store and retrieve data relationally
* `xepplaystocks.cs` to see how to quickly store objects
* `nativeplaystocks.cs` to see how to run methods within InterSystems IRIS
* `multiplay.cs` to see multi-model access using ADO.NET, XEP, and Native API

## Configuration files

* `connections.config`: contains connection details for **ADO.NET**, **XEP**, **Native API** and **multi-model**.

## How to Run

1.  Verify you have an [<span class="urlformat">instance of InterSystems IRIS</span>](https://learning.intersystems.com/course/view.php?name=Get%20InterSystems%20IRIS), and an IDE that supports Node.js (such as **Visual Studio Code**). If you are using AWS, Azure, or GCP, that you have followed the steps to [change the password for InterSystems IRIS](https://docs.intersystems.com/irislatest/csp/docbook/DocBook.UI.Page.cls?KEY=ACLOUD#ACLOUD_interact).

2.  If you are using AWS, GCP, or Microsoft Azure, load the sample stock data into InterSystems IRIS:  
    `$ iris load http://github.com/intersystems/Samples-Stock-Data`  
    If you are using InterSystems Labs, the sample stock data is already loaded. You can skip to the next step.

3. Clone the repo and open it in your IDE.

4. In your IDE:

    * Open `config.txt` file and modify the `IP` and `password` to be the correct values for your InterSystems IRIS instance. 
`Port` and `username` are most likely the defaults but you can verify those as well.

5. In the terminal of your IDE:

    * Navigate to ADO.NET code directory: `cd ADO`
    * Run ADO sample code: `dotnet net`
    
    XEP, the Native API, and muti-model code work similarly.  
