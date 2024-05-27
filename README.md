# Semesterprojekt: Fabian Scharfetter
 
## Softwaredesign (Architektur)

## Softwarebeschreibung
Die Geburtstagslisten-Software ist eine plattformübergreifende Anwendung, die es Benutzern ermöglicht, Geburtstage zu verwalten und anzuzeigen. Die Anwendung besteht aus drei Hauptkomponenten:

### **Java-Server mit MongoDB-Datenbankanbindung:**  
Der Java-Server fungiert als Backend der Anwendung. Er ist mit einer MongoDB-Datenbank verbunden, in der Geburtstagsdaten gespeichert werden. Der Server bietet REST-API-Endpunkte zum Abrufen und Speichern von Geburtstagsinformationen. Server-Port ist 8081.
  
### **Webseite mit dynamischem Kalender:**   
Die Website dient als Frontend/Benutzeroberfläche der Anwendung. Sie besteht aus 3 Dateien (index.html, style.css, calender-script.js).  
Folgendermaßen ist die Seite aufgebaut:
- Kalender, der per JavaScript dynamisch geladen wird
- Ausgabe-Feld, bei dem die vom Server geladenen Geburtsdaten angezeigt werden.  
Die Geburtstage werden vom Server mit einem HTTP-Request (localhost:8081/geb-liste/geburtstage) geladen.    
Die Website bietet 2 Anzeigemodi --> Alle Geburtstage werden angezeigt ODER Die Geburtstage in dem Monat, der gerade am Kalender angezeigt wird
- Eingabe-Feld, bei dem neue Geburtstage hinzugefügt werden können & mittels POST-Befehl an den Server geschickt werden
  
### **C#-Client:**
Der C#-Client ist eine eigenständige WPF-Anwendung, die es Benutzern ermöglicht, Geburtstage  zu verwalten. Der Client lädt Geburtstagsdaten asynchron vom Java-Server herunter und stellt sie in einem Ausgabefeld dar. Wenn die Verbindung zum Server nicht hergestellt werden kann, dann ist es möglich den Client offline zu nutzen. Benutzer können Geburtstage über die Benutzeroberfläche des Clients hinzufügen & löschen, wobei die Änderungen sofort mit dem Java-Server synchronisiert werden.  
Der Client ist ähnlich wie die Website aufgebaut:
- Kalender, ist ein 'Calender' Element von WPF
- Eingabefeld, bei dem neue Geburtstage hinzugefügt werden können & mittels POST-Befehl an den Server geschickt werden
- Ausgabefeld, bei dem die vom Server geladenen Geburtsdaten angezeigt werden.  
Die Geburtstage werden vom Server asynchron geladen.  
Die Website bietet 2 Anzeigemodi --> Alle Geburtstage werden angezeigt ODER Die Geburtstage in dem Monat, der gerade am Kalender angezeigt wird  
ZUSÄTZLICH können Geburtsdaten bei einem Doppelklick gelöscht werden.

## API-Beschreibung

## Verwendung der API (ev. mit Code-Ausschnitten)

## Ev. Diagramme (Use-Cases, Übersichtsdiagramme)

## Diskussion der Ergebnisse (Zusammenfassung, Hintergründe, Ausblick, etc.)

## Quellenverzeichnis / Links
- [YouTube Tutorial für Kalender mit HTML CSS & JavaScript](https://www.youtube.com/watch?v=Z1BGAivZRlE)

## Und wichtig: Die Grafiken müssen mittels Mermaid erstellt werden! 
(GitHub bietet eine Unterstützung für Mermaid Diagramme)
