# Sølvberget Apps 

##  Webapp: <img src="https://capsvg.visualstudio.com/_apis/public/build/definitions/e7f3a515-6ede-49d6-9ed9-dec8141747da/27/badge"/>

Webprosjektene bruker Bower og Grunt. Etter utsjekk:

1. npm install -g grunt bower grunt-cli

i mappen til webprosjektet (webapp):

2. npm install
3. bower install
4. grunt --force
5. grunt server

OBS! Det er satt opp CI mot master-branchen slik at ny versjon av webappen blir pushet ut til http://solvbergetwebapp.azurewebsites.net/ hver gang en commit blir pushet til master. Branch derfor ut eller bruk develop-branchen for utvikling.