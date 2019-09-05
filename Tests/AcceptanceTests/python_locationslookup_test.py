import unittest
import json
from SetupHelpers.LocationsService import LocationsService


class LocationsServiceTests(unittest.TestCase):

    @classmethod
    def setUp(self):
        f = open('TestSettings.json')
        self.settings_json = json.load(f)
        f.close()
        self.locations_lookup_api = LocationsService(self.settings_json['LocationsLookupAPI']['BaseURL'])
        return self.locations_lookup_api

    def test_get_by_ip(self):
        result = self.locations_lookup_api.get_location_by_ip('123.12.12.12')
        self.assertEqual('cit_Ijid', result['City'])

    def test_get_by_city(self):
        result = self.locations_lookup_api.get_locations_by_city('cit_Ijid')
        self.assertEqual('cit_Ijid', result[0]['City'])
        self.assertEqual(12, len(result))
        for item in result:
            self.assertEqual('cit_Ijid', item['City'])

    @classmethod
    def tearDown(self):
        pass

    if __name__ == '__main__':
        unittest.main()
