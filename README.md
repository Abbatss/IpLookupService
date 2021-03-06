# Service to finde coordinates by IP or locations by city

Service uses in memory DB. It can be found here : ./src/Common/DataBase/geobase.dat

# DB Description

DB has binary format. The file sequentially stores:
# 60 byte - Header
	int   version;           // DB version
	sbyte name[32];          // DB name
	ulong timestamp;         // DB creation time
	int   records;           // Overall records count
	uint  offset_ranges;     // offset from the beginning of the file to the beginning of the list of records with geoinformation
	uint  offset_cities;     // offset from the beginning of the file to the beginning of the index, sorted by city name
	uint  offset_locations;  // offset from the beginning of the file to the beginning of the list of location records
# 12 byte * Header.records (records count) - List of rows with IP ranges, sorted by ip_from and ip_to
	uint ip_from;           // beginning of the range of IP addresses
	uint ip_to;             // end of IP range
	uint location_index;     // location record index
# 96 byte * Header.records (records count)  - list of records with location information with coordinates (longitude and latitude)
	sbyte country[8];        // country name (random string with the prefix "cou_")
	sbyte region[12];        // region name (random string with the prefix "reg_")
	sbyte postal[12];        // zip code (random string with the prefix "pos_")
	sbyte city[24];          // city name (random string with the prefix "cit_")
	sbyte organization[32];  // organization name (random string with the prefix "org_")
	float latitude;          // latitude
	float longitude;         // longitude
## 4 byte * Header.records (records count)  - list of location record indexes sorted by city name


# working solution in AWS: 
	1. SPA: http://locations-lookup.s3-website-us-east-1.amazonaws.com/
		a) IP :111.111.111.111
		b) City: Ivocyze
	2. API: https://ib7hey7l6d.execute-api.us-east-1.amazonaws.com/develop/
		a) IP :111.111.111.111
		b) City: Ivocyze
	
# To run Acceptance tests :

install python 3.7 

open cmd on root folder  cd ./Tests/AcceptanceTests

python -m pytest

# GeoDataBaseLoadPerformanceTests - test for load DB in memory. It will fail if load time > 30 

# To run unit test for SPA Web site :

./src/SPAWeb/GeoInformationSPA/ClientApp > ng test


# Test for DB load in memory - IPLookup.API.InMemoryDataBase.Test.GeoDataBaseLoadPerformanceTests

It looks like Indedex with link to location ordered by City - is not orderedby City.
Test - IPLookup.API.InMemoryDataBase.Test.GeoDataBaseClientTests.GetCitiesIndex_CityName_Order_Test


# .net 4.7 solution file ./src/IPLookupService.sln

solution can be run from MS Visual Stidio 2019 or from docker

Visual studio:
1. Open solution ./src/IPLookupService.sln.
2. Rebuild GeoInformationSPA (make sure that rosly folder is present in ./bin . If not - clean and rebuild GeoInformationSPA)
3. Rebuild IPLookup.API.Host
4. setup multy startup projects for solution (IPLookup.API.Host, GeoInformationSPA)
5. ⏵ start

Docker:
to run win Docker which includes .net 4.7.2 and iis :
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

# .net core 2.1 solution file ./src/Core_IPLookupService.sln

solution can be run from MS Visual Stidio 2019 or from docker

Visual studio:
1. ⏵ start IPLookup.API.Host
2. powershell -> cd /src/SPAWeb/GeoInformationSPA/ClientApp
3. ng serve (Node.js 10+ should be installed to have ability to run Node test server)

Docker:
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

# Services also can be deployed to AWS 
0) aws account should have permissions to create S3 buckets and Lambdas.
1) S3 bucket + CloudFrom from SPA (put ./src/SPAWeb/GeoInformationSPA/ClientAppClientApp/dist folder to S3 bucket. Configure S3 bucket to handler static sites)
2) Deploy API to lambda:
	a) Publish IPLookup.API.Host.Lambda.csproj project using Lambda profile.
	b) Zip published folder
	c) Put archieve to s3 bucket
	d) update terraform.ts with bucket name and key name from c) . Set s3 bucket to store terraform state. 
	e) Run terraform script ./deploy/aws/api/terraform.ts

AWS cloud serverless solution fits best for usage spec: 10 000 000 users per day and 100 000 000 .
Cost ~25$ per day  for geo redundunt, autoscale, multi region solution with fast access throw the world.
I wanted to put inmemory db to DynamoDB and use DAX + DynamoDB to have quick and chip storage but I have no time to do that righ now.
