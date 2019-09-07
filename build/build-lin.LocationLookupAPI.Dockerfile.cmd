docker build -f .\..\lin.LocationLookupAPI.Dockerfile  -t lin-locationlookup-api ../.

docker run --name lin-locationlookup-api -d -p 44365:80 lin-locationlookup-api