using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WMT.Classes;
using static WMT.Classes.CarrierTypes;
using static WMT.Classes.Items;

namespace WMT.Commands
{
    internal class Json
    {

        public static async Task Read()
        {
            // removes SSL error
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Do you wish to proceed to permanently deleting the warehouse and load a new one? (y/n)");
            var confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {


                // Get/delete zones from API
                var zoneResponse = await client.GetAsync("https://localhost:2902/zones");
                zoneResponse.EnsureSuccessStatusCode();
                var zoneContent = await zoneResponse.Content.ReadAsStringAsync();
                var zoneDelete = JsonConvert.DeserializeObject<List<Locations.Zone>>(zoneContent);
                foreach (var zone in zoneDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/zones/{zone.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Zones deleted");


                // Get/delete items from api
                var itemResponse = await client.GetAsync("https://localhost:2902/items");
                itemResponse.EnsureSuccessStatusCode();
                var itemContent = await itemResponse.Content.ReadAsStringAsync();
                var itemDelete = JsonConvert.DeserializeObject<List<Items.Root>>(itemContent);
                foreach (var item in itemDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/items/{item.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Items deleted");

                // Get/delete locations from API 
                var locationResponse = await client.GetAsync("https://localhost:2902/locations");
                locationResponse.EnsureSuccessStatusCode();
                var locationContent = await locationResponse.Content.ReadAsStringAsync();
                var locationDelete = JsonConvert.DeserializeObject<List<Locations.Root>>(locationContent);
                foreach (var location in locationDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/locations/{location.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Locations deleted");


                // Get/delete carriers from api
                var carrierResponse = await client.GetAsync("https://localhost:2902/carriers");
                carrierResponse.EnsureSuccessStatusCode();
                var carrierContent = await carrierResponse.Content.ReadAsStringAsync();
                var carrierDelete = JsonConvert.DeserializeObject<List<Carriers.Root>>(carrierContent);
                foreach (var carrier in carrierDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/carriers/{carrier.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Carriers deleted");

                // Get/delete articles from API
                var articleResponse = await client.GetAsync("https://localhost:2902/articles");
                articleResponse.EnsureSuccessStatusCode();
                var articleContent = await articleResponse.Content.ReadAsStringAsync();
                var articleDelete = JsonConvert.DeserializeObject<List<Articles.Root>>(articleContent);
                foreach (var article in articleDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/articles/{article.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Articles deleted");

                // Get/delete carriertypes from api
                var carriertypeResponse = await client.GetAsync("https://localhost:2902/carriertypes");
                carriertypeResponse.EnsureSuccessStatusCode();
                var carriertypeContent = await carriertypeResponse.Content.ReadAsStringAsync();
                var carriertypeDelete = JsonConvert.DeserializeObject<List<CarrierTypes.Root>>(carriertypeContent);
                foreach (var carriertype in carriertypeDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/carriertypes/{carriertype.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Carriertypes deleted");

                // get/delete locationtypes from api
                var locationtypeResponse = await client.GetAsync("https://localhost:2902/locationtypes");
                locationtypeResponse.EnsureSuccessStatusCode();
                var locationtypeContent = await locationtypeResponse.Content.ReadAsStringAsync();
                var locationtypeDelete = JsonConvert.DeserializeObject<List<LocationTypes.Root>>(locationtypeContent);
                foreach (var locationtype in locationtypeDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/locationtypes/{locationtype.id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Locationtypes deleted");
            }
            else
            {
                Console.WriteLine("Operation cancelled");
                return;
            }

            // Reads a json file chosen from the folder and displays its contents in the console
            string folderPath = @"Jsons\";

            // Get all files in the specified folder
            string[] files = Directory.GetFiles(folderPath);

            // List all files with index numbers
            Console.WriteLine($"Found {files.Length} files in folder '{folderPath}'");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
            }

            // Ask for number input to choose which file to read
            Console.Write("Enter the number of the file to read: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int fileNumber) || fileNumber < 1 || fileNumber > files.Length)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            string chosenFile = files[fileNumber - 1];
            JObject json = JObject.Parse(File.ReadAllText(chosenFile));

            // Loop through the articles and post each article to the API
            JArray articles = (JArray)json["config"]["articles"];
            foreach (JObject article in articles)
            {
                var payload = new
                {
                    id = (string)article["id"],
                    identifiers = new
                    {
                        label = (string)article["identifiers"]["label"]
                    },
                    description = (string)article["description"],
                    dimensions = new
                    {
                        length = (int)article["dimensions"]["length"],
                        width = (int)article["dimensions"]["width"],
                        height = (int)article["dimensions"]["height"],
                        weight = (int)article["dimensions"]["weight"],
                        volume = (int)article["dimensions"]["volume"]
                    }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var postResponse = await client.PostAsync("https://localhost:2902/articles", content);
                postResponse.EnsureSuccessStatusCode();
            }
            Console.WriteLine("Articles created");

            // Loop through the locationtypes and post each locationtype to the API
            JArray locationTypes = (JArray)json["config"]["locationTypes"];
            foreach (JObject locationType in locationTypes)
            {
                var payload = new
                {
                    id = (string)locationType["id"],
                    dimensions = new
                    {
                        length = (int)locationType["dimensions"]["length"],
                        width = (int)locationType["dimensions"]["width"],
                        height = (int)locationType["dimensions"]["height"]
                    },
                    limitations = new
                    {
                        maxweight = (int)locationType["limitations"]["maxWeight"],
                        maxvolume = (int)locationType["limitations"]["maxVolume"]
                    },
                    positionLabels = new[] { "" }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var postResponse = await client.PostAsync("https://localhost:2902/locationtypes", content);
                postResponse.EnsureSuccessStatusCode();
            }
            Console.WriteLine("Locationtypes created");

            // Loop through the carriertypes and post each carriertype to the API
            JArray carrierTypes = (JArray)json["config"]["carrierTypes"];
            foreach (JObject carrierType in carrierTypes)
            {
                var payload = new
                {
                    id = (string)carrierType["id"],
                    labelPattern = (string)carrierType["id"],
                    dimensions = new
                    {
                        length = (int)carrierType["dimensions"]["length"],
                        width = (int)carrierType["dimensions"]["width"],
                        height = (int)carrierType["dimensions"]["height"]
                    },
                    limitations = new
                    {
                        maxWeight = (int)carrierType["limitations"]["maxWeight"],
                        maxVolume = (int)carrierType["limitations"]["maxVolume"]
                    },
                    positionLabels = new[] { "" }
                };



                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var postResponse = await client.PostAsync("https://localhost:2902/carriertypes", content);
                postResponse.EnsureSuccessStatusCode();
            }
            Console.WriteLine("carriertype created");

            // Get the Zones/Locations from the JSON file
            JArray zones = (JArray)json["layout"]["zones"];
            foreach (JObject zone in zones)
            {
                // create zones
                var zonePayload = new
                {
                    id = (string)zone["label"],
                    identifiers = new
                    {
                        label = (string)zone["label"]
                    }
                };

                var zoneContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(zonePayload),
                    Encoding.UTF8, "application/json");

                var zonePostResponse = await client.PostAsync("https://localhost:2902/zones", zoneContent);
                zonePostResponse.EnsureSuccessStatusCode();
                var zoneId = JObject.Parse(await zonePostResponse.Content.ReadAsStringAsync())["id"].ToString();

                JArray locations = (JArray)zone["locations"];
                foreach (JObject location in locations)
                {
                    JObject range = (JObject)location["Range"];
                    JObject start = (JObject)range["start"];
                    JObject end = (JObject)range["end"];

                    // Extract the location label and type from the start and end objects
                    string startLabel = (string)start["label"];
                    string startType = (string)start["type"];
                    string endLabel = (string)end["label"];
                    string endType = (string)end["type"];

                    // Split the label into its component parts
                    string[] startParts = startLabel.Split('-');
                    string[] endParts = endLabel.Split('-');

                    // Determine the number of iterations needed for each changing part of the label
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

                    // Generate the labels for each location in the range
                    for (int i = 0; i < numIterations; i++)
                    {
                        string[] newParts = new string[4];
                        for (int j = 3; j >= 0; j--)
                        {
                            if (startParts[j] != endParts[j])
                            {
                                int startNum = int.Parse(startParts[j]);
                                int endNum = int.Parse(endParts[j]);
                                int numPerIteration = 1;
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (startParts[k] != endParts[k])
                                    {
                                        int startNum2 = int.Parse(startParts[k]);
                                        int endNum2 = int.Parse(endParts[k]);
                                        numPerIteration *= (endNum2 - startNum2 + 1);
                                    }
                                }
                                int num = (i / numPerIteration) % (endNum - startNum + 1);
                                newParts[j] = (startNum + num).ToString("D2");
                            }
                            else
                            {
                                newParts[j] = startParts[j];
                            }
                        }
                        string newLabel = string.Join("-", newParts);
                        string newType = startType;

                        // Use the new label and type to create a location object and post it to the API
                        var payload = new
                        {
                            Identifiers = new { Label = newLabel },
                            locationType = newType
                        };

                        var content = new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                            Encoding.UTF8,
                            "application/json");

                        var postResponse = await client.PostAsync("https://localhost:2902/locations", content);
                        postResponse.EnsureSuccessStatusCode();
                        var locationId = JObject.Parse(await postResponse.Content.ReadAsStringAsync())["id"].ToString();

                        // Add the location to the selected zone
                        var addLocationPayload = "\"" + locationId + "\"";
                        var addLocationContent = new StringContent(addLocationPayload, Encoding.UTF8, "application/json");
                        var addLocationResponse = await client.PostAsync($"https://localhost:2902/zones/{zoneId}/locations", addLocationContent);

                    }
                }
            }
            Console.WriteLine("layout created");
        }
        public static async Task Write()
        {
            // removes SSL error.
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // get carriertypes from api
            var getResponseCT = await client.GetAsync("https://localhost:2902/carriertypes");
            getResponseCT.EnsureSuccessStatusCode();
            var getResponseString = await getResponseCT.Content.ReadAsStringAsync();

            // Loop through all carriertypes and write them into a list
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


            // Create a list of CarrierTypes.Root objects from the carriertypes list
            List<CarrierTypes.Root> carrierTypeRoots = new List<CarrierTypes.Root>();
            foreach (CarrierTypes.Root ct in carrierTypes)
            {
                carrierTypeRoots.Add(ct);
            }

            //get locationtypes from api
            var getResponseLT = await client.GetAsync("https://localhost:2902/locationtypes");
            getResponseLT.EnsureSuccessStatusCode();
            var getResponseStringLT = await getResponseLT.Content.ReadAsStringAsync();

            // loop through all location types and write them into a list
            var locationTypes = new List<LocationTypes.Root>();
            foreach (var locationType in JArray.Parse(getResponseStringLT))
            {
                var newLocationType = new LocationTypes.Root
                {
                    id = (string)locationType["id"],
                    Dimensions = new LocationTypes.Dimensions
                    {
                        Length = (int)locationType["dimensions"]["length"],
                        Width = (int)locationType["dimensions"]["width"],
                        Height = (int)locationType["dimensions"]["height"]
                    },
                    Limitations = new LocationTypes.Limitations
                    {
                        MaxWeight = (int)locationType["limitations"]["maxWeight"],
                        MaxVolume = (int)locationType["limitations"]["maxVolume"]
                    }
                };
                locationTypes.Add(newLocationType);
            }

            // Create a list of Locationtypes.Root objects from the locationtypes list
            List<LocationTypes.Root> locationTypeRoots = new List<LocationTypes.Root>();
            foreach (LocationTypes.Root lt in locationTypes)
            {
                locationTypeRoots.Add(lt);
            }

            //get articles from api
            var getResponseA = await client.GetAsync("https://localhost:2902/articles");
            getResponseA.EnsureSuccessStatusCode();
            var getResponseStringA = await getResponseA.Content.ReadAsStringAsync();

            // loop through all articles and write them into a list
            var articles = new List<Articles.Root>();
            foreach (var article in JArray.Parse(getResponseStringA))
            {
                var newArticle = new Articles.Root
                {
                    Id = (string)article["id"],
                    Identifiers = new Articles.Identifiers
                    {
                        Label = (string)article["identifiers"]["label"]
                    },
                    Description = (string)article["description"],
                    Dimensions = new Articles.Dimensions
                    {
                        Length = (int)article["dimensions"]["length"],
                        Width = (int)article["dimensions"]["width"],
                        Height = (int)article["dimensions"]["height"],
                        Weight = (int)article["dimensions"]["weight"],
                        Volume = (int)article["dimensions"]["volume"]
                    }
                };
                articles.Add(newArticle);
            }

            List<Articles.Root> articleRoots = new List<Articles.Root>();
            foreach (Articles.Root a in articles)
            {
                articleRoots.Add(a);
            }

            List<Dictionary<string, object>> zones = new List<Dictionary<string, object>>();

            // get zones from API
            var getResponseZ = await client.GetAsync("https://localhost:2902/zones");
            getResponseZ.EnsureSuccessStatusCode();
            var getResponseStringZ = await getResponseZ.Content.ReadAsStringAsync();

            // loop through all zones and get the id from that zone to then get the locations connected to it through the API
            foreach (var zone in JArray.Parse(getResponseStringZ))
            {
                var newZone = new Dictionary<string, object>
                {
                    { "label", (string)zone["identifiers"]["label"] },
                    { "locations", new List<Dictionary<string, object>>() }
                };

                // get locations for a zone and sort them by label in ascending order
                var getResponseL = await client.GetAsync($"https://localhost:2902/zones/{zone["id"]}/locations");
                getResponseL.EnsureSuccessStatusCode();
                var getResponseArrayL = JArray.Parse(await getResponseL.Content.ReadAsStringAsync());
                var locations = getResponseArrayL
                    .OrderBy(location => (string)location["identifiers"]["label"])
                    .ToList();

                if (locations.Count > 0)
                {
                    var firstLocation = new Dictionary<string, object>
                    {
                        { "label", (string)locations.First()["identifiers"]["label"] },
                        { "type", (string)locations.First()["locationType"] }
                    };

                    var lastLocation = new Dictionary<string, object>
                    {
                        { "label", (string)locations.Last()["identifiers"]["label"] },
                        { "type", (string)locations.Last()["locationType"] }
                    };

                    var range = new Dictionary<string, object>
                    {
                        { "start", firstLocation },
                        { "end", lastLocation }
                    };

                    var locationEntry = new Dictionary<string, object>
                    {
                        { "Range", range }
                    };

                    ((List<Dictionary<string, object>>)newZone["locations"]).Add(locationEntry);
                }

                zones.Add(newZone);
            }

            List<object> jsonOutput = new List<object>(zones);

            // Create a list of Articles.Root objects from the articles list

            var payload = new
            {
                config = new
                {
                    carrierTypes = carrierTypeRoots,
                    locationTypes = locationTypeRoots,
                    articles = articleRoots
                },
                layout = new
                {
                    zones = jsonOutput
                },
                content = new
                {
                    carriers = new { },
                    locationContent = new { },
                    carrierContent = new { }
                }
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(payload, jsonSettings);


            // Ask the user for a filename and write the JSON to the file.
            Console.Write("Enter a filename for the JSON file: ");
            string folderPath = @"Jsons\";
            Directory.CreateDirectory(folderPath); // create the folder if it doesn't exist
            string filename = folderPath + Console.ReadLine() + ".json";
            while (File.Exists(filename))
            {
                Console.WriteLine("A file with that name already exists. Please enter a different filename:");
                filename = folderPath + Console.ReadLine() + ".json";

            }
            using (var sw = new StreamWriter(filename))
            {
                sw.Write(json);
            }
            Console.WriteLine($"Json file succesfully saved inside: " + folderPath);

        }

        public static async Task ItemRead()
        {
            // removes SSL error
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Do you wish to proceed to permanently delete the items inside the warehouse and load new ones? (y/n)");
            var confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                // Get/delete items from api
                var itemResponse = await client.GetAsync("https://localhost:2902/items");
                itemResponse.EnsureSuccessStatusCode();
                var itemContent = await itemResponse.Content.ReadAsStringAsync();
                var itemDelete = JsonConvert.DeserializeObject<List<Items.Root>>(itemContent);
                foreach (var item in itemDelete)
                {
                    var deleteResponse = await client.DeleteAsync($"https://localhost:2902/items/{item.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
                Console.WriteLine("Items deleted");
            }
            else
            {
                Console.WriteLine("Operation cancelled");
                return;
            }

            // Show the file picker to choose a JSON file
            string chosenFilePath = ShowFilePicker();
            if (string.IsNullOrEmpty(chosenFilePath))
            {
                Console.WriteLine("No file selected");
                return;
            }

            // Read the JSON data from the chosen file
            string jsonData = await File.ReadAllTextAsync(chosenFilePath);
            JObject json = JObject.Parse(jsonData);

            JToken locationsToken = json["itemConfig"]["locations"];
            if (locationsToken != null)
            {
                foreach (JObject locationData in locationsToken.Children<JObject>())
                {
                    string locationId = locationData.Value<string>("id");
                    string locationLabel = locationData.Value<string>("label");
                    JArray itemsArray = locationData.Value<JArray>("items");

                    // Get the position ID for the current location
                    string positionId = await RetrievePositionId(client, "https://localhost:2902/locations", locationId, locationLabel);
                    if (positionId == null)
                    {
                        Console.WriteLine($"Position ID not found for location: {locationId} - {locationLabel}");
                        continue;
                    }

                    foreach (JObject itemData in itemsArray.Children<JObject>())
                    {
                        string articleId = itemData.Value<string>("articleid");
                        int amount = itemData.Value<int>("amount");

                        if (articleId != null)
                        {
                            // Create the items for the current article
                            for (int i = 0; i < amount; i++)
                            {
                                // Create the item
                                var createItemPayload = new { articleId = articleId };
                                var createItemJson = JsonConvert.SerializeObject(createItemPayload);
                                var createItemContent = new StringContent(createItemJson, System.Text.Encoding.UTF8, "application/json");

                                var createItemResponse = await client.PostAsync("https://localhost:2902/items", createItemContent);
                                createItemResponse.EnsureSuccessStatusCode();

                                // Extract the item ID from the response
                                string createItemResponseJson = await createItemResponse.Content.ReadAsStringAsync();
                                var createItemResponseObj = JObject.Parse(createItemResponseJson);
                                string itemId = createItemResponseObj["id"].Value<string>();

                                // Update the item with the position ID and whereabouts
                                var updateItemPayload = new { whereabouts = positionId };
                                var updateItemJson = JsonConvert.SerializeObject(updateItemPayload);
                                var updateItemContent = new StringContent(updateItemJson, System.Text.Encoding.UTF8, "application/json");

                                HttpResponseMessage updateResponse = await client.PutAsync($"https://localhost:2902/items/{itemId}/whereabouts", updateItemContent);
                                updateResponse.EnsureSuccessStatusCode();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid article ID");
                        }
                    }
                }
            }
        }

        // retrieves the positionId from a location
        static async Task<string> RetrievePositionId(HttpClient httpClient, string locationsUrl, string locationId, string locationLabel)
        {
            var response = await httpClient.GetAsync(locationsUrl);
            response.EnsureSuccessStatusCode();
            var jsonData = await response.Content.ReadAsStringAsync();
            var locations = JArray.Parse(jsonData);

            foreach (var location in locations)
            {
                var id = location["id"].ToString();
                var label = location["identifiers"]["label"].ToString();

                if (id == locationId || label == locationLabel)
                {
                    var positions = location["positions"];
                    if (positions != null && positions.HasValues)
                    {
                        var positionId = positions[0]["id"].ToString();
                        return positionId;
                    }
                }
            }

            return null;
        }


        // lets the user choose what file to read
        static string ShowFilePicker()
        {
            string folderPath = "ItemJsons";
            string[] files = Directory.GetFiles(folderPath);
            Console.WriteLine($"Found {files.Length} files in folder '{folderPath}'");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
            }
            Console.Write("Enter the number of the file to read: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int fileNumber) || fileNumber < 1 || fileNumber > files.Length)
            {
                Console.WriteLine("Invalid input");
                return null;
            }
            return files[fileNumber - 1];
        }


        public static async Task ItemWrite()
        {
            // removes SSL error
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Url's
            string itemsUrl = "https://localhost:2902/items";
            string locationsUrl = "https://localhost:2902/locations";
            string positionsBaseUrl = "https://localhost:2902/positions";

            var itemConfig = new JObject();
            var locationsArray = new JArray();
            itemConfig["itemConfig"] = new JObject();
            itemConfig["itemConfig"]["locations"] = locationsArray;

            var locations = await RetrieveLocations(client, locationsUrl);

            // loops through every location for contents
            foreach (var location in locations)
            {
                var locationId = location["id"].ToString();
                var locationLabel = await RetrieveLocationLabel(client, locationsUrl, locationId);


                var positionId = await RetrievePositionIdForLocation(client, locationsUrl, locationId);
                if (positionId == null)
                {
                    Console.WriteLine($"Skipping location with missing position: {locationId}");
                    continue;
                }

                var items = await RetrieveItemsForPosition(client, $"https://localhost:2902/positions/{positionId}/content");

                var locationObject = new JObject();
                locationObject["id"] = locationId;
                locationObject["label"] = locationLabel; // Set the label for the location
                var itemsArray = new JArray();
                locationObject["items"] = itemsArray;

                var articleCounts = new Dictionary<string, int>();

                // loops through all items
                foreach (var item in items)
                {
                    if (item is JObject itemObject)
                    {
                        var itemId = itemObject["id"].ToString();

                        var itemData = await RetrieveItemData(client, $"https://localhost:2902/items/{itemId}");
                        if (itemData == null || itemData["article"] == null)
                            continue;

                        var articleId = itemData["article"]["id"].ToString();

                        if (!string.IsNullOrEmpty(articleId))
                        {
                            if (articleCounts.ContainsKey(articleId))
                                articleCounts[articleId]++;
                            else
                                articleCounts[articleId] = 1;
                        }
                    }
                }

                // counts amount of articles
                foreach (var kvp in articleCounts)
                {
                    var articleId = kvp.Key;
                    var count = kvp.Value;

                    var itemData = new JObject();
                    itemData["articleid"] = articleId;
                    itemData["amount"] = count;

                    itemsArray.Add(itemData);
                }

                if (itemsArray.HasValues)
                {
                    locationsArray.Add(locationObject);
                }
            }

            // writes the JSON file
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
        }


        // retrieve all items from a position
        static async Task<JArray> RetrieveItemsForPosition(HttpClient httpClient, string positionUrl)
        {
            var response = await httpClient.GetAsync(positionUrl);
            response.EnsureSuccessStatusCode();
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(jsonData);

            JArray items = null;

            if (data.TryGetValue("items", out var itemsToken) && itemsToken.Type == JTokenType.Array)
            {
                items = (JArray)itemsToken;
            }

            return items ?? new JArray();
        }

        // retrieve positionid that belongs to the location
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

        // retrieve the infromation about items
        static async Task<JObject> RetrieveItemData(HttpClient httpClient, string itemUrl)
        {
            var response = await httpClient.GetAsync(itemUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonData);
            }
            else
            {
                return null;
            }
        }

        //ask for filename
        static string GetFileNameFromUser()
        {
            Console.Write("Enter the name for the JSON file: ");
            return Console.ReadLine();
        }

        // Get locations
        static async Task<JArray> RetrieveLocations(HttpClient httpClient, string locationsUrl)
        {
            var response = await httpClient.GetAsync(locationsUrl);
            response.EnsureSuccessStatusCode();
            var jsonData = await response.Content.ReadAsStringAsync();
            var locations = JArray.Parse(jsonData);
            return locations;
        }
        // retrieve the label of the location
        static async Task<string> RetrieveLocationLabel(HttpClient httpClient, string locationsUrl, string locationId)
        {
            var url = $"{locationsUrl}/{locationId}";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(jsonData);

            var labelToken = data["identifiers"]["label"];
            if (labelToken != null)
            {
                return labelToken.ToString();
            }

            return null;
        }


    }
}