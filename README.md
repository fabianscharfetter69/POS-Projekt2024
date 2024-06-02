# Semesterprojekt: Fabian Scharfetter
 
## Softwaredesign (Architektur)
Die Geburtstags-Liste wurde in einer Client-Client-Server-Architektur entwicklet, wobei der Client als WPF-Anwendung oder WebApp implementiert ist und diese beide mit dem Server (welcher auf Spring Boot basiert) kommunizieren. Die Daten werden in einer MongoDB-Datenbank gespeichert.

```mermaid
graph TD;
  A[WPF Client] <--> C[Spring Boot Server];
  B[Web-App Client] <--> C[Spring Boot Server];
  C[Spring Boot Server] <--> D[MongoDB Datenbank];
```

## Softwarebeschreibung
Die Geburtstagslisten-Software ist eine plattformübergreifende Anwendung, die es Benutzern ermöglicht, Geburtstage zu verwalten und anzuzeigen. Es bietet eine benutzerfreundliche Oberfläche sowohl als Desktopanwendung (WPF) als auch als Webanwendung.  
Die Anwendung besteht aus drei Hauptkomponenten: 

<details>
<summary>Java-Server mit MongoDB-Datenbankanbindung</summary>  
   
Der Java-Server fungiert als Backend der Anwendung. Er ist mit einer MongoDB-Datenbank verbunden, in der Geburtstagsdaten gespeichert werden. Der Server bietet folgende REST-API-Endpunkte zum Abrufen und Speichern von Geburtstagsinformationen. Server-Port ist 8081.  
</details>

<details>
<summary>Webseite mit dynamischem Kalender</summary> 
   
Die Website dient als Frontend/Benutzeroberfläche der Anwendung. Sie besteht aus 3 Dateien (index.html, style.css, calender-script.js).  
Folgendermaßen ist die Seite aufgebaut:  
  
1) Kalender, der per JavaScript dynamisch geladen wird  
2) Ausgabe-Feld, bei dem die vom Server geladenen Geburtsdaten angezeigt werden.    
Die Geburtstage werden vom Server mit einem HTTP-Request (localhost:8081/geb-liste/geburtstage) geladen.      
Die Website bietet 2 Anzeigemodi --> Alle Geburtstage werden angezeigt ODER Die Geburtstage in dem Monat, der gerade am Kalender angezeigt wird
3) Eingabe-Feld, bei dem neue Geburtstage hinzugefügt werden können & mittels POST-Befehl an den Server geschickt werden
</details>

<details>
<summary>C#-Client</summary>  
Der C#-Client ist eine eigenständige WPF-Anwendung, die es Benutzern ermöglicht, Geburtstage  zu verwalten. Der Client lädt Geburtstagsdaten asynchron vom Java-Server herunter und stellt sie in einem Ausgabefeld dar. Wenn die Verbindung zum Server nicht hergestellt werden kann, dann ist es möglich den Client offline zu nutzen. Benutzer können Geburtstage über die Benutzeroberfläche des Clients hinzufügen & löschen, wobei die Änderungen sofort mit dem Java-Server synchronisiert werden.  
  
Der Client ist ähnlich wie die Website aufgebaut:
1) Kalender, ist ein 'Calender' Element von WPF  
2) Eingabefeld, bei dem neue Geburtstage hinzugefügt werden können & mittels POST-Befehl an den Server geschickt werden  
3) Ausgabefeld, bei dem die vom Server geladenen Geburtsdaten angezeigt werden.

Die Geburtstage werden vom Server asynchron geladen.  
  
Die Website bietet 2 Anzeigemodi --> Alle Geburtstage werden angezeigt ODER Die Geburtstage in dem Monat, der gerade am Kalender angezeigt wird  
ZUSÄTZLICH können Geburtsdaten bei einem Doppelklick gelöscht werden.
</details>

## API-Beschreibung

### GeburtstagController

<details>
  <summary>/geb-liste</summary>
Dieser Endpunkt ist der generelle Endpoint der API, welcher vor dem jeweiligen spezifischen Endpoint geschrieben werden muss.
</details>

<details>
  <summary>/geburtstag [POST]</summary>
Dieser Enpoint führt zur Methode "addGeburtstag()" und ermöglicht das Hinzufügen von Geburtstagen in die MongoDB-Datenbank auf dem Server. Er nimmt einen Geburtstag im JSON-Format entgegen und speichert ihn mithilfe des "geburtstagService" und gibt das gespeicherte Geburtstagsobjekt zurück.
 
**JSON-Body:**
  ```json
  {
    "name": "Max Mustermann",
    "day": "01",
    "month": "01",
    "year": "2000"
  }
  ```
</details>

