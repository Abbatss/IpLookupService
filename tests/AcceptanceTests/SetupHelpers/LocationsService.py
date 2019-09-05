import requests
import json


class LocationsService:

    def __init__(self, url):
        self.url = url

    def get_location_by_ip(self, ip):
        get_response = requests.get(self.url + '/location?ip=' + ip, verify=False)
        return json.loads(get_response.text)

    def get_locations_by_city(self, city):
        get_response = requests.get(self.url + '/locations?city=' + city, verify=False)
        return json.loads(get_response.text)
