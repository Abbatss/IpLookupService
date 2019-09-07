docker build -f .\..\lin.LocationLookupSite.Dockerfile  -t lin-locationlookup-spa ../.

docker run --name lin-locationlookup-spa -d -p 44372:80 lin-locationlookup-spa