<details>
  <summary>/geburtstage [POST]</summary>
Ähnlich wie /geburtstag, aber hier können mehrere Geburtstage auf einmal hinzugefügt werden.

**JSON-Body:**
  ```json
  [
    {
     "name": "Max Mustermann",
     "day": "01",
     "month": "01",
     "year": "2000"
    },
    {
     "name": "Paul Pansen",
     "day": "02",
     "month": "01",
     "year": "2001"
    }
  ]
  ```
</details>

<details>
  <summary>/geburtstage [GET]</summary>
Dieser Endpunkt ist ein GET-Endpunkt mit der URL "/geburtstage", der alle Geburtstage aus der MongoDB-Datenbank abruft. Er ruft die Methode "findAll" auf, um eine Liste aller Geburtstage zu erhalten, und gibt sie zurück.
**JSON-Body:**
  ```json
  [
    {
        "id": "6620e86ab06cf02b3d9e813a",
        "name": "Philipp Kirchtag",
        "day": "02",
        "month": "09",
        "year": "2005"
    },
    {
        "id": "6620e87cb06cf02b3d9e813c",
        "name": "Luca Jenerwein",
        "day": "24",
        "month": "10",
        "year": "2005"
    }
  ]
  ```
</details>

<details>
  <summary>/geburtstag/{id} [DELETE]</summary>
Dieser Endpunkt ist ein DELETE-Endpunkt mit der URL "/geburtstag/{id}", der dazu dient, einen Geburtstag aus der MongoDB-Datenbank basierend auf der ID zu löschen. Die ID des zu löschenden Geburtstags wird aus dem Pfad der URL extrahiert. Die Methode "deleteGeburtstag" ruft die Methode "delete" des "geburtstagService" auf, um den Geburtstag zu löschen.
</details>

### WebController

<details>
  <summary>/website</summary>
Dieser Endpunkt ist ein GET-Endpunkt mit der URL "/website", der die Hauptseite der Website (index.html) zurückgibt.</details>

<details>
  <summary>/newGeburtstag</summary>
Dieser Endpunkt wird aufgerufen, wenn ein neuer Geburtstag über das Formular auf der Website gesendet wird. Er erwartet zwei Parameter: "nameInput" für den Namen des Geburtstagskindes und "dateInput" für das Geburtsdatum.  
   
Die Methode teilt das Datum in seine Bestandteile auf (Tag, Monat, Jahr) und erstellt dann ein neues Geburtstagsobjekt damit. Dieses Geburtstagsobjekt wird dann in ein "GeburtstagDTO" (Data Transfer Object) umgewandelt.  
  
Anschließend wird eine HTTP-POST-Anfrage an einen anderen Endpunkt des Servers gesendet, der den Geburtstag speichert. Die URL des Endpunkts ist "http://localhost:8081/geb-liste/geburtstag", und die Daten werden als "GeburtstagDTO" gesendet.  
  
Die Antwort wird überprüft, und je nachdem, ob die Operation erfolgreich war oder nicht, werden entsprechende Meldungen ausgegeben.

Schließlich wird eine Weiterleitung zur Hauptseite der Website durchgeführt.</details>

## Verwendung der API (ev. mit Code-Ausschnitten)

<details>
  <Summary>MongoDB-Datenbank</summary>

  **Geburtstag hinzufügen:** So werden Geburtstage in der MongoDb-Datenbank gespeichert.  
  URL: localhost:8081/geb-liste/geburtstag

  **Json-Body:**
  ```json
    {
        "name": "Cristiano Ronaldo",
        "day": "05",
        "month": "02",
        "year": "1985"
    }
  ```
  **Rückgabe:**
  ```json
    {
        "id": "665c4bfe168b7b0269b2e86c",
        "name": "Cristiano Ronaldo",
        "day": "05",
        "month": "02",
        "year": "1985"
    }
  ```
**Geburtstage abfragen:** So werden Geburtstage aus der MongoDb-Datenbank abgefragt.  
  URL: localhost:8081/geb-liste/geburtstage

  **Rückgabe:**
  ```json
[
    {
        "id": "665c4bfe168b7b0269b2e86c",
        "name": "Cristiano Ronaldo",
        "day": "05",
        "month": "02",
        "year": "1985"
    },
    {
        "id": "6620ece7b06cf02b3d9e8148",
        "name": "Tobias Ziller",
        "day": "03",
        "month": "11",
        "year": "2005"
    }
]
  ```

**Geburtstag löschen:** So werden Geburtstage gelöscht.  
  URL: localhost:8081/geb-liste/geburtstag/{id}  bzw. localhost:8081/geb-liste/geburtstag/665c4bfe168b7b0269b2e86c
  
  **Rückgabe bei Erfolg (Anzahl der gelöschten Objekte):**
  ```json
    1
  ```
