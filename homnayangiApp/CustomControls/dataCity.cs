using homnayangiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class dataCity
    {
        private static dataCity _instance;
        public static dataCity Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new dataCity();
                }
                return _instance;
            }
        }

        private dataCity() 
        {
            
        }
        public async Task ReloadData()
        {
            await loadCiTy();
            await getDictricts(listProvince[0].province_id);
        }
        private async Task loadCiTy()
        {
            Uri uri = new("https://vapi.vnappmob.com/api/province/");
            try
            {
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(content))
                    {
                        JsonElement root = document.RootElement;
                        if (root.TryGetProperty("results", out JsonElement resultsElement))
                        {
                            List<Province> provinces = new List<Province>();
                            foreach (JsonElement provinceElement in resultsElement.EnumerateArray())
                            {
                                Province province = new Province();
                                province.province_id = provinceElement.GetProperty("province_id").GetString();
                                province.province_name = provinceElement.GetProperty("province_name").GetString();
                                provinces.Add(province);
                            }
                            listProvince = provinces;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi truy vấn địa điểm", "Ok");
            }
        }
        public async Task getDictricts(string id)
        {
            Uri uri = new("https://vapi.vnappmob.com/api/province/district/" + id);
            try
            {
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(content))
                    {
                        JsonElement root = document.RootElement;
                        if (root.TryGetProperty("results", out JsonElement resultsElement))
                        {
                            List<string> districts = new List<string>();
                            foreach (JsonElement provinceElement in resultsElement.EnumerateArray())
                            {
                                Dictrict district = new Dictrict();
                                district.dictrict_name = provinceElement.GetProperty("district_name").GetString();
                                districts.Add(district.dictrict_name.ToString());
                            }
                            listDistrict = districts;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi truy vấn địa điểm", "Ok");
            }
        }
        // Thêm các thuộc tính để lưu trữ thông tin của bạn
        public List<Province> listProvince { get; set; } = new List<Province>();
        public List<string> listDistrict { get; set; } = new List<string>();
    }
}
