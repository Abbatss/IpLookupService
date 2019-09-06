# Service to finde Locations by IP or City

Service uses in memory DB. It can be found here : ./src/Common/DataBase/geobase.dat

To run Acceptance tests :

install python 3.7 

open cmd on root folder

python -m pytest

GeoDataBaseLoadPerformanceTests - test for load DB in memory. It will fail if load time > 30 

To run unit test for SPA Web site :

./src/SPAWeb/GeoInformationSPA/ClientApp > ng test


.net 4.7 solution file ./src/IPLookupService.sln
solution can be run from MS Visual Stidio 2019 or from docker compose

.net core 2.1 solution file ./src/Core_IPLookupService.sln