</details>

<details>
  <Summary>Spring-Boot Server</summary>

  **Beschreibung:** Hinzufügen von Daten  
  **Java-Endpoint:**
  ```java
    @PostMapping("/geburtstag")
    @ResponseStatus(HttpStatus.CREATED)
    public GeburtstagDTO addGeburtstag(@RequestBody GeburtstagDTO geburtstagDTO) {
        return geburtstagService.save(geburtstagDTO);
    }
  ```
  **Java-Backend (MongoDBGeburtstagRepository.java):**  
  Hier werden die Daten in die Datenbank gespeichert.
  ```java
    public Geburtstag save(Geburtstag geburtstagEntity) {
        geburtstagEntity.setId(new ObjectId());
        geburtstagCollection.insertOne(geburtstagEntity);
        return geburtstagEntity;
    }
  ```
  
  
  **Beschreibung:** Abfragen von Daten  
  **Java-Endpoint:**
  ```java
    @GetMapping("/geburtstage")
    public List<GeburtstagDTO> findAllGeburtstage() {
        return geburtstagService.findAll();
    }
  ```
  **Java-Backend (MongoDBGeburtstagRepository.java):**  
  Hier werden die Daten von der Datenbank gelesen und in einer Liste zurückgegeben.
  ```java
    public List<Geburtstag> findAll() {
    return geburtstagCollection.find().into(new ArrayList<>());
    }
  ```  
  
  
  **Beschreibung:** Löschen von Daten  
  **Java-Endpoint:**
  ```java
    @DeleteMapping("/geburtstag/{id}")
    public Long deleteGeburtstag(@PathVariable String id) {
    return geburtstagService.delete(id);
    }
  ```
  **Java-Backend (MongoDBGeburtstagRepository.java):**  
  Hier wird nach einem Objekt mit der in der URL übergebenen ID gesucht und bei Erfolg wird dieses gelöscht.
  ```java
    public long delete(String id) {
    return geburtstagCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }
  ```
</details>

<details>
  <Summary>WPF-Client</summary>

  **Beschreibung:** Laden der Daten vom Server

  **C#-Code:**
  ```csharp
    public async void getGeburtstageFromServer()
    {
        try
        {
            Trace.WriteLine("Laden vom Server");
    
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8081/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    
                HttpResponseMessage response = await client.GetAsync("geb-liste/geburtstage");
                if (response.IsSuccessStatusCode)
                {
                    Trace.WriteLine("Success");
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
    
                    var geburtstageFromServer = JsonSerializer.Deserialize<ObservableCollection<Geburtstag>>(jsonString, options);
    
                    geburtstagsliste.Clear();
                    foreach (var geburtstag in geburtstageFromServer)
                    {
                        geburtstagsliste.Add(geburtstag);
                    }
    
                    //Laden in Ausgabe
                    erstelleAusgabe();
                }
                else
                {
                    MessageBox.Show($"Server returned {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
        }
    }
  ```

  **Beschreibung:** Hinzufügen von Daten

  **C#-Code:**
  ```csharp
    private async Task addGeburtstagToServer(Geburtstag geb)
    {
        using (HttpClient client = new HttpClient())
        {
            // Url zum posten
            string url = "http://localhost:8081/geb-liste/geburtstag";
    
            //Json erstellen
            string data = "{";
            data += $"  \"name\": \"{geb.Name}\",     \"day\": \"{geb.Day}\",     \"month\": \"{geb.Month}\",     \"year\": \"{geb.Year}\"  ";
            data += "}";
            Trace.WriteLine(data);
    
            try
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
    
                HttpResponseMessage response = await client.PostAsync(url, content);
    
                response.EnsureSuccessStatusCode();
    
                Console.WriteLine("Daten erfolgreich gesendet.");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Fehler beim Senden der Daten: {e.Message}");
            }
        }
    }
  ```

  **Beschreibung:** Löschen von Daten

  **C#-Code:**
  ```csharp
    private async void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Popup anzeigen
        int index = listView.SelectedIndex;
    
        if (index >= 0 && index < geburtstagsliste.Count)
        {
            Trace.WriteLine($"List View DoubleClick -> {geburtstagsliste[index].Name}");
            var result = MessageBox.Show($"Do you want to delete {geburtstagsliste[index].Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Element löschen
                string id = geburtstagsliste[index].Id;
                string url = $"http://localhost:8081/geb-liste/geburtstag/{id}";
    
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync(url);
    
                        //Laden in Ausgabe
                        getGeburtstageFromServer();
    
                        if (response.IsSuccessStatusCode)
                        {
                            Trace.WriteLine($"Geburtstag mit ID {id} erfolgreich gelöscht.");
                        }
                        else
                        {
                            Trace.WriteLine($"Fehler beim Löschen des Geburtstags mit ID {id}. Statuscode: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Fehler beim Löschen des Geburtstags: {ex.Message}");
                    }
                }
            }
        }            
    }
  ```
