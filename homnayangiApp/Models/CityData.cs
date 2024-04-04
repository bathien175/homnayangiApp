using System.Text.Json;
using System;
using System.Net.Http;

namespace homnayangiApp.Models
{
    public class CityData
    {
        public List<Province> listCity = new List<Province>();
        public List<String> listDistrict = new List<String>();
        public CityData()
        {
            loadCiTy();
            getDictricts("1");
        }

        private async void loadCiTy()
        {
            Uri uri = new("https://vapi.vnappmob.com/api/province/");
            try
            {
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    listCity = JsonSerializer.Deserialize<List<Province>>(content);
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }

        public async void getDictricts(string id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://vapi.vnappmob.com/api/province/district/"+id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List< Dictrict > list = JsonSerializer.Deserialize<List<Dictrict>>(content);
                if(list.Count > 0)
                {
                    foreach ( var dict in list)
                    {
                        listDistrict.Add(dict.dictrict_name);
                    }
                }
            }
        }
    }
    public class Province
    {
        public string province_id { get; set; } = string.Empty;
        public string province_name { get; set; } = string.Empty;
    }
    public class Dictrict
    {
        public string province_id { get; set; } = string.Empty;
        public string dictrict_name { get; set; } = string.Empty;
    }
}
