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
For version control I used Bitbucket through Sourcetree, later on with the project I put everything on Github.

## Documentation:
#### Commands
There are multiple commands or menu-options that can be used, I put in a help command to let the user know what commands there are and what options you have so it is more accessible for people. 
All commands are: `help`, `json.read`, `json.write`, `item.read`, `item.write`.
*(There are checks so the user is send to the `help` command when a wrong string is entered.)*
You can choose to use the menu, there it is just some switch cases that lets the user choose which option they want to use.

### Options
##### - "help"
Gives the user a list of commands they can use, they are listed underneath here.
##### - "json.read"
Lets the user read out a JSON file, it asks which JSON it wants to read and then puts it inside the API, there it creates a testing environment for the programmers working on the warehouse software, so its easy and quick to test the thing they make.
It is used because every warehouse is a little different but has the same setup, so with settings it can be set up for testing.
##### - "json.write"
This lets the user write the created JSON that they maybe changed/edited so it can later be used to test another time, so you can switch between warehouses and back up the warehouse for later.
When a JSON is written it save it at `bin\Debug\net6.0\Jsons`, here all JSON files are stored.
##### - "item.read"
Lets the user read out a JSON file, this time for items, a warehouse is created but when used for testing it is sometimes filled with items, items can vary per warehouse, but the setup stays the same.
So it is kinda used as a second setting file, when read it fills the already created warehouse with items, when a warehouse has yet to be created it will tell the user this so the warehouse is set up first.
##### - "item.write"
This lets the user write the created JSON for the item list they made or edited so it can later be used to test another time, so you can back up the items stored in that warehouse and use it for later or put it inside another warehouse.
When a JSON is written it save it at `bin\Debug\net6.0\ItemJsons`, here all JSON files are stored.

***

## Code snippets:

### Code:
#### A snippet that removes an SSL error:
*(Because of some circumstances inside the code and API caused the API to not work correctly or completely not, so i put in a bit of "unsafe" code to remove the SSL check alltogether.)*
```
var handler = new HttpClientHandler
{
ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
using var client = new HttpClient(handler);
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
```

### Models:
#### Example of some classes, picked from multiple files:
*(It contains some different types of classes containing, type, limitations and zones. just examples from different files.)*
```
        public class Type
        {
            public string Id { get; set; }
            public string LabelPattern { get; set; }
            public Dimensions Dimensions { get; set; }
            public Limitations Limitations { get; set; }
            public List<string> PositionLabels { get; set; }
        }

        public class Limitations
        {
            [JsonProperty("maxWeight")]
            public int MaxWeight { get; set; }
            [JsonProperty("maxVolume")]
            public int MaxVolume { get; set; }
        }

        public class Zones
        {
            public string? Id { get; set; }
            public Identifiers? Identifiers { get; set; }
            public List<LocationRange> Locations { get; set; }
        }
```

### Jsons:
#### example of item JSON, filled with **FAKE** test-data:
*(itemconfig, the locations are for what label the location has, items are for what articles and how much are in this specific location, the ID is for API purposes.)*
```
{
    "itemConfig": {
        "locations": [
            {
                "id": "040070b7-AferVas-4d11-9ece-cb6asda5315c97",
                "label": "B2-01-01-03",
                "items": [
                    {
                        "articleid": "ART001",
                        "amount": 5
                    },
                    {
                        "articleid": "ART003",
                        "amount": 7
                    }
                ]
            },
            {
                "id": "7920ad90-2b92-4f31-9768-AferVased64",
                "label": "B1-01-02-09",
                "items": [
                    {
                        "articleid": "ART002",
                        "amount": 20
                    }
                ]
            }
        ]
    }
}
```

#### example of a warehouse JSON, filled with **FAKE** test-data:
*(It configures: Carriertypes, Locationtypes, Articletypes, the layout and it has content options.)*
```
{
    "config": {
        "carrierTypes": [
            {
                "id": "CarrierTestType1",
                "dimensions": {
                    "length": 100,
                    "width": 200,
                    "height": 10
                },
                "limitations": {
                    "maxWeight": 150,
                    "maxVolume": 20
                },
                "positions": []
            }
        ],
        "locationTypes": [
            {
                "id": "consolidationTest",
                "dimensions": {
                    "length": 10,
                    "width": 1000,
                    "height": 10
                },
                "limitations": {
                    "maxWeight": 500,
                    "maxVolume": 10000
                }
            }
        ],
        "articles": [
            {
                "id": "ART001",
                "identifiers": {
                    "label": "100001010"
                },
                "description": "SmallTestArticle1",
                "dimensions": {
                    "length": 5,
                    "width": 50,
                    "height": 10,
                    "weight": 50,
                    "volume": 3
                }
            }
        ]
    },
    "layout": {
        "zones": [
            {
                "label": "BulkTestZone_1",
                "locations": [
                    {
                        "Range": {
                            "start": {
                                "label": "B1-01-01-01",
                                "type": "vak_medium"
                            },
                            "end": {
                                "label": "B1-02-03-10",
                                "type": "vak_medium"
                            }
                        }
                    }
                ]
            }
        ]
    },
    "content": {
        "carriers": {},
        "locationContent": {},
        "carrierContent": {}
    }
}
```

