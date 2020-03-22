# Nuntius Messanger

## Releases
https://github.com/ArthFink/NuntiusReleases/releases/tag/alpha_1.1.0

## Testen
Zum testen empfehlen wir Ihnen min. einen Android Emulator zu verwenden. Um Nachrichten zu empfangen sollten Sie einen anderen Emulator(für das beste Nuntius Erlebnis) oder die UWP App verwenden. 
Notifications werden nur gesendet, wenn man nicht in dem Chat ist von dem man eine Nachricht erhalten hat.

## Bekannte Bugs
- Chats können mit nicht registrieten Nutzern erstellt werden
- IP Adress kann nicht in UWP geändert werden, da UWP seltsam ist, noch seltsamer als Xamrin Froms
  - IP kann in nuntiusClientChat/Controller/NetworkController.cs Zeile 20 eingestellt werden
  - In der Android App funktioniert dies aber
  
- Auf manchen Android Geräten kann der Schalter fürs Registrieren nicht betätigt werden. (Dummy Account zum Testen: Nutzername: "test-user" Password: "123") 

## Leider fehlende Features
- Ablaufen von Tokens
- Notifications in IOS und UWP  
- RSA Verschlüsslung der Requests und Resposes funtioniert ist aber auskommentiert damit die UWP App funtioniert

## Probleme
Falls Sie Probleme mit unserem Projekt haben, helfen wir Ihnen gerne über Teams.
