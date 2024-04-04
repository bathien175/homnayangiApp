using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class dataSignIn
    {
        private static dataSignIn _instance;
        public static dataSignIn Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new dataSignIn();
                }
                return _instance;
            }
        }

        private dataSignIn() { }

        // Thêm các thuộc tính để lưu trữ thông tin của bạn
        public string userName { get; set; } = String.Empty;
        public string userPhone { get; set; } = String.Empty;
        public string userPass { get; set; } = String.Empty;
        public string userCity { get; set; } = String.Empty;
        public string userID { get; set; } = String.Empty;
        public int userCityId { get; set; }
        public string userDistrict { get; set; } = String.Empty;
        public string userGender { get; set; } = "Nam";
        public string userDatebirth { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public ImageSource? userImage { get; set; } = null;
        public string? userImageByte { get; set; }
        public List<tagControl> listTag { get; set; } = new List<tagControl>();


        public void clearData()
        {
            userID = string.Empty;
            userName = String.Empty;
            userPass = String.Empty;
            userCity = String.Empty;
            userDistrict = String.Empty;
            userGender = String.Empty;
            userCityId = 0;
            userDatebirth = DateTime.Now.ToString("dd-MM-yyyy");
            userImage = null;
            userImageByte = null; 
            listTag.Clear();
        }
    }
}
