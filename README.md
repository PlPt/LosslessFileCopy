# LosslessFileCopy
A .NET File Copy program, which can continue abroaded copy progresses.

German Description
------------------------------------
LosslessFileCopy ist wie der Name schon sagt ein copierprogramm, welches Dateien verlustlos kopieren kann.
Während Windows gerade bei großen Dateien die über das Netzwerk ö.ä. übertragen werden, gerne mal abbricht und die angefangene kopie damit löscht, löst LosslessFileCopy dieses Problem, indem die Datei "stückchenweise" ind Datenpaketen mit standardmäßig 15mb kopiert.
Bricht dann eine kopie ab, greift erstmal der interne Versuchslogik und es wird versucht an der stelle weiter zu machen wo aufgehört wurde. Ist das nicht erfolgreich, bleibt die teilweise kopierte Datei auf dem Zeillaufwerk liegen. Ein erneuter manueller Start des Kopierforgangs, erkennt die angefangene Datei und kopiert an der stelle weiter, wo vorher aufgehört wurde. Damit wird vermieden, dass von 0 angefangen werden muss.

Bisher unterstütze Features:
1. Datei wird in Datenpaketen von standardmäßig 15MB kopiert
2. Abgebrochene Dateien können an der Abbruchstelle wieder weiterkopiert werden 
3. Abbruch der kopie erfolgt erst dann, wenn mehrmaliges Wiederholen des schreib oder lesevorgangs nicht funktioniert
4. Auch ganze Verzeichnisse können kopiert werden ( dabei gibt es aber im Moment noch Bigs beim Anzeigen des Kopierstatuses)


Geplante Features:
1. Richtige Jobwarteschlange, in der jeder Kopierauftrag inidviduell gestartet/pausiert udn abgebrochen werden kann
2. Verbesserte GUI/Mehr interaktions und einstellungsmöglichkeiten + ausgabe des ErrorLog (falls notwendig)



English description (short)
-----------------------------------------

LosslessFileCopy is a program with which you can easily copy large files or directories without the fear of a copy restart caused by any (network) error.
You can continue the copy of a file which you copy with this program.
Windows copies can not be continued.

Features:
1. Copy files in "data packets" (default 15mb)
2. Can continue a Copy, if the process abroaded earlier so you don't have to copy the whole file again
3. Can copy full directories
4. Retry logic, if copy failed


Planned Features:
3. Job Queue
4. better GUI with File and Folder Dialogs
