# WS.Finances.Core
A .Net Core application to track spending

## Projects
<dl>
  <dt>WS.Finances.Core.Lib</dt>
  <dd>Common library code</dd>
  <dt>WS.Finances.Core.Console</dt>
  <dd>Command Line Version of the app. All functionality is available in the command line application. I developed this application first so I could focus on the functionality and not get bogged down with the user interface.</dd>  <dt>WS.Finances.Core.Web</dt>
  <dd>Web Version of the app. All functionality is available in the web application.</dd>
</dl>

## Data Storage
All data is stored in JSON files in the configured data directory. It makes sense to configure the same directory for both the commmand line app and the web app. This data storage mechanism was chosen to avoid having to install configure and run a database of some sort for ultra-portability.

## Configuration

### Configuration Files
Both the command line and web app have an appsettings.json file. Change the `AppSettings.BaseDirectory` setting to point to the directory you want to hold the data files.

### Required Data Files

#### Accounts.json
You will need to have an `Accounts.json` file in your `AppSettings.BaseDirectory` folder with the following format:

    [
      {
        "Name": "Bank of Novi Pazar Current Account",
        "StartRow": 3,
        "TimestampColumn": 0,
        "TimestampFormat": "dd/MM/yyyy",
        "DescriptionColumn": 1,
        "MoneyInColumn": null,
        "MoneyOutColumn": 2,
        "TotalColumn": null
      },
      {
        "Name": "Bank of Novi Pazar Credit Card",
        "StartRow": 5,
        "TimestampColumn": 1,
        "TimestampFormat": "d/M/yyyy",
        "DescriptionColumn": 2,
        "MoneyInColumn": 7,
        "MoneyOutColumn": 8,
        "TotalColumn": 5
      }
    ]

All indexes are 0-based, so the first row or column would be `0`, the second `1`, etc.

#### Map.json
You will also need a `Map.json` file in your `AppSettings.BaseDirectory` folder. This file provides the categories that you will be able to map transactions to in the two apps and also allows the specification of regular expressions that are used to automatically map transactions, making the import procedure much faster. It uses the following format:

    [
      {
        "Section": "Other Outgoings",
        "Category": "Supermarket",
        "Patterns": [
          "WM MORRISONS STORE",
          "ASDA (?!F).*",
          "S(AINSBURY'?S|ainsbury'?s)( .*)?",
          "TESCO STORES?.*"
        ],
        "Position": 2
      },
      {
        "Section": "Other Outgoings",
        "Category": "Eating Out",
        "Patterns": [
          "TABLE TABLE\\d*",
          "PREMIER INN\\d*",
          "WAGAMAMA.*"
        ],
        "Position": 5
      },
      {
        "Section": "Other Outgoings",
        "Category": "House and Decorating",
        "Patterns": [
          "IKEA",
          "ARGOS LTD",
          "B & Q"
        ],
        "Position": 10
      },
      {
        "Section": "Income",
        "Category": "Salary",
        "Patterns": [],
        "Position": 1
      }
    ]

## Building and Running the Application

### WS.Finances.Core.Console
To build the command line app navigate to the `WS.Finances.Core.Console` directory and issue the following command:

    dotnet build

To run the application issue the following command:

    dotnet run [-- <command> [parameters]]

To list the possible commands issue the following command:

    dotnet run

To run a parameterless command or to list the parameters of a parameterized command issue the following command:

    dotnet run -- <command>

Eg.

    dotnet run -- breakdown

To run a parameterized command issue the following command:

    dotnet run -- <command> <parameters>

Eg.

    dotnet run -- breakdown -y=2018 -m=June -c=Fun

### WS.Finances.Core.Web
To build the web app navigate to the `WS.Finances.Core.Web` directory and issue the following command:

    dotnet build

To run the application issue the following command:

    dotnet run

The console will report which URL the web app will be listening to.

## Future Developments
I hope to add the ability to create and edit the `Accounts.json` and the `Map.json` configuration files from within the application