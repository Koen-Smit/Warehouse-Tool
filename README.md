# Curio MBO Jaar 2, Stageperiode 1: Warehouse-Tool (2023)
- Stageperiode: (06-02-2023 / 07-07-2023)
- Stage-opdracht: voldaan
- Stack: (C#, JSON, Docker)

## Project Overview
Dit project betreft een console-gebaseerde tool die via de commandline gebruikt kan worden. De tool laadt JSON-configuratiebestanden en verstuurt deze naar een API, waarmee een test-warehouse wordt opgezet. Vervolgens kunnen aangepaste instellingen worden opgeslagen voor later gebruik. De JSON-bestanden zijn door mij gemaakt op basis van bestaande templates die ik heb ontvangen samen met de API.

**Belangrijk:** Deze applicatie is niet langer bruikbaar. Ik kreeg de opdracht om de API van dit bedrijf te gebruiken voor dit project, maar ik heb nu geen toegang meer. Wel heb ik alle gegevens van de voorbeeld-code vervangen door testdata die geen verband houdt met de oorspronkelijke data van het bedrijf. Alles is goed gedocumenteerd en de opdracht was vrijwel afgerond aan het einde van mijn stageperiode.

## Terugblik / Verbetering
- Terugkijkend op deze code, jaren later, zie ik honderden verbeterpunten. Een van de grootste verbeterpunten is objectgeoriënteerd programmeren (OOP); veel code staat nu in enkele bestanden in plaats van goed gestructureerde klassen. Daarnaast is de code soms slordig en kan het netter en beter leesbaar worden gemaakt. Ik kan me ook voorstellen dat de manier waarop ik dingen heb aangepakt veel efficiënter had gekund en dat sommige onderdelen dubbel voorkomen, **maar het werkte wel!**

- Hoewel ik de code zeker flink zou kunnen verbeteren, kies ik ervoor om dit project niet aan te passen. Dit was mijn **complete** stageproject en ik wil het behouden als een herinnering aan hoe ver ik ben gekomen in mijn **leerproces**. Het biedt me de mogelijkheid om terug te kijken en te zien hoe mijn vaardigheden zich ontwikkeld hebben, zonder de oorspronkelijke versie te veranderen.

### Functionaliteit:

- Warehouse-configuratie -> Instellen van testomgevingen met JSON-bestanden.
- Itembeheer -> Toevoegen, bewerken en opslaan van artikelen in een magazijnconfiguratie.
- Command-line interface -> Ondersteuning voor handmatige invoer en een menu-interface.

#### **Beschikbare commando's:**
De tool bevat verschillende commando's waarmee de gebruiker interactie heeft met de JSON-bestanden en API:

- help → Toont een lijst met beschikbare commando's.
- json.read → Leest een JSON-configuratiebestand in en stuurt het naar de API om een test-warehouse op te zetten.
- json.write → Slaat gewijzigde warehouse-instellingen op als een JSON-bestand.
- item.read → Leest een JSON-bestand met itemgegevens en vult het warehouse met testartikelen.
- item.write → Slaat de itemconfiguratie op als een JSON-bestand.

Wanneer een ongeldig commando wordt ingevoerd, wordt de gebruiker automatisch doorgestuurd naar de help-sectie.

---

## Vereisten/Ontwikkeling:
De API die in dit project is gebruikt, was specifiek ontwikkeld voor het stagebedrijf(En kan daardoor dit programma niet meer testen). Voor ontwikkeling en testen werd gebruikgemaakt van `Docker`.

- De applicatie begon als een menu-gebaseerde console-applicatie, maar later is een directe commandline-interface toegevoegd voor efficiënter gebruik na feedback van het team.

- Versiebeheer: Ontwikkeling gebeurde eerst met Bitbucket via Sourcetree, later is de code overgezet naar GitHub om al mijn projecten bij elkaar te houden.

- In dit project heb ik veel geleerd over C#, werken met SCRUM en waardevolle werkervaring opgedaan binnen een bedrijfsomgeving.

---

##  Voorbeeld code:
Hieronder enkele codefragmenten uit het project.

####  Iteraties berekenen voor labelwijzigingen:
```C#
int numIterations = 1;
for (int i = 3; i >= 0; i--)
{
    if (startParts[i] != endParts[i])
    {
        int startNum = int.Parse(startParts[i]);
        int endNum = int.Parse(endParts[i]);
        numIterations *= (endNum - startNum + 1);
    }
}
```

####  API-aanroep voor ophalen en verwijderen van zones:
```C#
var zoneResponse = await client.GetAsync("https://localhost:8080/zones");
zoneResponse.EnsureSuccessStatusCode();
var zoneContent = await zoneResponse.Content.ReadAsStringAsync();
var zoneDelete = JsonConvert.DeserializeObject<List<Locations.Zone>>(zoneContent);
foreach (var zone in zoneDelete)
{
    var deleteResponse = await client.DeleteAsync($"https://localhost:8080/zones/{zone.Id}");
    deleteResponse.EnsureSuccessStatusCode();
}
Console.WriteLine("Zones deleted");
```

#### Haal positieId op die hoort bij de locatie:
```C#
static async Task<string> RetrievePositionIdForLocation(HttpClient httpClient, string locationsUrl, string locationId)
{
    var url = $"{locationsUrl}/{locationId}";
    var response = await httpClient.GetAsync(url);
    response.EnsureSuccessStatusCode();
    var jsonData = await response.Content.ReadAsStringAsync();
    var data = JObject.Parse(jsonData);

    var positions = data["positions"] as JArray;
    if (positions != null && positions.Count > 0)
    {
        var position = positions[0];
        return position["id"].ToString();
    }

    return null;
}
```

#### JSON-bestand schrijven:
```C#
// 
if (locationsArray.HasValues)
{
    string jsonData = itemConfig.ToString();

    string folderPath = "ItemJsons";
    if (!Directory.Exists(folderPath))
        Directory.CreateDirectory(folderPath);

    string fileName = GetFileNameFromUser();
    if (string.IsNullOrEmpty(fileName))
    {
        Console.WriteLine("No file name entered");
        return;
    }

    string filePath = Path.Combine(folderPath, $"{fileName}.json");
    await File.WriteAllTextAsync(filePath, jsonData);
    Console.WriteLine($"Data written to {filePath}");
}
else
{
    Console.WriteLine("No locations with items found. Skipping JSON writing.");
}
```

#### Loop door alle carriertypes en schrijf het in een lijst:
```C#
var carrierTypes = new List<CarrierTypes.Root>();
foreach (var carriertype in JArray.Parse(getResponseString))
{
    var newCarriertype = new CarrierTypes.Root
    {
        Id = (string)carriertype["id"],
        Positions = new List<Position>(),
        Dimensions = new CarrierTypes.Dimensions
        {
            Length = (int)carriertype["dimensions"]["length"],
            Width = (int)carriertype["dimensions"]["width"],
            Height = (int)carriertype["dimensions"]["height"]
        },
        Limitations = new CarrierTypes.Limitations
        {
            MaxWeight = (int)carriertype["limitations"]["maxWeight"],
            MaxVolume = (int)carriertype["limitations"]["maxVolume"]
        },
    };
    carrierTypes.Add(newCarriertype);
}
```

#### Verwijder SSL error:
*Dit voorkomt een error die zorgt dat ik niet kon connecten met de API, nu weet ik hoe het komt maar toen had ik deze code toegevoegd als oplossing.*
```C#
var handler = new HttpClientHandler
{
ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
using var client = new HttpClient(handler);
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
```

---

## JSON Voorbeelden:
#### Voorbeeld van een item JSON (met NEP-testdata):
```json
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

#### Voorbeeld van een warehouse JSON (met NEP-testdata):
```json
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

---

## Beperkingen

- De applicatie kan niet functioneren zonder toegang tot de API.

- Ontwikkeld als testomgeving en niet bedoeld voor productief gebruik.

- Ik heb overwogen om een lokale databaseversie te implementeren voor verdere ontwikkeling, maar heb uiteindelijk besloten mijn focus op andere projecten te richten die grotere uitdagingen en prioriteit hadden. Daarnaast wil ik dit project, zoals eerder aangegeven, behouden zoals het was aan het einde van mijn stage, omdat het voor mij waardevol is om het als referentie te houden voor mijn voortgang en leerproces.

---