</details>

<details>
  <Summary>WebApp-Client</summary>

  **Beschreibung:** Laden der Daten vom Server  
  **JS-Code:**
 ```js
    // Geburtstage von der API laden und alle anzeigen
    fetch('http://localhost:8081/geb-liste/geburtstage')
        .then(response => response.json())
        .then(data => {
            displayBirthdays(data);             // Geburtstage anzeigen
        })
        .catch(error => {
            console.error('Error fetching birthdays:', error);
        });
  ```

  **Beschreibung:** Hinzufügen von Daten  
  **HTML:**
 ```html
<!-- Fenster zum Hinzufügen von Geburtstagen -->
<form id="birthdayForm" class="popup-content" action="/newGeburtstag" method="POST">
    <p>Geburtstagskind hinzufügen:</p>
    <input type="date" id="dateInput" name="dateInput" required>
    <input type="text" id="nameInput" name="nameInput" placeholder="Name" required>
    <button type="submit" id="submitBtn">Hinzufügen</button>
    <p id="errorMsg" class="error-message"></p>
</form>
  ```

  **Java-Endpoint:**
 ```java
@PostMapping("/newGeburtstag")
    public String newGeburtstag(@RequestParam("nameInput") String name, @RequestParam("dateInput") String dateString){

        System.out.println("Geburtstagskind: " + name);
        System.out.println("Datum: " + dateString);
        String[] arrDate = dateString.split("-");

        try {
            Geburtstag geb = new Geburtstag(null, name, arrDate[2], arrDate[1], arrDate[0]);
            GeburtstagDTO geburtstagDTO = new GeburtstagDTO(geb);

            // Senden der Daten an den GeburtstagController
            ResponseEntity<GeburtstagDTO> response = restTemplate.postForEntity("http://localhost:8081/geb-liste/geburtstag", geburtstagDTO, GeburtstagDTO.class);
            if (response.getStatusCode().is2xxSuccessful()) {
                System.out.println("Geburtstag erfolgreich hinzugefügt: " + response.getBody());
            } else {
                System.out.println("Fehler beim Hinzufügen des Geburtstags");
            }

        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

        return "redirect:/website";
    }
  ```
</details>

## Diagramme

### Klassendiagramm des WPF-Clients
```mermaid
classDiagram
    Home o-- Geburtstag
```

### Klassendiagramm des Spring-Boot Servers
```mermaid
classDiagram
    Application o-- GeburtstagsController
    Application o-- WebController
    Application o-- MongoDBConfigurations

    GeburtstagsController o-- GeburtstagService
    GeburtstagServiceImpl o-- GeburtstagsRepository
    GeburtstagService o-- GeburtstagDTO
    GeburtstagsRepository o-- Geburtstag
    MongoDBGeburtstagsRepository o-- MongoClient

    GeburtstagsRepository <-- MongoDBGeburtstagsRepository
    GeburtstagService <-- GeburtstagServiceImpl
```

## Diskussion der Ergebnisse  

  ### Zusammenfassung  
Die entwickelte Geburtstagslisten-Software ist eine plattformübergreifende Anwendung, die es Benutzern ermöglicht, Geburtstage zu verwalten und anzuzeigen. Sie besteht aus einem Java-Server mit MongoDB-Anbindung, einer Webseite mit dynamischem Kalender und einem C#-Client. Die Kommunikation zwischen den Komponenten erfolgt über REST-APIs.

  ### Hintergründe  
Die Entscheidung für eine Client-Client-Server-Architektur ermöglicht eine flexible Nutzung der Anwendung auf verschiedenen Plattformen. Die Verwendung von Spring Boot und MongoDB bietet gute Backend-Lösung, während die Frontend-Komponenten die Benutzerfreundlichkeit durch eine intuitive Benutzeroberfläche verbessern.

  ### Ausblick  
Das Projekt bietet eine solide Grundlage für die Verwaltung von Geburtstagslisten, könnte aber durch zusätzliche Funktionen wie Erinnerungen für bevorstehende Geburtstage oder die Integration von Accounts mit Regristrierung und Anmeldung erweitert werden, um Benutzern mehr Mehrwert zu bieten und die Benutzerbindung zu stärken.

## Quellenverzeichnis / Links
- [YouTube Tutorial für Kalender mit HTML CSS & JavaScript](https://www.youtube.com/watch?v=Z1BGAivZRlE)
