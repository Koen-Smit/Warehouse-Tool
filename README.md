# Warehouse-Tool
Curio Internship: 6 februari t/m 7 juli.

***
## Usage:
*(Not useable anymore, no access to API etc.)*
Console-based tool that can be used through the commandline, it can load a JSON setting file that sends the data to the API, 
then sets up a test-warehouse and can be used to save alternate settings that were changed later on. 
I made the JSON setting files myself from the already existent templates I recieved.

## Development:
The API used here was specifically created for the company where my intership was. I used Docker to develop and test the API/app.
The development started as a menu type of console app but later on i added the option to use it as a commandline so its faster and more efficient to use.

## Documentation:
#### Commands
There are multiple commands or menu-options that can be used, I put in a help command to let the user know what commands there are and what options you have so it is more accessible for people. 
All commands are: `help`, `json.read`, `json.write`, `item.read`, `item.write`.
*(There are checks so the user is send to the `help` command when a wrong string is entered.)*
You can choose to use the menu, there it is just some switch cases that lets the user choose which option they want to use.

#### Options
#####`help`
#####`json.read`
#####`json.write`
#####`item.read`
#####`item.write`

