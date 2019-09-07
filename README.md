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

solution can be run from MS Visual Stidio 2019 or from docker

to run win Dockers which include .net 4.7.2 and iis :
0. switch docker to work with windows containers.
1. Publish src\SPAWeb\GeoInformationSPA project from VS Studio using Docker profile
2. Publish src\API\IPLookup.API.Host project from VS Studio using Docker profile
3. run build\build-win.LocationLookupSite.Dockerfile.cmd
4. run build\build-win.LocationLookupAPI.Dockerfile.cmd

Also you can download allready built images from my repo.
to do that:
open CMD. write:
1. docker run --name win-locationlookup-spa -d -p 44372:44372 maskevich/win-locationlookup-spa
2. docker run --name win-locationlookup-api -d -p 44375:44365 maskevich/win-locationlookup-api

Open site:

Windows docker host has bug: localhost address is not workin:
you need to:
1. docker inspect --format "{{ .NetworkSettings.IPAddress }}" win-locationlookup-spa
2. docker inspect --format "{{ .NetworkSettings.IPAddress }}" win-locationlookup-api
3. open spa by http://{IpFrom 1}
4. specify API base url on settings taab: http://{IpFrom 2}/api

.net core 2.1 solution file ./src/Core_IPLookupService.sln

solution can be run from MS Visual Stidio 2019 or from docker

0. switch docker to work with linux containers.
1. run build\build-lin.LocationLookupSite.Dockerfile.cmd
2. run build\build-lin.LocationLookupAPI.Dockerfile.cmd

Also you can download allready built images from my repo.
to do that:
open CMD. write:
1. docker run -d -p 44372:80 maskevich/lin-locationlookup-spa
1. docker run -d -p 44365:80 maskevich/lin-locationlookup-api

open API swagger : http://localhost:44365

open SPA site: http://localhost:44372


remove containers :
Docker rm lin-locationlookup-spa -f

Docker rm lin-locationlookup-api -f