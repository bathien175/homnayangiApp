using Firebase.Database;
using homnayangiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class dataLogin
    {
        private static dataLogin _instance;
        public static dataLogin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new dataLogin();
                }
                return _instance;
            }
        }

        private dataLogin() { }

        // Thêm các thuộc tính để lưu trữ thông tin của bạn
        private User _curentUser =  new User();
        private bool _isAdmin = false;
        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;
            }
        }
        public User currUser 
        { 
            get 
            {
                return _curentUser;
            } 
            set 
            {
                _curentUser = value;
                if (value.IDUser == "@admin@")
                {
                    IsAdmin = true;
                }
                else
                {
                    IsAdmin = false;
                }
            }  
        } 


        public void clearData()
        {
            currUser = new User();
        }
    }
}
