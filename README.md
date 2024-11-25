# quickstarts-dotnet
This sample code shows how to connect a .NET application to an InterSystems server using your choice of ADO.NET, XEP, the InterSystems IRIS Native API for .NET.

To use this sample code, deployed for you in a sandbox environment, visit the [.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS). 

To deploy on your own system, follow the steps below.

## Repository Contents

* `ADO/` demonstrates how to store and retrieve data using syntax of a relational database. 
* `XEP/` demonstrates how to quickly store objects in a scalable manner.
* `NativeAPI/` shows how to run methods within InterSystems IRIS.
* `MultiModel/` shows multi-model access using ADO.NET, XEP, and Native API.
* `ZSampleCodeAndData/` contains sample class definitions and data for use with the code samples.

## How to Run

1. Verify you have an [<span class="urlformat">instance of InterSystems IRIS</span>](https://learning.intersystems.com/course/view.php?name=Get%20InterSystems%20IRIS), and an IDE that supports .NET (such as **Microsoft Visual Studio**). 

3. Clone this repo.

3. Load the sample stock data into InterSystems IRIS from `ZSampleCodeAndData`. 

4. Update `config.txt`, which contains connection details for your instance. Update the server address details, user credentials, and the name of the namespace where you loaded the sample data.

5. Get the required drivers from the installation directory of your InterSystems instance and put them in the packages folder of this cloned repo, following [the instructions in documentation](https://docs.intersystems.com/components/csp/docbook/DocBook.UI.Page.cls?KEY=PAGE_extconnex#PAGE_extconnex_dotnet).

6. Run the sample code and follow along with the prompts.